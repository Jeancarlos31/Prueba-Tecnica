# Prueba Técnica – Gestión de Productos

Aplicación web con backend en **ASP.NET Core Web API (C#)** y frontend en **Angular**, que
implementa un CRUD completo de productos.

## Estructura del repositorio

```
backend/
  ProductosApi.sln
  ProductosApi/            Proyecto Web API (.NET 10, C#)
frontend/                  Proyecto Angular (standalone components)
```

## Backend – ProductosApi (ASP.NET Core Web API)

### Endpoints

| Método | Ruta                  | Descripción                     | Códigos de respuesta                              |
|--------|-----------------------|----------------------------------|-----------------------------------------------------|
| GET    | `/api/Products`       | Lista todos los productos        | 200 OK                                               |
| GET    | `/api/Products/{id}`  | Obtiene un producto por Id       | 200 OK, 404 Not Found                                |
| POST   | `/api/Products`       | Crea un producto                 | 201 Created, 400 Bad Request                         |
| PUT    | `/api/Products/{id}`  | Actualiza un producto existente  | 204 No Content, 400 Bad Request, 404 Not Found       |
| DELETE | `/api/Products/{id}`  | Elimina un producto              | 204 No Content, 404 Not Found                        |

La documentación interactiva (Swagger) queda disponible en `/swagger` al ejecutar el proyecto
en modo Development.

### Requisitos

- .NET 10 SDK
- SQL Server (LocalDB, Developer Edition, o una instancia en Docker)

### Configuración de la base de datos

El proyecto usa **Entity Framework Core con el proveedor de SQL Server**. La cadena de conexión
está en `backend/ProductosApi/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductosDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

Por defecto apunta a **SQL Server LocalDB** (incluido con Visual Studio en Windows). Si se
prefiere usar una instancia completa de SQL Server (local o en Docker), reemplazar por algo como:

```json
"Server=localhost;Database=ProductosDb;User Id=sa;Password=TuPassword123;TrustServerCertificate=True"
```

Al iniciar la aplicación, `Program.cs` ejecuta `Database.EnsureCreated()`, que crea la base de
datos y la tabla `Products` automáticamente (con 3 productos de ejemplo) si no existen. No es
necesario ejecutar migraciones manualmente.

### Ejecutar el backend

```bash
cd backend/ProductosApi
dotnet restore
dotnet run
```

Por defecto queda disponible en `https://localhost:7050` (HTTPS) y `http://localhost:5050`
(HTTP), configurado en `Properties/launchSettings.json`.

## Frontend – Angular

### Funcionalidades

- Listar productos
- Ver detalle de un producto de la lista
- Eliminar producto
- Crear producto
- Editar producto
- Consultar producto por Id (campo de búsqueda en el listado)
- Si el backend responde con un status distinto de éxito (2xx), se muestra una alerta
  (componente Bootstrap `alert`) con un mensaje descriptivo, mediante un `HttpInterceptor`
  centralizado (`core/interceptors/error.interceptor.ts`).

### Requisitos

- Node.js 18+ y npm
- Angular CLI (`npm install -g @angular/cli`) — opcional, también puede usarse con `npx`

### Configuración

La URL del backend se configura en `frontend/src/environments/environment.ts`:

```ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7050/api/Products'
};
```

Ajustar el puerto si el backend se ejecuta en uno distinto.

### Ejecutar el frontend

```bash
cd frontend
npm install
npm start   # equivalente a: ng serve
```

La aplicación queda disponible en `http://localhost:4200`.

> El backend ya tiene configurado CORS para aceptar peticiones desde
> `http://localhost:4200` / `https://localhost:4200`.

## Herramientas y frameworks utilizados (justificación)

- **ASP.NET Core Web API (.NET 10, C#)**: requerido por la prueba; se usó el modelo de hosting
  mínimo (`Program.cs`) por ser el estándar actual de .NET para Web APIs, más simple y con menos
  código repetitivo que el modelo clásico de `Startup.cs`.
- **Entity Framework Core + SQL Server**: ORM oficial de Microsoft, permite trabajar con un
  enfoque Code-First (modelo `Product` en C#) sin escribir SQL manualmente, y SQL Server es una
  de las opciones sugeridas en el enunciado. Se usó `EnsureCreated()` en lugar de migraciones
  para simplificar la puesta en marcha del proyecto (no requiere ejecutar comandos adicionales
  de `dotnet ef`), manteniendo datos semilla para pruebas inmediatas.
- **Swashbuckle / Swagger**: expone documentación interactiva del API (`/swagger`), útil para
  probar los endpoints sin necesidad del frontend.
- **Angular (standalone components)**: framework sugerido por el enunciado; se usó la API
  moderna de *standalone components* (sin `NgModule`), que es el enfoque recomendado desde
  Angular 17+, reduce configuración y clarifica las dependencias de cada componente.
- **Reactive Forms**: para el formulario de creación/edición de productos, con validaciones
  (nombre obligatorio, precio y stock no negativos) y mensajes de error en pantalla.
- **HttpInterceptor funcional**: centraliza el manejo de errores HTTP en un solo lugar, evitando
  repetir lógica de "mostrar alerta" en cada componente que llama al backend.
- **Bootstrap 5 (vía CDN)**: una de las opciones sugeridas en el enunciado para estilos; se
  cargó directamente desde CDN en `index.html` para no añadir dependencias npm adicionales al
  proyecto y mantener el frontend liviano.

## Notas

- No se utilizó scaffolding tipo `GridView` ni asistentes visuales de Visual Studio: tanto los
  controladores del backend como los componentes del frontend fueron escritos manualmente.
- El backend solo usa C# (requisito de la prueba).
