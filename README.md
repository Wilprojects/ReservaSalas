# API de Gestión de Reservas de Salas

API REST desarrollada con **ASP.NET Core y .NET 10** para gestionar reservas de salas de reuniones.

El proyecto fue desarrollado como parte del entregable **“Documentación de una API REST con OpenAPI”**, aplicando OpenAPI como contrato de la API, Swagger UI para exploración y pruebas, autenticación JWT Bearer y generación de un cliente .NET mediante NSwag.

---

## Tecnologías utilizadas

- .NET 10
- ASP.NET Core Minimal APIs
- OpenAPI
- Swagger UI
- JWT Bearer Authentication
- NSwag
- Newtonsoft.Json
- C#
- Git / GitHub

---

## Funcionalidades

La API permite:

- Obtener todas las reservas.
- Filtrar reservas por fecha.
- Obtener una reserva por identificador.
- Crear una nueva reserva.
- Actualizar una reserva existente.
- Eliminar una reserva.
- Validar los datos enviados.
- Proteger los endpoints mediante JWT Bearer.
- Explorar y probar la API desde Swagger UI.
- Generar un cliente .NET desde la especificación OpenAPI.

---

## Modelo de reserva

Cada reserva contiene la siguiente información:

- Id.
- Nombre de la sala.
- Fecha de reserva.
- Hora de inicio.
- Hora de fin.
- Nombre del responsable.
- Cantidad de asistentes.
- Motivo de la reunión.
- Estado.

Los estados disponibles son:

- `Pendiente`
- `Confirmada`
- `Cancelada`

---

## Endpoints

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/reservas` | Obtiene todas las reservas |
| GET | `/api/reservas?fecha={fecha}` | Filtra las reservas por fecha |
| GET | `/api/reservas/{id}` | Obtiene una reserva por Id |
| POST | `/api/reservas` | Crea una nueva reserva |
| PUT | `/api/reservas/{id}` | Actualiza una reserva existente |
| DELETE | `/api/reservas/{id}` | Elimina una reserva |

Los endpoints ubicados bajo `/api/reservas` requieren autenticación JWT Bearer.

El endpoint raíz `/` permanece público y puede utilizarse para comprobar que la API está ejecutándose.

---

## Códigos HTTP utilizados

| Código | Descripción |
|---|---|
| `200 OK` | Operación realizada correctamente |
| `201 Created` | Reserva creada correctamente |
| `204 No Content` | Reserva eliminada correctamente |
| `400 Bad Request` | Error de validación o datos inválidos |
| `401 Unauthorized` | Token JWT inexistente o inválido |
| `404 Not Found` | Reserva no encontrada |

---

## Validaciones

Entre las reglas de validación implementadas se encuentran:

- El nombre de la sala es obligatorio.
- El nombre del responsable es obligatorio.
- El motivo de la reunión es obligatorio.
- La cantidad de asistentes debe ser mayor a cero.
- La hora de fin debe ser posterior a la hora de inicio.
- Los estados permitidos son `Pendiente`, `Confirmada` y `Cancelada`.

La validación se realiza mediante **Data Annotations** e `IValidatableObject`.

---

## Persistencia

Para este entregable no se utiliza una base de datos.

Las reservas se almacenan en memoria mediante un repositorio registrado como **Singleton**.

Por este motivo, los cambios realizados durante la ejecución se pierden cuando la aplicación se reinicia.

---

## Requisitos

Para ejecutar el proyecto se requiere:

- .NET SDK 10.
- Git, únicamente si se desea clonar el repositorio.

Para comprobar la versión instalada:

```bash
dotnet --version
```

---

## Restaurar dependencias

Desde la raíz del proyecto:

```bash
dotnet restore
```

También se deben restaurar las herramientas locales del repositorio:

```bash
dotnet tool restore
```

---

## Compilar la solución

Ejecutar:

```bash
dotnet build
```

La solución contiene los proyectos:

```text
ReservaSalas.Api
ReservaSalas.Client
```

---

## Ejecutar la API

Desde la raíz del repositorio:

```bash
dotnet run --project src/ReservaSalas.Api
```

La terminal mostrará las URLs disponibles, por ejemplo:

```text
http://localhost:PUERTO
https://localhost:PUERTO
```

Los números de puerto pueden variar según la configuración local.

---

## Swagger UI

Con la aplicación ejecutándose, acceder a:

```text
http://localhost:PUERTO/swagger
```

o a la URL HTTPS correspondiente.

Swagger UI permite visualizar:

- Endpoints.
- Parámetros Path.
- Parámetros Query.
- Request Bodies.
- Response Models.
- Schemas.
- Códigos HTTP.
- Autenticación Bearer.
- Ejecución de solicitudes mediante `Try it out`.

---

## OpenAPI

La especificación OpenAPI se encuentra disponible durante la ejecución en:

```text
/openapi/v1.json
```

También se encuentra disponible en YAML:

```text
/openapi/v1.yaml
```

Dentro del repositorio se incluyen:

```text
docs/openapi.json
docs/openapi.yaml
```

Además se conserva:

```text
docs/openapi-contract.yaml
```

Este último corresponde al contrato diseñado inicialmente antes de completar la implementación.

---

## Autenticación JWT

Los endpoints `/api/reservas` están protegidos mediante JWT Bearer.

Para crear un token de desarrollo ejecutar:

```bash
dotnet user-jwts create --project src/ReservaSalas.Api/ReservaSalas.Api.csproj --name "SwaggerUser"
```

El comando devolverá un token similar a:

```text
eyJ...
```

El token generado es únicamente para desarrollo y pruebas locales.

**No debe almacenarse en el repositorio.**

---

## Utilizar JWT desde Swagger

1. Ejecutar la API.
2. Abrir `/swagger`.
3. Presionar `Authorize`.
4. Pegar únicamente el JWT generado.
5. Presionar `Authorize`.
6. Ejecutar un endpoint de `/api/reservas`.

Swagger agregará automáticamente el encabezado:

```http
Authorization: Bearer <token>
```

Sin un token válido los endpoints protegidos responden:

```text
401 Unauthorized
```

---

## Seguridad en OpenAPI

La especificación OpenAPI documenta el esquema:

```yaml
securitySchemes:
  Bearer:
    type: http
    scheme: bearer
    bearerFormat: JWT
```

Los endpoints protegidos también contienen el requisito de seguridad Bearer correspondiente.

---

## Cliente .NET generado con NSwag

El proyecto incluye un cliente .NET generado automáticamente desde:

```text
docs/openapi.json
```

El cliente se encuentra en:

```text
src/ReservaSalas.Client/Generated/ReservasApiClient.cs
```

NSwag se encuentra registrado como herramienta local del repositorio.

Para restaurarlo:

```bash
dotnet tool restore
```

---

## Regenerar el cliente NSwag

Desde la raíz del repositorio ejecutar:

```bash
dotnet tool run nswag openapi2csclient /input:docs/openapi.json /classname:ReservasApiClient /namespace:ReservaSalas.Client.Generated /output:src/ReservaSalas.Client/Generated/ReservasApiClient.cs /OperationGenerationMode:SingleClientFromOperationId /GenerateClientInterfaces:true /InjectHttpClient:true
```

El archivo generado no debe modificarse manualmente.

Si cambia el contrato de la API, se debe:

1. Actualizar la implementación.
2. Regenerar `docs/openapi.json`.
3. Ejecutar nuevamente NSwag.

---

## Métodos generados

A partir de los `operationId` definidos en OpenAPI, NSwag genera métodos asociados a las operaciones de la API.

Entre ellos:

```text
GetReservasAsync(...)
GetReservaByIdAsync(...)
CreateReservaAsync(...)
UpdateReservaAsync(...)
DeleteReservaAsync(...)
```

Esto demuestra la relación:

```text
Minimal API
    ↓
OpenAPI operationId
    ↓
NSwag
    ↓
Cliente C#
```

---

## Estructura del proyecto

```text
ReservaSalas
│
├── .config
│   └── dotnet-tools.json
│
├── docs
│   ├── evidencias
│   ├── openapi-contract.yaml
│   ├── openapi.json
│   └── openapi.yaml
│
├── src
│   │
│   ├── ReservaSalas.Api
│   │   ├── Dtos
│   │   ├── Endpoints
│   │   ├── Mappings
│   │   ├── Models
│   │   ├── Repositories
│   │   ├── Program.cs
│   │   └── ReservaSalas.Api.csproj
│   │
│   └── ReservaSalas.Client
│       ├── Generated
│       │   └── ReservasApiClient.cs
│       └── ReservaSalas.Client.csproj
│
├── .gitignore
├── global.json
├── README.md
└── ReservaSalas.sln
```

---

## Arquitectura simplificada

```text
Cliente HTTP / Swagger
          |
          v
     Minimal API
          |
          v
        DTOs
          |
          v
      Validación
          |
          v
       Mapping
          |
          v
 IReservaRepository
          |
          v
 ReservaRepository
          |
          v
    List<Reserva>
```

---

## Flujo OpenAPI

```text
Código C#
    |
    v
ASP.NET Core OpenAPI
    |
    v
/openapi/v1.json
    |
    +----------> Swagger UI
    |
    +----------> docs/openapi.json
                       |
                       v
                     NSwag
                       |
                       v
              ReservasApiClient.cs
```

---

## Buenas prácticas aplicadas

- Separación entre modelos internos y DTOs.
- Validaciones centralizadas.
- Uso correcto de códigos HTTP.
- `operationId` únicos.
- XML Comments.
- Documentación de parámetros Path y Query.
- Documentación de Request y Response.
- Autenticación JWT Bearer.
- No almacenar tokens ni secretos en el repositorio.
- OpenAPI generado desde la implementación.
- Cliente generado automáticamente desde el contrato OpenAPI.
- Código generado separado dentro de la carpeta `Generated`.

---

## Evidencias

Las capturas de funcionamiento de la API se encuentran en:

```text
docs/evidencias/
```

Se incluyen evidencias de:

- Swagger UI con los endpoints documentados.
- Autenticación JWT Bearer.
- Botón `Authorize`.
- Respuesta `401 Unauthorized`.
- Respuesta `200 OK`.
- Creación de recursos con `201 Created`.
- Validaciones con `400 Bad Request`.
- Recursos inexistentes con `404 Not Found`.
- Security Schemes en OpenAPI.
- Schemas de Request y Response.
- Cliente .NET generado mediante NSwag.
- Compilación correcta de la solución.

---

## Consideraciones de seguridad

- No almacenar JWT reales en el repositorio.
- No almacenar claves de firma en el código fuente.
- No incluir contraseñas ni secretos en OpenAPI o en las evidencias.
- Los JWT generados con `dotnet user-jwts` son únicamente para desarrollo local.
