using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Register.Web.Contraints;

namespace Register.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}",
            //    defaults: new { controller = "Landing", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { "Register.Web.Controllers" }
            //);
            routes.MapRoute(
              name: "Landing",
              url: "Landing/{action}/{id}",
              defaults: new { controller = "Landing", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "Register.Web.Controllers" }
            );

            //routes.MapRoute(
            //   name: "TestTypeApp",
            //   url: "{testtypeid}/{controller}/{action}/{id}",
            //   defaults: new { controller = "Landing", action = "Index", id = UrlParameter.Optional, testtypeid = UrlParameter.Optional, },
            //   constraints: new { testtypeid = @"\d+" },
            //   namespaces: new string[] { "Register.Web.Controllers" }
            //);

            routes.MapRoute(
               name: "TestTypeApp",
               url: "{testtypeid}/{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               //constraints: new RouteWithTestTypeContraint(),
               constraints: new { testtypeid = @"\d+" },
               namespaces: new string[] { "Register.Web.Controllers" }
            );

            //routes.MapRoute("PageNotFound", "{*url}", new { controller = "Error", action = "NotFound" });
            routes.MapRoute("PageNotFound", "{*url}", new { controller = "Landing", action = "Index" });

        }
    }
}
