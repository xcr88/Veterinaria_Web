<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="CerrarSesion.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.CerrarSesion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Cerrar sesión - Veterinaria</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Cerrar sesión</h1>

            <asp:Label
                ID="lblUsuario"
                runat="server" />

            <p>¿Desea salir del sistema?</p>

            <asp:Button
                ID="btnCerrarSesion"
                runat="server"
                Text="Sí, cerrar sesión"
                OnClick="btnCerrarSesion_Click" />

            <asp:HyperLink
                ID="lnkCancelar"
                runat="server"
                Text="Cancelar y regresar"
                NavigateUrl="~/Vistas/MantenimientoPropietarios.aspx" />
        </div>
    </form>
</body>
</html>