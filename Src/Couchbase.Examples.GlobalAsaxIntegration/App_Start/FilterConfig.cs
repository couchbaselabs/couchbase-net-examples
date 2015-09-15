using System.Web;
using System.Web.Mvc;

namespace Couchbase.Examples.GlobalAsaxIntegration
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
