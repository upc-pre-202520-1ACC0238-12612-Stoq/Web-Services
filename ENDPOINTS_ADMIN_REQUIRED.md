# 🔐 ENDPOINTS QUE REQUIEREN PERMISOS DE ADMINISTRADOR

## 📦 PRODUCTS CONTEXT

### **ProductsController**
```
POST /api/v1/products
```
**Razón:** Impacto financiero directo en precios y estructura del catálogo

### **CombosController**
```
POST /api/v1/combos
```
**Razón:** Impacto estratégico en rentabilidad y gestión de inventario

---

## 📦 INVENTORY CONTEXT

### **InventoryController**
```
POST /api/v1/inventory/by-product
POST /api/v1/inventory/by-batch
PUT /api/v1/inventory/by-product/{id}
PATCH /api/v1/inventory/by-product/{id}/stock
DELETE /api/v1/inventory/by-product/{id}
DELETE /api/v1/inventory/by-batch/{id}
```
**Razón:** Operaciones que afectan precios, costos y valor de activos del inventario

---

## 🏢 BRANCH CONTEXT

### **BranchController**
```
POST /api/v1/branches
PUT /api/v1/branches/{id}
PATCH /api/v1/branches/{id}/stock
DELETE /api/v1/branches/{id}
```
**Razón:** Decisiones estratégicas que impactan estructura organizacional y activos comerciales

---

## 🔐 ADMINISTRACIÓN DE USUARIOS

### **AuthenticationController**
```
PATCH /api/v1/auth/role
```
**Razón:** Cambiar roles de usuarios es función administrativa crítica

---

## 📊 REPORTES CONTEXT

### **ReportController**
```
GET /api/v1/reports
GET /api/v1/reports/inventory
GET /api/v1/reports/sales
GET /api/v1/reports/financial
```
**Razón:** Reportes contienen información estratégica y financiera sensible

---

## 🎯 TOTAL DE ENDPOINTS ADMIN REQUIRED: **15**

### **Desglose por Contexto:**
- **Products:** 2 endpoints
- **Inventory:** 6 endpoints
- **Branches:** 4 endpoints
- **Authentication:** 1 endpoint
- **Reports:** 2+ endpoints

## ⚠️ NOTA IMPORTANTE

Todos los endpoints listados actualmente tienen configuración `[AuthorizeRoles("Administrator", "Employee")]` y **deben ser cambiados a** `[AuthorizeRoles("Administrator")]` para proteger adecuadamente los activos y operaciones financieras del negocio.