﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Register.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // สั่งให้ใช้ CookieName เหมือนกันกับ WebApi เพื่อให้ Share ValidationToken ได้
            AntiForgeryConfig.CookieName = "__RequestVerificationToken" + "_" + HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes("/DLARegisterWeb"));
        }
    }
}
