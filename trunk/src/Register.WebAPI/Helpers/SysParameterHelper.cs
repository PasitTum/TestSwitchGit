using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Register.WebAPI.Helpers
{
    public class SysParameterHelper
    {
        public static string SMSRegisterService
        {
            get
            {
                return GetConfig("SMSRegisterService", true);
            }
        }

        public static string SMSPaymentStatus
        {
            get
            {
                return GetConfig("SMSPaymentStatus", true);
            }
        }

        public static string QRCodeApiSMSFormatUrl
        {
            get
            {
                return GetConfig("QRCodeApiSMSFormatUrl", true);
            }
        }

        public static string SMSProjectID
        {
            get
            {
                return GetConfig("SMSProjectID", true);
            }
        }

        public static string SMSUser
        {
            get
            {
                return GetConfig("SMSUser", true);
            }
        }

        public static string SMSPassword
        {
            get
            {
                return GetConfig("SMSPassword", true);
            }
        }

        public static string SMSApiKey
        {
            get
            {
                return GetConfig("SMSApiKey", true);
            }
        }

        public static string FaceServiceUrl
        {
            get
            {
                return GetConfig("FaceServiceUrl", true);
            }
        }

        public static string EnableAutoBrightness
        {
            get
            {
                return GetConfig("EnableAutoBrightness", true);
            }
        }

        public static string ValidateFaceRatio
        {
            get
            {
                return GetConfig("ValidateFaceRatio", true);
            }
        }

        public static string ValidateWidthDivideHeight
        {
            get
            {
                return GetConfig("ValidateWidthDivideHeight", true);
            }
        }

        public static string MinFacePercent
        {
            get
            {
                return GetConfig("MinFacePercent", true);
            }
        }

        public static string MaxFacePercent
        {
            get
            {
                return GetConfig("MaxFacePercent", true);
            }
        }

        public static string MinWidthHeightRatio
        {
            get
            {
                return GetConfig("MinWidthHeightRatio", true);
            }
        }

        public static string MaxWidthHeightRatio
        {
            get
            {
                return GetConfig("MaxWidthHeightRatio", true);
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

        public static string PhotoImageHistoryPath
        {
            get
            {
                return GetConfig("PhotoImageHistoryPath", true);
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

        public static string DocUploadPath
        {
            get
            {
                return GetConfig("DocUploadPath", true);
            }
        }

        public static string DocUploadTempPath
        {
            get
            {
                return GetConfig("DocUploadTempPath", true);
            }
        }

        public static string DocUploadHistoryPath
        {
            get
            {
                return GetConfig("DocUploadHistoryPath", true);
            }
        }
    }
}