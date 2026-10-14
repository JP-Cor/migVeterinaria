# MigVeterinaria 🐾

**Sistema web de gestión veterinaria** desarrollado con ASP.NET Web Forms y SQL Server.

El proyecto permite administrar información de propietarios, mascotas y hojas clínicas, además de gestionar cuentas de usuario con autenticación y roles.

> **Estado:** proyecto académico en desarrollo. Los módulos principales y el inicio de sesión ya se han probado localmente; todavía quedan mejoras de seguridad y pruebas antes de su publicación.

## Tecnologías utilizadas

- **Lenguaje:** C#
- **Framework:** ASP.NET Web Forms sobre .NET Framework 4.7.2
- **Base de datos:** Microsoft SQL Server
- **Acceso a datos:** ADO.NET (`System.Data.SqlClient`)
- **Interfaz:** HTML, CSS y Bootstrap (plantilla Web Forms)
- **IDE:** Visual Studio
- **Autenticación:** Forms Authentication
- **Contraseñas:** PBKDF2-HMAC-SHA256 con sal aleatoria

## Funcionalidades implementadas

| Módulo | Funcionalidades | Estado |
| --- | --- | --- |
| Propietarios | Registro, consulta, búsqueda, edición y eliminación según el módulo desarrollado | Funcional en pruebas locales |
| Mascotas | Gestión de registros de mascotas | Funcional en pruebas locales |
| Hojas clínicas | Gestión de hojas clínicas | Funcional en pruebas locales |
| Inicio de sesión | Validación de credenciales contra SQL Server | Funcional |
| Cierre de sesión | Invalidación de la autenticación y limpieza de sesión | Funcional |
| Usuarios | Listado, creación, edición de datos y desactivación | Implementado; pruebas adicionales pendientes |
| Roles | `ADMIN` y `USUARIO` | Comprobado en el acceso a `Usuarios.aspx` |

### Control de acceso

- Las páginas `Propietarios.aspx`, `Mascotas.aspx`, `HojasClinicas.aspx` y `Usuarios.aspx` requieren autenticación mediante reglas en `Web.config`.
- La página `Usuarios.aspx` ejecuta una validación adicional del rol `ADMIN` consultando la base de datos.
- Se comprobó que un usuario con rol `USUARIO` no puede acceder a la administración de usuarios (respuesta HTTP 403).
- **Pendiente:** validar el estado activo de cada usuario en todas las solicitudes protegidas, para impedir que una sesión ya iniciada siga funcionando después de desactivar la cuenta.

## Estructura general del proyecto

```text
MigVerterinaria/
├── MigVerterinaria.slnx
└── WebSite1/
    ├── App_Code/
    │   ├── ConexionBD.cs
    │   ├── Propietario.cs
    │   ├── PropietarioRepositorio.cs
    │   ├── Mascota.cs
    │   ├── MascotaRepositorio.cs
    │   ├── HojaClinica.cs
    │   ├── HojaClinicaRepositorio.cs
    │   ├── Usuario.cs
    │   ├── UsuarioRepositorio.cs
    │   ├── SeguridadClave.cs
    │   └── SeguridadAcceso.cs
    ├── Default.aspx
    ├── Login.aspx
    ├── Login.aspx.cs
    ├── CerrarSesion.aspx
    ├── CerrarSesion.aspx.cs
    ├── Propietarios.aspx
    ├── Mascotas.aspx
    ├── HojasClinicas.aspx
    ├── Usuarios.aspx
    ├── Usuarios.aspx.cs
    ├── Site.master
    ├── Site.master.cs
    └── Web.config              # Configuración local: NO subir credenciales
```

> La estructura muestra los archivos principales. El proyecto también contiene archivos de estilos, scripts y componentes de la plantilla ASP.NET.

## Base de datos

**Nombre usado en el entorno de desarrollo:** `VETERINARIA_WEB_DB`.

El sistema utiliza procedimientos almacenados para las operaciones de acceso a datos.

### Tabla de usuarios

La tabla `VT_USUARIOS` incluye, entre otros, los siguientes campos:

| Campo | Descripción |
| --- | --- |
| `USU_ID` | Identificador del usuario |
| `USU_USUARIO` | Nombre de usuario |
| `USU_CLAVE_HASH` | Hash de la contraseña |
| `USU_ESTADO` | `A` (activo) o `I` (inactivo) |
| `USU_ROL` | `ADMIN` o `USUARIO` |
| `USU_ADICIONADO_POR` | Usuario que registró el dato |
| `USU_FECHA_ADICION` | Fecha de creación |
| `USU_MODIFICADO_POR` | Usuario que modificó el registro |
| `USU_FECHA_MODIFICACION` | Fecha de modificación |

### Procedimientos almacenados del módulo Usuarios

- `sp_CrearUsuario`
- `sp_ListarUsuarios`
- `sp_BuscarUsuarioPorId`
- `sp_ActualizarUsuario`
- `sp_DesactivarUsuario`
- `sp_ObtenerUsuarioParaLogin`

**Importante:** para ejecutar el proyecto en otra computadora se necesita crear la base de datos y sus objetos. Los scripts de creación de tablas y procedimientos deben incorporarse al repositorio si aún no están versionados.

## Requisitos para ejecutar el proyecto

1. Windows con Visual Studio compatible con proyectos ASP.NET Web Forms de .NET Framework.
2. .NET Framework 4.7.2 y las herramientas de desarrollo web correspondientes.
3. Microsoft SQL Server y una instancia accesible desde la aplicación.
4. Base de datos `VETERINARIA_WEB_DB` con sus tablas y procedimientos almacenados.
5. Permisos de conexión y ejecución de procedimientos almacenados para la identidad utilizada por la aplicación.

## Instalación local

### 1. Clonar el repositorio

```bash
git clone https://github.com/JP-Cor/migVeterinaria.git
```

### 2. Abrir el proyecto

Abrir la solución `MigVerterinaria.slnx` en Visual Studio, o abrir la carpeta del sitio web mediante la opción correspondiente de Visual Studio.

### 3. Preparar SQL Server

Crear la base de datos `VETERINARIA_WEB_DB` y ejecutar los scripts SQL del proyecto. Verificar que estén creadas las tablas y los procedimientos almacenados requeridos.

### 4. Configurar la conexión local

Crear o configurar el archivo `Web.config` localmente, con una entrada `Conexion` dentro de `<connectionStrings>`:

```xml
<connectionStrings>
  <add name="Conexion"
       connectionString="Data Source=SERVIDOR\INSTANCIA;Initial Catalog=VETERINARIA_WEB_DB;Integrated Security=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Sustituir `SERVIDOR\INSTANCIA` por la instancia SQL Server del equipo. Conservar también las demás secciones de configuración ASP.NET requeridas por el proyecto.

**No publicar el `Web.config` real con información privada.** En el entorno de desarrollo actual se mantiene fuera del control de versiones.

### 5. Crear la cuenta administradora inicial

La primera cuenta `ADMIN` debe aprovisionarse mediante un procedimiento local y controlado que utilice `SeguridadClave.GenerarHash()`. No guardar contraseñas en texto plano ni publicar páginas temporales de instalación.

### 6. Ejecutar

Iniciar el proyecto desde Visual Studio y abrir `Login.aspx`. Después de autenticarse, el sistema redirige a `Default.aspx`.

## Seguridad

- Las contraseñas nuevas se almacenan como hashes PBKDF2-HMAC-SHA256 con sal aleatoria.
- El acceso al módulo de usuarios requiere autenticación y rol `ADMIN`.
- La autenticación utiliza Forms Authentication; la autenticación por cookies OWIN de la plantilla original fue deshabilitada.
- Las operaciones de base de datos usan consultas parametrizadas o procedimientos almacenados.
- La aplicación todavía **no está lista para exponerse a Internet**: faltan pruebas de autorización, control de sesiones de usuarios desactivados, revisión de privilegios SQL Server, endurecimiento de configuración y despliegue con HTTPS.

## Próximos pasos

- [ ] Probar el comportamiento de una sesión existente después de desactivar el usuario `prueba01`.
- [ ] Validar globalmente que el usuario autenticado siga activo antes de acceder a módulos protegidos.
- [ ] Mostrar el enlace **Administración de Usuarios** únicamente a administradores.
- [ ] Mejorar la presentación de los errores de acceso denegado (HTTP 403).
- [ ] Reforzar las operaciones administrativas, la integridad de los datos y los permisos de SQL Server.
- [ ] Completar las pruebas funcionales y de seguridad del módulo Usuarios.
- [ ] Incorporar y documentar los scripts SQL necesarios para instalar la base de datos desde cero.
- [ ] Preparar la configuración de producción con HTTPS y secretos fuera del repositorio.

## Repositorio

**GitHub:** [JP-Cor/migVeterinaria](https://github.com/JP-Cor/migVeterinaria)

## Estado del desarrollo

Documentación actualizada hasta el **Paso 135** del desarrollo guiado. La prueba de desactivación de `prueba01` con una sesión ya iniciada está **pendiente**, no completada.
