# Products API | Prueba Técnica Analista Desarrollador

API REST para gestión de catálogo de productos y control de stock, construida con .NET 8 aplicando Clean Architecture, persistencia en PostgreSQL y desplegada en Railway.

## URL de despliegue

- **Swagger UI**: https://products-production-bc2b.up.railway.app/swagger
- **API base**: https://products-production-bc2b.up.railway.app/api

## Arquitectura

El proyecto aplica **Clean Architecture**, con dependencias apuntando hacia el dominio. Se organiza en cuatro proyectos dentro de una misma solución:

| Proyecto | Responsabilidad | Dependencias |
|----------|-----------------|--------------|
| `Products.Domain` | Entidad `Product` con las variantes del negocio (stock no negativo, precio positivo) | Ninguna |
| `Products.Application` | Puertos (interfaces) que definen los contratos hacia infraestructura | `Domain` |
| `Products.Infrastructure` | Implementación de persistencia con EF Core + PostgreSQL | `Application`, `Domain` |
| `Products.API` | Controladores REST, configuración de Swagger, punto de entrada | `Application`, `Infrastructure` |

## Tecnologías utilizadas

- **.NET 8** (C#)
- **ASP.NET Core** para la API REST
- **Entity Framework Core 8** como ORM
- **PostgreSQL** como base de datos (Railway gestionado)
- **Swagger / OpenAPI** para documentación
- **Docker** para empaquetado y despliegue
- **Railway** para hosting

## Cómo ejecutar localmente

### Requisitos previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) o superior
- PostgreSQL local o una instancia accesible (opcional, se puede usar un contenedor Docker)
- Git

### Pasos

```bash
# 1. Clonar el repositorio
git clone https://github.com/martinwilches008/Products.git
cd Products

# 2. Restaurar dependencias
dotnet restore

# 3. Compilar la solución
dotnet build

# 4. Configurar la cadena de conexión
# Opción A: variable de entorno
export DATABASE_URL="postgresql://usuario:clave@localhost:5432/products"
# En Windows PowerShell:
$env:DATABASE_URL = "postgresql://usuario:clave@localhost:5432/products"

# Opción B: editar appsettings.Development.json y agregar una sección ConnectionStrings

# 5. Ejecutar la API
dotnet run --project src/Products.Api
```

## Ejecutar con Docker

```bash
# Construir la imagen
docker build -t products-api .

# Ejecutar el contenedor
docker run -p 8080:8080 \
  -e DATABASE_URL="postgresql://usuario:clave@host:5432/products" \
  products-api
```

## Endpoints

| Método | Ruta | Descripción | Códigos |
|--------|------|-------------|---------|
| `POST` | `/api/products` | Crea un producto | `201`, `400` |
| `GET` | `/api/products/{id}` | Consulta un producto por ID | `200`, `404` |
| `GET` | `/api/products?page=1&pageSize=10` | Lista productos paginados | `200` |
| `PATCH` | `/api/products/{id}/stock` | Ajusta el stock (suma o resta) | `204`, `400`, `404` |

## Ejemplos de uso

### Crear producto

```bash
curl -X POST "https://products-production-bc2b.up.railway.app/api/products" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Producto de prueba",
    "description": "Descripcion del producto",
    "price": 19.99,
    "initialStock": 100
  }'
```

### Consultar productos por ID

```bash
curl "https://products-production-bc2b.up.railway.app/api/products/ba728f89-829a-4a9e-a40f-0093c399b96f"
```

### Aumentar Stock

```bash
curl -X PATCH "https://products-production-bc2b.up.railway.app/api/products/ba728f89-829a-4a9e-a40f-0093c399b96f/stock" \
  -H "Content-Type: application/json" \
  -d '{"quantity": 50}'
```

### Disminuir Stock

```bash
curl -X PATCH "https://products-production-bc2b.up.railway.app/api/products/ba728f89-829a-4a9e-a40f-0093c399b96f/stock" \
  -H "Content-Type: application/json" \
  -d '{"quantity": -20}'
```

### Validación: Intentar dejar Stock en negativo

```bash
curl -X PATCH "https://products-production-bc2b.up.railway.app/api/products/ba728f89-829a-4a9e-a40f-0093c399b96f/stock" \
  -H "Content-Type: application/json" \
  -d '{"quantity": -9999}'
```

#### Uso de PATCH en lugar de PUT para el stock

> El endpoint de stock realiza una actualización parcial (solo modifica un campo), por lo que semánticamente corresponde a PATCH. PUT requeriría enviar el recurso completo lo cual no se esta realizando en esta API.

## Estructura del repositorio

```bash
Products/
├── Products.sln
├── Dockerfile
├── README.md
├── .gitignore
└── src/
    ├── Products.Domain/
    │   ├── Entities/
    │   │   └── Product.cs
    │   └── Products.Domain.csproj
    ├── Products.Application/
    │   ├── Ports/
    │   │   └── IProductRepository.cs
    │   └── Products.Application.csproj
    ├── Products.Infrastructure/
    │   ├── Persistence/
    │   │   └── AppDbContext.cs
    │   ├── Repositories/
    │   │   └── ProductRepository.cs
    │   ├── Migrations/
    │   │   ├── 20261014045111_InitialCreate.cs
    │   │   ├── 20261014045111_InitialCreate.Designer.cs
    │   │   └── AppDbContextModelSnapshot.cs
    │   └── Products.Infrastructure.csproj
    └── Products.Api/
        ├── Controllers/
        │   └── ProductsController.cs
        ├── Models/
        │   └── RequestModels.cs
        ├── Program.cs
        └── Products.API.csproj
```