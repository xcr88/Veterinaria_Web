using System;
using Veterinaria_Web.Repositorios;

namespace Veterinaria_Web.Vistas
{
    public partial class PruebaConexion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Se ejecuta al presionar "Probar conexión".
        protected void btnProbarConexion_Click(
            object sender, EventArgs e)
        {
            try
            {
                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                if (repositorio.ProbarConexion())
                {
                    lblResultado.Text =
                        "Conexión correcta con la base de datos.";

                    lblResultado.ForeColor =
                        System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                lblResultado.Text =
                    "No se pudo conectar: " +
                    Server.HtmlEncode(ex.Message);

                lblResultado.ForeColor =
                    System.Drawing.Color.Red;
            }
        }
    }
}