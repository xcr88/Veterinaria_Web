<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="MantenimientoPropietarios.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.MantenimientoPropietarios" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <title>Mantenimiento de propietarios</title>

    <link href="~/Estilos/Estilos.css"
        rel="stylesheet"
        runat="server" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="pagina">

            <header class="encabezado">
                <h1>Mantenimiento de propietarios</h1>

                <p>Registro y administración de propietarios de mascotas.</p>

                <br />

                <nav class="acciones" aria-label="Navegación principal">
                    <asp:HyperLink
                        ID="lnkMascotas"
                        runat="server"
                        CssClass="boton boton-secundario"
                        Text="Ir a mascotas"
                        NavigateUrl="~/Vistas/MantenimientoMascotas.aspx" />

                    <asp:HyperLink
                        ID="lnkHojasClinicas"
                        runat="server"
                        CssClass="boton boton-secundario"
                        Text="Ir a hojas clínicas"
                        NavigateUrl="~/Vistas/MantenimientoHojasClinicas.aspx" />

                    <asp:HyperLink
                        ID="lnkCerrarSesion"
                        runat="server"
                        CssClass="boton boton-editar"
                        Text="Cerrar sesión"
                        NavigateUrl="~/Vistas/CerrarSesion.aspx" />
                </nav>
            </header>

            <asp:Label
                ID="lblMensaje"
                runat="server"
                CssClass="mensaje"
                role="status"
                aria-live="polite" />

            <main class="contenedor">

                <section class="tarjeta formulario">
                    <h2>Datos del propietario</h2>

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtIdentificacion"
                        Text="Identificación:" />

                    <asp:TextBox
                        ID="txtIdentificacion"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="15" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtPrimerNombre"
                        Text="Primer nombre:" />

                    <asp:TextBox
                        ID="txtPrimerNombre"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="25" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtSegundoNombre"
                        Text="Segundo nombre (opcional):" />

                    <asp:TextBox
                        ID="txtSegundoNombre"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="25" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtPrimerApellido"
                        Text="Primer apellido:" />

                    <asp:TextBox
                        ID="txtPrimerApellido"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="25" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtSegundoApellido"
                        Text="Segundo apellido (opcional):" />

                    <asp:TextBox
                        ID="txtSegundoApellido"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="25" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtTelefono"
                        Text="Teléfono:" />

                    <asp:TextBox
                        ID="txtTelefono"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="8"
                        inputmode="numeric" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtCorreo"
                        Text="Correo electrónico:" />

                    <asp:TextBox
                        ID="txtCorreo"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="150" />

                    <div class="acciones">
                        <asp:Button
                            ID="btnGuardar"
                            runat="server"
                            CssClass="boton boton-primario"
                            Text="Guardar propietario"
                            OnClick="btnGuardar_Click" />

                        <asp:Button
                            ID="btnActualizar"
                            runat="server"
                            CssClass="boton boton-editar"
                            Text="Actualizar propietario"
                            Enabled="False"
                            OnClick="btnActualizar_Click" />

                        <asp:Button
                            ID="btnEliminar"
                            runat="server"
                            CssClass="boton boton-peligro"
                            Text="Eliminar propietario"
                            Enabled="False"
                            CausesValidation="False"
                            OnClientClick="return confirm('¿Desea eliminar este propietario? Esta acción no se puede deshacer.');"
                            OnClick="btnEliminar_Click" />

                        <asp:Button
                            ID="btnNuevo"
                            runat="server"
                            CssClass="boton boton-secundario"
                            Text="Nuevo propietario"
                            CausesValidation="False"
                            OnClick="btnNuevo_Click" />
                    </div>
                </section>

                <section class="tarjeta listado">
                    <h2>Propietarios registrados</h2>

                    <div class="tabla-contenedor">
                        <asp:GridView
                            ID="gvPropietarios"
                            runat="server"
                            CssClass="tabla"
                            GridLines="None"
                            AutoGenerateColumns="False"
                            DataKeyNames="ID_PROPIETARIO"
                            EmptyDataText="No hay propietarios registrados."
                            OnSelectedIndexChanged="gvPropietarios_SelectedIndexChanged">

                            <SelectedRowStyle BackColor="#F6E2B3" />

                            <Columns>
                                <asp:CommandField
                                    ShowSelectButton="True"
                                    SelectText="Seleccionar"
                                    CausesValidation="False"
                                    ControlStyle-CssClass="boton boton-secundario" />

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
                </section>

            </main>
        </div>
    </form>
</body>
</html>