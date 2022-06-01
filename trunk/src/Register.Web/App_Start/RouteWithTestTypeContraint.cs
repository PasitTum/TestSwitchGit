using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Text.RegularExpressions;

namespace Register.Web.Contraints
{
    // ถ้าไม่มีการระบุ Testtypeid ที่ url ไม่ให้ไปต่อ
    public class RouteWithTestTypeContraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext,
                            Route route,
                            string parameterName,
                            RouteValueDictionary values,
                            RouteDirection routeDirection
                        )
        {
            // ถ้าไม่มีการระบุ Testtypeid ที่ url ไม่ให้ไปต่อ
            // return httpContext.Request.IsLocal;
             return values.ContainsKey("testtypeid") && Regex.IsMatch((string)values["testtypeid"], @"\d+");
        }
    }
}