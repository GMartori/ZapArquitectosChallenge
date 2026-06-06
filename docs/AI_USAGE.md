# AI_USAGE.md

Registro del uso de herramientas de IA durante el desarrollo de la prueba técnica Zap Arquitectos.

## Herramientas utilizadas

| Herramienta | Uso |
|---|---|
| Cursor (Claude) | Análisis de arquitectura, scaffolding, implementación por fases, documentación |
| Notion MCP | Hub de arquitectura, roadmap, prompts de referencia |
| Trello MCP | Kanban por fase (Por hacer / En progreso / Hecho) |

## Prompts representativos

### Prompt 1 — Análisis y diseño de arquitectura (2026-06-06)

**Tarea:** Definir modelo de datos, algoritmo AABB, estructura de carpetas y roadmap **sin generar código**.

**Prompt (resumido):**
> Actúa como Arquitecto de Software Senior especializado en .NET. Objetivo: ANÁLISIS y DISEÑO DE ARQUITECTURA (sin código). Referencia al documento de la prueba técnica de prismas 3D. Estructurar en 4 bloques: (1) Supuestos y Modelo de Datos, (2) Estrategia Algoritmo Espacial AABB, (3) Arquitectura y Carpetas, (4) Plan de Acción por Pasos. KISS, ~4 h efectivas.

**Adoptado:**
- Pipeline JSON → Validación → Geometría → Reporte
- Core + Console (escalable a WPF)
- Reglas de validación separadas del dominio espacial
- O(n²) para intersección, contención y aislamiento
- Supuestos documentados (IDs duplicados, bordes cerrados)

**Descartado:** Over-engineering (octrees, múltiples capas, DI elaborado).

**Validación:** Revisión manual contra [consigna](https://docs.google.com/document/d/1ReZhomQqdr2NzBu3TZ_QtVFFSzEJaCiU/edit) y dataset de ejemplo (K, L, E inválidos; B⊂A, G⊂F esperados).

---

### Prompt 2 — Bootstrap y flujo de trabajo por fases (2026-06-06)

**Tarea:** Ubicar el proyecto, clonar repo y acordar ritual de cierre por fase.

**Prompts (texto del usuario):**
> ¿Dónde recomendás crear el proyecto? Tengo repo: github.com/GMartori/ZapArquitectosChallenge

> Con cada fase me gustaría ir avanzando en la documentación de Notion y las tarjetas de Trello… Podríamos actualizar esto antes de seguir con la fase 1?

> Bien, podríamos en cada fase hacer el push al repo? Necesitaríamos ir alimentando el work-history.txt

**Adoptado:**
- Clonar en `C:\Users\Usuario\ZapArquitectosChallenge`
- Solución .NET 9: `SpatialAnalysis.Core` + `SpatialAnalysis.Console`
- Al cerrar cada fase: `dotnet build` → `work-history.txt` → commit → push → Trello (Hecho) → Notion (roadmap)

**Descartado:** Trabajar fuera del repo GitHub provisto.

**Validación:** Fase 0 con build/run OK; JSON copiado al output del proyecto Console.

---

### Prompt 3 — Implementación iterativa por fase (2026-06-06)

**Tarea:** Implementar Fases 1–5 siguiendo la consigna sin desviarse del alcance esencial.

**Prompts (patrón repetido):**
> Vamos con la fase 1… Al terminar: validación de errores, compilación correcta, actualización de roadmap, Notion y Trello, repo y work-history.txt. Tengamos siempre presente lo solicitado en el documento inicial.

> Avancemos con fase 2… siempre el mismo proceso.

> Vamos con fase 3… / Sigamos… / si, sigamos…

**Adoptado:**
- Fase 1: `SpatialObjectDto` + `SpatialObjectJsonReader`
- Fase 2: reglas de validación + `ProcessingBatch` (8 válidos / 4 inválidos)
- Fase 3: `AabbGeometry` + `SpatialAnalyzer`
- Fase 4: aislados + resumen §7.5
- Fase 5: `ConsoleReportWriter` + export CLI

**Descartado / corregido:**
- `ValidationReportWriter` separado → unificado en `ConsoleReportWriter`
- Colisión de namespace `Console` → `global::System.Console` en el writer

**Validación:** `dotnet build` 0 errores en cada fase; resultados del dataset base comparados con expectativas de la consigna (`sample-output.txt`).

---

### Prompt 4 — Auditoría de alineación antes de Fase 6 (2026-06-06)

**Tarea:** Verificar que lo implementado cumple lo esencial antes de casos adicionales.

**Prompt:**
> Antes de seguir con la fase 6, quisiera hacer un repaso sobre el documento inicial si aún estamos alineados con lo propuesto… Quiero evitar desviaciones sobre lo marcado como esencial y necesario…

**Adoptado:**
- Confirmación de cobertura §7.1–§7.5
- Identificación de deuda documental: `PROCESS.md`, `AI_USAGE.md` (parcial), `TEST_CASES.md` (vacío)
- §7.6 y entrega final diferidos a fases 6–7

**Descartado:** Mejoras opcionales (tests auto, DI, export múltiple) antes de cerrar obligatorios.

**Validación:** Checklist manual contra consigna; usuario autorizó continuar con Fase 6 dejando docs para Fase 7.

---

### Prompt 5 — Casos adicionales y corrección de bordes (2026-06-06)

**Tarea:** §7.6 — agregar ≥2 casos propios y documentar en `TEST_CASES.md`.

**Prompt:**
> Si más adelante vamos a completar lo que venimos con deuda (PROCESS.md y AI_USAGE.md), sigamos con la fase siguiente…

**Adoptado:**
- `data/cases/case-01-face-contact.json` — contacto por cara
- `data/cases/case-02-nested-containment.json` — Outer ⊃ Middle ⊃ Inner
- `docs/TEST_CASES.md` con intención, esperado y obtenido
- Fix en `AabbGeometry.Intersects`: `<` → `<=` (intervalos cerrados)

**Descartado:** Casos extra opcionales (separación por ε, caja 1×1×1) — no necesarios para cumplir “≥2 casos”.

**Validación:** Tres ejecuciones (`case-01`, `case-02`, `objects.json`) sin regresión en el dataset base.

---

## Validación final

| Aspecto | Cómo se validó |
|---|---|
| Compilación | `dotnet build` — 0 errores en todas las fases |
| Dataset consigna | 12/8/4 válidos/inválidos; 4 intersecciones; 2 contenidos; 1 aislado (J) |
| Casos propios | `TEST_CASES.md` — esperado = obtenido en ambos casos |
| Alineación consigna | Repaso manual §7.1–§7.6 antes y después de Fase 6 |
| IA como apoyo | Arquitectura y scaffolding por IA; decisiones finales y criterios geométricos revisados manualmente contra el documento y la salida de consola |

La IA aceleró scaffolding, estructura de carpetas y redacción de documentación. La corrección de intervalos cerrados en `Intersects` surgió al diseñar el caso de contacto por cara y verificar la salida real frente al supuesto documentado.
