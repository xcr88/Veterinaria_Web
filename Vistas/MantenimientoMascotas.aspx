<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="MantenimientoMascotas.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.MantenimientoMascotas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Mantenimiento de mascotas</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Mantenimiento de mascotas</h1>

            <asp:Label ID="lblUsuario" runat="server" />

            <br /><br />

            <asp:HyperLink
                ID="lnkPropietarios"
                runat="server"
                Text="Ir a propietarios"
                NavigateUrl="~/Vistas/MantenimientoPropietarios.aspx" />

            &nbsp;|&nbsp;

            <asp:HyperLink
                ID="lnkCerrarSesion"
                runat="server"
                Text="Cerrar sesión"
                NavigateUrl="~/Vistas/CerrarSesion.aspx" />

            <h2>Datos de la mascota</h2>

            <asp:Label runat="server"
                AssociatedControlID="txtNombre"
                Text="Nombre:" />
            <br />
            <asp:TextBox ID="txtNombre"
                runat="server" MaxLength="15" />
            <br /><br />

            <asp:Label runat="server"
                AssociatedControlID="txtFechaNacimiento"
                Text="Fecha de nacimiento:" />
            <br />
            <asp:TextBox ID="txtFechaNacimiento"
                runat="server" TextMode="Date" />
            <br /><br />

            <asp:Label runat="server"
                AssociatedControlID="ddlSexo"
                Text="Sexo:" />
            <br />
            <asp:DropDownList ID="ddlSexo" runat="server">
                <asp:ListItem Text="Seleccione" Value="" />
                <asp:ListItem Text="Hembra" Value="H" />
                <asp:ListItem Text="Macho" Value="M" />
            </asp:DropDownList>
            <br /><br />

            <asp:Label runat="server"
                AssociatedControlID="txtPeso"
                Text="Peso en kilogramos:" />
            <br />
            <asp:TextBox ID="txtPeso"
                runat="server" MaxLength="6" />
            <br />
            <small>Ejemplo: 4.50 o 4,50. Máximo dos decimales.</small>
            <br /><br />

            <asp:Label runat="server"
                AssociatedControlID="txtAlergias"
                Text="Alergias (opcional):" />
            <br />
            <asp:TextBox ID="txtAlergias"
                runat="server"
                TextMode="MultiLine"
                Rows="3"
                MaxLength="150" />
            <br /><br />

            <asp:Label runat="server"
                AssociatedControlID="ddlPropietario"
                Text="Propietario:" />
            <br />
            <asp:DropDownList
                ID="ddlPropietario"
                runat="server" />
            <br /><br />

            <asp:Button
                ID="btnGuardar"
                runat="server"
                Text="Guardar mascota"
                OnClick="btnGuardar_Click" />

            <asp:Button
                ID="btnActualizar"
                runat="server"
                Text="Actualizar mascota"
                Enabled="False"
                OnClick="btnActualizar_Click" />

            <asp:Button
                ID="btnNuevo"
                runat="server"
                Text="Nueva mascota"
                CausesValidation="False"
                OnClick="btnNuevo_Click" />

            <br /><br />

            <asp:Label ID="lblMensaje" runat="server" />

            <hr />

            <h2>Mascotas registradas</h2>

            <asp:GridView
                ID="gvMascotas"
                runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="ID_MASCOTA"
                EmptyDataText="No hay mascotas registradas."
                OnSelectedIndexChanged="gvMascotas_SelectedIndexChanged">

                <SelectedRowStyle BackColor="LightYellow" />

                <Columns>
                    <asp:CommandField
                        ShowSelectButton="True"
                        SelectText="Seleccionar"
                        CausesValidation="False" />

                    <asp:BoundField
                        DataField="ID_MASCOTA"
                        HeaderText="ID" />

                    <asp:BoundField
                        DataField="NOMBRE"
                        HeaderText="Nombre" />

                    <asp:BoundField
                        DataField="FECHA_NACIMIENTO"
                        HeaderText="Fecha de nacimiento"
                        DataFormatString="{0:dd/MM/yyyy}" />

                    <asp:BoundField
                        DataField="SEXO"
                        HeaderText="Sexo" />

                    <asp:BoundField
                        DataField="PESO"
                        HeaderText="Peso (kg)"
                        DataFormatString="{0:F2}" />

                    <asp:BoundField
                        DataField="ALERGIAS"
                        HeaderText="Alergias" />

                    <asp:BoundField
                        DataField="ID_PROPIETARIO"
                        HeaderText="ID propietario" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>