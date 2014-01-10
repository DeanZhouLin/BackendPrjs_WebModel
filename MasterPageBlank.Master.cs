﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using Com.BaseLibrary.Contract;
using System.Text;
using Com.BaseLibrary.Common.Security;
using Com.BaseLibrary.Utility;

namespace Jufine.Backend.WebModel
{
    public partial class MasterPageBlank : System.Web.UI.MasterPage
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


    


        public List<Resource> RootResourceList;
      
       

       


        
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