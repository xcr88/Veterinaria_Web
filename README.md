# Veterinaria_Web

Aplicación académica desarrollada con ASP.NET Web Forms y SQL Server para administrar propietarios, mascotas y hojas clínicas de una veterinaria.

## Tecnologías

- C#.
- ASP.NET Web Forms.
- .NET Framework 4.8.1.
- SQL Server.
- ADO.NET con procedimientos almacenados.
- HTML y CSS.

La presentación utiliza la paleta de colores de los proyectos de referencia BibliotecaWEB y WebAgenda.

## Funcionalidades

- Ingreso con usuario y contraseña.
- Validación de cuentas activas.
- Cierre de sesión.
- Sesión configurada para vencer después de cinco minutos de inactividad.
- Registro, consulta, actualización y eliminación de propietarios.
- Registro, consulta, actualización y eliminación de mascotas.
- Registro, consulta, actualización y eliminación de atenciones clínicas.
- Validaciones de campos obligatorios, longitudes, teléfono, correo, fechas y peso.
- Auditoría de creación y modificación.
- Bitácora de operaciones y accesos.

Las relaciones de la base de datos impiden eliminar propietarios con mascotas asociadas y mascotas con hojas clínicas registradas.

## Requisitos

- Windows.
- Visual Studio con la carga de trabajo Desarrollo de ASP.NET y web.
- .NET Framework 4.8.1 y su paquete de desarrollo.
- SQL Server o SQL Server Express.
- SQL Server Management Studio para ejecutar el script de la base de datos.

## Estructura

- `BD`: script de creación de la base de datos.
- `Modelos`: clases que representan los datos.
- `Repositorios`: acceso a SQL Server mediante procedimientos almacenados.
- `Vistas`: páginas Web Forms y sus eventos.
- `Utilidades`: escritura de la bitácora.
- `Estilos`: hoja de estilos compartida.
- `App_Data`: archivos generados durante la ejecución.
- `Web.config`: conexión y configuración de la aplicación.

## Instalación

### 1. Obtener el proyecto

Clonar o descargar el repositorio y abrir la solución en Visual Studio.

Restaurar los paquetes NuGet si Visual Studio lo solicita.

### 2. Crear la base de datos

Abrir en SQL Server Management Studio el archivo:

`BD/BD_MANT_VETERINARIA.sql`

Ejecutarlo en la instancia de SQL Server que utilizará la aplicación.

El script crea la base de datos `VETERINARIA_WEB`, sus cuatro tablas y sus 17 procedimientos almacenados.

Las tablas principales son:

- `PROPIETARIOS`
- `MASCOTAS`
- `HOJAS_CLINICAS`
- `USUARIOS`

El script de estructura no incluye los datos de prueba ni las cuentas creadas durante el desarrollo.

### 3. Crear un usuario de la aplicación

Después de crear la base de datos, ejecutar este ejemplo en SQL Server Management Studio:

```sql
USE VETERINARIA_WEB;
GO

EXEC sp_CrearUsuario
    @USUARIO = N'usuario_prueba',
    @CLAVE = N'Prueba123*',
    @ADICIONADO_POR = N'Configuracion';
GO
```

Ejecutar la creación una sola vez por nombre de usuario.

La contraseña admite hasta 10 caracteres. La cuenta debe estar activa para permitir el ingreso.

Para este ejercicio académico, las contraseñas de la aplicación se almacenan según el esquema utilizado en clase.

### 4. Configurar la conexión

En `Web.config`, ajustar la conexión llamada `Conexion` con los datos de la instancia local.

Ejemplo de autenticación de SQL Server:

```xml
<connectionStrings>
  <clear />
  <add name="Conexion"
       connectionString="Data Source=SERVIDOR\INSTANCIA;Initial Catalog=VETERINARIA_WEB;Integrated Security=False;Connect Timeout=30;Application Name=Veterinaria_Web;User ID=sa;Password=CONTRASEÑA_SQL;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Sustituir `SERVIDOR\INSTANCIA` y `CONTRASEÑA_SQL` por los valores correspondientes.

La cuenta de SQL Server utilizada para la conexión es diferente del usuario que ingresa al sistema desde el navegador.

### 5. Ejecutar

1. Compilar la solución.
2. En la carpeta `Vistas`, establecer `Ingreso.aspx` como página de inicio.
3. Ejecutar con IIS Express.
4. Ingresar con el usuario creado en la base de datos.

Si ya existe una sesión activa, la página de ingreso redirige al mantenimiento de propietarios.

## Uso

Se recomienda registrar los datos en este orden:

1. Propietario.
2. Mascota asociada al propietario.
3. Atención clínica asociada a la mascota.

En cada mantenimiento:

- Guardar registra un nuevo elemento.
- Seleccionar carga un registro en el formulario.
- Actualizar guarda las modificaciones del registro seleccionado.
- Eliminar solicita confirmación antes de borrar.
- Nuevo limpia el formulario y permite registrar otro elemento.

Los enlaces del encabezado permiten cambiar entre los tres mantenimientos y cerrar sesión.

## Auditoría y bitácora

Los registros incluyen información del usuario y la fecha de creación y modificación.

La bitácora se genera automáticamente en:

`App_Data/bitacora.log`

Registra fecha, hora, usuario, acción y detalle de la operación. Incluye accesos correctos, accesos rechazados, cierre de sesión y operaciones de los mantenimientos.

La aplicación necesita permiso de escritura en `App_Data`.

Los archivos `.log` se excluyen del repositorio mediante `.gitignore`.

## Sesión

La sesión utiliza el modo `InProc` y está configurada con cinco minutos de inactividad.

Al vencer, el siguiente acceso a un mantenimiento redirige al ingreso. Una página que ya está abierta no cambia automáticamente al cumplirse el tiempo.

Reiniciar la aplicación o modificar su configuración puede eliminar la sesión y requerir un nuevo ingreso.

## Comprobaciones realizadas

- Ingreso correcto y rechazo de credenciales incorrectas.
- Cierre de sesión y protección de los mantenimientos.
- Creación, consulta, actualización y eliminación en los tres módulos.
- Registro de operaciones en la bitácora.
- Auditoría con el usuario conectado.
- Bloqueo de eliminación de propietarios con mascotas.
- Bloqueo de eliminación de mascotas con hojas clínicas.
- Navegación entre módulos.
