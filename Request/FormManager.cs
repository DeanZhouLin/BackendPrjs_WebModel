using System.Web;

namespace Jufine.Backend.WebModel.Request
{
    public class FormManager
    {
        #region fileds
        private HttpRequest m_HttpRequest;
        #endregion

        public FormManager(HttpRequest request)
        {
            m_HttpRequest = request;
        }

        public string GetValue(string paramName)
        {
            string tmp = string.IsNullOrEmpty(m_HttpRequest.Form.Get(paramName)) ? string.Empty : m_HttpRequest.Form.Get(paramName);
            return System.Web.HttpUtility.HtmlEncode(tmp).Trim();
        }
    }
}
