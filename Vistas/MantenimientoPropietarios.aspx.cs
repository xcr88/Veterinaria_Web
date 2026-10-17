using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Veterinaria_Web.Modelos;
using Veterinaria_Web.Repositorios;

namespace Veterinaria_Web.Vistas
{
    public partial class MantenimientoPropietarios
        : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Carga inicial de la lista.
            if (!IsPostBack)
            {
                CargarPropietarios();
            }
        }

        private void CargarPropietarios()
        {
            PropietarioRepositorio repositorio =
                new PropietarioRepositorio();

            try
            {
                gvPropietarios.DataSource =
                    repositorio.ConsultarTodos();

                gvPropietarios.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudieron consultar los propietarios: " +
                    ex.Message,
                    false);
            }
        }

        protected void btnGuardar_Click(
            object sender, EventArgs e)
        {
            // Valida antes de enviar los datos a SQL Server.
            if (!ValidarDatos())
            {
                return;
            }

            try
            {
                Propietario propietario = new Propietario
                {
                    IDENTIFICACION = txtIdentificacion.Text.Trim(),
                    PRIMER_NOMBRE = txtPrimerNombre.Text.Trim(),
                    SEGUNDO_NOMBRE = txtSegundoNombre.Text.Trim(),
                    PRIMER_APELLIDO = txtPrimerApellido.Text.Trim(),
                    SEGUNDO_APELLIDO = txtSegundoApellido.Text.Trim(),
                    TELEFONO = txtTelefono.Text.Trim(),
                    CORREO = txtCorreo.Text.Trim(),

                    // Temporal hasta implementar el ingreso.
                    ADICIONADO_POR = "Admin"
                };

                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                int idGenerado = repositorio.Crear(propietario);

                LimpiarCampos();

                MostrarMensaje(
                    "Propietario guardado correctamente. ID: " +
                    idGenerado,
                    true);

                CargarPropietarios();
            }
            catch (SqlException ex)
            {
                // SQL Server usa estos códigos para valores duplicados.
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    MostrarMensaje(
                        "Ya existe un propietario con esa identificación.",
                        false);
                }
                else
                {
                    MostrarMensaje(
                        "No se pudo guardar el propietario: " +
                        ex.Message,
                        false);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo completar la operación: " +
                    ex.Message,
                    false);
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtIdentificacion.Text) ||
                string.IsNullOrWhiteSpace(txtPrimerNombre.Text) ||
                string.IsNullOrWhiteSpace(txtPrimerApellido.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MostrarMensaje(
                    "Complete identificación, primer nombre, " +
                    "primer apellido, teléfono y correo.",
                    false);

                return false;
            }

            // Verifica los tamaños también en el servidor.
            if (txtIdentificacion.Text.Trim().Length > 15 ||
                txtPrimerNombre.Text.Trim().Length > 25 ||
                txtSegundoNombre.Text.Trim().Length > 25 ||
                txtPrimerApellido.Text.Trim().Length > 25 ||
                txtSegundoApellido.Text.Trim().Length > 25 ||
                txtCorreo.Text.Trim().Length > 150)
            {
                MostrarMensaje(
                    "Uno de los campos supera la longitud permitida.",
                    false);

                return false;
            }

            // Acepta exactamente ocho dígitos del 0 al 9.
            if (!Regex.IsMatch(
                txtTelefono.Text.Trim(), @"\A[0-9]{8}\z"))
            {
                MostrarMensaje(
                    "El teléfono debe contener ocho dígitos.",
                    false);

                return false;
            }

            try
            {
                string correo = txtCorreo.Text.Trim();
                MailAddress direccion = new MailAddress(correo);

                if (direccion.Address != correo)
                {
                    MostrarMensaje(
                        "Escriba solamente la dirección de correo.",
                        false);

                    return false;
                }
            }
            catch (FormatException)
            {
                MostrarMensaje(
                    "El correo electrónico no es válido.",
                    false);

                return false;
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtIdentificacion.Text = "";
            txtPrimerNombre.Text = "";
            txtSegundoNombre.Text = "";
            txtPrimerApellido.Text = "";
            txtSegundoApellido.Text = "";
            txtTelefono.Text = "";
            txtCorreo.Text = "";
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = Server.HtmlEncode(mensaje);

            lblMensaje.ForeColor = correcto
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Red;
        }
    }
}