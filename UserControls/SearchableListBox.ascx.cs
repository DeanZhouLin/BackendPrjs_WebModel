using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jufine.Backend.WebModel.UserControls
{
    public partial class SearchableListBox : UserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public event EventHandler DoSearch;
        public event EventHandler SelectedIndexChanged;

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (DoSearch != null)
            {
                DoSearch(sender, e);
            }
            //panelListBox.Visible = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                // lbItem.Attributes.Add("onchange", string.Format("onchangeHandler('#{0}','#{1}')", lbItem.ClientID, txtText.ClientID));

            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //ScriptManager.RegisterStartupScript(this.upList, this.GetType(), "", "InitSearchableListBox();", true);
        }

        public ListBox CurrentListBox
        {
            get { return this.lbItem; }
        }

        public string InputText
        {
            get { return hfText.Value; }
        }

        public string SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(txtText.Text) || txtText.Text != hfText.Value)
                {
                    return null;
                }
                return hfValue.Value;
            }
            set
            {
                hfValue.Value = value;
                CurrentListBox.SelectedValue = value;
                txtText.Text = CurrentListBox.SelectedItem.Text;
                hfText.Value = txtText.Text;
            }
        }

        public Unit Width
        {
            set
            {
                this.txtText.Width = value;
            }
        }

        protected void lbItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(sender, e);
            }
        }
    }
}