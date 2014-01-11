using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.BaseLibrary.Contract;
using System.Web.UI;
using Jufine.Backend.WebModel.Request;
using System.IO;
using Com.BaseLibrary.Utility;
using System.Web.UI.WebControls;
using Com.BaseLibrary.Entity;
using System.Reflection;
using Com.BaseLibrary.Common.Security;
using System.Web;
using Com.BaseLibrary.Logging;
//using JuFine.Backend.ServiceContracts;
//using Com.Backend.Configuration;

namespace Jufine.Backend.WebModel
{
    public class PageBase : Page
    {
        /// <summary>
        /// 标识页面是否需要登录
        /// </summary>
        public virtual bool IsNeedLogin
        {
            get { return true; }
        }
        /// <summary>
        /// 标识页面是否需要鉴权
        /// </summary>
        public virtual bool IsNeedAuth
        {
            get { return true; }
        }

        public virtual bool IsNeedMasterPage
        {
            get { return true; }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (IsNeedMasterPage)
            {
                MasterPageFile = MasterPageVirtualPathProvider.MasterPageFileLocation;
            }
            //else
            //{
            //    // todo:mason
            //    //   MasterPageFile = MasterPageVirtualPathProvider.MasterPageBlankFileLocation;
            //}
            CurrentScriptManager = IsNeedMasterPage ? (Master.FindControl("ToolkitScriptManager1") as ScriptManager) : null;
            PageUrl = Request.Url.OriginalString;
            QueryStringManager = new QueryStringManager(Request);
            FormManager = new FormManager(Request);
            CurrentUser = UserFactory.CreateUser();
        }

        public string BuildBrandImageUrl(object imageName)
        {
            return GetUploadFullFileName(imageName, "BrandImageUploadPath");
        }

        public string BuildProductImageUrl(object imageName)
        {
            return GetUploadFullFileName(imageName, "ProductInfoImageUploadPath");
        }

        public string BuildItemImageUrl(object imageName)
        {
            return GetUploadFullFileName(imageName, "ItemInfoImageUploadPath");
        }

        public string BuildItemImageFilePath(object imageName)
        {
            return GetServerUploadFile(imageName, "ItemInfoImageUploadPath");
        }

        public string BuildTuanImageUrl(object imageName)
        {
            return GetUploadFullFileName(imageName, "TuanItemImageUploadPath");
        }

        public string BuildTuanImageFilePath(object imageName)
        {
            return GetServerUploadFile(imageName, "TuanItemImageUploadPath");
        }

        public string BuildCommContentImageUrl(object imageName)
        {
            return GetUploadFullFileName(imageName, "CommContentImageUploadPath");
        }

        public string BuildMerchantImageUrl(object imageName)
        {
            return GetUploadFullFileName(imageName, "MerchantImageUploadPath");
        }

        public string BuildItemStockFileUrl(object imageName)
        {
            return GetUploadFullFileName(imageName, "ItemStockUploadPath");
        }

        public string BuildPageItemImageUrl(object imageName)
        {
            return GetUploadFullFileName(imageName, "PageObjectImageUploadPath");
        }

        public string BuildPageItemImageFilePath(object imageName)
        {
            return GetServerUploadFile(imageName, "PageObjectImageUploadPath");
        }

        public string BuildEDMObjectImageUrl(object imageName)
        {
            return GetUploadFullFileName(imageName, "EDMObjectImageUploadPath");
        }

        public string BuildUrl(object url)
        {
            if (StringUtil.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            if (!url.ToString().Contains("http://"))
            {
                return "http://" + url;
            }
            return url.ToString();
        }

        public string BuildStaticResourceUrl(string resouceName)
        {
            return Request.ApplicationPath + "/MasterPageDir/" + resouceName;
        }
        private static string GetUploadFullFileName(object imageName, string uploadFileSettingConfig)
        {
            string fileName = "noimage.jpg";
            if (imageName != null && !string.IsNullOrEmpty(imageName.ToString()))
            {
                fileName = imageName.ToString();
            }
            string directoryPath = WebsiteConfiguration.Current.UploadFileSettingList.Find(c =>
                   c.Name.ToUpper() == uploadFileSettingConfig.ToUpper()).ImageUrlPath;

            return directoryPath.TrimEnd('/') + "/" + fileName;
        }

        protected static string GetServerUploadFile(object imageName, string uploadFileSettingConfig)
        {
            string fileName = "noimage.jpg";
            if (imageName != null && !string.IsNullOrEmpty(imageName.ToString()))
            {
                fileName = imageName.ToString();
            }
            string directoryPath = WebsiteConfiguration.Current.UploadFileSettingList.Find(c =>
                   c.Name.ToUpper() == uploadFileSettingConfig.ToUpper()).UploadFileItemList[0].UploadFilePath;
            //File.Exists("//backend.qa.Jufine.com/dev/content/pii/120703100336.jpg")
            return directoryPath.Replace('\\', '/') + "/" + fileName;
        }

        public static IUserFactory UserFactory;

        /// <summary>
        /// 网站根路径
        /// e.g. http://localhost/Security/
        /// </summary>
        public static string WebsiteUrl;
        static PageBase()
        {
            WebsiteUrl = VirtualPathUtility.ToAbsolute("~/");
            UserFactory = Assembly.Load("Jufine.Backend.Security.Authentication")
                .CreateInstance("Jufine.Backend.Security.Authentication.UserFactory") as IUserFactory;
        }

        /// <summary>
        /// 当前页面路径
        /// </summary>
        public string PageUrl { get; private set; }
        public QueryStringManager QueryStringManager { get; private set; }
        public FormManager FormManager { get; private set; }
        public IUser CurrentUser { get; private set; }
        public ScriptManager CurrentScriptManager { get; private set; }

        #region QueryString & Form

        #endregion
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);

            CheckPermssion();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            CheckPageControlPermssion();
        }
        /// <summary>
        /// 检查当前用户是否有权限
        /// </summary>
        private void CheckPermssion()
        {

            if (IsNeedLogin && !CurrentUser.IsLogin)
            {
                Response.Redirect("/Security/Login.aspx?backUrl=" + PageUrl);
            }
            if (IsNeedAuth)
            {
                if (!CurrentUser.HasPermssion(Request.Url.LocalPath.ToUpper()))
                {
                    GotoFirstWelcomePage();
                    Response.End();
                }
            }
        }

        public static T CreateService<T>()
            where T : class
        {
            return ServiceFactory.CreateService<T>();
        }


        public void SetFocusControl(string id)
        {
            CurrentScriptManager.SetFocus(id);
        }

        public void SetFocusControl(Control ctl)
        {
            CurrentScriptManager.SetFocus(ctl);
        }

        public void ShowMessageBox(string message)
        {
            message = parseText(message);
            string script = string.Format("alert(\"{0}\")", message);
            ExecuteJavascript(script);
        }

        public void ShowMessageBox(string title, string message)
        {
            message = parseText(message);
            string script = string.Format("alert(\"{0}\\n_______________________________________\\n\\n{1}\")", title, message);
            ExecuteJavascript(script);
        }

        public void ExecuteJavascript(string script)
        {
            if (!script.EndsWith(";"))
                script = script + ";";
            ScriptManager.RegisterClientScriptBlock(this, GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private string parseText(string text)
        {
            StringBuilder sb = new StringBuilder(text);

            sb.Replace("\"", "\\\"");

            sb.Replace("<", "\\<");
            sb.Replace(">", "\\>");

            StringReader sr = new StringReader(sb.ToString());
            StringWriter sw = new StringWriter();

            while (sr.Peek() > -1)
            {

                string temp = sr.ReadLine();

                sw.Write(temp + "\\n");
            }

            return sw.GetStringBuilder().ToString();
        }

        public bool ValidateControl(params WebControl[] controlList)
        {
            string message;
            WebControl wc;
            if (!ValidationHelper.ValidateControl(out message, out wc, controlList))
            {
                ShowMessageBox("提交失败，发生以下错误：", message);
                SetFocus(wc);
                return false;
            }
            return true;
        }

        public virtual bool ValidateControl(WebControl container, bool isShowMsg = true)
        {
            List<WebControl> controlList = new List<WebControl>();

            FindChildrenControls(controlList, container);
            if (controlList.Count == 0)
            {
                controlList.Add(container);
            }
            if (isShowMsg)
            {
                return ValidateControl(controlList.ToArray());
            }
            string message;
            WebControl wc;
            return ValidationHelper.ValidateControl(out message, out wc, controlList.ToArray());
        }
        private void FindChildrenControls(List<WebControl> controlList, Control container)
        {
            foreach (Control control in container.Controls)
            {
                if (control.Controls.Count > 0)
                {
                    FindChildrenControls(controlList, control);
                }
                else if (control is WebControl)
                {
                    WebControl webControl = control as WebControl;
                    if (webControl.Enabled)
                    {
                        string Rel = webControl.Attributes["Rel"];
                        if (!StringUtil.IsNullOrEmpty(Rel))
                        {
                            controlList.Add(webControl);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 清空指定容器说包含的子控件的数据
        /// </summary>
        /// <param name="container"></param>
        public void ClearControlInput(Control container)
        {
            if (container.Controls.Count == 0)
            {
                return;
            }
            foreach (Control item in container.Controls)
            {
                if (item is TextBox && (item as TextBox).Attributes["noclear"] == null)
                {
                    (item as TextBox).Text = null;
                }
                else if (item is DropDownList)
                {
                    DropDownList ddl = item as DropDownList;
                    if (ddl.Items.Count > 0)
                    {
                        ddl.SelectedIndex = 0;
                    }
                }
                else if (item is CheckBox && item.ID != "ckbStatus")
                {
                    (item as CheckBox).Checked = false;
                }
                else if (item is HiddenField)
                {
                    (item as HiddenField).Value = null;
                }
                else if (item.Controls.Count > 0)
                {
                    ClearControlInput(item);
                }
            }
        }
        public void NoRecords<T>(GridView gv) where T : new()
        {
            if (gv.Rows.Count > 0 && gv.Rows[0].Cells[0].Text != "无记录信息")
            {
                return;
            }
            List<T> record = new List<T>();
            T obj = new T();
            record.Add(obj);
            gv.DataSource = record;
            gv.DataBind();
            int columnCount = gv.Rows[0].Cells.Count;
            gv.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = columnCount;
            gv.Rows[0].Cells[0].Text = "无记录信息";
            CheckBox chk = gv.HeaderRow.FindControl("ckbSelectAll") as CheckBox;
            if (chk != null)
            {
                chk.Visible = false;
            }
        }
        public void SetSortOrder<T>(QueryConditionInfo<T> queryCondition, string sortExpression) where T : class, new()
        {
            OrderFiledInfo orderFiled = queryCondition.OrderFileds.Find(c => c.FieldName == sortExpression);
            if (orderFiled == null)
            {
                queryCondition.AddOrder(sortExpression, OrderDirection.ASC);
            }
            else
            {
                queryCondition.AddOrder(sortExpression, orderFiled.OrderDirection == OrderDirection.ASC ?
                    OrderDirection.DESC : OrderDirection.ASC);
            }
        }
        public void SetOrderHeaderStyle<T>(GridView gridView, QueryConditionInfo<T> queryCondition) where T : class, new()
        {
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                var ss = gridView.Columns[i];
                ss.HeaderText = ss.HeaderText.Replace("↑", "").Replace("↓", "");
                if (queryCondition.OrderFileds.Count > 0 && ss.SortExpression == queryCondition.OrderFileds[0].FieldName)
                {
                    string direction = queryCondition.OrderFileds[0].OrderDirection == OrderDirection.ASC ? "↑" : "↓";
                    ss.HeaderText = ss.HeaderText.Replace("↑", "").Replace("↓", "") + direction;
                }
            }
        }

        /// <summary>
        /// 为容器控件添加Enter和Esc按键默认绑定按钮
        /// </summary>
        /// <param name="container">容器控件，如：panel</param>
        /// <param name="okButton">Enter按键绑定按钮</param>
        /// <param name="cancelButton">Esc按键绑定按钮</param>
        public void AddEnterEscPress(WebControl container, WebControl okButton, WebControl cancelButton)
        {
            container.Attributes.Add("onkeypress", string.Format("javascript:return FireDefaultButton(event, '{0}','{1}');",
                okButton.ClientID, cancelButton.ClientID));
        }

        public string ToPriceFormmat(object obj)
        {
            decimal dec = Converter.ToDecimal(obj, 0m);
            return dec.ToString("###0.00");
        }
        public virtual void FillContentsValueWithEntity<T>(T t, Control container)
        {
            List<PropertyInfo> propertyList = EntityTypeManager.GetPropertyList<T>();
            foreach (Control item in container.Controls)
            {
                try
                {
                    if (item is GridView)
                    {
                        continue;
                    }
                    if (item.Controls.Count > 0)
                    {
                        FillContentsValueWithEntity(t, item);
                    }
                    else if (item is TextBox)
                    {
                        TextBox txt = (item as TextBox);
                        //if (txt.ReadOnly || !txt.Enabled)
                        //{
                        //    continue;
                        //}
                        string value = GetPropertyValue(t, propertyList, txt);
                        //if (!string.IsNullOrEmpty(value))
                        //{
                        txt.Text = value;
                        //}
                    }
                    else if (item is Label)
                    {
                        Label txt = (item as Label);
                        //if (txt.ReadOnly || !txt.Enabled)
                        //{
                        //    continue;
                        //}
                        string propertyName = txt.Attributes["PropertyName"];

                        if (string.IsNullOrEmpty(propertyName))
                        {
                            continue;
                        }

                        string value = GetPropertyValue(t, propertyList, txt);
                        //if (!string.IsNullOrEmpty(value))
                        //{
                        txt.Text = value;
                        //}
                    }
                    //else if (item is DropDownList)
                    //{
                    //    DropDownList ddl = item as DropDownList;
                    //    //if (!ddl.Enabled || ddl.Items == null || ddl.Items.Count == 0)
                    //    //{
                    //    //    continue;
                    //    //}
                    //    if (ddl.Items == null || ddl.Items.Count == 0)
                    //    {
                    //        continue;
                    //    }
                    //    string value = GetPropertyValue(t, propertyList, ddl);
                    //    if (!string.IsNullOrEmpty(value))
                    //    {
                    //        ddl.SelectedValue = value;
                    //    }
                    //}
                    else if (item is RadioButtonList)
                    {
                        RadioButtonList rbl = item as RadioButtonList;
                        //if (!rbl.Enabled || rbl.Items == null || rbl.Items.Count == 0)
                        //{
                        //    continue;
                        //}
                        if (rbl.Items.Count == 0)
                        {
                            continue;
                        }
                        string value = GetPropertyValue(t, propertyList, rbl);
                        if (!string.IsNullOrEmpty(value))
                        {
                            rbl.SelectedValue = value;
                        }
                    }
                    else if (item is CheckBox)
                    {
                        CheckBox ckb = (item as CheckBox);
                        //if (!ckb.Enabled)
                        //{
                        //    continue;
                        //}

                        string value = GetPropertyValue(t, propertyList, ckb);
                        if (!string.IsNullOrEmpty(value))
                        {
                            ckb.Checked = (value.Trim() == "1");
                        }
                    }
                }
                catch (Exception e)
                {
                    ShowMessageBox("控件" + item.ID + "引发异常|" + e.Message);
                }
            }

        }
        public virtual void FillContentValueWithEntity<T>(T t, Control container)
        {
            List<PropertyInfo> propertyList = EntityTypeManager.GetPropertyList<T>();
            foreach (Control item in container.Controls)
            {
                try
                {
                    if (item is GridView)
                    {
                        continue;
                    }
                    if (item.Controls.Count > 0)
                    {
                        FillContentValueWithEntity(t, item);
                    }
                    else if (item is TextBox)
                    {
                        TextBox txt = (item as TextBox);
                        //if (txt.ReadOnly || !txt.Enabled)
                        //{
                        //    continue;
                        //}
                        string value = GetPropertyValue(t, propertyList, txt);
                        //if (!string.IsNullOrEmpty(value))
                        //{
                        txt.Text = value;
                        //}
                    }
                    else if (item is Label)
                    {
                        Label txt = (item as Label);
                        //if (txt.ReadOnly || !txt.Enabled)
                        //{
                        //    continue;
                        //}
                        string propertyName = txt.Attributes["PropertyName"];

                        if (string.IsNullOrEmpty(propertyName))
                        {
                            continue;
                        }

                        string value = GetPropertyValue(t, propertyList, txt);
                        //if (!string.IsNullOrEmpty(value))
                        //{
                        txt.Text = value;
                        //}
                    }
                    else if (item is DropDownList)
                    {
                        DropDownList ddl = item as DropDownList;
                        //if (!ddl.Enabled || ddl.Items == null || ddl.Items.Count == 0)
                        //{
                        //    continue;
                        //}
                        if (ddl.Items.Count == 0)
                        {
                            continue;
                        }
                        string value = GetPropertyValue(t, propertyList, ddl);
                        if (!string.IsNullOrEmpty(value))
                        {
                            ddl.SelectedValue = value;
                        }
                    }
                    else if (item is RadioButtonList)
                    {
                        RadioButtonList rbl = item as RadioButtonList;
                        //if (!rbl.Enabled || rbl.Items == null || rbl.Items.Count == 0)
                        //{
                        //    continue;
                        //}
                        if (rbl.Items.Count == 0)
                        {
                            continue;
                        }
                        string value = GetPropertyValue(t, propertyList, rbl);
                        if (!string.IsNullOrEmpty(value))
                        {
                            rbl.SelectedValue = value;
                        }
                    }
                    else if (item is CheckBox)
                    {
                        CheckBox ckb = (item as CheckBox);
                        //if (!ckb.Enabled)
                        //{
                        //    continue;
                        //}

                        string value = GetPropertyValue(t, propertyList, ckb);
                        if (!string.IsNullOrEmpty(value))
                        {
                            ckb.Checked = (value.Trim() == "1");
                        }
                    }
                }
                catch (Exception e)
                {
                    ShowMessageBox("控件" + item.ID + "引发异常|" + e.Message);
                }
            }

        }
        private static string GetPropertyValue(object obj, List<PropertyInfo> propertyList, WebControl control)
        {
            string propertyName = control.Attributes["PropertyName"];

            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            PropertyInfo property = propertyList.Find(c => c.Name == propertyName);
            if (property != null)
            {
                object value = property.GetValue(obj, null);

                if (value != null && !string.IsNullOrEmpty(value.ToString().Trim()))
                {
                    return value.ToString();
                }
            }

            return null;
        }

        public void HandleException(Exception ex)
        {
            Logger.CurrentLogger.DoWrite("Backend", Title, "错误", ex.Message, ex.ToString());
        }

        public virtual void FillEntityWithContentValue<T>(T t, Control container, bool hasExeLabel = false)
        {
            List<PropertyInfo> propertyList = EntityTypeManager.GetPropertyList<T>();
            foreach (Control item in container.Controls)
            {
                if (item is GridView)
                {
                    continue;
                }
                if (item.Controls.Count > 0)
                {
                    FillEntityWithContentValue(t, item);
                }
                else if (item is TextBox)
                {
                    TextBox txt = (item as TextBox);
                    if (txt.ReadOnly || !txt.Enabled)
                    {
                        continue;
                    }
                    string value = string.IsNullOrEmpty(txt.Text) ? string.Empty : txt.Text.Trim();
                    if (txt.TextMode == TextBoxMode.MultiLine && value.Length > txt.MaxLength && txt.MaxLength > 0)
                    {
                        value = value.Substring(0, txt.MaxLength);
                    }
                    FillPropertyValue(t, propertyList, txt, value);
                }
                else if (item is Label)
                {
                    if (hasExeLabel)
                    {
                        Label txt = (item as Label);
                        string value = string.IsNullOrEmpty(txt.Text) ? string.Empty : txt.Text.Trim();
                        FillPropertyValue(t, propertyList, txt, value);
                    }
                }
                else if (item is DropDownList)
                {
                    DropDownList ddl = item as DropDownList;
                    if (!ddl.Enabled || ddl.Items.Count == 0)
                    {
                        continue;
                    }
                    FillPropertyValue(t, propertyList, ddl, ddl.SelectedValue);
                }
                else if (item is RadioButtonList)
                {
                    RadioButtonList rbl = item as RadioButtonList;
                    if (!rbl.Enabled || rbl.Items.Count == 0)
                    {
                        continue;
                    }
                    FillPropertyValue(t, propertyList, rbl, rbl.SelectedValue);
                }
                else if (item is CheckBox)
                {
                    CheckBox ckb = (item as CheckBox);
                    if (!ckb.Enabled)
                    {
                        continue;
                    }
                    FillPropertyValue(t, propertyList, ckb, ckb.Checked ? "1" : "0");
                }

                //else if (item is HiddenField)
                //{
                //    HiddenField hdf = (item as HiddenField);
                //    FillPropertyValue(t, propertyList, hdf.ID, hdf.Value);
                //}
            }
        }
        private static void FillPropertyValue(object obj, List<PropertyInfo> propertyList, WebControl control, string value)
        {
            string propertyName = control.Attributes["PropertyName"];

            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }
            PropertyInfo property = propertyList.Find(c => c.Name == propertyName);
            if (property != null)
            {
                if (string.IsNullOrEmpty(value))
                {
                    property.SetValue(obj, null, null);
                }
                else
                {
                    property.SetValue(obj, StringUtil.ConvertToType(value, property.PropertyType), null);
                }
            }
        }

        protected override void OnError(EventArgs e)
        {
            base.OnError(e);
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                Logger.CurrentLogger.DoWrite(AppDomain.CurrentDomain.FriendlyName, "BackendUI", "Error", ex.Message, ex.ToString());
            }
            if (ex is HttpRequestValidationException)
            {
                ShowMessageBox("请您输入合法字符串");
                Server.ClearError(); // 如果不ClearError()这个异常会继续传到Application_Error()。
            }
        }

        #region Resource

        /// <summary>
        /// 判断控件是否可见
        /// </summary>
        /// <param name="ctlName"></param>
        /// <param name="preAuditUserName"></param>
        /// <returns></returns>
        public bool GetControlVisible(string ctlName, string preAuditUserName)
        {
            return CurrentUser.GetUserAuditWorkFlowResource(ctlName, preAuditUserName, CurrentResourcePage.ID, PageControlResouceList).HasAuth;
        }

        private Resource m_CurrentResourcePage;
        public Resource CurrentResourcePage
        {
            get
            {
                if (m_CurrentResourcePage == null)
                {
                    string url = Request.RawUrl.ToUpper();
                    m_CurrentResourcePage = ResourceList.Find(c => UrlEqual(url, c.ResourceAddress));
                    if (m_CurrentResourcePage == null)
                    {
                        if (IsNeedMasterPage || IsNeedAuth || IsNeedLogin)
                        {
                            GotoFirstWelcomePage();
                        }
                    }
                    //else
                    //{
                    //    Resource firstChildResouce = ResourceList.Find(c => c.ResourceType == 1 && c.ParentID == m_CurrentResourcePage.ID);
                    //    //if (firstChildResouce != null)
                    //    //{
                    //    //    Response.Redirect(firstChildResouce.ResourceAddress);
                    //    //}
                    //}
                }
                return m_CurrentResourcePage;
            }
        }

        private List<Resource> m_PageControlResouceList;

        /// <summary>
        /// 当前页面下用户所具有访问权限的控件
        /// </summary>
        public List<Resource> PageControlResouceList
        {
            get
            {
                if (m_PageControlResouceList == null)
                {
                    m_PageControlResouceList = CurrentUser.GetPageControlResouceList(CurrentResourcePage.ID);
                }
                return m_PageControlResouceList;
            }
        }

        private List<Resource> m_AllSubResouceList;
        /// <summary>
        /// 当前页面下的所有资源
        /// </summary>
        public List<Resource> AllSubResouceList
        {
            get
            {
                if (m_AllSubResouceList == null)
                {
                    m_AllSubResouceList = CurrentUser.GetAllSubResouceList(CurrentResourcePage.ID);
                }
                return m_AllSubResouceList;
            }
        }

        private void GotoFirstWelcomePage()
        {
            Resource firstResource = ResourceList.Find(c => c.ResourceType == 1 && c.ParentID == 0);
            if (firstResource != null)
            {
                Response.Redirect(firstResource.ResourceAddress);
            }
        }
        private void CheckPageControlPermssion()
        {
            //如果当前页面不需要登录或者权限控制, 则不需要对页面控件进行权限检查
            if (!IsNeedLogin || !IsNeedAuth)
            {
                return;
            }

            if (CurrentResourcePage != null)
            {

                //没有权限的
                List<Resource> noPermissionSubResourceList = AllSubResouceList.FindAll(c => !PageControlResouceList.Exists(d => d.ID == c.ID));
                // List<Resource> noPermissionColumnSubResourceList = noPermissionSubResourceList.FindAll(c => c.ResourceType == 1);
                if (noPermissionSubResourceList.Count > 0)
                {
                    SetVisible(noPermissionSubResourceList, noPermissionSubResourceList, Page);
                }

            }
        }
        private static void SetVisible(List<Resource> resourceList, List<Resource> columnResourceList, Control container)
        {
            if (container is GridView)
            {
                SetGridViewItemVisible(resourceList, columnResourceList, container);
            }
            foreach (Control control in container.Controls)
            {
                Resource resource = resourceList.Find(c => c.ControlIDList.Contains(control.ID));

                if (resource != null)
                {
                    control.Visible = false;
                    resourceList.Remove(resource);
                }
                else
                {
                    if (control.Controls.Count > 0)
                    {
                        SetVisible(resourceList, columnResourceList, control);
                    }
                }
                if (resourceList.Count == 0)
                {
                    break;
                }
            }
        }
        private static void SetGridViewItemVisible(List<Resource> resourceList, List<Resource> columnResourceList, Control container)
        {
            GridView gv = container as GridView;
            foreach (Resource res in resourceList)
            {
                foreach (string controlId in res.ControlIDList)
                {
                    foreach (GridViewRow gvRow in gv.Rows)
                    {
                        Control control = gvRow.FindControl(controlId);
                        if (control == null)
                        {
                            break;
                        }
                        control.Visible = false;
                    }
                }
            }

            SetGridViewColumnVisibleByPermission(columnResourceList, gv);
        }
        private static void SetGridViewColumnVisibleByPermission(List<Resource> columnResourceList, GridView gv)
        {

            foreach (DataControlField column in gv.Columns)
            {
                string fullColumnName = gv.ID + column.AccessibleHeaderText;
                if (column is BoundField)
                {
                    fullColumnName = gv.ID + (column as BoundField).DataField;
                }
                if (columnResourceList.Exists(c => c.ResourceName == fullColumnName))
                {
                    column.Visible = false;
                }
            }
        }
        public void SetEnable(Control container, bool b)
        {
            foreach (Control control in container.Controls)
            {
                if (control.Controls.Count > 0)
                {
                    SetEnable(control, b);
                }
                else if (control is WebControl)
                {
                    WebControl webControl = control as WebControl;
                    if (webControl is TextBox || webControl is CheckBox || webControl is Button || webControl is DropDownList || webControl is RadioButtonList)
                    {
                        webControl.Enabled = false;
                    }
                }
            }
        }
        private static bool UrlEqual(string url, string resourceAddress)
        {
            // ProductImageMgmt.aspx?pno=*&categoryid=23
            string ss = url.TrimStart('~').TrimStart('/').ToUpper();
            string rp = resourceAddress.TrimStart('~').TrimStart('/').ToUpper();
            return ss.Split('?')[0].Equals(rp.Split('?')[0]);

            //if (rp.Contains("*"))
            //{
            //    if (ss.Split('?')[0].Equals(rp.Split('?')[0]))
            //    {
            //        if (ss.Split('?').Length > 1 && rp.Split('?').Length > 1)
            //        {
            //            List<string> paraUrl = ss.Split('?')[1].Split('&').ToList();
            //            List<string> paraRP = rp.Split('?')[1].Split('&').ToList();

            //            for (int i = 0; i < paraUrl.Count; i++)
            //            {
            //                if (paraUrl[i].Split('=')[0] != paraRP[i].Split('=')[0])
            //                {
            //                    return false;
            //                }
            //                else
            //                {
            //                    if (paraRP[i].Split('=')[1] != "*" && paraUrl[i].Split('=')[1] != paraRP[i].Split('=')[1])
            //                    {
            //                        return false;
            //                    }
            //                }
            //            }
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }

            //}

            //return ss == rp;
        }
        public Resource CurrentRootResource
        {
            get
            {
                if (CurrentResourcePage != null)
                {
                    if (CurrentResourcePage.ParentResource == null)
                    {
                        return CurrentResourcePage;
                    }
                    return CurrentResourcePage.ParentResource;

                }
                return null;
            }
        }
        public List<Resource> m_ResourceList;
        public List<Resource> ResourceList
        {
            get
            {
                if (m_ResourceList == null)
                {
                    if (CurrentUser.IsLogin)
                    {
                        m_ResourceList = CurrentUser.GetUserResouceList();
                        if (m_ResourceList == null)
                        {
                            m_ResourceList = new List<Resource>();
                        }
                        ConstructResourceTree(null);
                    }
                    else
                    {
                        m_ResourceList = new List<Resource>();
                    }
                }
                return m_ResourceList;
            }
        }


        private List<Resource> ConstructResourceTree(Resource parentResource)
        {
            int ParentID = parentResource == null ? 0 : parentResource.ID;
            List<Resource> list = new List<Resource>();
            foreach (var resource in m_ResourceList)
            {
                if (resource.ParentID == ParentID)
                {
                    resource.ParentResource = parentResource;
                    list.Add(resource);
                    resource.SubResourceList = ConstructResourceTree(resource);
                }
            }
            return list;
        }
        #endregion

        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            //防止非法访问。
            if (!CheckControlPermission(sourceControl, eventArgument))
            {
                ShowMessageBox("对不起！你没有权限访问该功能。");
                return;
            }
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        public bool CheckControlPermission(IPostBackEventHandler sourceControl, string eventArgument)
        {
            if (IsNeedAuth || IsNeedLogin)
            {
                string controlName = (sourceControl as Control).ID;
                if (AllSubResouceList.Exists(c => String.Equals(c.ResourceAddress, controlName, StringComparison.OrdinalIgnoreCase)))
                {
                    return PageControlResouceList.Exists(c => String.Equals(c.ResourceAddress, controlName, StringComparison.OrdinalIgnoreCase));
                }
            }
            return true;
        }

        public void RegisterStartupScript(Control control, Type type, string key, string script, bool addScriptTags)
        {
            ScriptManager.RegisterStartupScript(control, type, key, script, addScriptTags);
        }

        #region CommonMethods

        /// <summary>
        /// 封装了Try 。。。 Catch 的通用操作
        /// </summary>
        /// <param name="mainDoAction"></param>
        /// <param name="finallyDoAction"></param>
        /// <param name="showExceptionMsg"></param>
        protected virtual void CoreExecAction(Action mainDoAction, Action finallyDoAction = null, bool showExceptionMsg = true)
        {
            try
            {
                mainDoAction();
            }
            catch (Exception ex)
            {
                if (showExceptionMsg)
                {
                    ShowMessageBox(ErrorInfoBase.GetQuickError(ex.Message));
                }
            }
            finally
            {
                if (finallyDoAction != null)
                {
                    try
                    {
                        finallyDoAction();
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox(ErrorInfoBase.GetQuickError(ex.Message));
                    }
                }
            }
        }

        /// <summary>
        /// 封装了列表中的LinkButton的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="clickAction"></param>
        /// <param name="btnAttributes"></param>
        /// <param name="otherParams"></param>
        /// <returns></returns>
        protected virtual int CoreExecLnkBtnClick(object sender, Action<object[]> clickAction, List<string> btnAttributes = null, List<string> otherParams = null)
        {
            //获取需要的参数
            LinkButton btn = sender as LinkButton;

            List<object> objs = new List<object>();

            if (btnAttributes != null)
            {
                foreach (var btnAttribute in btnAttributes)
                {
                    objs.Add(GetValueByPropName(btn, btnAttribute));
                }
            }

            if (otherParams != null)
            {
                foreach (var otherParam in otherParams)
                {
                    objs.Add(otherParam);
                }
            }

            //执行方法
            clickAction(objs.ToArray());

            //返回lnkBtn的行号
            var r = GetValueByPropName(btn, "RowIndex");
            return Convert.ToInt32(r);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="controlId"></param>
        /// <param name="showDetail"></param>
        /// <param name="cRowIndex"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        protected virtual int PreviousShowAction(GridView gv, string controlId, Action<object[]> showDetail, int cRowIndex, string propName = "CommandArgument")
        {
            if (cRowIndex <= 0) return cRowIndex;
            var btn = gv.Rows[--cRowIndex].FindControl(controlId);
            var key = GetValueByPropName(btn, propName);
            showDetail(new[] { key, true, false, true });
            return cRowIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="controlId"></param>
        /// <param name="showDetail"></param>
        /// <param name="cRowIndex"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        protected virtual int NextShowAction(GridView gv, string controlId, Action<object[]> showDetail, int cRowIndex, string propName = "CommandArgument")
        {
            if (cRowIndex >= gv.Rows.Count - 1) return cRowIndex;
            var btn = gv.Rows[++cRowIndex].FindControl(controlId);
            var key = GetValueByPropName(btn, propName);
            showDetail(new[] { key, true, false, true });
            return cRowIndex;
        }

        /// <summary>
        /// 封装改变弹出框表头显示文字的方法
        /// </summary>
        /// <param name="lblTitle"></param>
        /// <param name="upTitle"></param>
        /// <param name="title"></param>
        protected virtual void ShowUpdateTitle(Label lblTitle, UpdatePanel upTitle, string title)
        {
            lblTitle.Text = title;
            upTitle.Update();

            //前台对应代码
            //<asp:UpdatePanel ID="upTitle" runat="server" UpdateMode="Conditional">
            //   <ContentTemplate>
            //      <asp:Label runat="server" ID="lblTiltle" Text="新增"></asp:Label>
            //   </ContentTemplate>
            //</asp:UpdatePanel>
        }

        /// <summary>
        /// 判断传入值是否为null，返回String
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        protected static string CommonIsNull(object obj, string nullValue = "")
        {
            return obj == null ? nullValue : obj.ToString();
        }

        #region RepeaterExtension

        protected static void BindRepeaterRadioButtonEventInPreRender(Repeater repeater, string findControlName, string jsFuncName = "uncheck")
        {
            for (int i = 0; i < repeater.Items.Count; i++)
            {
                var rb = (RadioButton)repeater.Items[i].FindControl(findControlName);
                var sTemp = "";
                for (int j = 0; j < repeater.Items.Count; j++)
                {
                    if (j != i)
                    {
                        var rb2 = (RadioButton)repeater.Items[j].FindControl(findControlName);
                        sTemp = sTemp + jsFuncName + "('" + rb2.ClientID + "');";
                    }
                }
                if (sTemp != "")
                {
                    rb.Attributes.Add("onclick", sTemp);
                }
            }
        }

        protected static void BindRepeaterCheckBoxEventInPreRender(Repeater repeater, string findControlName, string jsFuncName = "SetCheck")
        {
            CheckBox chkAll = null;
            for (int i = 0; i < repeater.Items.Count; i++)
            {

                var rb = (CheckBox)repeater.Items[i].FindControl(findControlName);
                if (i == 0)
                {
                    chkAll = rb;
                }
                var sTemp = "";
                for (int j = 0; j < repeater.Items.Count; j++)
                {
                    if (j != i)
                    {
                        var rb2 = (CheckBox)repeater.Items[j].FindControl(findControlName);
                        sTemp = sTemp + jsFuncName + "('" + chkAll.ClientID + "','" + rb.ClientID + "','" + rb2.ClientID + "');";
                    }
                }
                if (sTemp != "")
                {
                    rb.Attributes.Add("onclick", sTemp);
                }
            }
        }

        protected static string GetRepeaterCheckedRadioButtonValue(Repeater repeater, string findControlName)
        {
            foreach (RepeaterItem rpItem in repeater.Items)
            {
                RadioButton rdb = rpItem.FindControl(findControlName) as RadioButton;
                if (rdb != null && rdb.Checked)
                {
                    return rdb.ToolTip;
                }
            }
            return "-1";
        }

        protected static List<string> GetRepeaterCheckedCheckBoxValues(Repeater repeater, string findControlName)
        {
            List<string> res = new List<string>();
            foreach (RepeaterItem rpItem in repeater.Items)
            {
                CheckBox ckx = rpItem.FindControl(findControlName) as CheckBox;
                if (ckx != null && ckx.Checked)
                {
                    res.Add(ckx.ToolTip);
                }
            }
            return res;
        }

        protected static int GetRepeaterCheckedCheckBoxSumValue(Repeater repeater, string findControlName)
        {
            var temp = GetRepeaterCheckedCheckBoxValues(repeater, findControlName);
            return temp.Where(c =>
            {
                int i;
                return int.TryParse(c, out i) && i > 0;
            }).Sum(c => int.Parse(c));
        }

        protected static void CommonBindRepeater<T>(Repeater repeater, IEnumerable<T> list) where T : class ,new()
        {
            repeater.DataSource = list;
            repeater.DataBind();
        }

        protected static void CommonBindRepeater<T>(Repeater repeater, IEnumerable<T> list, bool flag, string defualtValue, string defaultText) where T : class ,new()
        {
            CommonBindRepeater(repeater, flag ? InsListByPropValues(list, defualtValue, defaultText, 0) : list);
        }

        #endregion

        #region GridViewExtension

        protected static List<int> GetGVCheckedRowList(GridView gv, string ckbName = "ckbSelect")
        {
            List<Int32> keyList = new List<Int32>();
            foreach (GridViewRow row in gv.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow)
                {
                    continue;
                }
                CheckBox ckbSelect = row.FindControl(ckbName) as CheckBox;
                if (!ckbSelect.Checked || !ckbSelect.Visible) continue;
                int key;
                int.TryParse(ckbSelect.ToolTip, out key);
                keyList.Add(key);
            }
            return keyList;
        }

        protected static void SelectAllChangedAction(GridView gv, bool isSelected, string ckbName = "ckbSelect")
        {
            foreach (GridViewRow row in gv.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow)
                {
                    continue;
                }
                CheckBox ckbSelect = row.Cells[0].FindControl(ckbName) as CheckBox;
                if (ckbSelect != null)
                {
                    ckbSelect.Checked = isSelected;
                }
            }
        }

        protected static void CkbSelectAllCheckedChangedInGridView(object sender, GridView holdGridView, UpdatePanel holdUpdatePanel, string ckbName = "ckbSelect")
        {
            CheckBox ckbSelectAll = sender as CheckBox;
            foreach (GridViewRow row in holdGridView.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow)
                {
                    continue;
                }
                CheckBox ckbSelect = row.Cells[0].FindControl(ckbName) as CheckBox;
                if (ckbSelect != null)
                {
                    ckbSelect.Checked = ckbSelectAll.Checked;
                }
            }
            holdUpdatePanel.Update();
        }

        #endregion

        #region DDLExtension

        protected static void CommonBindDDL(ListControl ddl, int start = 1, int end = 1, string bindValue = null)
        {
            ddl.Items.Clear();
            for (int i = start; i <= end; i++)
            {
                ListItem item = new ListItem { Value = i.ToString(), Text = i.ToString() };
                ddl.Items.Add(item);
            }
            ddl.DataBind();
            if (bindValue != null && ddl.Items.FindByValue(bindValue) != null)
            {
                ddl.SelectedValue = bindValue;
            }
        }

        protected static void CommonBindDDL<T>(ListControl ddl, IEnumerable<T> list, bool flag = false, string defualtValue = "0", string defaultText = "请选择")
        {
            ddl.DataSource = list;
            ddl.DataBind();
            if (!flag) return;
            ListItem item = new ListItem { Value = defualtValue, Text = defaultText };
            ddl.Items.Insert(0, item);
            ddl.SelectedIndex = 0;
        }

        #endregion

        #region RadionButtonListExtension

        protected static void CommonBindRBList<T>(RadioButtonList rbList, IEnumerable<T> list, bool flag = false, string defualtValue = "0", string defaultText = "请选择", string selectedValue = "")
        {
            rbList.DataSource = list;
            rbList.DataBind();
            if (!string.IsNullOrEmpty(selectedValue))
            {
                try
                {
                    rbList.SelectedValue = selectedValue;
                }
                catch (Exception)
                {
                }
            }
            if (!flag) return;
            ListItem item = new ListItem { Value = defualtValue, Text = defaultText };
            rbList.Items.Insert(0, item);
            if (string.IsNullOrEmpty(selectedValue))
            {
                rbList.SelectedIndex = 0;
            }
        }

        #endregion

        #region ParamsObjectsExtension

        protected static bool? GetBoolValueFromParamsObjects(object[] paramObjects, int valPos)
        {
            object t = GetValueFromParamsObjects(paramObjects, valPos);
            if (t == null) return null;
            bool temp;
            bool.TryParse(t.ToString(), out temp);
            return temp;
        }

        protected static int GetIntValueFromParamsObjects(object[] paramObjects, int valPos, int defaultValue = 0)
        {
            object t = GetValueFromParamsObjects(paramObjects, valPos);
            if (t == null) return defaultValue;
            int temp;
            int.TryParse(t.ToString(), out temp);
            return temp;
        }

        protected static object GetValueFromParamsObjects(object[] paramObjects, int valPos, object defaultValue = null)
        {
            object res = defaultValue;
            if (paramObjects != null && paramObjects.Length >= valPos)
            {
                res = paramObjects[valPos - 1];
            }
            return res;
        }

        #endregion

        #region DateTimeExtension

        protected static double GetDateDiffValue(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //你想转的格式
            return ts3.TotalMinutes;
        }

        protected static string CommonFormatDateTime(object dateTime)
        {
            return dateTime == null ? "" : ((DateTime)dateTime).ToString("yyyy-MM-dd HH:mm:ss");
        }

        #endregion

        #region GetValueByPropName

        protected static T GetValueByPropName<T>(object source, string propName, out object resValue, object[] index = null) where T : class
        {
            resValue = GetValueByPropName(source, propName, index);
            return source as T;
        }

        protected static object GetValueByPropName(object source, string propName, object[] index = null)
        {
            var temp = source.GetType().GetProperty(propName);
            if (temp != null)
            {
                return temp.GetValue(source, index);
            }
            temp = source.GetType().GetProperty("Attributes");

            if (temp == null) return null;

            var res = temp.GetValue(source, null) as AttributeCollection;
            return res[propName];
        }

        #endregion

        #region InsertIntoList

        protected static IEnumerable<T> InsListByPropValues<T>(IEnumerable<T> source, Dictionary<string, object> insPropValues, int insertIndex) where T : class ,new()
        {
            var temp = new List<T>(source.ToArray());
            T t = new T();

            Type thisType = typeof(T);

            insPropValues = insPropValues ?? new Dictionary<string, object>();

            foreach (var initPropValue in insPropValues)
            {
                var propName = initPropValue.Key;
                var propValue = initPropValue.Value;
                thisType.GetProperty(propName).SetValue(t, propValue, null);
            }

            temp.Insert(insertIndex, t);
            return temp;
        }

        protected static IEnumerable<T> InsListByPropValues<T>(IEnumerable<T> source, string defualtValue, string defaultText, int insertIndex) where T : class, new()
        {
            Dictionary<string, object> dic = new Dictionary<string, object> { { "CodeText", defaultText }, { "CodeValue", defualtValue } };
            return InsListByPropValues(source, dic, insertIndex);
        }

        #endregion

        #endregion

    }
    public static class TreeViewHelper
    {
        /// <summary>
        /// 刷新整个ResourceTree（用于显示更新数据数据后的效果）
        /// </summary>
        public static TreeNode RefreshTree<T>(this TreeView treeView, IEnumerable<T> totalResourceList, UpdatePanel updatePanel = null, int selectResourceID = -1,
             string nValue = "ID",
            string nParentID = "ParentID",
            string nDisplayOrder = "DisplayOrder",
            string nDisplayName = "DisplayName",
            string nStatus = "Status") where T : DataContractBase
        {

            AddTreeNodes(treeView, totalResourceList, nValue: nValue, nParentID: nParentID, nDisplayName: nDisplayName, nDisplayOrder: nDisplayOrder, nStatus: nStatus);
            TreeNode node = null;
            if (selectResourceID > -1)
            {
                node = FindNodeByResourceId(treeView, selectResourceID);

                if (node != null)
                {
                    node.Select();
                    ExpandToNode(node);
                }
            }

            if (updatePanel != null)
            {
                updatePanel.Update();
            }
            return node;
        }

        /// <summary>
        /// 构建树结构
        /// </summary>
        /// <param name="parentTreeNode">父节点</param>
        /// <param name="allResourceList"></param>
        /// <param name="showDic"></param>
        /// <param name="action"></param>
        /// <param name="nValue"></param>
        /// <param name="nParentID"></param>
        /// <param name="nDisplayOrder"></param>
        /// <param name="nDisplayName"></param>
        /// <param name="nStatus"></param>
        private static void AddTreeNodes<T>(
            object parentTreeNode,
            IEnumerable<T> allResourceList,
            Dictionary<int, string> showDic = null,
            TreeNodeSelectAction action = TreeNodeSelectAction.SelectExpand,
            string nValue = "ID",
            string nParentID = "ParentID",
            string nDisplayOrder = "DisplayOrder",
            string nDisplayName = "DisplayName",
            string nStatus = "Status") where T : DataContractBase
        {
            int resourceId = 0;
            TreeNodeCollection childNodes;

            var treeNode = parentTreeNode as TreeNode;
            if (treeNode != null)
            {
                resourceId = Convert.ToInt32(treeNode.Value);
                childNodes = treeNode.ChildNodes;
            }
            else
            {
                var treeView = parentTreeNode as TreeView;
                treeView.Nodes.Clear();
                childNodes = treeView.Nodes;
            }

            if (showDic == null)
            {
                showDic = new Dictionary<int, string> { { 0, "（禁用）" } };
            }

            foreach (var resource in allResourceList.Where(c => Convert.ToInt32(c.GetValue(nParentID, -1)) == resourceId))
            {
                var value = resource.GetValue(nValue, "").ToString();
                var name = resource.GetValue(nDisplayName);
                var order = resource.GetValue(nDisplayOrder, "").ToString();
                var status = Convert.ToInt32(resource.GetValue(nStatus, -1));
                var displayText = order + "." + name + (showDic.ContainsKey(status) ? showDic[status] : "");

                TreeNode node = new TreeNode
                {
                    Value = value,
                    ToolTip = order,
                    Text = displayText,
                    SelectAction = action
                };

                AddTreeNodes(node, allResourceList, showDic, nValue: nValue, nParentID: nParentID, nDisplayName: nDisplayName, nDisplayOrder: nDisplayOrder, nStatus: nStatus);
                childNodes.Add(node);
            }
        }

        private static TreeNode FindNodeByResourceId(Object parentTreeNode, int currentResourceID)
        {
            var tv = parentTreeNode as TreeNode;
            TreeNodeCollection childNodes = tv != null ? tv.ChildNodes : (parentTreeNode as TreeView).Nodes;

            foreach (TreeNode treeNode in childNodes)
            {
                if (currentResourceID == Int32.Parse(treeNode.Value))
                {
                    return treeNode;
                }
                TreeNode findTreeNode = FindNodeByResourceId(treeNode, currentResourceID);
                if (findTreeNode != null)
                {
                    return findTreeNode;
                }
            }
            return null;
        }

        private static void SetParentNode(TreeNode node)
        {
            while (true)
            {
                if (node.Parent != null)
                {
                    node.Parent.Checked = true;
                    node = node.Parent;
                    continue;
                }
                break;
            }
        }

        private static void SetSelectNode(TreeNodeCollection tnc, bool select)
        {
            if (tnc == null || tnc.Count <= 0) return;
            foreach (TreeNode node in tnc)
            {
                node.Checked = @select;
                SetSelectNode(node.ChildNodes, @select);
            }
        }

        private static void SetSelectInverseNode(TreeNodeCollection tnc)
        {
            if (tnc == null || tnc.Count <= 0) return;
            foreach (TreeNode node in tnc)
            {
                node.Checked = !node.Checked;
                SetSelectInverseNode(node.ChildNodes);
            }
        }

        private static void ExpandToNode(TreeNode node)
        {
            while (true)
            {
                if (node.Parent != null)
                {
                    node.Parent.Expand();
                    node = node.Parent;
                    continue;
                }
                break;
            }
        }


        public static void CollapseAllTreeNode(this TreeView treeView)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                node.CollapseAll();
            }
        }

        public static TreeNode SelectTreeNodeByNodeValue(this TreeNodeCollection tnc, string id)
        {
            if (tnc == null)
            {
                return null;
            }

            foreach (TreeNode node in tnc)
            {
                if (node.Value == id)
                {
                    node.Select();
                    ExpandToNode(node);
                    return node;
                }
                TreeNode findNode = SelectTreeNodeByNodeValue(node.ChildNodes, id);
                if (findNode != null)
                {
                    return findNode;
                }
            }
            return null;
        }

    }

}
