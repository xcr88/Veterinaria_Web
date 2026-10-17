<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="MantenimientoPropietarios.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.MantenimientoPropietarios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Mantenimiento de propietarios</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Mantenimiento de propietarios</h1>

            <%-- Muestra mensajes sobre la consulta. --%>
            <asp:Label
                ID="lblMensaje"
                runat="server" />

            <br /><br />

            <%-- Define las columnas de la lista de propietarios. --%>
            <asp:GridView
                ID="gvPropietarios"
                runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="ID_PROPIETARIO"
                EmptyDataText="No hay propietarios registrados.">

                <Columns>
                    <asp:BoundField
                        DataField="ID_PROPIETARIO"
                        HeaderText="ID" />

                    <asp:BoundField
                        DataField="IDENTIFICACION"
                        HeaderText="Identificación" />

                    <asp:BoundField
                        DataField="PRIMER_NOMBRE"
                        HeaderText="Primer nombre" />

                    <asp:BoundField
                        DataField="SEGUNDO_NOMBRE"
                        HeaderText="Segundo nombre" />

                    <asp:BoundField
                        DataField="PRIMER_APELLIDO"
                        HeaderText="Primer apellido" />

                    <asp:BoundField
                        DataField="SEGUNDO_APELLIDO"
                        HeaderText="Segundo apellido" />

                    <asp:BoundField
                        DataField="TELEFONO"
                        HeaderText="Teléfono" />

                    <asp:BoundField
                        DataField="CORREO"
                        HeaderText="Correo electrónico" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>