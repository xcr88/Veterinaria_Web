<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="MantenimientoMascotas.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.MantenimientoMascotas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <title>Mantenimiento de mascotas</title>

    <link href="~/Estilos/Estilos.css"
        rel="stylesheet"
        runat="server" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="pagina">

            <header class="encabezado">
                <h1>Mantenimiento de mascotas</h1>

                <p>
                    <asp:Label
                        ID="lblUsuario"
                        runat="server" />
                </p>

                <br />

                <nav class="acciones"
                    aria-label="Navegación principal">

                    <asp:HyperLink
                        ID="lnkPropietarios"
                        runat="server"
                        CssClass="boton boton-secundario"
                        Text="Ir a propietarios"
                        NavigateUrl="~/Vistas/MantenimientoPropietarios.aspx" />

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
                    <h2>Datos de la mascota</h2>

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtNombre"
                        Text="Nombre:" />

                    <asp:TextBox
                        ID="txtNombre"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="15" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtFechaNacimiento"
                        Text="Fecha de nacimiento:" />

                    <asp:TextBox
                        ID="txtFechaNacimiento"
                        runat="server"
                        CssClass="entrada"
                        TextMode="Date" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="ddlSexo"
                        Text="Sexo:" />

                    <asp:DropDownList
                        ID="ddlSexo"
                        runat="server"
                        CssClass="selector">

                        <asp:ListItem
                            Text="Seleccione"
                            Value="" />

                        <asp:ListItem
                            Text="Hembra"
                            Value="H" />

                        <asp:ListItem
                            Text="Macho"
                            Value="M" />
                    </asp:DropDownList>

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtPeso"
                        Text="Peso en kilogramos:" />

                    <asp:TextBox
                        ID="txtPeso"
                        runat="server"
                        CssClass="entrada"
                        MaxLength="6"
                        inputmode="decimal"
                        aria-describedby="ayudaPeso" />

                    <small id="ayudaPeso">
                        Ejemplo: 4.50 o 4,50. Máximo dos decimales.
                    </small>

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtAlergias"
                        Text="Alergias (opcional):" />

                    <asp:TextBox
                        ID="txtAlergias"
                        runat="server"
                        CssClass="entrada"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="150" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="ddlPropietario"
                        Text="Propietario:" />

                    <asp:DropDownList
                        ID="ddlPropietario"
                        runat="server"
                        CssClass="selector" />

                    <div class="acciones">
                        <asp:Button
                            ID="btnGuardar"
                            runat="server"
                            CssClass="boton boton-primario"
                            Text="Guardar mascota"
                            OnClick="btnGuardar_Click" />

                        <asp:Button
                            ID="btnActualizar"
                            runat="server"
                            CssClass="boton boton-editar"
                            Text="Actualizar mascota"
                            Enabled="False"
                            OnClick="btnActualizar_Click" />

                        <asp:Button
                            ID="btnEliminar"
                            runat="server"
                            CssClass="boton boton-peligro"
                            Text="Eliminar mascota"
                            Enabled="False"
                            CausesValidation="False"
                            OnClientClick="return confirm('¿Desea eliminar esta mascota? Esta acción no se puede deshacer.');"
                            OnClick="btnEliminar_Click" />

                        <asp:Button
                            ID="btnNuevo"
                            runat="server"
                            CssClass="boton boton-secundario"
                            Text="Nueva mascota"
                            CausesValidation="False"
                            OnClick="btnNuevo_Click" />
                    </div>
                </section>

                <section class="tarjeta listado">
                    <h2>Mascotas registradas</h2>

                    <div class="tabla-contenedor">
                        <asp:GridView
                            ID="gvMascotas"
                            runat="server"
                            CssClass="tabla"
                            GridLines="None"
                            AutoGenerateColumns="False"
                            DataKeyNames="ID_MASCOTA"
                            EmptyDataText="No hay mascotas registradas."
                            OnSelectedIndexChanged="gvMascotas_SelectedIndexChanged">

                            <SelectedRowStyle BackColor="#F6E2B3" />

                            <Columns>
                                <asp:CommandField
                                    ShowSelectButton="True"
                                    SelectText="Seleccionar"
                                    CausesValidation="False"
                                    ControlStyle-CssClass="boton boton-secundario" />

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
                </section>

            </main>
        </div>
    </form>
</body>
</html>