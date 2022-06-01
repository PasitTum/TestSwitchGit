using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSP.Lib.Extension;
using System.Configuration;

namespace Register.Web.Helpers
{
    public class SysParameterHelper
    {
        public static string ApplicationName
        {
            get
            {
                return GetConfig("ApplicationName", "");
            }
        }
        public static string AppRoot
        {
            get
            {
                return GetConfig("AppRoot", "");
            }
        }

        public static int CaptchaExpiresMinutes
        {
            get
            {
                return GetConfig("CaptchaExpiresMinutes", "15").ToInt();
            }
        }

        public static string CaptchaStorage
        {
            get
            {
                return GetConfig("CaptchaStorage", "");
            }
        }
        public static string ApiUrlServerSide
        {
            get
            {
                return GetConfig("ApiUrlServerSide", true);
            }
        }

        public static string ApiUrlClientSide
        {
            get
            {
                return GetConfig("ApiUrlClientSide", true);
            }
        }

        public static string ReportPath
        {
            get
            {
                return GetConfig("ReportPath", true);
            }
        }

        //public static string TestTypeID
        //{
        //    get
        //    {
        //        return GetConfig("TestTypeID", true);
        //    }
        //}

        public static string ExamType
        {
            get
            {
                return GetConfig("ExamType", true);
            }
        }

        public static bool IsTesting
        {
            get
            {
                return GetConfig("IsTesting", "N") == "Y";
            }
        }

        public static string GoogleAnalyticID
        {
            get
            {
                return GetConfig("GoogleAnalyticID", true);
            }
        }

        public static string FaceServiceUrl
        {
            get
            {
                return GetConfig("FaceServiceUrl", true);
            }
        }

        public static bool EnableButtonCamera
        {
            get
            {
                return GetConfig("EnableButtonCamera", "N") == "Y";
            }
        }

        public static string CalendarCacheExpiration
        {
            get
            {
                return GetConfig("CalendarCacheExpiration", "00:01:00");
            }
        }

        public static string FaceDetectTimer
        {
            get
            {
                return GetConfig("FaceDetectTimer", "2");
            }
        }

        public static string DefaultCacheExpiration
        {
            get
            {
                return GetConfig("DefaultCacheExpiration", "00:02:00");
            }
        }

        private static string GetConfig(string key, bool throwIfEmpty = false)
        {
            var rtn = ConfigurationManager.AppSettings[key];
            if (throwIfEmpty && string.IsNullOrWhiteSpace(rtn))
            {
                throw new Exception(string.Format("ไม่พบตัวแปร [{0}] ใน web.config", key));
            }
            return rtn;
        }

        private static string GetConfig(string key, string defaultValue)
        {
            var rtn = GetConfig(key);
            if (string.IsNullOrWhiteSpace(rtn))
            {
                return defaultValue;
            }
            return rtn;
        }

        //DocUploadPath
        public static string DocUploadPath
        {
            get
            {
                return GetConfig("DocUploadPath", false);
            }
        }
      

        public static string DocUploadTempPath
        {
            get
            {
                return GetConfig("DocUploadTempPath", false);
            }
        }

        public static string PhotoImagePath
        {
            get
            {
                return GetConfig("PhotoImagePath", true);
            }
        }

        public static string PhotoImageTempPath
        {
            get
            {
                return GetConfig("PhotoImageTempPath", true);
            }
        }

        public static string PhotoDomain
        {
            get
            {
                return GetConfig("PhotoDomain", true);
            }
        }

        public static string PhotoUser
        {
            get
            {
                return GetConfig("PhotoUser", true);
            }
        }

        public static string PhotoPassword
        {
            get
            {
                return GetConfig("PhotoPassword", true);
            }
        }
    }
}