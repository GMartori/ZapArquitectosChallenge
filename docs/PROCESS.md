# PROCESS.md

Documento de proceso y decisiones de diseño — Zap Arquitectos Challenge.

## Interpretación del problema

La consigna pide una herramienta en C# que procese **prismas rectangulares 3D alineados a ejes** (AABB) a partir de un JSON. El flujo tiene tres etapas:

1. **Lectura** del archivo JSON (§7.1).
2. **Validación** de cada registro; los inválidos se reportan pero **no** participan del análisis espacial (§7.2).
3. **Análisis espacial** sobre válidos: intersección, contención total y aislamiento (§7.3–§7.5), con reporte consolidado (§7.4).

Cada objeto se modela con origen `(X, Y, Z)` = esquina mínima y dimensiones `(Width, Height, Depth)`. No hay rotación: los ejes del prisma coinciden con los ejes del sistema de coordenadas.

## Metodología y herramientas de trabajo

Además del stack .NET, el desarrollo se apoyó en una metodología orientada a especificación, integraciones MCP y skills de buenas prácticas en Cursor.

### Spec-Driven Development (SDD)

**Spec-Driven Development** — desarrollo guiado por la especificación — fue el marco principal del proyecto:

1. **Especificación fuente:** [documento de consigna](https://docs.google.com/document/d/1ReZhomQqdr2NzBu3TZ_QtVFFSzEJaCiU/edit) (§7.1–§7.6) como contrato antes de escribir código.
2. **Sesión 0 — diseño sin código:** análisis de arquitectura, supuestos y roadmap alineados a la consigna.
3. **Fases numeradas:** cada entregable del documento (parsing, validación, geometría, reporte, casos adicionales) mapeado a una fase con criterio de cierre explícito.
4. **Verificación continua:** al cerrar cada fase, comparar salida real (`dotnet run`, `sample-output.txt`, `TEST_CASES.md`) contra lo esperado en la spec.
5. **Auditoría de alineación:** repaso formal de §7.1–§7.5 antes de Fase 6 para evitar desviaciones sobre lo esencial.
6. **Deuda documental trazada:** ítems fuera de la spec técnica inmediata (`PROCESS.md`, `AI_USAGE.md`) registrados y cerrados en Fase 7.

El SDD permitió priorizar lo obligatorio (validación, intersección, contención, aislados, reporte) y postergar mejoras opcionales (tests auto, DI elaborado, indexación espacial).

### MCPs (Model Context Protocol)

Integraciones MCP en Cursor para orquestar el flujo fuera del IDE:

| MCP | Uso en este proyecto |
|---|---|
| **Notion** | Hub del challenge: arquitectura, modelo de datos, algoritmo AABB, roadmap, prompts IA, entregables |
| **Trello** | Kanban por fase (Por hacer / En progreso / Hecho) con checklist de entrega |
| **GitHub** | Repo `GMartori/ZapArquitectosChallenge` — commit y push al cerrar cada fase |
| **Google Workspace** | Acceso y referencia al documento de consigna (Google Doc) |
| **Tavily** | Búsqueda web puntual para contexto de la prueba y enlaces externos cuando hizo falta |

Otros MCPs disponibles en el entorno (Railway, Supabase, Playwright, etc.) no formaron parte del alcance de esta prueba take-home.

**Ritual de cierre por fase (SDD + MCPs):**

```
Spec (§7.x) → Implementar → dotnet build/run → work-history.txt
    → git commit + push (GitHub) → Trello (Hecho) → Notion (roadmap)
```

### Skills y buenas prácticas

Skills de Cursor y reglas de trabajo que guiaron decisiones de código y arquitectura:

| Skill / práctica | Aplicación |
|---|---|
| **Análisis antes de código** | Primera sesión solo diseño; sin scaffolding hasta acordar pipeline y supuestos |
| **KISS y alcance mínimo** | O(n²), sin octree, sin DI container, sin capa de persistencia en la entrega |
| **Separación de responsabilidades** | Core reutilizable vs Console; validación separada de geometría |
| **Convenciones del repo** | Nombres, carpetas y estilo alineados fase a fase; cambios focalizados por commit |
| **caveman-policy** | Comunicación comprimida en sesiones de código; documentación (Notion, PROCESS, PRs) en prosa completa |
| **caveman-commit** | Mensajes de commit concisos en Conventional Commits, con el “por qué” cuando aporta |
| **Validación manual explícita** | Dataset base + casos adicionales + `sample-output.txt` en lugar de tests automatizados por límite de tiempo |
| **Documentar supuestos** | IDs duplicados, bordes cerrados, campos JSON ausentes — decisiones explícitas en README y PROCESS |

Principio rector: la IA acelera scaffolding y documentación; las decisiones de arquitectura y los criterios geométricos se validaron manualmente contra la especificación y la salida de consola.

## Estructura de la solución

Arquitectura **Core + Console** para separar lógica de negocio de la capa de presentación (consola hoy, WPF en el futuro).

```
JSON → DTOs → Validación → Objetos válidos + Errores
                              ↓
                         Motor AABB (O(n²))
                              ↓
                    Intersecciones · Contenciones · Aislados
                              ↓
                         Reporte (consola + archivo)
```

### Proyectos

| Proyecto | Responsabilidad |
|---|---|
| `SpatialAnalysis.Core` | DTOs, parsing, dominio, validación, geometría, análisis |
| `SpatialAnalysis.Console` | CLI, composición, reportería (`ConsoleReportWriter`) |

### Carpetas clave en Core

- `Input/` — `SpatialObjectDto` (reflejo 1:1 del JSON).
- `Parsing/` — `SpatialObjectJsonReader` (`System.Text.Json`).
- `Domain/` — value objects (`Point3D`, `Dimensions`, `AxisAlignedBox`) y entidad `SpatialObject` (solo válidos).
- `Validation/` — reglas modulares + `SpatialObjectValidator` → `ProcessingBatch`.
- `Geometry/` — `AabbGeometry` (intersección y contención).
- `Analysis/` — `SpatialAnalyzer` y modelos de resultado.

Ver árbol completo en `docs/folder-tree.txt`.

## Decisiones de diseño

| Decisión | Elección | Justificación |
|---|---|---|
| Arquitectura | Core + Console | Reutilizar lógica en UI futura sin reescribir |
| Parsing | `System.Text.Json` | Nativo en .NET 9, sin dependencias extra |
| Validación | Reglas separadas (`IObjectValidationRule`) | Extensible; cada regla una responsabilidad |
| IDs duplicados | Invalidar **todos** los registros con ese Id | Criterio conservador, fácil de explicar |
| Intersección en borde | Sí cuenta (`<=` en los tres ejes) | Intervalos cerrados AABB estándar |
| Contención | Inclusiva (`inner.Min >= outer.Min`, `inner.Max <= outer.Max`) | Predecible; cajas idénticas = contenidas |
| Aislado | Sin intersección ni contención con ningún otro válido | Incluye contención en la definición de “relación” |
| Complejidad espacial | O(n²) pares | Suficiente para n ≈ 10–50 del dataset |
| Inválidos en análisis | Excluidos del motor; incluidos en reporte | Alineado con §7.2 |
| Export | `output/results.txt` por defecto; `--no-export` / `--output` | Cumple §7.4 sin sobre-ingeniería |

### Algoritmo AABB

Para origen `(X,Y,Z)` y dimensiones `(W,H,D)`:

- `Min = (X, Y, Z)`
- `Max = (X+W, Y+H, Z+D)`

**Intersección:** solapamiento en los tres ejes con intervalos cerrados.

**Contención:** `inner` contenido en `outer` si cumple la inclusión en X, Y y Z.

**Aislamiento:** para cada objeto V, ningún otro W válido cumple intersección ni contención con V.

## Simplificaciones por tiempo

Decisiones conscientes para cumplir el alcance en ~4 h:

1. **Sin tests automatizados** — validación manual con `objects.json`, casos adicionales y `sample-output.txt`.
2. **Sin DI container** — instanciación directa en `Program.cs` / `CompositionRoot`.
3. **Sin capa de persistencia** — solo archivos JSON de entrada y TXT de salida.
4. **O(n²) fijo** — sin octree, sweep line ni indexación espacial.
5. **DTO sin distinción “campo ausente” vs “cero”** — `System.Text.Json` deserializa campos numéricos faltantes como `0`; no se diferencia de un cero explícito (ver supuesto abajo).
6. **Un único formato de reporte** — texto plano en consola/archivo; sin HTML, PDF ni serialización JSON de resultados.
7. **Reglas de validación fijas** — no hay configuración externa de reglas.

### Supuesto documentado: campos JSON ausentes

Si un campo numérico no está en el JSON, el deserializador lo asigna a `0`. Eso puede disparar `NonPositiveDimension` igual que un `Width: 0` explícito. No se implementó un modelo con `JsonElement` o tipos nullable por eje para distinguir “faltante” de “cero”, por costo/beneficio en el tiempo disponible.

## Mejoras futuras

Ordenadas por impacto si el proyecto escala:

1. **Tests unitarios** — `AabbGeometry`, reglas de validación, `SpatialAnalyzer` con casos de borde.
2. **Proyecto de tests** (`xUnit` o `NUnit`) integrado en CI.
3. **Indexación espacial** — grid uniforme u octree si n >> 100.
4. **Capa WPF** — consumir `SpatialAnalysis.Core` con visualización 3D.
5. **Persistencia** — ver sección siguiente.
6. **Parsing robusto** — detectar campos ausentes vs valores explícitos.
7. **Múltiples formatos de export** — JSON, CSV, HTML.
8. **Pipeline configurable** — reglas y umbrales por archivo de configuración.

## Casos adicionales

Dos casos propios en `data/cases/` (§7.6), documentados en `docs/TEST_CASES.md`:

| Caso | Archivo | Qué valida |
|---|---|---|
| Contacto por cara | `case-01-face-contact.json` | Intersección con intervalos cerrados (cajas que se tocan en X=5) |
| Contención anidada | `case-02-nested-containment.json` | Outer ⊃ Middle ⊃ Inner (3 intersecciones, 3 contenciones) |

Durante el caso 1 se detectó que `Intersects` usaba `<` estricto; se corrigió a `<=` para alinear código, README y este documento.

**Dataset base** (`data/objects.json`): 12 procesados · 8 válidos · 4 inválidos · 4 intersecciones · 2 contenidos · 1 aislado (J). Referencia en `docs/sample-output.txt`.

## Persistencia en base de datos

No implementada en esta entrega (alcance consola + archivos). Diseño propuesto para una extensión futura:

### Esquema relacional sugerido

```sql
-- Lote de procesamiento
CREATE TABLE ProcessingRun (
    Id          INT IDENTITY PRIMARY KEY,
    SourceFile  NVARCHAR(500) NOT NULL,
    ProcessedAt DATETIME2 NOT NULL,
    TotalCount  INT NOT NULL,
    ValidCount  INT NOT NULL,
    InvalidCount INT NOT NULL
);

-- Objetos (válidos e inválidos)
CREATE TABLE SpatialObjectRecord (
    Id          INT IDENTITY PRIMARY KEY,
    RunId       INT NOT NULL REFERENCES ProcessingRun(Id),
    SourceIndex INT NOT NULL,
    ObjectId    INT NOT NULL,
    Name        NVARCHAR(200),
    Category    NVARCHAR(100),
    X, Y, Z     FLOAT NOT NULL,
    Width, Height, Depth FLOAT NOT NULL,
    IsValid     BIT NOT NULL
);

CREATE TABLE ValidationError (
    Id          INT IDENTITY PRIMARY KEY,
    RecordId    INT NOT NULL REFERENCES SpatialObjectRecord(Id),
    Code        NVARCHAR(50) NOT NULL,
    Field       NVARCHAR(50),
    Message     NVARCHAR(500) NOT NULL
);

-- Resultados espaciales (solo entre válidos del mismo RunId)
CREATE TABLE IntersectionPair (
    RunId INT, ObjectAId INT, ObjectBId INT,
    PRIMARY KEY (RunId, ObjectAId, ObjectBId)
);

CREATE TABLE ContainmentPair (
    RunId INT, InnerId INT, OuterId INT,
    PRIMARY KEY (RunId, InnerId, OuterId)
);

CREATE TABLE IsolatedObject (
    RunId INT, ObjectId INT,
    PRIMARY KEY (RunId, ObjectId)
);
```

### Flujo de persistencia

1. Insertar `ProcessingRun` al iniciar.
2. Guardar cada registro con su flag `IsValid` y errores asociados.
3. Tras `SpatialAnalyzer.Analyze`, persistir pares e aislados referenciando `SpatialObjectRecord.Id`.
4. El reporte TXT/consola podría generarse desde SQL o seguir desde memoria.

`SpatialAnalysis.Core` permanecería sin referencia a EF/SQL; la capa de infraestructura viviría en un proyecto `SpatialAnalysis.Infrastructure` nuevo.
