using System;
using System.Data.SqlClient;
using Veterinaria_Web.Modelos;
using Veterinaria_Web.Repositorios;
using Veterinaria_Web.Utilidades;

namespace Veterinaria_Web.Vistas
{
    public partial class Ingreso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // =====================================================
        // VALIDAR EL INGRESO
        // =====================================================

        protected void btnIngresar_Click(
            object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            lnkContinuar.Visible = false;

            // Cada intento comienza sin una cuenta seleccionada.
            Session.Remove("ID_USUARIO");
            Session.Remove("Usuario");

            string usuario = txtUsuario.Text.Trim();

            // No quitamos espacios ni cambiamos la contraseña.
            string clave = txtClave.Text;

            if (string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(clave))
            {
                MostrarMensaje(
                    "Escriba el usuario y la contraseña.",
                    false);

                return;
            }

            // Valida también en el servidor para evitar
            // enviar valores mayores que los parámetros SQL.
            if (usuario.Length > 50 || clave.Length > 10)
            {
                MostrarMensaje(
                    "El usuario admite hasta 50 caracteres " +
                    "y la contraseña hasta 10.",
                    false);

                return;
            }

            try
            {
                UsuarioRepositorio repositorio =
                    new UsuarioRepositorio();

                Usuario cuenta =
                    repositorio.ValidarIngreso(usuario, clave);

                if (cuenta == null)
                {
                    bool logCorrecto = RegistrarEnBitacora(
                        usuario,
                        "INGRESO_RECHAZADO",
                        "No se encontró una cuenta activa " +
                        "con las credenciales proporcionadas.");

                    MostrarMensaje(
                        "Usuario o contraseña incorrectos." +
                        (logCorrecto
                            ? ""
                            : " Aviso: no se pudo escribir en la bitácora."),
                        false);

                    return;
                }

                // Conserva únicamente los datos necesarios.
                // La contraseña no se guarda en la sesión.
                Session["ID_USUARIO"] = cuenta.ID_USUARIO;
                Session["Usuario"] = cuenta.USUARIO;

                bool registrado = RegistrarEnBitacora(
                    cuenta.USUARIO,
                    "INGRESO_CORRECTO",
                    "ID de usuario: " + cuenta.ID_USUARIO);

                if (!registrado)
                {
                    // El acceso fue válido aunque fallara el archivo.
                    MostrarMensaje(
                        "Ingreso correcto, pero no se pudo escribir " +
                        "en la bitácora. Puede continuar con el enlace.",
                        false);

                    lnkContinuar.Visible = true;
                    return;
                }

                // Abre el mantenimiento después del ingreso.
                Response.Redirect(
                    "~/Vistas/MantenimientoPropietarios.aspx",
                    false);

                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            catch (SqlException ex)
            {
                bool logCorrecto = RegistrarEnBitacora(
                    usuario,
                    "ERROR_INGRESO",
                    "Código SQL: " + ex.Number);

                MostrarMensaje(
                    "No se pudo consultar la base de datos. " +
                    "Revise la conexión." +
                    (logCorrecto
                        ? ""
                        : " Tampoco se pudo escribir en la bitácora."),
                    false);
            }
            catch (Exception ex)
            {
                bool logCorrecto = RegistrarEnBitacora(
                    usuario,
                    "ERROR_INGRESO",
                    "Tipo de error: " + ex.GetType().Name);

                MostrarMensaje(
                    "No se pudo completar el ingreso." +
                    (logCorrecto
                        ? ""
                        : " Tampoco se pudo escribir en la bitácora."),
                    false);
            }
        }

        // =====================================================
        // REGISTRAR ACCESOS Y ERRORES
        // =====================================================

        private bool RegistrarEnBitacora(
            string usuario,
            string accion,
            string detalle)
        {
            try
            {
                EscribirLog.Registrar(usuario, accion, detalle);
                return true;
            }
            catch (Exception ex)
            {
                // No vuelve a intentar escribir en el mismo archivo.
                System.Diagnostics.Trace.TraceWarning(
                    "No se pudo escribir el LOG de ingreso. Tipo: " +
                    ex.GetType().Name);

                return false;
            }
        }

        // =====================================================
        // MOSTRAR MENSAJES
        // =====================================================

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = Server.HtmlEncode(mensaje);

            lblMensaje.ForeColor = correcto
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Red;
        }
    }
}