using System;

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