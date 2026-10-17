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

            <%-- Campos para registrar un propietario. --%>
            <h2>Agregar propietario</h2>

            <asp:Label
                runat="server"
                AssociatedControlID="txtIdentificacion"
                Text="Identificación:" />
            <br />
            <asp:TextBox
                ID="txtIdentificacion"
                runat="server"
                MaxLength="15" />
            <br /><br />

            <asp:Label
                runat="server"
                AssociatedControlID="txtPrimerNombre"
                Text="Primer nombre:" />
            <br />
            <asp:TextBox
                ID="txtPrimerNombre"
                runat="server"
                MaxLength="25" />
            <br /><br />

            <asp:Label
                runat="server"
                AssociatedControlID="txtSegundoNombre"
                Text="Segundo nombre (opcional):" />
            <br />
            <asp:TextBox
                ID="txtSegundoNombre"
                runat="server"
                MaxLength="25" />
            <br /><br />

            <asp:Label
                runat="server"
                AssociatedControlID="txtPrimerApellido"
                Text="Primer apellido:" />
            <br />
            <asp:TextBox
                ID="txtPrimerApellido"
                runat="server"
                MaxLength="25" />
            <br /><br />

            <asp:Label
                runat="server"
                AssociatedControlID="txtSegundoApellido"
                Text="Segundo apellido (opcional):" />
            <br />
            <asp:TextBox
                ID="txtSegundoApellido"
                runat="server"
                MaxLength="25" />
            <br /><br />

            <asp:Label
                runat="server"
                AssociatedControlID="txtTelefono"
                Text="Teléfono:" />
            <br />
            <asp:TextBox
                ID="txtTelefono"
                runat="server"
                MaxLength="8" />
            <br /><br />

            <asp:Label
                runat="server"
                AssociatedControlID="txtCorreo"
                Text="Correo electrónico:" />
            <br />
            <asp:TextBox
                ID="txtCorreo"
                runat="server"
                MaxLength="150" />
            <br /><br />

            <%-- Ejecuta el método de guardado del archivo C#. --%>
            <asp:Button
                ID="btnGuardar"
                runat="server"
                Text="Guardar propietario"
                OnClick="btnGuardar_Click" />

            <br /><br />

            <%-- Muestra el resultado del guardado o la consulta. --%>
            <asp:Label
                ID="lblMensaje"
                runat="server" />

            <hr />

            <h2>Propietarios registrados</h2>

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