using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Register.Web
{
    // ใช้สำหรับ Set ค่าคงที่ใส่ใน ViewBag ให้กับทุกๆ Action เลย
    public class ExamTypeFilter : ActionFilterAttribute
    {
        public string ExamType { get; set; }

        public ExamTypeFilter(string examType)
        {
            this.ExamType = examType;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ExamType = this.ExamType;

            //var testTypeID = filterContext.RequestContext.HttpContext.Request.QueryString["testTypeID"];
            //var iTestTypeID = 0;
            //if (int.TryParse(testTypeID, out iTestTypeID)) {
            //    if (iTestTypeID != 0)
            //    {
            //        filterContext.Controller.ViewBag.IsInitial = true;
            //        filterContext.Controller.ViewBag.TestTypeID = iTestTypeID;
            //    }
            //}
        }

    }
}
