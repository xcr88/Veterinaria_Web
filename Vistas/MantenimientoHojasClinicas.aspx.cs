using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Veterinaria_Web.Modelos;
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

                CargarMascotas();
                LimpiarCampos();
                CargarHojasClinicas();
            }
        }

        // =====================================================
        // CARGAR LAS MASCOTAS DISPONIBLES
        // =====================================================

        private void CargarMascotas()
        {
            ddlMascota.Items.Clear();

            ddlMascota.Items.Add(
                new ListItem("Seleccione una mascota", ""));

            try
            {
                MascotaRepositorio repositorio =
                    new MascotaRepositorio();

                foreach (Mascota mascota
                    in repositorio.ConsultarTodos())
                {
                    string descripcion =
                        "ID: " + mascota.ID_MASCOTA +
                        " - " + mascota.NOMBRE +
                        " (propietario: " +
                        mascota.ID_PROPIETARIO + ")";

                    ddlMascota.Items.Add(
                        new ListItem(
                            descripcion,
                            mascota.ID_MASCOTA.ToString()));
                }

                btnGuardar.Enabled =
                    ddlMascota.Items.Count > 1;

                if (!btnGuardar.Enabled)
                {
                    MostrarMensaje(
                        "Primero registre una mascota.",
                        false);
                }
            }
            catch (Exception ex)
            {
                btnGuardar.Enabled = false;

                MostrarError(
                    "CONSULTAR_MASCOTAS",
                    "No se pudieron cargar las mascotas",
                    ex);
            }
        }

        // =====================================================
        // CONSULTAR LAS ATENCIONES
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
                MostrarError(
                    "CONSULTAR_HOJAS_CLINICAS",
                    "No se pudieron consultar las hojas clínicas",
                    ex);
            }
        }

        // =====================================================
        // CREAR UNA ATENCIÓN
        // =====================================================

        protected void btnGuardar_Click(
            object sender, EventArgs e)
        {
            HojaClinica hoja;

            if (!IntentarLeerHoja(out hoja))
            {
                return;
            }

            try
            {
                // Comprueba que la mascota todavía exista.
                MascotaRepositorio mascotas =
                    new MascotaRepositorio();

                if (mascotas.ConsultarPorId(hoja.ID_MASCOTA) == null)
                {
                    CargarMascotas();

                    MostrarMensaje(
                        "La mascota seleccionada ya no existe.",
                        false);

                    return;
                }

                hoja.ADICIONADO_POR = UsuarioActual;

                HojaClinicaRepositorio repositorio =
                    new HojaClinicaRepositorio();

                int idGenerado = repositorio.Crear(hoja);

                string aviso = RegistrarEnBitacora(
                    "CREAR_HOJA_CLINICA",
                    "ID: " + idGenerado +
                    ". Mascota: " + hoja.ID_MASCOTA);

                LimpiarCampos();

                MostrarMensaje(
                    "Atención guardada correctamente. ID: " +
                    idGenerado + aviso,
                    true);

                CargarHojasClinicas();
            }
            catch (SqlException ex)
            {
                string aviso = RegistrarEnBitacora(
                    "ERROR_CREAR_HOJA_CLINICA",
                    "Código SQL: " + ex.Number);

                if (ex.Number == 547)
                {
                    MostrarMensaje(
                        "Revise la mascota y los datos de la atención. " +
                        "No cumplen una relación o validación " +
                        "de la base." + aviso,
                        false);
                }
                else
                {
                    MostrarMensaje(
                        "No se pudo guardar la atención: " +
                        ex.Message + aviso,
                        false);
                }
            }
            catch (Exception ex)
            {
                MostrarError(
                    "CREAR_HOJA_CLINICA",
                    "No se pudo guardar la atención",
                    ex);
            }
        }

        // =====================================================
        // VALIDAR Y LEER EL FORMULARIO
        // =====================================================

        private bool IntentarLeerHoja(out HojaClinica hoja)
        {
            hoja = null;

            int idMascota;

            if (!int.TryParse(
                ddlMascota.SelectedValue,
                out idMascota) ||
                idMascota <= 0)
            {
                MostrarMensaje(
                    "Seleccione una mascota.",
                    false);

                return false;
            }

            DateTime fechaAtencion;

            if (!DateTime.TryParseExact(
                txtFechaAtencion.Text,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fechaAtencion))
            {
                MostrarMensaje(
                    "Seleccione una fecha de atención válida.",
                    false);

                return false;
            }

            if (fechaAtencion > DateTime.Today)
            {
                MostrarMensaje(
                    "La fecha de atención no puede ser futura.",
                    false);

                return false;
            }

            string sintomas = txtSintomas.Text.Trim();
            string diagnostico = txtDiagnostico.Text.Trim();
            string tratamiento = txtTratamiento.Text.Trim();

            if (string.IsNullOrWhiteSpace(sintomas) ||
                string.IsNullOrWhiteSpace(diagnostico) ||
                string.IsNullOrWhiteSpace(tratamiento))
            {
                MostrarMensaje(
                    "Complete síntomas, diagnóstico y tratamiento.",
                    false);

                return false;
            }

            // Los TextBox multilínea también se validan aquí.
            if (sintomas.Length > 150 ||
                diagnostico.Length > 150 ||
                tratamiento.Length > 150)
            {
                MostrarMensaje(
                    "Síntomas, diagnóstico y tratamiento " +
                    "admiten hasta 150 caracteres cada uno.",
                    false);

                return false;
            }

            hoja = new HojaClinica
            {
                FECHA_ATENCION = fechaAtencion,
                SINTOMAS = sintomas,
                DIAGNOSTICO = diagnostico,
                TRATAMIENTO = tratamiento,
                ID_MASCOTA = idMascota
            };

            return true;
        }

        // =====================================================
        // PREPARAR UNA ATENCIÓN NUEVA
        // =====================================================

        protected void btnNueva_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            CargarMascotas();
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            if (ddlMascota.Items.Count > 0)
            {
                ddlMascota.SelectedIndex = 0;
            }

            txtFechaAtencion.Text = DateTime.Today.ToString(
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture);

            txtSintomas.Text = "";
            txtDiagnostico.Text = "";
            txtTratamiento.Text = "";
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