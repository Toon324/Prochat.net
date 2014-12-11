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
                if (DataAccess.UserDatabaseConnector.UserIsValid(model.Username, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    Session["Username"] = model.Username;

                    return RedirectToAction("index", "chat");
                }
                {
                    ModelState.AddModelError("", "Invalid Username or Password.");
                }
            }
            return View();
        }

    }
}