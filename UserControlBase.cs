using System;
using System.Web.UI;

namespace Jufine.Backend.WebModel
{
    public class UserControlBase : UserControl
    {
        private PageBase m_CurrentPageBase;
        public PageBase CurrentPageBase
        {
            get
            {
                if (m_CurrentPageBase == null)
                {
                    m_CurrentPageBase = this.Page as PageBase;
                    if (m_CurrentPageBase == null)
                    {
                        throw new Exception("页面必须继承于PageBase。");
                    }
                }
                return m_CurrentPageBase;
            }
        }
    }
}
