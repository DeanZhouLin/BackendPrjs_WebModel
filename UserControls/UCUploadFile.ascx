<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="UCUploadFile.ascx.cs" Inherits="Jufine.Backend.WebModel.UCUploadFile" %>
<asp:PlaceHolder runat="server" ID="phUploadFile">
	<iframe id="ifmUpload" src="<%=this.UploadFilePageUrl %>" scrolling="no" frameborder="<%=HideFrameBorder?0:1 %>" height="<%=FrameHeight %>" marginheight="0" marginwidth="0" width="<%=FrameWidth %>" scrolling="auto">
	</iframe>
	<asp:HiddenField ID="hdfFileName" runat="server" />
    <asp:HiddenField ID="hfFileNamePrefix" runat="server" />
	<asp:Button ID="btnUploadedFile" runat="server" Text="" Style="display: none" OnClick="btnUploadedFile_Click" />
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="phErrorMessage">
	<%=this.ErrorMessage %></asp:PlaceHolder>
