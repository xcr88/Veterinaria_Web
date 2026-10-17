<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Ingreso.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.Ingreso" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <title>Ingreso - Veterinaria</title>

    <link href="~/Estilos/Estilos.css"
        rel="stylesheet"
        runat="server" />

    <style>
        .pagina-ingreso {
            max-width: 560px;
            margin: 40px auto;
        }

        .pagina-ingreso .encabezado h1 {
            font-size: 30px;
        }

        .pagina-ingreso .acciones .boton {
            width: 100%;
        }

        @media (max-width: 600px) {
            .pagina-ingreso {
                margin: 12px auto;
            }
        }
    </style>
</head>
<body>
    <form id="form1"
        runat="server"
        defaultbutton="btnIngresar">

        <main class="pagina pagina-ingreso">

            <header class="encabezado">
                <h1>Veterinaria</h1>
                <p>Administración de propietarios, mascotas y atenciones clínicas.</p>
            </header>

            <section class="tarjeta formulario">
                <h2>Ingreso al sistema</h2>

                <asp:Label
                    ID="lblMensaje"
                    runat="server"
                    CssClass="mensaje"
                    role="status"
                    aria-live="polite" />

                <asp:Label
                    runat="server"
                    AssociatedControlID="txtUsuario"
                    Text="Usuario:" />

                <asp:TextBox
                    ID="txtUsuario"
                    runat="server"
                    CssClass="entrada"
                    MaxLength="50"
                    autocomplete="username" />

                <asp:Label
                    runat="server"
                    AssociatedControlID="txtClave"
                    Text="Contraseña:" />

                <asp:TextBox
                    ID="txtClave"
                    runat="server"
                    CssClass="entrada"
                    TextMode="Password"
                    MaxLength="10"
                    autocomplete="current-password" />

                <div class="acciones">
                    <asp:Button
                        ID="btnIngresar"
                        runat="server"
                        CssClass="boton boton-primario"
                        Text="Ingresar"
                        OnClick="btnIngresar_Click" />

                    <asp:HyperLink
                        ID="lnkContinuar"
                        runat="server"
                        CssClass="boton boton-secundario"
                        Text="Continuar al mantenimiento de propietarios"
                        NavigateUrl="~/Vistas/MantenimientoPropietarios.aspx"
                        Visible="False" />
                </div>
            </section>

        </main>
    </form>
</body>
</html>