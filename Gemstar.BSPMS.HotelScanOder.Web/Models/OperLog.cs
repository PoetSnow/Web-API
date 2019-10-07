using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models
{
    public class OperLog
    {
        public string GetLogInstanceFromExceptionContext(ExceptionContext exceptionContext)
        {
            StringBuilder str = new StringBuilder("");


            var routeData = exceptionContext.RouteData;
            var controllerName = routeData.Values["controller"] as string;
            var actionName = routeData.Values["action"] as string;
            var areaName = "";
            if (routeData.DataTokens.ContainsKey("area"))
            {
                areaName = routeData.DataTokens["area"].ToString();
            }
            str.Append("Url：" + string.Format("/{0}/{1}/{2}", areaName, controllerName, actionName) + "");
            //   log.Url = string.Format("/{0}/{1}/{2}", areaName, controllerName, actionName);

            //var controllerDescriptor = new ReflectedControllerDescriptor(exceptionContext.Controller.GetType());
            //var businessTypeAttributes = controllerDescriptor.GetCustomAttributes(typeof(BusinessTypeAttribute), false);
            //var businessTypeName = businessTypeAttributes.Length > 0 ? ((BusinessTypeAttribute)businessTypeAttributes[0]).Name : controllerDescriptor.ControllerName;
            //log.Name = businessTypeName;

            var request = exceptionContext.HttpContext.Request;
            //var remoteIp = UrlHelperExtension.GetRemoteClientIPAddress(request);
            //log.Ip = remoteIp;


            var infoBuilder = new StringBuilder();
            infoBuilder.AppendFormat("异常信息:{0}", exceptionContext.Exception.Message).AppendLine();
            if (exceptionContext.Exception.InnerException != null)
            {
                var inner = exceptionContext.Exception.InnerException;
                while (inner.InnerException != null)
                {
                    inner = inner.InnerException;
                }
                infoBuilder.AppendFormat("内部异常信息:{0}", inner.Message).AppendLine();
            }
            infoBuilder.AppendFormat("调用堆栈:{0}", exceptionContext.Exception.StackTrace).AppendLine();

            var keys = request.QueryString.Keys;
            infoBuilder.Append("调用时的查询参数:");
            foreach (string key in keys)
            {
                infoBuilder.AppendFormat("{0}:{1};", key, request.QueryString[key]);
            }
            infoBuilder.AppendLine();
            keys = request.Form.Keys;
            infoBuilder.Append("调用时的form参数:");
            foreach (string key in keys)
            {
                infoBuilder.AppendFormat("{0}:{1};", key, request.Form[key]);
            }
            infoBuilder.AppendLine();
            keys = request.Cookies.Keys;
            infoBuilder.Append("调用时的cookie参数:");
            foreach (string key in keys)
            {
                infoBuilder.AppendFormat("{0}:{1};", key, request.Cookies[key].Value);
            }
            infoBuilder.AppendLine();
            str.AppendFormat("Info：" + infoBuilder.ToString());
            //  log.Info = infoBuilder.ToString();

            return str.ToString();
        }
    }
}