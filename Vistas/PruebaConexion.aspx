<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PruebaConexion.aspx.cs" Inherits="Veterinaria_Web.Vistas.PruebaConexion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <div>
    <h1>Prueba de conexión</h1>

    <asp:Button
    ID="btnProbarConexion"
    runat="server"
    Text="Probar conexión"
    OnClick="btnProbarConexion_Click" />

    <br /><br />

    <asp:Label
        ID="lblResultado"
        runat="server" />
</div>
    </form>
</body>
</html>
