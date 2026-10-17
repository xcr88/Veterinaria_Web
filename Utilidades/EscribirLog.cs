using System;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace Veterinaria_Web.Utilidades
{
    public static class EscribirLog
    {
        // Evita que dos solicitudes escriban al mismo tiempo
        // dentro del mismo proceso de la aplicación.
        private static readonly object bloqueo = new object();

        public static void Registrar(
            string usuario,
            string accion,
            string detalle)
        {
            // Obtiene la ruta física de App_Data.
            string carpeta = HostingEnvironment.MapPath("~/App_Data");

            if (string.IsNullOrEmpty(carpeta))
            {
                throw new InvalidOperationException(
                    "No se pudo localizar la carpeta App_Data.");
            }

            string archivo = Path.Combine(carpeta, "bitacora.log");

            // Mantiene cada registro en una sola línea.
            string linea =
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") +
                " | Usuario: " + LimpiarTexto(usuario) +
                " | Acción: " + LimpiarTexto(accion) +
                " | Detalle: " + LimpiarTexto(detalle);

            lock (bloqueo)
            {
                // Crea la carpeta si todavía no existe.
                Directory.CreateDirectory(carpeta);

                // Agrega la línea sin borrar los registros anteriores.
                File.AppendAllText(
                    archivo,
                    linea + Environment.NewLine,
                    Encoding.UTF8);
            }
        }

        private static string LimpiarTexto(string texto)
        {
            return (texto ?? "")
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("|", "/");
        }
    }
}