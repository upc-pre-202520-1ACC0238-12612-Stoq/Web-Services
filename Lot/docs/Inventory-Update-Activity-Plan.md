# 📋 PLAN DE ACTIVIDAD: Update Inventory By Product

## 🎯 OBJETIVO ÚNICO

**Implementar únicamente y exclusivamente la funcionalidad de actualización para Inventory By Product.**

---

## ⚠️ RESTRICCIONES EXPLÍCITAS

### ❌ NO SE HARÁ:
- ❌ Testing (Unit Tests, Integration Tests)
- ❌ Documentación adicional
- ❌ Implementación para InventoryByBatch
- ❌ Implementación para Branch
- ❌ Optimizaciones de performance
- ❌ Refactorización de código existente
- ❌ Análisis de arquitectura

### ✅ SE HARÁ ÚNICAMENTE:
- ✅ Crear los 4 archivos nuevos para Update
- ✅ Modificar los 4 archivos existentes para Update
- ✅ Implementar los 2 endpoints PUT/PATCH
- ✅ Seguir exactamente el plan de implementación

---

## 📋 ACTIVIDADES EXACTAS (8 Tareas)

### FASE 1: DOMINIO (2 Tareas)

#### Tarea 1: Crear Update Command
**Archivo:** `Lot/Inventaries/Domain/Model/Commands/UpdateInventoryByProductCommand.cs`
- ✅ Crear clase `UpdateInventoryByProductCommand`
- ✅ Usar namespace: `Lot.Inventaries.Domain.Model.Commands`
- ✅ Propiedades: `Id`, `ProductoId?`, `Cantidad?`, `Precio?`, `StockMinimo?`

#### Tarea 2: Modificar Interfaz Command Service
**Archivo:** `Lot/Inventaries/Domain/Services/IInventoryByProductCommandService.cs`
- ✅ Agregar método: `Task<InventoryByProduct?> Handle(UpdateInventoryByProductCommand command)`

### FASE 2: APLICACIÓN (2 Tareas)

#### Tarea 3: Implementar Handle en Command Service
**Archivo:** `Lot/Inventaries/Application/Internal/CommandServices/InventoryByProductCommandService.cs`
- ✅ Implementar método `Handle(UpdateInventoryByProductCommand command)`
- ✅ Validar ProductoId si cambia
- ✅ Usar UnitOfWork pattern
- ✅ Recargar con relaciones

#### Tarea 4: Agregar método Update() a Entidad
**Archivo:** `Lot/Inventaries/Domain/Model/Aggregates/InventoryByProduct.cs`
- ✅ Agregar método: `Update(productoId, cantidad, precio, stockMinimo)`
- ✅ Actualizar solo si valores son diferentes
- ✅ Mantener recalculo de StockBajo

### FASE 3: API (4 Tareas)

#### Tarea 5: Crear Update Resource
**Archivo:** `Lot/Inventaries/Interfaces/REST/Resources/UpdateInventoryByProductResource.cs`
- ✅ Crear clase `UpdateInventoryByProductResource`
- ✅ Propiedades opcionales: `ProductoId?`, `Cantidad?`, `Precio?`, `StockMinimo?`

#### Tarea 6: Crear Update Stock Resource
**Archivo:** `Lot/Inventaries/Interfaces/REST/Resources/UpdateStockResource.cs`
- ✅ Crear clase `UpdateStockResource`
- ✅ Propiedades requeridas: `Cantidad`, `Precio`, `StockMinimo`

#### Tarea 7: Crear Command Assembler
**Archivo:** `Lot/Inventaries/Interfaces/REST/Transform/UpdateInventoryByProductCommandAssembler.cs`
- ✅ Crear clase estática `UpdateInventoryByProductCommandAssembler`
- ✅ Método: `ToCommandFromResource(int id, UpdateInventoryByProductResource resource)`

#### Tarea 8: Agregar Endpoints al Controller
**Archivo:** `Lot/Inventaries/Interfaces/REST/InventoryController.cs`
- ✅ Agregar endpoint PUT: `UpdateByProduct(int id, UpdateInventoryByProductResource resource)`
- ✅ Agregar endpoint PATCH: `UpdateProductStock(int id, UpdateStockResource stockResource)`

---

## 📊 ESCOPO EXACTO DEL TRABAJO

### 🔧 SOLO ESTO:
- ✅ 4 archivos nuevos para Update
- ✅ 4 archivos existentes modificados para Update
- ✅ 2 endpoints REST (PUT y PATCH)

### 🚫 NO ESTO INCLUIDO:
- ❌ Testing de ningún tipo
- ❌ Documentación adicional
- ❌ Análisis de arquitectura
- ❌ Optimizaciones
- ❌ Refactorización

---

## ✅ CRITERIO DE FINALIZACIÓN

La actividad está **completada** cuando:
- ✅ Los 8 archivos están creados/modificados
- ✅ El código compila sin errores
- ✅ Los endpoints responden correctamente a las requests

---

## 🎯 ACTIVIDAD TERMINA CUANDO

**NO necesitamos:**
- Tests pasando
- Documentación generada
- Performance validada
- Code review aprobado

**SOLO necesitamos:**
- Código implementado
- Compilación exitosa
- Funcionalidad básica operativa