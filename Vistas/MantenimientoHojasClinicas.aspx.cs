using System;
using System.Web;
using Veterinaria_Web.Repositorios;
using Veterinaria_Web.Utilidades;

namespace Veterinaria_Web.Vistas
{
    public partial class MantenimientoHojasClinicas
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

                CargarHojasClinicas();
            }
        }

        // =====================================================
        // CONSULTAR LAS HOJAS CLÍNICAS
        // =====================================================

        private void CargarHojasClinicas()
        {
            try
            {
                HojaClinicaRepositorio repositorio =
                    new HojaClinicaRepositorio();

                gvHojasClinicas.DataSource =
                    repositorio.ConsultarTodos();

                gvHojasClinicas.DataBind();

                string aviso = RegistrarEnBitacora(
                    "CONSULTAR_HOJAS_CLINICAS",
                    "Consulta del listado de atenciones.");

                if (!string.IsNullOrEmpty(aviso))
                {
                    MostrarMensaje(
                        "Listado de atenciones cargado." + aviso,
                        false);
                }
            }
            catch (Exception ex)
            {
                string aviso = RegistrarEnBitacora(
                    "ERROR_CONSULTAR_HOJAS_CLINICAS",
                    "Tipo de error: " + ex.GetType().Name);

                MostrarMensaje(
                    "No se pudieron consultar las hojas clínicas: " +
                    ex.Message + aviso,
                    false);
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