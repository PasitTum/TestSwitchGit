using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSP.Lib.Extension;
// using CSP.Lib.Diagnostic;
using System.Runtime.Caching;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Register.WebAPI.Helpers
{
    public class CacheHelper
    {
        public static T GetCachingData<T>(string key)
        {
            if (IsCacheExpired(key))
            {
                MemoryCache.Default.Remove(key);
            }
            return (T)MemoryCache.Default[key];
        }

        public static void SetCachingData(string key, object value, string expiredTimeSpan)
        {
            if (value == null)
            {
                MemoryCache.Default.Remove(key);
            }
            else
            {
                SetCacheExpiration(key, expiredTimeSpan);
                MemoryCache.Default[key] = value;
            }
        }

        public static bool IsCacheExpired(string key)
        {
            var expiredKey = "EXPIRED_" + key;
            DateTime? expiredTime = (DateTime?)MemoryCache.Default[expiredKey];
            if (expiredTime != null)
            {
                return expiredTime <= DateTime.Now;
            }
            else
            {
                return false;
            }
        }

        private static void SetCacheExpiration(string key, string timeSpan)
        {
            var expiredKey = "EXPIRED_" + key;
            if (string.IsNullOrWhiteSpace(timeSpan))
            {
                MemoryCache.Default.Remove(expiredKey);
            }
            else
            {
                MemoryCache.Default[expiredKey] = DateTime.Now.Add(TimeSpan.Parse(timeSpan));
            }
        }

    }
}