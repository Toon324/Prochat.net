using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Prochat.Controllers;

namespace Prochat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //RouteTable.Routes.MapRoute("GroupChat", "Group/{groupName}", new { controller = "ChatController", action = "Index"});
            //RouteTable.Routes.MapRoute("GroupAndRoomChat", "chat/Group/{groupName}/{room}", new { controller = "ChatController", action = "Index", groupName = "Prochat", room = "General" });

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }
    }
}
