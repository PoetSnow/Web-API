using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Common
{
    /// <summary>
    /// 在有些action是由前端异步请求时，并且返回的数据是json时，增加此属性，以便在发生异常时，返回失败对应的json格式，以便前端可以正常处理，而不是前端无法处理，直接没有任何反应
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class JsonExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                //返回异常JSON
                filterContext.Result = new JsonResult
                {
                    Data = JsonResultData.Failure(filterContext.Exception)
                };
            }
        }
    }
}
