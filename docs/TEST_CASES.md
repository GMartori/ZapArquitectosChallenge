# TEST_CASES.md

Casos adicionales propios (§7.6). Cada uno se ejecuta con su archivo JSON:

```bash
dotnet run --project src/SpatialAnalysis.Console -- data/cases/case-01-face-contact.json --no-export
dotnet run --project src/SpatialAnalysis.Console -- data/cases/case-02-nested-containment.json --no-export
```

---

## Caso 1 — Contacto por cara

**Archivo:** `data/cases/case-01-face-contact.json`

**Intención:** Validar el criterio de intersección con intervalos cerrados. Dos cajas iguales de 5×5×5 se tocan en la cara X=5 sin solapamiento de volumen.

| Objeto | Origen | Tamaño | Rango X |
|---|---|---|---|
| Left (101) | (0,0,0) | 5×5×5 | [0, 5] |
| Right (102) | (5,0,0) | 5×5×5 | [5, 10] |

**Esperado:**
- 2 válidos, 0 inválidos
- 1 intersección: Left ∩ Right (contacto por cara)
- 0 contenciones
- 0 aislados

**Obtenido:** Coincide con lo esperado.

**Nota:** Este caso motivó alinear `AabbGeometry.Intersects` con `<=` (intervalos cerrados), coherente con README y PROCESS.

---

## Caso 2 — Contención anidada triple

**Archivo:** `data/cases/case-02-nested-containment.json`

**Intención:** Validar contención en estructura anidada Outer ⊃ Middle ⊃ Inner.

| Objeto | Origen | Tamaño |
|---|---|---|
| Outer (201) | (0,0,0) | 20×20×20 |
| Middle (202) | (5,5,5) | 10×10×10 |
| Inner (203) | (8,8,8) | 4×4×4 |

**Esperado:**
- 3 válidos, 0 inválidos
- 3 intersecciones: Outer∩Middle, Outer∩Inner, Middle∩Inner
- 3 contenciones: Middle⊂Outer, Inner⊂Middle, Inner⊂Outer
- 0 aislados

**Obtenido:** Coincide con lo esperado.

---

## Dataset base (consigna)

**Archivo:** `data/objects.json`

Resultados de referencia documentados en `docs/sample-output.txt` (12 procesados, 8 válidos, 4 inválidos, 4 intersecciones, 2 contenidos, 1 aislado).
