using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prochat.Controllers
{
    public class ChatController : Controller
    {
        public ActionResult Index(string id)
        {
            var username = Session["Username"] as string;
            if (username == null)
                return RedirectToAction("proceedToLogin", "logout");

           // var user = new User { UserName = username, Group = "Prochat", Room = "General" };
            var user = new User {UserName = username, Group = id, Room = "General"};
            return View(user);
        }

        public ActionResult Call()
        {
            return Call();
        }

        public ActionResult Group(string id)
        {
            var username = Session["Username"] as string;
            if (username == null)
                return RedirectToAction("proceedToLogin", "logout");

            var rooms = DataAccess.GroupsDatabaseConnector.GetListOfRoomsInGroup(id);

            var user = new User { UserName = username, Group = id, Room = "General", RoomsList = rooms };
            return View(user);
        }
 
    }
}