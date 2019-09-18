using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CryptoGeeks.API.Controllers
{
    public class HelperController : ApiController
    {
        // GET: Helper
        public string GetMyIp()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
    }
}