# API.Samuel – Estudio Fotográfico Enfoque

API RESTful en C# (.NET 10) conectada a SQL Server para administrar los **clientes de un estudio fotográfico**. Incluye registro, consulta, actualización y eliminación de clientes, e inicio de sesión validando las credenciales guardadas en la base de datos.

- **URL pública:** https://apisamuel.somee.com
- **Repositorio:** `https://github.com/TU-USUARIO/API.Samuel` – rama `main` *(reemplazar)*
- **Autor:** Samuel

## Arquitectura

| Capa | Proyecto | Responsabilidad |
|---|---|---|
| Presentación | `API.Samuel` | Controladores (endpoints) y configuración |
| Lógica de negocio | `API.Samuel.LOGICA.NEGOCIO` | Validaciones de los datos |
| Acceso a datos | `API.Samuel.DAPPER` | Conexión a SQL Server y procedimientos almacenados |
| Entidades | `API.Samuel.ENTIDADES` | `ClienteDto`, `InicioSesionDto` y `Respuesta<T>` |

Todas las respuestas tienen el mismo formato:

```json
{ "ok": true, "mensaje": "Texto descriptivo", "valorRetorno": { } }
```

## Endpoints

Las peticiones con cuerpo usan `Content-Type: application/json`.

| # | Funcionalidad | Método | Ruta |
|---|---|---|---|
| 1 | Agregar un cliente | `POST` | `/api/Clientes` |
| 2 | Actualizar un cliente | `PUT` | `/api/Clientes/{id}` |
| 3 | Eliminar un cliente por id | `DELETE` | `/api/Clientes/{id}` |
| 4 | Consultar todos los clientes | `GET` | `/api/Clientes` |
| 5 | Consultar un cliente por id | `GET` | `/api/Clientes/{id}` |
| 6 | Iniciar sesión | `POST` | `/api/InicioSesion` |

### 1. Agregar un cliente – `POST /api/Clientes`

```json
{
  "correo": "laura.brenes@ejemplo.com",
  "contrasena": "Enfoque2026*",
  "nombre": "Laura",
  "apellido1": "Brenes",
  "apellido2": "Zúñiga",
  "telefono": "+506 8890-3321",
  "provincia": "Guanacaste",
  "idTipoSesion": 1
}
```

Responde con el cliente creado y su `idCliente`. Si el correo ya existe, responde `"ok": false`.

### 2. Actualizar un cliente – `PUT /api/Clientes/{id}`

Ejemplo: `PUT /api/Clientes/2`

```json
{
  "correo": "diego.solano@ejemplo.com",
  "contrasena": "Enfoque2026*",
  "nombre": "Diego",
  "apellido1": "Solano",
  "apellido2": "Araya",
  "telefono": "+506 6034-9999",
  "provincia": "San José",
  "idTipoSesion": 6
}
```

### 3. Eliminar un cliente – `DELETE /api/Clientes/{id}`

Ejemplo: `DELETE /api/Clientes/6`. No lleva cuerpo.

### 4. Consultar todos los clientes – `GET /api/Clientes`

No lleva cuerpo.

### 5. Consultar un cliente por id – `GET /api/Clientes/{id}`

Ejemplo: `GET /api/Clientes/1`. No lleva cuerpo.

### 6. Iniciar sesión – `POST /api/InicioSesion`

```json
{
  "correo": "valeria.mora@ejemplo.com",
  "contrasena": "Enfoque2026*"
}
```

Con credenciales correctas responde `"ok": true` con el `idCliente` y el nombre completo; si no, `"ok": false` y *"Correo o contraseña incorrectos"*.

## Tipos de sesión (`idTipoSesion`)

| Id | Tipo |
|---|---|
| 1 | Boda |
| 2 | Quince años |
| 3 | Retrato profesional |
| 4 | Familiar |
| 5 | Producto / comercial |
| 6 | Graduación |

## Otros

- `GET /` – confirma que la API está en línea y lista los endpoints.
- `GET /openapi/v1.json` – documentación OpenAPI.

## Base de datos

- `EstudioEnfoqueDB.sql` – crea la base completa en SQL Server local.
- `EstudioEnfoqueDB_Somee.sql` – crea tablas, procedimientos y datos en la base de Somee.

Los clientes de prueba son ficticios; todos usan la contraseña `Enfoque2026*`.
