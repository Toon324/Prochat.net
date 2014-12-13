﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Prochat.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return View();
        }

        public ActionResult ProceedToLogin()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "login");
        }
    }
}