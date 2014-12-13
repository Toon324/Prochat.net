using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prochat.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Index()
        {
            var username = Session["Username"] as string;
            if (username == null)
                return RedirectToAction("proceedToLogin", "logout");

            var user = new User { UserName = username };
            return View(user);
        }

        public ActionResult Call()
        {
            return Call();
        }
  
    }
}