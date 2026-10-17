using System;
using System.Web;
using Veterinaria_Web.Utilidades;

namespace Veterinaria_Web.Vistas
{
    public partial class CerrarSesion : System.Web.UI.Page
    {
        // Comprueba la sesión antes de procesar la página.
        protected override void OnInit(EventArgs e)
        {
            string usuario =
                Convert.ToString(Session["Usuario"]);

            if (Session["ID_USUARIO"] == null ||
                string.IsNullOrWhiteSpace(usuario))
            {
                Response.Redirect("~/Vistas/Ingreso.aspx", true);
                return;
            }

            ViewStateUserKey = Session.SessionID;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(
                HttpCacheability.NoCache);

            Response.Cache.SetNoStore();

            Response.Cache.SetExpires(
                DateTime.UtcNow.AddMinutes(-1));

            if (!IsPostBack)
            {
                lblUsuario.Text =
                    "Usuario conectado: " +
                    Server.HtmlEncode(
                        Convert.ToString(Session["Usuario"]));
            }
        }

        // Solo cierra la sesión al confirmar con el botón.
        protected void btnCerrarSesion_Click(
            object sender, EventArgs e)
        {
            string usuario =
                Convert.ToString(Session["Usuario"]);

            bool logCorrecto = true;

            try
            {
                // Registra al responsable antes de limpiar la sesión.
                EscribirLog.Registrar(
                    usuario,
                    "CIERRE_SESION",
                    "El usuario cerró su sesión.");
            }
            catch (Exception ex)
            {
                logCorrecto = false;

                System.Diagnostics.Trace.TraceWarning(
                    "No se pudo registrar el cierre de sesión. Tipo: " +
                    ex.GetType().Name);
            }
            finally
            {
                // La salida se realiza aunque falle el archivo LOG.
                Session.Clear();
                Session.Abandon();
            }

            string destino = logCorrecto
                ? "~/Vistas/Ingreso.aspx"
                : "~/Vistas/Ingreso.aspx?aviso=log";

            Response.Redirect(destino, false);

            Context.ApplicationInstance.CompleteRequest();
            return;
        }
    }
}