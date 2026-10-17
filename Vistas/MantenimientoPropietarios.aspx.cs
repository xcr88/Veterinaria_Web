using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using Veterinaria_Web.Modelos;
using Veterinaria_Web.Repositorios;
using Veterinaria_Web.Utilidades;

namespace Veterinaria_Web.Vistas
{
    public partial class MantenimientoPropietarios
        : System.Web.UI.Page
    {
        // Obtiene el nombre guardado al iniciar sesión.
        private string UsuarioActual
        {
            get
            {
                return Convert.ToString(Session["Usuario"]);
            }
        }

        // =====================================================
        // COMPROBAR LA SESIÓN ANTES DE PROCESAR LA PÁGINA
        // =====================================================

        protected override void OnInit(EventArgs e)
        {
            if (Session["ID_USUARIO"] == null ||
                string.IsNullOrWhiteSpace(UsuarioActual))
            {
                // Finaliza esta respuesta y dirige al ingreso.
                // Así tampoco se ejecutan los eventos de los botones.
                Response.Redirect("~/Vistas/Ingreso.aspx", true);
                return;
            }

            // Vincula el estado del formulario con esta sesión.
            ViewStateUserKey = Session.SessionID;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Evita almacenar esta página en la caché del navegador.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

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
                MostrarError(
                    "CONSULTAR_PROPIETARIOS",
                    "No se pudieron consultar los propietarios",
                    ex);
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

                string aviso = RegistrarEnBitacora(
                    "CONSULTAR_PROPIETARIO",
                    "ID: " + propietario.ID_PROPIETARIO);

                MostrarMensaje(
                    "Propietario seleccionado. ID: " +
                    propietario.ID_PROPIETARIO + aviso,
                    true);
            }
            catch (Exception ex)
            {
                PrepararNuevo();

                MostrarError(
                    "CONSULTAR_PROPIETARIO",
                    "No se pudo cargar el propietario",
                    ex);
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

                // Usa el usuario conectado en la auditoría.
                propietario.ADICIONADO_POR = UsuarioActual;

                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                int idGenerado = repositorio.Crear(propietario);

                string aviso = RegistrarEnBitacora(
                    "CREAR_PROPIETARIO",
                    "ID: " + idGenerado);

                PrepararNuevo();

                MostrarMensaje(
                    "Propietario guardado correctamente. ID: " +
                    idGenerado + aviso,
                    true);

                CargarPropietarios();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("CREAR_PROPIETARIO", ex);
            }
            catch (Exception ex)
            {
                MostrarError(
                    "CREAR_PROPIETARIO",
                    "No se pudo guardar el propietario",
                    ex);
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

                // Conserva la creación y registra quién modifica.
                propietario.MODIFICADO_POR = UsuarioActual;

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

                string aviso = RegistrarEnBitacora(
                    "ACTUALIZAR_PROPIETARIO",
                    "ID: " + propietario.ID_PROPIETARIO);

                int idActualizado = propietario.ID_PROPIETARIO;

                PrepararNuevo();

                MostrarMensaje(
                    "Propietario actualizado correctamente. ID: " +
                    idActualizado + aviso,
                    true);

                CargarPropietarios();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("ACTUALIZAR_PROPIETARIO", ex);
            }
            catch (Exception ex)
            {
                MostrarError(
                    "ACTUALIZAR_PROPIETARIO",
                    "No se pudo actualizar el propietario",
                    ex);
            }
        }

        // =====================================================
        // ELIMINAR
        // =====================================================

        protected void btnEliminar_Click(
            object sender, EventArgs e)
        {
            if (ViewState["ID_PROPIETARIO"] == null)
            {
                MostrarMensaje(
                    "Primero seleccione un propietario.",
                    false);

                return;
            }

            try
            {
                int idPropietario = Convert.ToInt32(
                    ViewState["ID_PROPIETARIO"]);

                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                bool eliminado =
                    repositorio.Eliminar(idPropietario);

                string aviso = "";

                if (eliminado)
                {
                    aviso = RegistrarEnBitacora(
                        "ELIMINAR_PROPIETARIO",
                        "ID: " + idPropietario);
                }

                PrepararNuevo();

                MostrarMensaje(
                    eliminado
                        ? "Propietario eliminado correctamente." + aviso
                        : "No se eliminó porque el registro ya no existe.",
                    eliminado);

                CargarPropietarios();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("ELIMINAR_PROPIETARIO", ex);
            }
            catch (Exception ex)
            {
                MostrarError(
                    "ELIMINAR_PROPIETARIO",
                    "No se pudo eliminar el propietario",
                    ex);
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
        // REGISTRAR EN EL LOG
        // =====================================================

        private string RegistrarEnBitacora(
            string accion,
            string detalle)
        {
            try
            {
                // El responsable es quien inició sesión.
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
        // MOSTRAR ERRORES
        // =====================================================

        private void MostrarErrorSql(
            string accion,
            SqlException ex)
        {
            string aviso = RegistrarEnBitacora(
                "ERROR_" + accion,
                "Código SQL: " + ex.Number);

            if (ex.Number == 2601 || ex.Number == 2627)
            {
                MostrarMensaje(
                    "Ya existe un propietario con esa identificación." +
                    aviso,
                    false);
            }
            else if (ex.Number == 547 &&
                     accion == "ELIMINAR_PROPIETARIO")
            {
                MostrarMensaje(
                    "No se puede eliminar este propietario " +
                    "porque tiene mascotas registradas." + aviso,
                    false);
            }
            else
            {
                MostrarMensaje(
                    "No se pudo completar la operación: " +
                    ex.Message + aviso,
                    false);
            }
        }

        private void MostrarError(
            string accion,
            string mensaje,
            Exception ex)
        {
            string aviso = RegistrarEnBitacora(
                "ERROR_" + accion,
                "Tipo de error: " + ex.GetType().Name);

            MostrarMensaje(
                mensaje + ": " + ex.Message + aviso,
                false);
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