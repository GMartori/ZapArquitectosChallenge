# Zap Arquitectos Challenge — Spatial Analysis

Herramienta en C# para procesar prismas rectangulares 3D (AABB): lectura JSON, validación y análisis espacial (intersección, contención, aislamiento).

## Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download) o superior

## Ejecución

```bash
dotnet run --project src/SpatialAnalysis.Console
```

Opciones de línea de comandos:

```bash
# JSON personalizado
dotnet run --project src/SpatialAnalysis.Console -- data/objects.json

# Exportar reporte a ruta específica
dotnet run --project src/SpatialAnalysis.Console -- --output output/results.txt

# Solo consola (sin archivo)
dotnet run --project src/SpatialAnalysis.Console -- --no-export
```

Por defecto la aplicación:
1. Lee `data/objects.json` (copiado al output en build)
2. Valida cada registro (§7.2) y analiza espacialmente los válidos (§7.3)
3. Muestra el reporte en consola y lo exporta a `output/results.txt`

## Estructura del proyecto

```
ZapArquitectosChallenge/
├── src/
│   ├── SpatialAnalysis.Core/     # Dominio, validación, geometría, análisis
│   └── SpatialAnalysis.Console/  # Entry point y reportería
├── data/
│   └── objects.json              # Dataset base de la consigna
└── docs/                         # Documentación de entrega
```

## Supuestos

- `(X, Y, Z)` = esquina mínima del prisma; sin rotación (ejes alineados).
- Dimensiones deben ser > 0 para considerar el objeto válido.
- IDs duplicados: se invalidan todos los registros que comparten el ID.
- Intersección en borde: cuenta (intervalos cerrados AABB).

## Documentación adicional

Ver carpeta `docs/`:

- `PROCESS.md` — proceso y decisiones de diseño
- `AI_USAGE.md` — uso de herramientas de IA
- `TEST_CASES.md` — casos adicionales
- `folder-tree.txt`, `work-history.txt`, `sample-output.txt`

## Repositorio

https://github.com/GMartori/ZapArquitectosChallenge
