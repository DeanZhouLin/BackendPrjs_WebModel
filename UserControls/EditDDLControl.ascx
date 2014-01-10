<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditDDLControl.ascx.cs"
    Inherits="Jufine.Backend.WebModel.UserControls.EditDDLControl" %>
<div style="position: relative">
    <div style="position: absolute; bottom: 0;">
        <asp:TextBox ID="txtEdit" runat="server" OnTextChanged="txtEdit_TextChanged" AutoPostBack="true"
            Height="18" Width="208"></asp:TextBox>
    </div>
 <asp:DropDownList data-placeholder="请选择" ID="ddl" runat="server" DataValueField="ID" DataTextField="ProductName"
        AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" Width="302"
        Height="22">
    </asp:DropDownList>
</div>
