<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="PruebaLog.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.FormularioWeb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Prueba del LOG</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Prueba del LOG</h1>

            <p>
                Presione el botón para agregar un registro
                al archivo de bitácora.
            </p>

            <asp:Button
                ID="btnProbarLog"
                runat="server"
                Text="Escribir registro de prueba"
                OnClick="btnProbarLog_Click" />

            <br /><br />

            <asp:Label
                ID="lblResultado"
                runat="server" />
        </div>
    </form>
</body>
</html>