using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Final
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
            routes.MapRoute(
                name: "Second",
                url: "{controller}/{action}/{id1}&{id2}",
                defaults: new { controller = "Items", action = "Index", id1 = UrlParameter.Optional,id2= UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Third",
               url: "{controller}/{action}/{id1}&{id2}&{id3}&{id4}",
               defaults: new { controller = "Items", action = "Category", id1 = UrlParameter.Optional, id2 = UrlParameter.Optional, id3 = UrlParameter.Optional, id4 = UrlParameter.Optional }
           );
        }
    }
}
