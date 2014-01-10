using System.Web;

using Com.BaseLibrary.Utility;

namespace Jufine.Backend.WebModel.Request
{
    public class QueryStringManager
    {
        #region fileds
        private HttpRequest m_HttpRequest;
        #endregion

        public QueryStringManager(HttpRequest request)
        {
            m_HttpRequest = request;
        }

        public string GetValue(string paramName)
        {
            return string.IsNullOrEmpty(m_HttpRequest.QueryString[paramName]) ? string.Empty : m_HttpRequest.QueryString[paramName];
        }
        public T GetValue<T>(string paramName, T defaultValue)
        {
            string value = string.IsNullOrEmpty(m_HttpRequest.QueryString[paramName]) ? string.Empty : m_HttpRequest.QueryString[paramName];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return StringUtil.ToType<T>(value);

        }
    }
}
