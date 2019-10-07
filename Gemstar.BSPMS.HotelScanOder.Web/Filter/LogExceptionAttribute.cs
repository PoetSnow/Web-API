using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Logs;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Filter
{
    /// <summary>
    /// 通用的异常错误信息记录
    /// 站点所有页面在异常发生后，均需要记录异常日志，并转向错误提示页面（异常内容的详略程度由具体需求决定）、
    /// 所有返回JSON数据的异步请求，不但需要记录异常日志，而且需要向客户端返回JSON格式的错误信息提示，而不是转向错误提示页面（异步请求也不可能转向错误提示页面）
    /// 采用AOP思想，将异常处理解耦
    /// 尽量精简声明Attribute的重复代码
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class LogExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                try
                {

                    string hid = "";
                    string module = "";
               
                    if (filterContext.HttpContext.Session != null)
                    {
                        hid = filterContext.HttpContext.Session["HotelId"] as string;
                        module = filterContext.HttpContext.Session["Module"] as string;
                    }
                    var operLog = new OperLog();
                    var content = operLog.GetLogInstanceFromExceptionContext(filterContext);
                    var posData = new OperLogPostModel()
                    {
                        Hid = hid,
                        OperType = "错误",
                        OperContent = content
                    };
                    JsonHelp.PostDataResult(posData, PostType.OperLog);
                    //logService.AddSysLog(filterContext, username);
                }
                catch
                {
                    //记录异常时如果重新抛出异常则不需要记录，继续处理之前的业务异常
                }
               
            }

            if (filterContext.Result is JsonResult)
            {
                //当结果为json时，设置异常已处理
                filterContext.ExceptionHandled = true;
            }
            else
            {
                //否则调用原始设置
                base.OnException(filterContext);
            }
        }
    }
}