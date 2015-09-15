using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Couchbase.Examples.OwinKatanaIntegration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var bucket = ClusterHelper.GetBucket("default");
            var result = bucket.Upsert("foo", "bar");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}