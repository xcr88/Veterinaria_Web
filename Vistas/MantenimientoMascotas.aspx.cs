using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Veterinaria_Web.Modelos;
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

                CargarPropietarios();
                CargarMascotas();
            }
        }

        // =====================================================
        // CARGAR LA LISTA DE PROPIETARIOS
        // =====================================================

        private void CargarPropietarios()
        {
            ddlPropietario.Items.Clear();

            ddlPropietario.Items.Add(
                new ListItem("Seleccione un propietario", ""));

            try
            {
                PropietarioRepositorio repositorio =
                    new PropietarioRepositorio();

                foreach (Propietario propietario
                    in repositorio.ConsultarTodos())
                {
                    string descripcion =
                        propietario.IDENTIFICACION + " - " +
                        propietario.PRIMER_NOMBRE + " " +
                        propietario.PRIMER_APELLIDO + " " +
                        propietario.SEGUNDO_APELLIDO;

                    ddlPropietario.Items.Add(
                        new ListItem(
                            descripcion.Trim(),
                            propietario.ID_PROPIETARIO.ToString()));
                }

                btnGuardar.Enabled =
                    ddlPropietario.Items.Count > 1;

                if (!btnGuardar.Enabled)
                {
                    MostrarMensaje(
                        "Primero registre un propietario.",
                        false);
                }
            }
            catch (Exception ex)
            {
                btnGuardar.Enabled = false;

                MostrarError(
                    "CONSULTAR_PROPIETARIOS",
                    "No se pudieron cargar los propietarios",
                    ex);
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

                if (!string.IsNullOrEmpty(aviso))
                {
                    MostrarMensaje(
                        "Listado de mascotas cargado." + aviso,
                        false);
                }
            }
            catch (Exception ex)
            {
                MostrarError(
                    "CONSULTAR_MASCOTAS",
                    "No se pudieron consultar las mascotas",
                    ex);
            }
        }

        // =====================================================
        // CREAR MASCOTA
        // =====================================================

        protected void btnGuardar_Click(
            object sender, EventArgs e)
        {
            DateTime fechaNacimiento;
            decimal peso;
            int idPropietario;

            if (!ValidarDatos(
                out fechaNacimiento,
                out peso,
                out idPropietario))
            {
                return;
            }

            try
            {
                // Comprueba que el propietario todavía exista.
                PropietarioRepositorio propietarios =
                    new PropietarioRepositorio();

                if (propietarios.ConsultarPorId(idPropietario) == null)
                {
                    CargarPropietarios();

                    MostrarMensaje(
                        "El propietario seleccionado ya no existe.",
                        false);

                    return;
                }

                Mascota mascota = new Mascota
                {
                    NOMBRE = txtNombre.Text.Trim(),
                    FECHA_NACIMIENTO = fechaNacimiento,
                    SEXO = ddlSexo.SelectedValue,
                    PESO = peso,
                    ALERGIAS = txtAlergias.Text.Trim(),
                    ID_PROPIETARIO = idPropietario,
                    ADICIONADO_POR = UsuarioActual
                };

                MascotaRepositorio repositorio =
                    new MascotaRepositorio();

                int idGenerado = repositorio.Crear(mascota);

                string aviso = RegistrarEnBitacora(
                    "CREAR_MASCOTA",
                    "ID: " + idGenerado +
                    ". Propietario: " + idPropietario);

                LimpiarCampos();

                MostrarMensaje(
                    "Mascota guardada correctamente. ID: " +
                    idGenerado + aviso,
                    true);

                CargarMascotas();
            }
            catch (SqlException ex)
            {
                string aviso = RegistrarEnBitacora(
                    "ERROR_CREAR_MASCOTA",
                    "Código SQL: " + ex.Number);

                if (ex.Number == 547)
                {
                    MostrarMensaje(
                        "Revise el propietario, el sexo y el peso. " +
                        "Los datos no cumplen una relación o validación " +
                        "de la base." + aviso,
                        false);
                }
                else
                {
                    MostrarMensaje(
                        "No se pudo guardar la mascota: " +
                        ex.Message + aviso,
                        false);
                }
            }
            catch (Exception ex)
            {
                MostrarError(
                    "CREAR_MASCOTA",
                    "No se pudo guardar la mascota",
                    ex);
            }
        }

        // =====================================================
        // VALIDAR EL FORMULARIO
        // =====================================================

        private bool ValidarDatos(
            out DateTime fechaNacimiento,
            out decimal peso,
            out int idPropietario)
        {
            fechaNacimiento = DateTime.MinValue;
            peso = 0;
            idPropietario = 0;

            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                nombre.Length > 15)
            {
                MostrarMensaje(
                    "Escriba un nombre de hasta 15 caracteres.",
                    false);

                return false;
            }

            // El control Date envía el valor como año-mes-día.
            if (!DateTime.TryParseExact(
                txtFechaNacimiento.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fechaNacimiento))
            {
                MostrarMensaje(
                    "Seleccione una fecha de nacimiento válida.",
                    false);

                return false;
            }

            if (fechaNacimiento > DateTime.Today)
            {
                MostrarMensaje(
                    "La fecha de nacimiento no puede ser futura.",
                    false);

                return false;
            }

            if (ddlSexo.SelectedValue != "H" &&
                ddlSexo.SelectedValue != "M")
            {
                MostrarMensaje(
                    "Seleccione el sexo de la mascota.",
                    false);

                return false;
            }

            // Acepta punto o coma decimal, sin separadores de miles.
            string textoPeso = txtPeso.Text.Trim();

            if (!Regex.IsMatch(
                textoPeso,
                @"\A[0-9]{1,3}([.,][0-9]{1,2})?\z"))
            {
                MostrarMensaje(
                    "Escriba un peso como 4.50 o 4,50, " +
                    "con máximo dos decimales.",
                    false);

                return false;
            }

            textoPeso = textoPeso.Replace(',', '.');

            if (!decimal.TryParse(
                textoPeso,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out peso) ||
                peso <= 0 ||
                peso > 999.99m)
            {
                MostrarMensaje(
                    "El peso debe estar entre 0.01 y 999.99 kg.",
                    false);

                return false;
            }

            if (txtAlergias.Text.Trim().Length > 150)
            {
                MostrarMensaje(
                    "Las alergias admiten hasta 150 caracteres.",
                    false);

                return false;
            }

            if (!int.TryParse(
                ddlPropietario.SelectedValue,
                out idPropietario) ||
                idPropietario <= 0)
            {
                MostrarMensaje(
                    "Seleccione un propietario.",
                    false);

                return false;
            }

            return true;
        }

        // =====================================================
        // LIMPIAR EL FORMULARIO
        // =====================================================

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            lblMensaje.Text = "";
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtFechaNacimiento.Text = "";
            ddlSexo.SelectedIndex = 0;
            txtPeso.Text = "";
            txtAlergias.Text = "";
            ddlPropietario.SelectedIndex = 0;
        }

        // =====================================================
        // LOG Y MENSAJES
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