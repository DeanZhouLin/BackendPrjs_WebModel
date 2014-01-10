using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Com.BaseLibrary.Utility;
using System.IO;

namespace Jufine.Backend.WebModel
{
    public partial class UCUploadFile : UserControlBase
    {

        public string FileUploadPathAppSettingKey { get; set; }
        public UploadFileSettingInfo UploadFileSetting { get; set; }

        public string ImageName
        {
            get { return hdfFileName.Value; }
            set { hdfFileName.Value = value; }
        }

        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }

        public string FileNamePrefix
        {
            get { return hfFileNamePrefix.Value; }
            set { hfFileNamePrefix.Value = value; }
        }


        private string m_FullFileName;
        public string FullFileName
        {
            get
            {
                if (string.IsNullOrEmpty(m_FullFileName))
                {
                    m_FullFileName = Path.Combine(UploadFileSetting.UploadFileItemList[0].UploadFilePath, ImageName);
                }
                return m_FullFileName;
            }
        }
        /// <summary>
        /// 是否隐藏内嵌IFrame的边框
        /// </summary>
        public bool HideFrameBorder { get; set; }


        protected string UploadFilePageUrl { get; set; }
        public Unit FrameHeight { get; set; }
        public Unit FrameWidth { get; set; }
        public string ErrorMessage { get; set; }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FrameWidth = FrameWidth == Unit.Empty ? 300 : FrameWidth;
            if (FrameWidth.Value < 300)
            {
                FrameWidth = 300;
            }

            if (string.IsNullOrEmpty(FileUploadPathAppSettingKey))
            {
                ErrorMessage = "上传路径配置名称尚未制定。";
                FrameHeight = FrameHeight == Unit.Empty ? 50 : FrameHeight; ;
            }
            else
            {
                UploadFileSetting = WebsiteConfiguration.Current.UploadFileSettingList.Find(c =>
                   c.Name.ToUpper() == FileUploadPathAppSettingKey.ToUpper());
                if (UploadFileSetting == null)
                {
                    ErrorMessage = "上传路径信息尚未在WebSite.Config文件中配置。";
                    FrameHeight = FrameHeight == Unit.Empty ? 50 : FrameHeight; ;
                }
                else
                {
                    if (FrameHeight == Unit.Empty || FrameHeight.Value < 40)
                    {
                        FrameHeight = CalculateFrameHeight();
                    }

                }
            }
        }

        private int CalculateFrameHeight()
        {
            int maxHeight = 300;

            int minHeight = 40;

            int frameHeight = 26;

            if (UploadFileSetting.IsUploadImage && !StringUtil.IsNullOrEmpty(UploadFileSetting.IsUploadImage))
            {
                if (UploadFileSetting.ImageHeight > 0)
                {
                    frameHeight = Math.Min(maxHeight, UploadFileSetting.ImageHeight + minHeight);
                }
                else if (UploadFileSetting.ImageWidth > 0)
                {
                    frameHeight = Math.Min(maxHeight, UploadFileSetting.ImageWidth + minHeight);
                }

                frameHeight = Math.Max(90, frameHeight);
            }
            return frameHeight;
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            phUploadFile.Visible = StringUtil.IsNullOrEmpty(ErrorMessage);
            phErrorMessage.Visible = !phUploadFile.Visible;
            UploadFilePageUrl = BuildUploadFilePageUrl();
        }

        private string BuildUploadFilePageUrl()
        {
            string uploadFilePageUrl = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("iup={0}", FileUploadPathAppSettingKey);
            if (!string.IsNullOrEmpty(ImageName))
            {
                sb.AppendFormat("&img={0}", ImageName);
            }

            if (!string.IsNullOrEmpty(FileNamePrefix))
            {
                sb.AppendFormat("&fnp={0}", FileNamePrefix);
            }

            sb.AppendFormat("&fh={0}", FrameHeight);
            sb.AppendFormat("&fw={0}", FrameWidth);

            sb.AppendFormat("&ih={0}", ImageHeight);
            sb.AppendFormat("&iw={0}", ImageWidth);
            //只需要触发事件的时候，才传递触发事件的空间ID给IFrame里面的页面
            if (FileUploaded != null)
            {
                sb.AppendFormat("&btnId={0}", btnUploadedFile.ClientID);
            }
            sb.AppendFormat("&hdfId={0}", hdfFileName.ClientID);
            sb.AppendFormat("&r={0}", DateTime.Now.ToString("MMddHHmmssfff")); 
            uploadFilePageUrl = string.Format("{0}/MasterPageDir/UserControls/UploadFile.aspx?{1}", Request.ApplicationPath, sb.ToString());
            return uploadFilePageUrl;
        }

        public event EventHandler FileUploaded;
        protected void btnUploadedFile_Click(object sender, EventArgs e)
        {
            if (FileUploaded != null)
            {
                FileUploaded(sender, e);
            }

        }



    }
}