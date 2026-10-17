<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="MantenimientoHojasClinicas.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.MantenimientoHojasClinicas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <title>Mantenimiento de hojas clínicas</title>

    <link href="~/Estilos/Estilos.css"
        rel="stylesheet"
        runat="server" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="pagina">

            <header class="encabezado">
                <h1>Mantenimiento de hojas clínicas</h1>

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
                        ID="lnkMascotas"
                        runat="server"
                        CssClass="boton boton-secundario"
                        Text="Ir a mascotas"
                        NavigateUrl="~/Vistas/MantenimientoMascotas.aspx" />

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
                    <h2>Datos de la atención</h2>

                    <asp:Label
                        runat="server"
                        AssociatedControlID="ddlMascota"
                        Text="Mascota:" />

                    <asp:DropDownList
                        ID="ddlMascota"
                        runat="server"
                        CssClass="selector" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtFechaAtencion"
                        Text="Fecha de atención:" />

                    <asp:TextBox
                        ID="txtFechaAtencion"
                        runat="server"
                        CssClass="entrada"
                        TextMode="Date" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtSintomas"
                        Text="Síntomas:" />

                    <asp:TextBox
                        ID="txtSintomas"
                        runat="server"
                        CssClass="entrada"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="150" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtDiagnostico"
                        Text="Diagnóstico:" />

                    <asp:TextBox
                        ID="txtDiagnostico"
                        runat="server"
                        CssClass="entrada"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="150" />

                    <asp:Label
                        runat="server"
                        AssociatedControlID="txtTratamiento"
                        Text="Tratamiento:" />

                    <asp:TextBox
                        ID="txtTratamiento"
                        runat="server"
                        CssClass="entrada"
                        TextMode="MultiLine"
                        Rows="3"
                        MaxLength="150" />

                    <div class="acciones">
                        <asp:Button
                            ID="btnGuardar"
                            runat="server"
                            CssClass="boton boton-primario"
                            Text="Guardar atención"
                            OnClick="btnGuardar_Click" />

                        <asp:Button
                            ID="btnActualizar"
                            runat="server"
                            CssClass="boton boton-editar"
                            Text="Actualizar atención"
                            Enabled="False"
                            OnClick="btnActualizar_Click" />

                        <asp:Button
                            ID="btnEliminar"
                            runat="server"
                            CssClass="boton boton-peligro"
                            Text="Eliminar atención"
                            Enabled="False"
                            CausesValidation="False"
                            OnClientClick="return confirm('¿Desea eliminar la atención seleccionada?');"
                            OnClick="btnEliminar_Click" />

                        <asp:Button
                            ID="btnNueva"
                            runat="server"
                            CssClass="boton boton-secundario"
                            Text="Nueva atención"
                            CausesValidation="False"
                            OnClick="btnNueva_Click" />
                    </div>
                </section>

                <section class="tarjeta listado">
                    <h2>Atenciones registradas</h2>

                    <div class="tabla-contenedor">
                        <asp:GridView
                            ID="gvHojasClinicas"
                            runat="server"
                            CssClass="tabla"
                            GridLines="None"
                            AutoGenerateColumns="False"
                            DataKeyNames="ID_HOJA"
                            EmptyDataText="No hay hojas clínicas registradas."
                            OnSelectedIndexChanged="gvHojasClinicas_SelectedIndexChanged">

                            <SelectedRowStyle BackColor="#F6E2B3" />

                            <Columns>
                                <asp:CommandField
                                    ShowSelectButton="True"
                                    SelectText="Seleccionar"
                                    CausesValidation="False"
                                    ControlStyle-CssClass="boton boton-secundario" />

                                <asp:BoundField
                                    DataField="ID_HOJA"
                                    HeaderText="ID atención" />

                                <asp:BoundField
                                    DataField="FECHA_ATENCION"
                                    HeaderText="Fecha de atención"
                                    DataFormatString="{0:dd/MM/yyyy}" />

                                <asp:BoundField
                                    DataField="ID_MASCOTA"
                                    HeaderText="ID mascota" />

                                <asp:BoundField
                                    DataField="SINTOMAS"
                                    HeaderText="Síntomas" />

                                <asp:BoundField
                                    DataField="DIAGNOSTICO"
                                    HeaderText="Diagnóstico" />

                                <asp:BoundField
                                    DataField="TRATAMIENTO"
                                    HeaderText="Tratamiento" />

                                <asp:BoundField
                                    DataField="ADICIONADO_POR"
                                    HeaderText="Adicionado por" />

                                <asp:BoundField
                                    DataField="FECHA_ADICION"
                                    HeaderText="Fecha de adición"
                                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                                <asp:BoundField
                                    DataField="MODIFICADO_POR"
                                    HeaderText="Modificado por" />

                                <asp:BoundField
                                    DataField="FECHA_MODIFICACION"
                                    HeaderText="Fecha de modificación"
                                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </section>

            </main>
        </div>
    </form>
</body>
</html>