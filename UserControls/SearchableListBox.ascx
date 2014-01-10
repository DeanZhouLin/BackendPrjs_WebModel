<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchableListBox.ascx.cs"
    Inherits="Jufine.Backend.WebModel.UserControls.SearchableListBox" %>
<div class="searchablelistbox" style="position:relative">
    <asp:HiddenField ID="hfValue" runat="server" />
    <asp:HiddenField ID="hfText" runat="server" />
    <asp:TextBox ID="txtText" runat="server" Width="250" Text=""></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="" OnClick="btnSearch_Click" Style="width: 24px;
        padding: 0px 1px 0px 1px; height: 24px;" CssClass="btnSearch" />
    <asp:UpdatePanel ID="upList" class="listpanel" runat="server" UpdateMode="Conditional"
        ChildrenAsTriggers="true" Style="position: absolute; width: 250px; height: 350px;
        z-index: 100; top:24px; left: 0px; display: none">
        <ContentTemplate>
            <asp:ListBox ID="lbItem" runat="server" Height="350px" Width="250px" OnSelectedIndexChanged="lbItem_SelectedIndexChanged">
            </asp:ListBox>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            searchableListBox({
                txtTextId: '#<%=txtText.ClientID %>',
                btnSearchId: '#<%=btnSearch.ClientID %>',
                upListId: '#<%=upList.ClientID %>',
                listBoxId: '#<%=lbItem.ClientID %>',
                hfValueId: '#<%=hfValue.ClientID %>',
                hfTextId: '#<%=hfText.ClientID %>'
            });
        });

        //        function InitSearchableListBox() {
        //            searchableListBox({
        //                txtTextId: '#<%=txtText.ClientID %>',
        //                btnSearchId: '#<%=btnSearch.ClientID %>',
        //                upListId: '#<%=upList.ClientID %>',
        //                listBoxId: '#<%=lbItem.ClientID %>',
        //                hfValueId: '#<%=hfValue.ClientID %>',
        //                hfTextId: '#<%=hfText.ClientID %>'
        //            });
        //        }
    
    </script>
</div>
