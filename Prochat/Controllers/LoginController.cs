using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Prochat.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == "test")
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    return RedirectToAction("index", "chat");
                }
            }
            return View();
        }

    }
}