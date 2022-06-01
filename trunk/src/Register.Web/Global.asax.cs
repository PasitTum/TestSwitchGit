using CaptchaMvc.Infrastructure;
using Register.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Register.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // สั่งให้ใช้ CookieName เหมือนกันกับ WebApi เพื่อให้ Share ValidationToken ได้
            AntiForgeryConfig.CookieName = "__RequestVerificationToken" + "_" + HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes("/DLARegisterWeb"));

            // สั่งให้ Captcha ไปใช้ Cookie แทนที่จะใช้ Session
            if (Helpers.SysParameterHelper.CaptchaStorage.ToUpper() == "COOKIE")
            {
                CaptchaUtils.CaptchaManager.StorageProvider = new CookieStorageProvider()
                {
                    Salt = new byte[] { 0x52, 0x53, 0x4f, 90, 0x39, 12, 0xf9, 210 },
                    Password = "mflv[1234",
                    ExpiresMinutes = SysParameterHelper.CaptchaExpiresMinutes
                };
            }
        }
    }
}
