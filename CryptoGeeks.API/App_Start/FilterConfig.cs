using Swashbuckle.Application;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CryptoGeeks.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

        }
    }
}
