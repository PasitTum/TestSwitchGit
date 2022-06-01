using CSP.Lib.Diagnostic;
using CSP.Lib.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Register.Models;
using Register.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;

namespace Register.Web.Biz
{
    public class CalendarBiz
    {
        private System.Diagnostics.TraceSwitch tsw = new System.Diagnostics.TraceSwitch("mySwitch", "CalendarBiz");

        public async Task<List<CalendarModel>> ListCalendarAsync(string systemType, int testTypeID)
        {
            var cacheKey = string.Format("{0}_{1}", systemType, "Schedules").ToUpper();
            var scheduleList = CacheHelper.GetCachingData<List<CalendarModel>>(cacheKey);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // ตรงนี้ใช้ async ไม่ได้  มันจะ error แปลกๆ
                var url = SysParameterHelper.ApiUrlServerSide + "/Master/TestTypeID/" + testTypeID + "/Schedules";
                var response = client.GetAsync(url).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs. 
                response.EnsureSuccessStatusCode();  // ถ้ามันไมใช่ status 200 จะ throw ให้

                scheduleList = await response.Content.ReadAsAsync<List<CalendarModel>>();
                if (scheduleList != null)
                {
                    CacheHelper.SetCachingData(cacheKey, scheduleList, SysParameterHelper.CalendarCacheExpiration);
                }
            }
            return scheduleList;
        }

        public async Task<CalendarModel> GetCalendarAsync(string systemType, string calendarCode, string controllerName, int testTypeID)
        {
            CalendarModel rtn = new CalendarModel()
            {
                ServiceResult = new CSP.Lib.Models.ResultInfo()
            };

            List<CalendarModel> scheduleList = null;
            try
            {
                scheduleList = await this.ListCalendarAsync(systemType, testTypeID);
            }
            catch (System.Net.Http.HttpRequestException hex)
            {
                rtn.ServiceResult.Success = false;
                rtn.ServiceResult.ErrorMessage = Resources.Messages.ApiRequestError;
                Log.WriteErrorLog(tsw.TraceError, hex);
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException || ex is CommunicationException)
                {
                    rtn.ServiceResult.Success = false;
                    rtn.ServiceResult.ErrorMessage = Resources.Messages.ServiceTimeOutError;
                }
                else
                {
                    rtn.ServiceResult.ErrorMessage = ex.Message;
                }
                Log.WriteErrorLog(tsw.TraceError, ex);
            }

            if (scheduleList != null)
            {
                rtn = scheduleList.FirstOrDefault(x => x.SCHEDULE_TYPE_CODE == calendarCode);
                if (rtn != null) rtn.ServiceResult = new CSP.Lib.Models.ResultInfo() { Success = true };
            }

            return rtn ?? new CalendarModel() { ServiceResult = new CSP.Lib.Models.ResultInfo() };
        }

        //public async Task<CalendarModel> GetCalendarAsync(string systemType, string calendarCode, string controllerName, int testTypeID)
        //{
        //    CalendarModel rtn = new CalendarModel()
        //    {
        //        ServiceResult = new CSP.Lib.Models.ResultInfo()
        //    };

        //    var cacheKey = string.Format("{0}_{1}", systemType, "Schedules").ToUpper();
        //    var url = SysParameterHelper.ApiUrlServerSide + "/Master/TestTypeID/" + testTypeID + "/Schedules";

        //    // var jsonResult = string.Empty;

        //    var scheduleList = CacheHelper.GetCachingData<List<CalendarModel>>(cacheKey);
        //    if (scheduleList == null)
        //    {
        //        try
        //        {
        //            using (var client = new HttpClient())
        //            {
        //                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                // ตรงนี้ใช้ async ไม่ได้  มันจะ error แปลกๆ
        //                var response = client.GetAsync(url).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs. 
        //                response.EnsureSuccessStatusCode();  // ถ้ามันไมใช่ status 200 จะ throw ให้

        //                scheduleList = await response.Content.ReadAsAsync<List<CalendarModel>>();
        //                if (scheduleList != null)
        //                {
        //                    CacheHelper.SetCachingData(cacheKey, scheduleList, SysParameterHelper.CalendarCacheExpiration);
        //                }
        //                //jsonResult = await response.Content.ReadAsStringAsync();
        //                //var datas = (JObject)JsonConvert.DeserializeObject(jsonResult);
        //                //rtn.SCHEDULE_TYPE_ID = int.Parse(datas.GetValueIgnoreCase("SCHEDULE_TYPE_ID"));
        //                //rtn.SCHEDULE_TYPE_NAME_TH = datas.GetValueIgnoreCase("SCHEDULE_TYPE_NAME_TH");
        //                //rtn.ACTIVE_FLAG = datas.GetValueIgnoreCase("ACTIVE_FLAG");
        //                //rtn.IS_USED = datas.GetValueIgnoreCase("IS_USED");
        //                //rtn.SCHEDULE_TYPE_CODE = datas.GetValueIgnoreCase("SCHEDULE_TYPE_CODE");
        //                //rtn.ServiceResult.Success = true;
        //                //if (rtn != null && rtn.ServiceResult.Success)
        //                //{
        //                //    CacheHelper.SetCachingData(cacheKey, rtn, SysParameterHelper.CalendarCacheExpiration);
        //                //}
        //            }
        //        }
        //        catch (System.Net.Http.HttpRequestException hex)
        //        {
        //            rtn.ServiceResult.Success = false;
        //            rtn.ServiceResult.ErrorMessage = Resources.Messages.ApiRequestError;
        //            Log.WriteErrorLog(tsw.TraceError, hex);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex is TimeoutException || ex is CommunicationException)
        //            {
        //                rtn.ServiceResult.Success = false;
        //                rtn.ServiceResult.ErrorMessage = Resources.Messages.ServiceTimeOutError;
        //            }
        //            else
        //            {
        //                rtn.ServiceResult.ErrorMessage = ex.Message;
        //            }
        //            Log.WriteErrorLog(tsw.TraceError, ex);
        //        }
        //    }

        //    if (scheduleList != null)
        //    {
        //        rtn = scheduleList.FirstOrDefault(x => x.SCHEDULE_TYPE_CODE == calendarCode);
        //        if (rtn != null) rtn.ServiceResult = new CSP.Lib.Models.ResultInfo() { Success = true };
        //    }

        //    return rtn ?? new CalendarModel() { ServiceResult = new CSP.Lib.Models.ResultInfo() };
        //}

    }
}