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

        // Verifica la sesión antes de procesar los controles.
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
                PrepararNueva();
                CargarHojasClinicas();
            }
        }

        private void CargarMascotas()
        {
            ddlMascota.Items.Clear();

            ddlMascota.Items.Add(
                new ListItem("Seleccione una mascota", ""));

            try
            {
                MascotaRepositorio repositorio =
                    new MascotaRepositorio();

                foreach (Mascota mascota in
                    repositorio.ConsultarTodos())
                {
                    string descripcion =
                        "ID: " + mascota.ID_MASCOTA +
                        " - " + mascota.NOMBRE +
                        " (propietario: " +
                        mascota.ID_PROPIETARIO + ")";

                    ddlMascota.Items.Add(
                        new ListItem(
                            descripcion,
                            mascota.ID_MASCOTA.ToString(
                                CultureInfo.InvariantCulture)));
                }

                if (ddlMascota.Items.Count == 1)
                {
                    MostrarMensaje(
                        "Primero registre una mascota.",
                        false);
                }
            }
            catch (Exception ex)
            {
                MostrarError(
                    "CONSULTAR_MASCOTAS",
                    "No se pudieron cargar las mascotas",
                    ex);
            }
        }

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
                        "Listado cargado." + aviso,
                        false);
                }
            }
            catch (Exception ex)
            {
                MostrarError(
                    "CONSULTAR_HOJAS_CLINICAS",
                    "No se pudieron cargar las atenciones",
                    ex);
            }
        }

        protected void btnGuardar_Click(
            object sender,
            EventArgs e)
        {
            // Impide crear otra atención mientras se está editando.
            if (ViewState["ID_HOJA"] != null)
            {
                MostrarMensaje(
                    "Presione Nueva atención para registrar otra.",
                    false);

                return;
            }

            HojaClinica hoja;

            if (!IntentarLeerHoja(out hoja))
            {
                return;
            }

            try
            {
                if (!ExisteMascota(hoja.ID_MASCOTA))
                {
                    return;
                }

                hoja.ADICIONADO_POR = UsuarioActual;

                HojaClinicaRepositorio repositorio =
                    new HojaClinicaRepositorio();

                int id = repositorio.Crear(hoja);

                string aviso = RegistrarEnBitacora(
                    "CREAR_HOJA_CLINICA",
                    "ID: " + id +
                    ". Mascota: " + hoja.ID_MASCOTA);

                PrepararNueva();

                MostrarMensaje(
                    "Atención guardada correctamente. ID: " +
                    id + aviso,
                    true);

                CargarHojasClinicas();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("CREAR_HOJA_CLINICA", ex);
            }
            catch (Exception ex)
            {
                MostrarError(
                    "CREAR_HOJA_CLINICA",
                    "No se pudo guardar la atención",
                    ex);
            }
        }

        protected void gvHojasClinicas_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            // Se descarta cualquier selección anterior.
            ViewState.Remove("ID_HOJA");

            btnActualizar.Enabled = false;
            btnGuardar.Enabled = false;

            try
            {
                if (gvHojasClinicas.SelectedDataKey == null)
                {
                    PrepararNueva();

                    MostrarMensaje(
                        "Seleccione una atención.",
                        false);

                    return;
                }

                int id = Convert.ToInt32(
                    gvHojasClinicas.SelectedDataKey.Value);

                HojaClinicaRepositorio repositorio =
                    new HojaClinicaRepositorio();

                HojaClinica hoja =
                    repositorio.ConsultarPorId(id);

                if (hoja == null)
                {
                    PrepararNueva();
                    CargarHojasClinicas();

                    MostrarMensaje(
                        "La atención seleccionada ya no existe.",
                        false);

                    return;
                }

                CargarMascotas();

                ListItem mascota = ddlMascota.Items.FindByValue(
                    hoja.ID_MASCOTA.ToString(
                        CultureInfo.InvariantCulture));

                if (mascota == null)
                {
                    PrepararNueva();

                    MostrarMensaje(
                        "No se pudo cargar la mascota de esta atención. " +
                        "Intente seleccionar nuevamente el registro.",
                        false);

                    return;
                }

                ddlMascota.ClearSelection();
                mascota.Selected = true;

                txtFechaAtencion.Text =
                    hoja.FECHA_ATENCION.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                txtSintomas.Text = hoja.SINTOMAS;
                txtDiagnostico.Text = hoja.DIAGNOSTICO;
                txtTratamiento.Text = hoja.TRATAMIENTO;

                ViewState["ID_HOJA"] = hoja.ID_HOJA;

                btnGuardar.Enabled = false;
                btnActualizar.Enabled = true;

                string aviso = RegistrarEnBitacora(
                    "CONSULTAR_HOJA_CLINICA",
                    "ID: " + hoja.ID_HOJA);

                MostrarMensaje(
                    "Atención seleccionada. ID: " +
                    hoja.ID_HOJA + aviso,
                    true);
            }
            catch (Exception ex)
            {
                PrepararNueva();

                MostrarError(
                    "CONSULTAR_HOJA_CLINICA",
                    "No se pudo cargar la atención seleccionada",
                    ex);
            }
        }

        protected void btnActualizar_Click(
            object sender,
            EventArgs e)
        {
            if (ViewState["ID_HOJA"] == null)
            {
                MostrarMensaje(
                    "Primero seleccione una atención.",
                    false);

                return;
            }

            HojaClinica hoja;

            if (!IntentarLeerHoja(out hoja))
            {
                return;
            }

            try
            {
                if (!ExisteMascota(hoja.ID_MASCOTA))
                {
                    return;
                }

                hoja.ID_HOJA = Convert.ToInt32(
                    ViewState["ID_HOJA"]);

                hoja.MODIFICADO_POR = UsuarioActual;

                HojaClinicaRepositorio repositorio =
                    new HojaClinicaRepositorio();

                bool actualizado =
                    repositorio.Actualizar(hoja);

                if (!actualizado)
                {
                    PrepararNueva();
                    CargarHojasClinicas();

                    MostrarMensaje(
                        "La atención ya no existe. " +
                        "No se realizaron cambios.",
                        false);

                    return;
                }

                string aviso = RegistrarEnBitacora(
                    "ACTUALIZAR_HOJA_CLINICA",
                    "ID: " + hoja.ID_HOJA +
                    ". Mascota: " + hoja.ID_MASCOTA);

                PrepararNueva();

                MostrarMensaje(
                    "Atención actualizada correctamente. ID: " +
                    hoja.ID_HOJA + aviso,
                    true);

                CargarHojasClinicas();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql(
                    "ACTUALIZAR_HOJA_CLINICA",
                    ex);
            }
            catch (Exception ex)
            {
                MostrarError(
                    "ACTUALIZAR_HOJA_CLINICA",
                    "No se pudo actualizar la atención",
                    ex);
            }
        }

        private bool ExisteMascota(int idMascota)
        {
            MascotaRepositorio repositorio =
                new MascotaRepositorio();

            if (repositorio.ConsultarPorId(idMascota) != null)
            {
                return true;
            }

            CargarMascotas();

            MostrarMensaje(
                "La mascota seleccionada ya no existe. " +
                "Seleccione otra mascota.",
                false);

            return false;
        }

        // Valida los datos tanto al crear como al actualizar.
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
                    "Ingrese una fecha de atención válida.",
                    false);

                return false;
            }

            if (fechaAtencion.Date > DateTime.Today)
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
                    "Complete los síntomas, el diagnóstico " +
                    "y el tratamiento.",
                    false);

                return false;
            }

            if (sintomas.Length > 150 ||
                diagnostico.Length > 150 ||
                tratamiento.Length > 150)
            {
                MostrarMensaje(
                    "Los síntomas, el diagnóstico y el tratamiento " +
                    "deben tener como máximo 150 caracteres cada uno.",
                    false);

                return false;
            }

            hoja = new HojaClinica
            {
                ID_MASCOTA = idMascota,
                FECHA_ATENCION = fechaAtencion.Date,
                SINTOMAS = sintomas,
                DIAGNOSTICO = diagnostico,
                TRATAMIENTO = tratamiento
            };

            return true;
        }

        protected void btnNueva_Click(
            object sender,
            EventArgs e)
        {
            lblMensaje.Text = "";

            CargarMascotas();
            PrepararNueva();
        }

        private void PrepararNueva()
        {
            ViewState.Remove("ID_HOJA");

            gvHojasClinicas.SelectedIndex = -1;

            if (ddlMascota.Items.Count > 0)
            {
                ddlMascota.SelectedIndex = 0;
            }

            txtFechaAtencion.Text =
                DateTime.Today.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);

            txtSintomas.Text = "";
            txtDiagnostico.Text = "";
            txtTratamiento.Text = "";

            btnGuardar.Enabled =
                ddlMascota.Items.Count > 1;

            btnActualizar.Enabled = false;
        }

        // Un fallo del LOG no cambia el resultado de la operación SQL.
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

        private void MostrarErrorSql(
            string accion,
            SqlException ex)
        {
            string aviso = RegistrarEnBitacora(
                "ERROR_" + accion,
                "Código SQL: " + ex.Number);

            if (ex.Number == 547)
            {
                MostrarMensaje(
                    "Revise la mascota y los datos de la atención. " +
                    "No cumplen una relación o validación " +
                    "de la base de datos." + aviso,
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

        private void MostrarMensaje(
            string mensaje,
            bool correcto)
        {
            lblMensaje.Text =
                Server.HtmlEncode(mensaje);

            lblMensaje.ForeColor = correcto
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Red;
        }
    }
}