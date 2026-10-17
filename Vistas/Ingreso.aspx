<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Ingreso.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.Ingreso" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Ingreso - Veterinaria</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Ingreso al sistema</h1>

            <asp:Label
                runat="server"
                AssociatedControlID="txtUsuario"
                Text="Usuario:" />
            <br />
            <asp:TextBox
                ID="txtUsuario"
                runat="server"
                MaxLength="50" />
            <br /><br />

            <asp:Label
                runat="server"
                AssociatedControlID="txtClave"
                Text="Contraseña:" />
            <br />
            <asp:TextBox
                ID="txtClave"
                runat="server"
                TextMode="Password"
                MaxLength="10" />
            <br /><br />

            <asp:Button
                ID="btnIngresar"
                runat="server"
                Text="Ingresar"
                OnClick="btnIngresar_Click" />

            <br /><br />

            <asp:Label
                ID="lblMensaje"
                runat="server" />

            <br /><br />

            <%-- Se muestra si el ingreso funciona, pero falla el LOG. --%>
            <asp:HyperLink
                ID="lnkContinuar"
                runat="server"
                Text="Continuar al mantenimiento de propietarios"
                NavigateUrl="~/Vistas/MantenimientoPropietarios.aspx"
                Visible="False" />
        </div>
    </form>
</body>
</html>