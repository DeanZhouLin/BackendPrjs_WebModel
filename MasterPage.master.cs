using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Com.BaseLibrary.Common.Security;
using Com.BaseLibrary.Utility;

namespace Jufine.Backend.WebModel
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected PageBase CurrentPage;

        protected string WebsiteUrl
        {
            get
            {
                return PageBase.WebsiteUrl;

            }
        }

        protected string LoginUrl
        {
            get
            {
                return "/Security/Login.aspx";
            }
        }

        protected string LogoutUrl
        {
            get
            {
                return "/Security/Login.aspx?op=logout";
            }
        }
        protected string LogoutNoAuthUrl
        {
            get
            {
                return "/Security/Login.aspx?op=noauth";
            }
        }
        protected string ModifyPassword
        {
            get
            {
                return "/Security/ModifyPassword.aspx";
            }
        }

        protected string UserName
        {
            get
            {
                if (CurrentPage.CurrentUser.IsLogin)
                {
                    return CurrentPage.CurrentUser.UserName;
                }
                else
                {
                    return "";
                }
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CurrentPage = Page as PageBase;
        }


        protected override void OnPreRender(EventArgs e)
        {

            if (CurrentPage.CurrentUser.IsLogin)
            {
                phHasLogin.Visible = true;
                phNonLogin.Visible = false;
            }

            base.OnPreRender(e);
        }


        public List<Resource> RootResourceList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMerchantList();
                BindMenu();
            }
            CheckMerchantID();
            if (CurrentPage.CurrentResourcePage != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("商城后台管理系统");
                if (CurrentPage.CurrentRootResource != null && CurrentPage.CurrentRootResource.Status == 1)
                {
                    sb.AppendFormat("-{0}-{1}", CurrentPage.CurrentRootResource.DisplayName, CurrentPage.CurrentResourcePage.DisplayName);
                }
                else
                {
                    sb.AppendFormat("-{0}", CurrentPage.CurrentResourcePage.DisplayName);
                }

                this.Page.Title = sb.ToString();
            }
        }

        private void CheckMerchantID()
        {
            int merchantID = Converter.ToInt32(ddlUserMerchantList.SelectedValue, 0);
            if (CurrentPage.CurrentUser.CurrentMerchantID != merchantID)
            {
                if (CurrentPage.CurrentUser.MerchantList == null || CurrentPage.CurrentUser.MerchantList.Count == 0)
                {
                    CurrentPage.Response.Redirect(this.LogoutNoAuthUrl);
                }
                if (CurrentPage.CurrentUser.MerchantList.Select(c => c.MerchantID).Contains(merchantID))
                {
                    CurrentPage.CurrentUser.CurrentMerchantID = Converter.ToInt32(ddlUserMerchantList.SelectedValue, 0);
                }
                else
                {
                    CurrentPage.CurrentUser.CurrentMerchantID = CurrentPage.CurrentUser.MerchantID;
                    //CurrentPage.ShowMessageBox("当前用户没有权限操作此商家，已跳转至用户默认商家");
                }
                CurrentPage.Response.Redirect(this.Request.Url.AbsolutePath);
            }
        }

        private void BindMerchantList()
        {
            List<UserMerchant> merchantList = CurrentPage.CurrentUser.MerchantList;
            ddlUserMerchantList.DataSource = merchantList;
            ddlUserMerchantList.DataBind();
            ddlUserMerchantList.SelectedValue = CurrentPage.CurrentUser.CurrentMerchantID.ToString();
        }


        protected void ddlUserMerchantList_SelectedIndexChanged(object sender, EventArgs e)
        {

            //BindMenu();
        }
        private void BindMenu()
        {
            //CurrentPage.CurrentUser.CurrentMerchantID = CurrentPage.CurrentUser.MerchantID;
            if (RootResourceList == null)
            {
                RootResourceList = CurrentPage.ResourceList.FindAll(c => c.ParentID == 0 && c.Status == 1 && c.ShowInMenu == 1);
                RootResourceList = RootResourceList.OrderBy(c => c.DisplayOrder).ToList();
            }
            if (CurrentPage.CurrentRootResource != null)
            {
                rptNavigationResource.DataSource = CurrentPage.CurrentRootResource.SubResourceList.Where(c => c.ShowInMenu == 1 && c.ResourceType == 1).OrderBy(c => c.DisplayOrder).ToList();
                rptNavigationResource.DataBind();
            }
            rptRootResource.DataSource = RootResourceList;
            rptRootResource.DataBind();
        }
        public string GetNavCss(object resourceID)
        {
            return ((int)resourceID) == CurrentPage.CurrentResourcePage.ID ? "select" : "";
        }

        public string GetRootCss(object resourceID)
        {
            if (CurrentPage.CurrentRootResource != null)
            {
                return ((int)resourceID) == CurrentPage.CurrentRootResource.ID ? "curr" : "";
            }
            return string.Empty;

        }
        public string GetRootCss1(object resourceID)
        {
            if (CurrentPage.CurrentRootResource != null)
            {
                return ((int)resourceID) == CurrentPage.CurrentRootResource.ID ? "composetab_sel" : "composetab_unsel";
            }
            return string.Empty;
        }

        public string BuildStaticResourceUrl(string resouceName)
        {
            return Request.ApplicationPath + "/MasterPageDir/" + resouceName;
        }

    }
}
