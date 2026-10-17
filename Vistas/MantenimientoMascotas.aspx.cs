using System;
using System.Web;
using Veterinaria_Web.Repositorios;
using Veterinaria_Web.Utilidades;

namespace Veterinaria_Web.Vistas
{
    public partial class MantenimientoMascotas
        : System.Web.UI.Page
    {
        private string UsuarioActual
        {
            get
            {
                return Convert.ToString(Session["Usuario"]);
            }
        }

        // =====================================================
        // COMPROBAR LA SESIÓN
        // =====================================================

        protected override void OnInit(EventArgs e)
        {
            if (Session["ID_USUARIO"] == null ||
                string.IsNullOrWhiteSpace(UsuarioActual))
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
                    Server.HtmlEncode(UsuarioActual);

                CargarMascotas();
            }
        }

        // =====================================================
        // CONSULTAR LAS MASCOTAS
        // =====================================================

        private void CargarMascotas()
        {
            try
            {
                MascotaRepositorio repositorio =
                    new MascotaRepositorio();

                gvMascotas.DataSource =
                    repositorio.ConsultarTodos();

                gvMascotas.DataBind();

                string aviso = RegistrarEnBitacora(
                    "CONSULTAR_MASCOTAS",
                    "Consulta del listado de mascotas.");

                lblMensaje.Text = Server.HtmlEncode(aviso);

                lblMensaje.ForeColor =
                    System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                string aviso = RegistrarEnBitacora(
                    "ERROR_CONSULTAR_MASCOTAS",
                    "Tipo de error: " + ex.GetType().Name);

                lblMensaje.Text = Server.HtmlEncode(
                    "No se pudieron consultar las mascotas: " +
                    ex.Message + aviso);

                lblMensaje.ForeColor =
                    System.Drawing.Color.Red;
            }
        }

        // =====================================================
        // REGISTRAR EN EL LOG
        // =====================================================

        private string RegistrarEnBitacora(
            string accion,
            string detalle)
        {
            try
            {
                EscribirLog.Registrar(
                    UsuarioActual,
                    accion,
                    detalle);

                return "";
            }
            catch (Exception)
            {
                return " Aviso: no se pudo escribir en la bitácora.";
            }
        }
    }
}