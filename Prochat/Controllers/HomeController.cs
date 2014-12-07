using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prochat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var username = Session["Username"] as string;
            if (username == null)
                username = "Default";

            var user = new User { UserName = username };
            return View(user);
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