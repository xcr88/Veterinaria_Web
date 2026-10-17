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
                PrepararNueva();
                CargarMascotas();
            }
        }

        // =====================================================
        // CARGAR PROPIETARIOS
        // =====================================================

        private bool CargarPropietarios()
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

                if (ddlPropietario.Items.Count == 1)
                {
                    MostrarMensaje(
                        "Primero registre un propietario.",
                        false);
                }

                return true;
            }
            catch (Exception ex)
            {
                MostrarError(
                    "CONSULTAR_PROPIETARIOS",
                    "No se pudieron cargar los propietarios",
                    ex);

                return false;
            }
        }

        // =====================================================
        // CONSULTAR MASCOTAS
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
        // SELECCIONAR MASCOTA
        // =====================================================

        protected void gvMascotas_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (gvMascotas.SelectedDataKey == null)
            {
                MostrarMensaje(
                    "Seleccione una mascota de la lista.",
                    false);

                return;
            }

            try
            {
                int idMascota = Convert.ToInt32(
                    gvMascotas.SelectedDataKey.Value);

                MascotaRepositorio repositorio =
                    new MascotaRepositorio();

                Mascota mascota =
                    repositorio.ConsultarPorId(idMascota);

                if (mascota == null)
                {
                    PrepararNueva();
                    CargarMascotas();

                    MostrarMensaje(
                        "La mascota seleccionada ya no existe.",
                        false);

                    return;
                }

                if (!CargarPropietarios())
                {
                    PrepararNueva();
                    return;
                }

                ListItem propietario =
                    ddlPropietario.Items.FindByValue(
                        mascota.ID_PROPIETARIO.ToString());

                if (propietario == null)
                {
                    PrepararNueva();

                    MostrarMensaje(
                        "No se encontró el propietario de esta mascota.",
                        false);

                    return;
                }

                // Conserva el ID para actualizar o eliminar.
                ViewState["ID_MASCOTA"] = mascota.ID_MASCOTA;

                txtNombre.Text = mascota.NOMBRE;

                txtFechaNacimiento.Text =
                    mascota.FECHA_NACIMIENTO.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                ddlSexo.SelectedValue = mascota.SEXO;

                txtPeso.Text = mascota.PESO.ToString(
                    "F2",
                    CultureInfo.InvariantCulture);

                txtAlergias.Text = mascota.ALERGIAS ?? "";

                ddlPropietario.SelectedValue =
                    mascota.ID_PROPIETARIO.ToString();

                btnGuardar.Enabled = false;
                btnActualizar.Enabled = true;
                btnEliminar.Enabled = true;

                string aviso = RegistrarEnBitacora(
                    "CONSULTAR_MASCOTA",
                    "ID: " + mascota.ID_MASCOTA);

                MostrarMensaje(
                    "Mascota seleccionada. ID: " +
                    mascota.ID_MASCOTA + aviso,
                    true);
            }
            catch (Exception ex)
            {
                PrepararNueva();

                MostrarError(
                    "CONSULTAR_MASCOTA",
                    "No se pudo cargar la mascota",
                    ex);
            }
        }

        // =====================================================
        // CREAR MASCOTA
        // =====================================================

        protected void btnGuardar_Click(
            object sender, EventArgs e)
        {
            if (ViewState["ID_MASCOTA"] != null)
            {
                MostrarMensaje(
                    "Pulse Nueva mascota para crear otro registro.",
                    false);

                return;
            }

            Mascota mascota;

            if (!IntentarLeerMascota(out mascota))
            {
                return;
            }

            try
            {
                if (!ExistePropietario(mascota.ID_PROPIETARIO))
                {
                    CargarPropietarios();

                    MostrarMensaje(
                        "El propietario seleccionado ya no existe.",
                        false);

                    return;
                }

                mascota.ADICIONADO_POR = UsuarioActual;

                MascotaRepositorio repositorio =
                    new MascotaRepositorio();

                int idGenerado = repositorio.Crear(mascota);

                string aviso = RegistrarEnBitacora(
                    "CREAR_MASCOTA",
                    "ID: " + idGenerado +
                    ". Propietario: " + mascota.ID_PROPIETARIO);

                PrepararNueva();

                MostrarMensaje(
                    "Mascota guardada correctamente. ID: " +
                    idGenerado + aviso,
                    true);

                CargarMascotas();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("CREAR_MASCOTA", ex);
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
        // ACTUALIZAR MASCOTA
        // =====================================================

        protected void btnActualizar_Click(
            object sender, EventArgs e)
        {
            if (ViewState["ID_MASCOTA"] == null)
            {
                MostrarMensaje(
                    "Primero seleccione una mascota.",
                    false);

                return;
            }

            Mascota mascota;

            if (!IntentarLeerMascota(out mascota))
            {
                return;
            }

            try
            {
                if (!ExistePropietario(mascota.ID_PROPIETARIO))
                {
                    CargarPropietarios();

                    MostrarMensaje(
                        "El propietario seleccionado ya no existe.",
                        false);

                    return;
                }

                mascota.ID_MASCOTA =
                    Convert.ToInt32(ViewState["ID_MASCOTA"]);

                mascota.MODIFICADO_POR = UsuarioActual;

                MascotaRepositorio repositorio =
                    new MascotaRepositorio();

                bool actualizado =
                    repositorio.Actualizar(mascota);

                if (!actualizado)
                {
                    PrepararNueva();
                    CargarMascotas();

                    MostrarMensaje(
                        "No se actualizó porque la mascota ya no existe.",
                        false);

                    return;
                }

                string aviso = RegistrarEnBitacora(
                    "ACTUALIZAR_MASCOTA",
                    "ID: " + mascota.ID_MASCOTA);

                int idActualizado = mascota.ID_MASCOTA;

                PrepararNueva();

                MostrarMensaje(
                    "Mascota actualizada correctamente. ID: " +
                    idActualizado + aviso,
                    true);

                CargarMascotas();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("ACTUALIZAR_MASCOTA", ex);
            }
            catch (Exception ex)
            {
                MostrarError(
                    "ACTUALIZAR_MASCOTA",
                    "No se pudo actualizar la mascota",
                    ex);
            }
        }

        // =====================================================
        // ELIMINAR MASCOTA
        // =====================================================

        protected void btnEliminar_Click(
            object sender, EventArgs e)
        {
            // Comprueba la selección también en el servidor.
            if (ViewState["ID_MASCOTA"] == null)
            {
                MostrarMensaje(
                    "Primero seleccione una mascota.",
                    false);

                return;
            }

            try
            {
                // No depende de los textos escritos en el formulario.
                int idMascota =
                    Convert.ToInt32(ViewState["ID_MASCOTA"]);

                MascotaRepositorio repositorio =
                    new MascotaRepositorio();

                bool eliminado = repositorio.Eliminar(idMascota);

                string aviso = "";

                if (eliminado)
                {
                    aviso = RegistrarEnBitacora(
                        "ELIMINAR_MASCOTA",
                        "ID: " + idMascota);
                }

                PrepararNueva();

                MostrarMensaje(
                    eliminado
                        ? "Mascota eliminada correctamente." + aviso
                        : "No se eliminó porque la mascota ya no existe.",
                    eliminado);

                CargarMascotas();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("ELIMINAR_MASCOTA", ex);
            }
            catch (Exception ex)
            {
                MostrarError(
                    "ELIMINAR_MASCOTA",
                    "No se pudo eliminar la mascota",
                    ex);
            }
        }

        private bool ExistePropietario(int idPropietario)
        {
            PropietarioRepositorio repositorio =
                new PropietarioRepositorio();

            return repositorio.ConsultarPorId(idPropietario) != null;
        }

        // =====================================================
        // VALIDAR Y LEER EL FORMULARIO
        // =====================================================

        private bool IntentarLeerMascota(out Mascota mascota)
        {
            mascota = null;

            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                nombre.Length > 15)
            {
                MostrarMensaje(
                    "Escriba un nombre de hasta 15 caracteres.",
                    false);

                return false;
            }

            DateTime fechaNacimiento;

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

            decimal peso;

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

            int idPropietario;

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

            mascota = new Mascota
            {
                NOMBRE = nombre,
                FECHA_NACIMIENTO = fechaNacimiento,
                SEXO = ddlSexo.SelectedValue,
                PESO = peso,
                ALERGIAS = txtAlergias.Text.Trim(),
                ID_PROPIETARIO = idPropietario
            };

            return true;
        }

        // =====================================================
        // PREPARAR UNA MASCOTA NUEVA
        // =====================================================

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            CargarPropietarios();
            PrepararNueva();
        }

        private void PrepararNueva()
        {
            ViewState.Remove("ID_MASCOTA");
            gvMascotas.SelectedIndex = -1;

            txtNombre.Text = "";
            txtFechaNacimiento.Text = "";
            ddlSexo.SelectedIndex = 0;
            txtPeso.Text = "";
            txtAlergias.Text = "";

            if (ddlPropietario.Items.Count > 0)
            {
                ddlPropietario.SelectedIndex = 0;
            }

            btnGuardar.Enabled =
                ddlPropietario.Items.Count > 1;

            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;
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

        private void MostrarErrorSql(
            string accion,
            SqlException ex)
        {
            string aviso = RegistrarEnBitacora(
                "ERROR_" + accion,
                "Código SQL: " + ex.Number);

            if (ex.Number == 547 &&
                accion == "ELIMINAR_MASCOTA")
            {
                MostrarMensaje(
                    "No se puede eliminar esta mascota " +
                    "porque tiene hojas clínicas registradas." + aviso,
                    false);
            }
            else if (ex.Number == 547)
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