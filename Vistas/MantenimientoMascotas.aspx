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

            <asp:Label
                ID="lblUsuario"
                runat="server" />

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

            <br /><br />

            <asp:Label
                ID="lblMensaje"
                runat="server" />

            <h2>Mascotas registradas</h2>

            <asp:GridView
                ID="gvMascotas"
                runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="ID_MASCOTA"
                EmptyDataText="No hay mascotas registradas.">

                <Columns>
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