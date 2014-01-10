<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="UploadFile.aspx.cs" Inherits="Jufine.Backend.WebModel.UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="pragma" content="no-cache">
    <meta http-equiv="cache-control" content="no-cache">
    <meta http-equiv="expires" content="0">
    <title></title>
    <style>
        .warp
        {
            word-break: break-all;
            font-size: 14px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function uploadedImage() {
          
            window.parent.document.getElementById("<%=this.HdfFileNameClientID %>").value = "<%=this.FileName %>";
            var btdId = "<%=this.BtnUploadedImageClientID %>";
            if (btdId != null && btdId.length > 0) {
                window.parent.document.getElementById(btdId).click();
            }
        }
//        window.parent.document.all("ifmUpload").style.height = document.body.scrollHeight;
//        window.parent.document.all("ifmUpload").style.width = document.body.scrollWidth;
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hdfFileName1" runat="server" />
    <asp:HiddenField ID="hdFileNamePrefix" runat="server" />
    <asp:HyperLink ID="hlFileName" Target="_blank" runat="server"><%=FileName %></asp:HyperLink>
    <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;" width="<%=FrameWidth%>">
        <tr runat="server" id="trImageDisplay">
            <td align="center" style="vertical-align: middle; padding: 2px;" valign="bottom">
                <%--<img width="1" height='<%=Convert.ToInt32(FrameHeight.Value)-40%>px' style="float: left;" />--%>
                <a href="<%=this.Image1.ImageUrl %>" title="点击看大图" target="_blank">
                    <asp:Image ID="Image1" runat="server" /></a>
            </td>
        </tr>
        <tr runat="server" id="trUploadArea">
            <td align="left" style="vertical-align: top; padding: 2px;" valign="top">
                <asp:FileUpload ID="flu" runat="server" /><asp:Button ID="btnUpload" runat="server"
                    Text="上传" OnClick="btnUpload_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <div class="warp">
                    (限以下文件类型：<%=UploadFileSetting.FileExtentions%>)</div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
