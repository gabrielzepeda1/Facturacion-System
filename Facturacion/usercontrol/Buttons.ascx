<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Buttons.ascx.vb" Inherits="usercontrol_Buttons" %>

    <div class="row mx-1 py-2 align-items-center justify-content-between">
        <div class="col-8 d-flex">
            <div class="">
                <button id="btnNuevo" type="button" class="btn btn-success" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                    <i class="fas fa-plus-circle"></i>&nbspNuevo</button>
                <asp:LinkButton ID="btnExportar" runat="server" CssClass="btn btn-secondary" ToolTip="Exportar"><i class="fas fa-file-excel"></i> Exportar</asp:LinkButton>
            </div>
        </div>

        <div class="col-4 d-flex">
            <div class="input-group">
                <div>
                    <asp:TextBox ID="txtSearch" CssClass="form-control" runat="server"></asp:TextBox>
                    <label class="form-label">Buscar</label>
                </div>
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" ToolTip="Buscar">
                     <i class="fas fa-search"></i>
                </asp:LinkButton>
            </div>
        </div>
    </div>

