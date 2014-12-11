using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Prochat.Controllers
{
    public class RegisterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                DataAccess.UserDatabaseConnector.RegisterUser(model.Username, model.Password);
              
                FormsAuthentication.SetAuthCookie(model.Username, false);
                Session["Username"] = model.Username;

                return RedirectToAction("index", "chat");

            }
            return View();
        }
    }
}