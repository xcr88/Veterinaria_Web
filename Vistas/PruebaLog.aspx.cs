using System;
using Veterinaria_Web.Utilidades;

namespace Veterinaria_Web.Vistas
{
    public partial class FormularioWeb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Se ejecuta al presionar el botón de prueba.
        protected void btnProbarLog_Click(
            object sender, EventArgs e)
        {
            try
            {
                EscribirLog.Registrar(
                    "Admin",
                    "PRUEBA",
                    "Comprobación de escritura del LOG");

                lblResultado.Text =
                    "Registro escrito correctamente en " +
                    "App_Data/bitacora.log.";

                lblResultado.ForeColor =
                    System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblResultado.Text =
                    "No se pudo escribir el LOG: " +
                    Server.HtmlEncode(ex.Message);

                lblResultado.ForeColor =
                    System.Drawing.Color.Red;
            }
        }
    }
}