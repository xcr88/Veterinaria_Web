using System;
using System.Data.SqlClient;
using System.Web;
using Veterinaria_Web.Modelos;
using Veterinaria_Web.Repositorios;
using Veterinaria_Web.Utilidades;

namespace Veterinaria_Web.Vistas
{
    public partial class Ingreso : System.Web.UI.Page
    {
        private bool TieneSesionActiva()
        {
            return Session["ID_USUARIO"] != null &&
                !string.IsNullOrWhiteSpace(
                    Convert.ToString(Session["Usuario"]));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(
                HttpCacheability.NoCache);

            Response.Cache.SetNoStore();

            Response.Cache.SetExpires(
                DateTime.UtcNow.AddMinutes(-1));

            // Si ya ingresó, abre el mantenimiento.
            if (TieneSesionActiva())
            {
                AbrirMantenimiento();
                return;
            }

            if (!IsPostBack &&
                Request.QueryString["aviso"] == "log")
            {
                MostrarMensaje(
                    "La sesión se cerró, pero no se pudo registrar " +
                    "la salida en la bitácora.",
                    false);
            }
        }

        protected void btnIngresar_Click(
            object sender,
            EventArgs e)
        {
            // También protege frente al envío de una pestaña
            // de ingreso que quedó abierta anteriormente.
            if (TieneSesionActiva())
            {
                AbrirMantenimiento();
                return;
            }

            lblMensaje.Text = "";
            lnkContinuar.Visible = false;

            // Solo descarta datos incompletos de una sesión inválida.
            Session.Remove("ID_USUARIO");
            Session.Remove("Usuario");

            string usuario = txtUsuario.Text.Trim();

            // La contraseña se valida exactamente como fue escrita.
            string clave = txtClave.Text;

            if (string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(clave))
            {
                MostrarMensaje(
                    "Escriba el usuario y la contraseña.",
                    false);

                return;
            }

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

                // No se guarda la contraseña en la sesión.
                Session["ID_USUARIO"] = cuenta.ID_USUARIO;
                Session["Usuario"] = cuenta.USUARIO;

                bool registrado = RegistrarEnBitacora(
                    cuenta.USUARIO,
                    "INGRESO_CORRECTO",
                    "ID de usuario: " + cuenta.ID_USUARIO);

                if (!registrado)
                {
                    MostrarMensaje(
                        "Ingreso correcto, pero no se pudo escribir " +
                        "en la bitácora. Puede continuar con el enlace.",
                        false);

                    lnkContinuar.Visible = true;
                    return;
                }

                AbrirMantenimiento();
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

        private void AbrirMantenimiento()
        {
            Response.Redirect(
                "~/Vistas/MantenimientoPropietarios.aspx",
                false);

            Context.ApplicationInstance.CompleteRequest();
        }

        private bool RegistrarEnBitacora(
            string usuario,
            string accion,
            string detalle)
        {
            try
            {
                EscribirLog.Registrar(
                    usuario,
                    accion,
                    detalle);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning(
                    "No se pudo escribir el LOG de ingreso. Tipo: " +
                    ex.GetType().Name);

                return false;
            }
        }

        private void MostrarMensaje(
            string mensaje,
            bool correcto)
        {
            lblMensaje.Text = Server.HtmlEncode(mensaje);

            lblMensaje.ForeColor = correcto
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Red;
        }
    }
}