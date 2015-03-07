using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prochat.Models;

namespace Prochat.Controllers
{
    public class lycosController : Controller
    {
        // GET: lycos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string id)
        {
            var model = new LycosModel {SearchTerm = id};
            return View(model);
        }
    }
}