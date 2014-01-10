using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jufine.Backend.WebModel;

namespace Jufine.Backend.WebModel
{
	public partial class WebForm1 : PageBase
	{
		public override bool IsNeedLogin
		{
			get
			{
				return false;
			}
		}
		protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}