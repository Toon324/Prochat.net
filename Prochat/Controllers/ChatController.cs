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

           return RedirectToAction("Group");
        }

        public ActionResult Group(string id, string room)
        {
            var username = Session["Username"] as string;
            if (username == null)
                return RedirectToAction("proceedToLogin", "logout");

            if (String.IsNullOrEmpty(id))
                id = "Prochat";

            var rooms = DataAccess.GroupsDatabaseConnector.GetListOfRoomsInGroup(id);

            if (String.IsNullOrEmpty(room))
                room = "General";

            var user = new User { UserName = username, Group = id, Room = room, RoomsList = rooms };
            return View(user);
        }
 
    }
}