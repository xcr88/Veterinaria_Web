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
            if (!IsPostBack)
            {
                CargarPropietarios();
            }
        }

        // =====================================================
        // CONSULTAR TODOS
        // =====================================================

        private void CargarPropietarios()
        {
            try
            {
                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

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

        // =====================================================
        // SELECCIONAR UN PROPIETARIO
        // =====================================================

        protected void gvPropietarios_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            // Comprueba que haya una fila con su identificador.
            if (gvPropietarios.SelectedDataKey == null)
            {
                MostrarMensaje(
                    "Seleccione un propietario de la lista.",
                    false);

                return;
            }

            try
            {
                // Obtiene la llave del registro, no su posición visual.
                int idPropietario = Convert.ToInt32(
                    gvPropietarios.SelectedDataKey.Value);

                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                Propietario propietario =
                    repositorio.ConsultarPorId(idPropietario);

                // El registro pudo eliminarse después de cargar la lista.
                if (propietario == null)
                {
                    PrepararNuevo();
                    CargarPropietarios();

                    MostrarMensaje(
                        "El propietario seleccionado ya no existe.",
                        false);

                    return;
                }

                // Conserva el ID entre las solicitudes de esta página.
                ViewState["ID_PROPIETARIO"] =
                    propietario.ID_PROPIETARIO;

                txtIdentificacion.Text =
                    propietario.IDENTIFICACION;

                txtPrimerNombre.Text =
                    propietario.PRIMER_NOMBRE;

                txtSegundoNombre.Text =
                    propietario.SEGUNDO_NOMBRE ?? "";

                txtPrimerApellido.Text =
                    propietario.PRIMER_APELLIDO;

                txtSegundoApellido.Text =
                    propietario.SEGUNDO_APELLIDO ?? "";

                txtTelefono.Text =
                    propietario.TELEFONO;

                txtCorreo.Text =
                    propietario.CORREO;

                // Todavía no implementamos actualizar.
                // Evita intentar insertar nuevamente este propietario.
                btnGuardar.Enabled = false;

                MostrarMensaje(
                    "Propietario seleccionado. ID: " +
                    propietario.ID_PROPIETARIO +
                    ". Para crear otro, pulse Nuevo propietario.",
                    true);
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo cargar el propietario: " +
                    ex.Message,
                    false);
            }
        }

        // =====================================================
        // PREPARAR UN REGISTRO NUEVO
        // =====================================================

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            PrepararNuevo();
            lblMensaje.Text = "";
        }

        private void PrepararNuevo()
        {
            ViewState.Remove("ID_PROPIETARIO");
            gvPropietarios.SelectedIndex = -1;
            btnGuardar.Enabled = true;

            LimpiarCampos();
        }

        // =====================================================
        // CREAR PROPIETARIO
        // =====================================================

        protected void btnGuardar_Click(
            object sender, EventArgs e)
        {
            // Comprueba también en el servidor el modo del formulario.
            if (ViewState["ID_PROPIETARIO"] != null)
            {
                MostrarMensaje(
                    "Pulse Nuevo propietario antes de crear otro registro.",
                    false);

                return;
            }

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

                PrepararNuevo();

                MostrarMensaje(
                    "Propietario guardado correctamente. ID: " +
                    idGenerado,
                    true);

                CargarPropietarios();
            }
            catch (SqlException ex)
            {
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

        // =====================================================
        // VALIDAR LOS DATOS DEL FORMULARIO
        // =====================================================

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

        // =====================================================
        // LIMPIAR CAMPOS Y MOSTRAR MENSAJES
        // =====================================================

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