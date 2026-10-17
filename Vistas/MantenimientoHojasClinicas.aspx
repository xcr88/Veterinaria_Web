<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="MantenimientoHojasClinicas.aspx.cs"
    Inherits="Veterinaria_Web.Vistas.MantenimientoHojasClinicas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Mantenimiento de hojas clínicas</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Mantenimiento de hojas clínicas</h1>

            <asp:Label
                ID="lblUsuario"
                runat="server" />

            <br /><br />

            <asp:HyperLink
                ID="lnkPropietarios"
                runat="server"
                Text="Ir a propietarios"
                NavigateUrl="~/Vistas/MantenimientoPropietarios.aspx" />

            &nbsp;|&nbsp;

            <asp:HyperLink
                ID="lnkMascotas"
                runat="server"
                Text="Ir a mascotas"
                NavigateUrl="~/Vistas/MantenimientoMascotas.aspx" />

            &nbsp;|&nbsp;

            <asp:HyperLink
                ID="lnkCerrarSesion"
                runat="server"
                Text="Cerrar sesión"
                NavigateUrl="~/Vistas/CerrarSesion.aspx" />

            <br /><br />

            <asp:Label
                ID="lblMensaje"
                runat="server" />

            <h2>Atenciones registradas</h2>

            <asp:GridView
                ID="gvHojasClinicas"
                runat="server"
                AutoGenerateColumns="False"
                DataKeyNames="ID_HOJA"
                EmptyDataText="No hay hojas clínicas registradas.">

                <Columns>
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
    </form>
</body>
</html>