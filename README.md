# Backend API Sistema Kardex (Compras, Ventas e Inventario)

Este proyecto es una Web API desarrollada en .NET 8 para la gestión de productos, compras, ventas y movimientos de inventario (Kardex). Incluye autenticación mediante JWT.

--------------------------------------------
TECNOLOGIAS UTILIZADAS
--------------------------------------------
- .NET 8 Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger

--------------------------------------------
REQUISITOS PREVIOS
--------------------------------------------
- Tener instalado .NET SDK 8
- Tener instalado SQL Server (Express o superior)
- Tener instalado Visual Studio o Visual Studio Code

--------------------------------------------
CONFIGURACION DEL PROYECTO
--------------------------------------------

1. Clonar el repositorio del backend

git clone <URL_DEL_REPOSITORIO_BACKEND>

2. Ingresar a la carpeta del proyecto

cd <NOMBRE_PROYECTO_BACKEND>

3. Configurar la cadena de conexión a base de datos en el archivo appsettings.json

Ejemplo:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=KardexDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

4. Restaurar paquetes NuGet

dotnet restore

5. Ejecutar migraciones en la base de datos

dotnet ef database update

Si no tienes migraciones creadas, primero ejecutar:

dotnet ef migrations add InitialCreate

--------------------------------------------
EJECUCION DEL PROYECTO
--------------------------------------------

1. Compilar el proyecto

dotnet build

2. Ejecutar el proyecto

dotnet run

La API se ejecutará por defecto en:

http://localhost:5000

--------------------------------------------
DOCUMENTACION SWAGGER
--------------------------------------------

Para probar los endpoints ingresar a:

http://localhost:5000/swagger

--------------------------------------------
AUTENTICACION JWT
--------------------------------------------

Para consumir los endpoints protegidos se debe enviar el token en el header:

Authorization: Bearer {token}

El token se obtiene desde el endpoint de login.

--------------------------------------------
ENDPOINTS PRINCIPALES
--------------------------------------------

PRODUCTOS
- GET /api/productos
- POST /api/productos
- PUT /api/productos

COMPRAS
- POST /api/compras

VENTAS
- POST /api/ventas

MOVIMIENTOS (KARDEX)
- GET /api/movimientos/{id_producto}

--------------------------------------------
REGLAS DE NEGOCIO
--------------------------------------------

COMPRAS:
- Se registra CompraCab y CompraDet
- Se actualiza el producto:
  - costo
  - precio venta = costo * 1.35
  - stock
- Se genera movimiento tipo Entrada

VENTAS:
- Se valida stock disponible
- Se calcula:
  - subtotal = cantidad * precio venta
  - igv = subtotal * 0.18
  - total = subtotal + igv
- Se genera movimiento tipo Salida

KARDEX:
- Muestra todos los movimientos del producto (entradas y salidas)
