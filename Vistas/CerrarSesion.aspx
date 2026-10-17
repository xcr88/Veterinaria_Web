<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="CerrarSesion.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.CerrarSesion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <title>Cerrar sesión - Veterinaria</title>

    <link href="~/Estilos/Estilos.css"
        rel="stylesheet"
        runat="server" />

    <style>
        .pagina-salida {
            max-width: 560px;
            margin: 40px auto;
        }

        .pagina-salida .encabezado h1 {
            font-size: 30px;
        }

        .pagina-salida .acciones {
            margin-top: 24px;
        }

        @media (max-width: 600px) {
            .pagina-salida {
                margin: 12px auto;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <main class="pagina pagina-salida">

            <header class="encabezado">
                <h1>Veterinaria</h1>
                <p>Cierre de sesión</p>
            </header>

            <section class="tarjeta">
                <h2>Cerrar sesión</h2>

                <p>
                    <asp:Label
                        ID="lblUsuario"
                        runat="server" />
                </p>

                <p>¿Desea salir del sistema?</p>

                <div class="acciones">
                    <asp:Button
                        ID="btnCerrarSesion"
                        runat="server"
                        CssClass="boton boton-peligro"
                        Text="Sí, cerrar sesión"
                        OnClick="btnCerrarSesion_Click" />

                    <asp:HyperLink
                        ID="lnkCancelar"
                        runat="server"
                        CssClass="boton boton-secundario"
                        Text="Cancelar y regresar"
                        NavigateUrl="~/Vistas/MantenimientoPropietarios.aspx" />
                </div>
            </section>

        </main>
    </form>
</body>
</html>