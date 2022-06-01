using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSP.Lib.Extension;
using Register.Web.Helpers;
using Register.Models;
using System.Web.Routing;
using CSP.Lib.Diagnostic;

namespace Register.Web
{
    public class CalendarOpenFilter : ActionFilterAttribute, IActionFilter
    {
        public string SystemType { get; set; }
        public string CalendarCode { get; set; }

        public EFilterReturnType ReturnType { get; set; }

        async void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!SysParameterHelper.IsTesting)
            {
                // var info = Helper.SystemHelper.GetUserInfo();
                var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                // var testTypeID = filterContext.RequestContext.HttpContext.Request.QueryString["testTypeID"].ToIntNull();
                // var testTypeID = SysParameterHelper.TestTypeID.ToIntNull();                
                int? testTypeID = null;
                if (filterContext.RouteData.Values.ContainsKey("testtypeid"))
                {
                    testTypeID = filterContext.RouteData.GetRequiredString("testtypeid").ToIntNull();
                }                
                if (testTypeID.IsNotNull())
                {
                    var biz = new Biz.CalendarBiz();
                    var auth = await biz.GetCalendarAsync(this.SystemType, this.CalendarCode, controllerName, testTypeID.Value);
                    if (!auth.ServiceResult.Success)
                    {
                        var msgModel = new MessageModel()
                        {
                            Message = auth.ServiceResult.ErrorMessage,
                            Title = auth.SCHEDULE_TYPE_NAME_TH,
                            MessageType = EMessageType.Warning
                        };
                        filterContext.Controller.ViewData.Model = msgModel;

                        if (this.ReturnType == EFilterReturnType.View)
                        {
                            filterContext.Result = new ViewResult
                            {
                                ViewName = "DisplayMessage",
                                ViewData = filterContext.Controller.ViewData
                            };
                        }
                        else if (this.ReturnType == EFilterReturnType.PartialView)
                        {
                            filterContext.Result = new PartialViewResult
                            {
                                ViewName = "_DisplayMessage",
                                ViewData = filterContext.Controller.ViewData
                            };
                        }
                        else
                        {
                            filterContext.Result = new JsonResult
                            {
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                                Data = new CSP.Lib.Models.ResultInfo() { ErrorMessage = msgModel.Message }
                            };
                        }
                    }
                    else
                    {
                        if (!auth.IS_USED.ToBoolean())
                        {
                            var msgModel = new MessageModel()
                            {
                                Message = Resources.Messages.CalendarNotOpen.ToFormat(auth.SCHEDULE_TYPE_NAME_TH),
                                Title = auth.SCHEDULE_TYPE_NAME_TH,
                                MessageType = EMessageType.Warning
                                //ActionName = filterContext.ActionDescriptor.ActionName,
                                //ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                //ErrorMessage = Resources.Messages.CalendarNotOpen,
                            };
                            filterContext.Controller.ViewData.Model = msgModel;

                            if (this.ReturnType == EFilterReturnType.View)
                            {
                                filterContext.Result = new ViewResult
                                {
                                    ViewName = "DisplayMessage",
                                    ViewData = filterContext.Controller.ViewData
                                };
                            }
                            else if (this.ReturnType == EFilterReturnType.PartialView)
                            {
                                filterContext.Result = new PartialViewResult
                                {
                                    ViewName = "_DisplayMessage",
                                    ViewData = filterContext.Controller.ViewData
                                };
                            }
                            else
                            {
                                filterContext.Result = new JsonResult
                                {
                                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                                    Data = new CSP.Lib.Models.ResultInfo() { ErrorMessage = msgModel.Message }
                                };
                            }

                        }
                    }
                }
                else
                {
                    // invalid request ไม่เจอ testtypeid
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary                                                        {
                                                            { "controller", "Landing" },
                                                            { "action", "Index" }
                                                        });
                }
            }

            OnActionExecuting(filterContext);
        }
    }

    public enum EFilterReturnType
    {
        View,
        PartialView,
        Json
    }

}
