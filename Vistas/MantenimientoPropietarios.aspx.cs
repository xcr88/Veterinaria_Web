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
                PrepararNuevo();
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
        // SELECCIONAR
        // =====================================================

        protected void gvPropietarios_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (gvPropietarios.SelectedDataKey == null)
            {
                MostrarMensaje(
                    "Seleccione un propietario de la lista.",
                    false);

                return;
            }

            try
            {
                int idPropietario = Convert.ToInt32(
                    gvPropietarios.SelectedDataKey.Value);

                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                Propietario propietario =
                    repositorio.ConsultarPorId(idPropietario);

                if (propietario == null)
                {
                    PrepararNuevo();
                    CargarPropietarios();

                    MostrarMensaje(
                        "El propietario seleccionado ya no existe.",
                        false);

                    return;
                }

                // Guarda el ID para actualizar o eliminar.
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

                btnGuardar.Enabled = false;
                btnActualizar.Enabled = true;
                btnEliminar.Enabled = true;

                MostrarMensaje(
                    "Propietario seleccionado. ID: " +
                    propietario.ID_PROPIETARIO,
                    true);
            }
            catch (Exception ex)
            {
                PrepararNuevo();

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
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;

            LimpiarCampos();
        }

        // =====================================================
        // CREAR
        // =====================================================

        protected void btnGuardar_Click(
            object sender, EventArgs e)
        {
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
                Propietario propietario = LeerFormulario();

                // Temporal hasta implementar el ingreso.
                propietario.ADICIONADO_POR = "Admin";

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
                MostrarErrorSql(ex);
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo guardar el propietario: " +
                    ex.Message,
                    false);
            }
        }

        // =====================================================
        // ACTUALIZAR
        // =====================================================

        protected void btnActualizar_Click(
            object sender, EventArgs e)
        {
            if (ViewState["ID_PROPIETARIO"] == null)
            {
                MostrarMensaje(
                    "Primero seleccione un propietario.",
                    false);

                return;
            }

            if (!ValidarDatos())
            {
                return;
            }

            try
            {
                Propietario propietario = LeerFormulario();

                propietario.ID_PROPIETARIO =
                    Convert.ToInt32(
                        ViewState["ID_PROPIETARIO"]);

                // Temporal hasta implementar el ingreso.
                propietario.MODIFICADO_POR = "Admin";

                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                bool actualizado =
                    repositorio.Actualizar(propietario);

                if (!actualizado)
                {
                    PrepararNuevo();
                    CargarPropietarios();

                    MostrarMensaje(
                        "No se actualizó porque el registro ya no existe.",
                        false);

                    return;
                }

                int idActualizado = propietario.ID_PROPIETARIO;

                PrepararNuevo();

                MostrarMensaje(
                    "Propietario actualizado correctamente. ID: " +
                    idActualizado,
                    true);

                CargarPropietarios();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql(ex);
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo actualizar el propietario: " +
                    ex.Message,
                    false);
            }
        }

        // =====================================================
        // ELIMINAR
        // =====================================================

        protected void btnEliminar_Click(
            object sender, EventArgs e)
        {
            // Valida la selección también en el servidor.
            if (ViewState["ID_PROPIETARIO"] == null)
            {
                MostrarMensaje(
                    "Primero seleccione un propietario.",
                    false);

                return;
            }

            try
            {
                // Usa el ID seleccionado, no los textos del formulario.
                int idPropietario = Convert.ToInt32(
                    ViewState["ID_PROPIETARIO"]);

                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                bool eliminado =
                    repositorio.Eliminar(idPropietario);

                PrepararNuevo();

                MostrarMensaje(
                    eliminado
                        ? "Propietario eliminado correctamente."
                        : "No se eliminó porque el registro ya no existe.",
                    eliminado);

                CargarPropietarios();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    // La llave foránea protege las mascotas relacionadas.
                    MostrarMensaje(
                        "No se puede eliminar este propietario " +
                        "porque tiene mascotas registradas.",
                        false);
                }
                else
                {
                    MostrarErrorSql(ex);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(
                    "No se pudo eliminar el propietario: " +
                    ex.Message,
                    false);
            }
        }

        // =====================================================
        // LEER LOS DATOS DEL FORMULARIO
        // =====================================================

        private Propietario LeerFormulario()
        {
            return new Propietario
            {
                IDENTIFICACION = txtIdentificacion.Text.Trim(),
                PRIMER_NOMBRE = txtPrimerNombre.Text.Trim(),
                SEGUNDO_NOMBRE = txtSegundoNombre.Text.Trim(),
                PRIMER_APELLIDO = txtPrimerApellido.Text.Trim(),
                SEGUNDO_APELLIDO = txtSegundoApellido.Text.Trim(),
                TELEFONO = txtTelefono.Text.Trim(),
                CORREO = txtCorreo.Text.Trim()
            };
        }

        // =====================================================
        // VALIDAR LOS DATOS
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
        // LIMPIAR CAMPOS
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

        // =====================================================
        // MENSAJES Y ERRORES
        // =====================================================

        private void MostrarErrorSql(SqlException ex)
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
                    "No se pudo completar la operación: " +
                    ex.Message,
                    false);
            }
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