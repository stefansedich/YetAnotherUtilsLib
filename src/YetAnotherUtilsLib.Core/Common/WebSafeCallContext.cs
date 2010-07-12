using System.Runtime.Remoting.Messaging;
using System.Web;

namespace YetAnotherUtilsLib.Core.Common
{
    public static class WebSafeCallContext
    {
        public static object GetData(string name)
        {
            if (WebContext != null)
                return WebContext.Items[name];

            return CallContext.GetData(name);
        }

        public static void SetData(string name, object value)
        {
            if (WebContext != null)
                WebContext.Items[name] = value;
            else
                CallContext.SetData(name, value);
        }

        public static void FreeDataSlot(string name)
        {

            if (WebContext != null)
                WebContext.Items.Remove(name);
            else
                CallContext.FreeNamedDataSlot(name);
        }

        private static HttpContext WebContext
        {
            get { return HttpContext.Current; }
        }
    }
}