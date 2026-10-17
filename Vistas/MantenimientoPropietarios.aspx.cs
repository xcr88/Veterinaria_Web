using System;
using Veterinaria_Web.Repositorios;

namespace Veterinaria_Web.Vistas
{
    public partial class MantenimientoPropietarios
        : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Consulta los datos solamente al abrir la página.
            if (!IsPostBack)
            {
                CargarPropietarios();
            }
        }

        // Consulta la base y muestra los propietarios en la tabla.
        private void CargarPropietarios()
        {
            try
            {
                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                gvPropietarios.DataSource =
                    repositorio.ConsultarTodos();

                gvPropietarios.DataBind();

                lblMensaje.Text = "";
            }
            catch (Exception ex)
            {
                lblMensaje.Text =
                    "No se pudieron consultar los propietarios: " +
                    Server.HtmlEncode(ex.Message);

                lblMensaje.ForeColor =
                    System.Drawing.Color.Red;
            }
        }
    }
}