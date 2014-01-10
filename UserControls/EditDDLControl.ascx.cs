using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jufine.Backend.WebModel.UserControls
{
    public partial class EditDDLControl : System.Web.UI.UserControl
    {
        public event EventHandler TextChanged;

        public event EventHandler SelectedIndexChanged;

        public int Width
        {
            set
            {
                ddl.Width = value;
                txtEdit.Width = (value - 20) < 0 ? 0 : value - 20;
            }
        }

        public string Text
        {
            get { return txtEdit.Text.ToString().Trim(); }
            set { txtEdit.Text = value; }
        }

        public int SelectedIndex
        {
            get
            { return ddl.SelectedIndex; }
            set
            { ddl.SelectedIndex = value; }
        }

        public string SelectedValue
        {
            get
            { return ddl.SelectedValue; }
            set
            { ddl.SelectedValue = value; }
        }

        public string SelectedText
        {
            get { return ddl.SelectedItem.Text.ToString().Trim(); }
            set { ddl.SelectedItem.Text = value; }
        }

        public string DataTextField
        {
            get
            {
                return ddl.DataTextField;
            }
            set
            {
                ddl.DataTextField = value;
            }
        }

        public string DataValueField
        {
            get { return ddl.DataValueField; }
            set
            {
                ddl.DataValueField = value;
            }
        }

        public object DataSource
        {
            get
            {
                return ddl.DataSource;
            }
            set
            {
                if (null != value)
                {
                    ddl.DataSource = value;
                }
                else
                {
                    ddl.DataSource = new object();
                }
            }
        }

        public override void DataBind()
        {
            ddl.DataBind();
        }

        public void InsertItem(int index, ListItem item)
        {
            ddl.Items.Insert(index, item);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void txtEdit_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(sender, e);
            }
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(sender, e);
            }
            //txtEdit.Text = ddl.SelectedItem.Text;
        }
    }
}