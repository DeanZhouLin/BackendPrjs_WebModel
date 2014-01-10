using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;
using Com.BaseLibrary.Web;
using Com.BaseLibrary.Utility;


namespace Jufine.Backend.WebModel
{
    public partial class UploadFile : PageBase
    {
        public override bool IsNeedMasterPage
        {
            get
            {
                return false;
            }
        }

        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }

        private string m_FullFileName;
        public string FullFileName
        {
            get
            {
                return m_FullFileName;
            }
        }

        public override bool IsNeedLogin
        {
            get
            {
                return false;
            }
        }

        public override bool IsNeedAuth
        {
            get
            {
                return false;
            }
        }

        public string BtnUploadedImageClientID { get; set; }

        public string HdfFileNameClientID { get; set; }

        public string FileName { get { return hdfFileName1.Value; } set { hdfFileName1.Value = value; } }
        public string FileNamePrefix { get { return hdFileNamePrefix.Value; } set { hdFileNamePrefix.Value = value; } }

        protected string ErrorMessage { get; set; }
        protected string InformationMessage { get; set; }

        public Unit FrameHeight { get; set; }
        public Unit FrameWidth { get; set; }

        protected UploadFileSettingInfo UploadFileSetting { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                FileName = Request.QueryString["img"];
                FileNamePrefix = Request.QueryString["fnp"];
            }

            UploadFileSetting = WebsiteConfiguration.Current.UploadFileSettingList.Find(c =>
                 c.Name.ToUpper() == QueryStringManager.GetValue("iup").ToUpper());


            BtnUploadedImageClientID = QueryStringManager.GetValue("btnId");
            HdfFileNameClientID = QueryStringManager.GetValue("hdfId");

            FrameHeight = Unit.Parse(QueryStringManager.GetValue("fh"));
            FrameWidth = Unit.Parse(QueryStringManager.GetValue("fw"));

            ImageHeight = Converter.ToInt32(QueryStringManager.GetValue("ih"), 0);
            ImageWidth = Converter.ToInt32(QueryStringManager.GetValue("iw"), 0);

            if (ImageHeight > 0 && ImageWidth > 0)
            {
                UploadFileSetting.ImageWidth = ImageWidth;
                UploadFileSetting.ImageHeight = ImageHeight;
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!StringUtil.IsNullOrEmpty(UploadFileSetting.ButtonText))
            {
                btnUpload.Text = UploadFileSetting.ButtonText;
            }

            if (UploadFileSetting != null)
            {
                hlFileName.NavigateUrl = UploadFileSetting.ImageUrlPath + FileName + "?r=" + DateTime.Now.ToString("yyMMddHHmmss");
                trImageDisplay.Visible = UploadFileSetting.IsUploadImage && !string.IsNullOrEmpty(UploadFileSetting.ImageUrlPath);
                if (UploadFileSetting.IsUploadImage && !string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(UploadFileSetting.ImageUrlPath))
                {

                    this.Image1.ImageUrl = UploadFileSetting.ImageUrlPath + FileName + "?r=" + DateTime.Now.ToString("yyMMddHHmmss");
                }
                SetImageDisplaySize();
                hlFileName.Visible = !UploadFileSetting.IsUploadImage;
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ShowMessageBox(ErrorMessage);
            }
            else if (!string.IsNullOrEmpty(InformationMessage))
            {
                if (!UploadFileSetting.NotShowOKMessage)
                {
                    ShowMessageBox(InformationMessage);
                }
                ExecuteJavascript("uploadedImage();");
            }

        }

        public int ImageMaxHeight { get; set; }

        private void SetImageDisplaySize()
        {
            if (!trImageDisplay.Visible)
            {
                return;
            }


            double maxWidth = FrameWidth.Value;
            double maxHeight = FrameHeight.Value - 30;
            if (UploadFileSetting.ImageHeight > 0 && UploadFileSetting.ImageWidth > 0)
            {
                double imageHeight = Math.Min(maxHeight, UploadFileSetting.ImageHeight);
                Image1.Height = Convert.ToInt32(imageHeight);
                Image1.Width = Convert.ToInt32((imageHeight / UploadFileSetting.ImageHeight) * UploadFileSetting.ImageWidth);
                ImageMaxHeight = Convert.ToInt32(imageHeight);
            }
            else if (UploadFileSetting.ImageHeight > 0)
            {
                double imageHeight = Math.Min(maxHeight, UploadFileSetting.ImageHeight);
                ImageMaxHeight = Convert.ToInt32(imageHeight);
                if (UploadImage == null)
                {
                    if (string.IsNullOrEmpty(Image1.ImageUrl))
                    {
                        Image1.Height = Convert.ToInt32(imageHeight);
                    }
                    else
                    {
                        if (Image1.Height == Unit.Empty)
                        {
                            Image1.Height = Convert.ToInt32(imageHeight);
                        }
                    }
                }
                else
                {
                    imageHeight = Math.Min(imageHeight, UploadImage.Height);
                    double imageWidth = (imageHeight * UploadImage.Width) / UploadImage.Height;

                    if (imageWidth < maxWidth)
                    {
                        Image1.Width = Convert.ToInt32(imageWidth);
                        Image1.Height = Convert.ToInt32(imageHeight);
                    }
                    else
                    {
                        Image1.Width = Convert.ToInt32(maxWidth);
                        Image1.Height = Convert.ToInt32((maxWidth * UploadImage.Height) / UploadImage.Width);
                    }
                }

            }
            else if (UploadFileSetting.ImageWidth > 0)
            {
                double imageWidth = Math.Min(maxWidth, UploadFileSetting.ImageWidth);
                ImageMaxHeight = Convert.ToInt32(imageWidth);
                if (UploadImage == null)
                {
                    if (string.IsNullOrEmpty(Image1.ImageUrl))
                    {
                        Image1.Height = Convert.ToInt32(imageWidth);
                    }
                    else
                    {
                        if (Image1.Height == Unit.Empty)
                        {
                            Image1.Height = Convert.ToInt32(imageWidth);
                        }
                    }
                }
                else
                {
                    imageWidth = Math.Min(imageWidth, UploadImage.Width);
                    double imageHeight = (imageWidth * UploadImage.Height) / UploadImage.Width;
                    if (imageHeight < ImageMaxHeight)
                    {
                        Image1.Width = Convert.ToInt32(imageWidth);
                        Image1.Height = Convert.ToInt32(imageHeight);
                    }
                    else
                    {
                        Image1.Height = Convert.ToInt32(maxHeight);
                        Image1.Width = Convert.ToInt32((maxHeight * UploadImage.Width) / UploadImage.Height);
                    }
                }
            }
            else
            {
                double imageHeight = 60;
                ImageMaxHeight = Convert.ToInt32(imageHeight);
                if (UploadImage == null)
                {
                    if (string.IsNullOrEmpty(Image1.ImageUrl))
                    {
                        Image1.Height = Convert.ToInt32(imageHeight);
                    }
                    else
                    {
                        if (Image1.Height == Unit.Empty)
                        {
                            Image1.Height = Convert.ToInt32(imageHeight);
                        }
                    }
                }
                else
                {
                    imageHeight = Math.Min(imageHeight, UploadImage.Height);
                    double imageWidth = (imageHeight * UploadImage.Width) / UploadImage.Height;

                    if (imageWidth < maxWidth)
                    {
                        Image1.Width = Convert.ToInt32(imageWidth);
                        Image1.Height = Convert.ToInt32(imageHeight);
                    }
                    else
                    {
                        Image1.Width = Convert.ToInt32(maxWidth);
                        Image1.Height = Convert.ToInt32((maxWidth * UploadImage.Height) / UploadImage.Width);
                    }
                }
            }
        }


        System.Drawing.Image UploadImage;
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(flu.FileName))
            {
                this.ErrorMessage = "请选择文件进行上传。";
                return;
            }

            if (!StringUtil.IsNullOrEmpty(UploadFileSetting.FileExtentions))
            {
                List<string> fileExtentions = UploadFileSetting.FileExtentions.Split('|').ToList();
                fileExtentions.RemoveAll(c => StringUtil.IsNullOrEmpty(c));
                string fileExtenstion = flu.FileName.Substring(flu.FileName.LastIndexOf('.')).TrimStart('.').ToUpper();
                if (!fileExtentions.Exists(c => c.ToUpper() == fileExtenstion))
                {
                    this.ErrorMessage = string.Format("上传格式必须为:{0}。", UploadFileSetting.FileExtentions);
                    return;
                }
            }
            if (UploadFileSetting.IsUploadImage)
            {
                UploadImage = GetImageFromStream();
                if (UploadImage == null)
                {
                    this.ErrorMessage = "上传文件无法转换成图片，请确认是否是图片文件。";
                    return;

                }
                if (UploadFileSetting.ImageWidth > 0 && UploadFileSetting.ImageHeight > 0)
                {
                    if (UploadImage.Width != UploadFileSetting.ImageWidth || UploadImage.Height != UploadFileSetting.ImageHeight)
                    {
                        this.ErrorMessage = string.Format("上传图片尺寸必须是{0} X {1}。",
                            UploadFileSetting.ImageWidth,
                            UploadFileSetting.ImageHeight);
                        return;

                    }
                }
                else if (UploadFileSetting.ImageWidth > 0)
                {
                    if (UploadImage.Width != UploadFileSetting.ImageWidth)
                    {
                        this.ErrorMessage = string.Format("上传图片长度必须是{0}。",
                            UploadFileSetting.ImageWidth);
                        return;

                    }
                }
                else if (UploadFileSetting.ImageHeight > 0)
                {
                    if (UploadImage.Height != UploadFileSetting.ImageHeight)
                    {
                        this.ErrorMessage = string.Format("上传图片高度必须是{0}。",
                            UploadFileSetting.ImageHeight);
                        return;

                    }
                }
            }

            try
            {
                string imageName = GetFileName();
                foreach (var item in UploadFileSetting.UploadFileItemList)
                {
                    if (item.IsLocalPath)
                    {
                        SaveImage(item.UploadFilePath, imageName);
                    }
                    else
                    {
                        using (WindowsImpersonation impersonation = new WindowsImpersonation(item.UserName, item.Password, item.Domain))
                        {
                            SaveImage(item.UploadFilePath, imageName);
                        }
                    }
                }
                FileName = imageName;
                this.InformationMessage = string.Format("上传文件{0}成功。", flu.FileName);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        private System.Drawing.Image GetImageFromStream()
        {
            try
            {
                System.Drawing.Image image = System.Drawing.Bitmap.FromStream(flu.FileContent);
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        ///当FileName为空的时候，需要生成一个新的文件名称，且名称在个目标复制路径均不存在同名文件
        ///如果FileName不为空，则表示需要覆盖该文件。
        /// </summary>
        /// <returns>File Name</returns>
        private string GetFileName()
        {
            string oldFileName = FileName;

            string fileExtenstion = flu.FileName.Substring(flu.FileName.LastIndexOf('.'));
            if (!StringUtil.IsNullOrEmpty(oldFileName))
            {
                oldFileName = flu.FileName.Substring(0, flu.FileName.LastIndexOf('.'));
            }

            string newFileName = string.Empty;
            if (string.IsNullOrEmpty(FileNamePrefix))
            {
                newFileName = oldFileName;

                if (StringUtil.IsNullOrEmpty(oldFileName))
                {
                    newFileName = DateTime.Now.ToString("yyMMddHHmmssfff");
                }
            }
            else
            {
                oldFileName = FileNamePrefix;
                newFileName = FileNamePrefix + DateTime.Now.ToString("yyMMddHHmmssfff");
            }

            foreach (var item in UploadFileSetting.UploadFileItemList)
            {
                string fullImageName = Path.Combine(item.UploadFilePath, newFileName + fileExtenstion);
                while (File.Exists(fullImageName))
                {
                    if (StringUtil.IsNullOrEmpty(oldFileName))
                    {
                        newFileName = DateTime.Now.ToString("yyMMddHHmmssfff");
                        fullImageName = Path.Combine(item.UploadFilePath, newFileName);

                    }
                    else
                    {
                        newFileName = oldFileName + DateTime.Now.ToString("yyMMddHHmmssfff");
                        fullImageName = Path.Combine(item.UploadFilePath, newFileName);
                    }
                }


            }

            return newFileName + fileExtenstion;
        }

        private void SaveImage(string path, string imageName)
        {
            string fullFileName = Path.Combine(path, imageName);
            while (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
                // todo mason
            }
            flu.SaveAs(fullFileName);
            FileName = imageName;
            m_FullFileName = fullFileName;
            // hlFileName.NavigateUrl = UploadFileSetting.ImageUrlPath + FileName + "?r=" + DateTime.Now.ToString("yyMMddHHmmss"); 
        }

    }

}
