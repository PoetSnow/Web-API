using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Web.Filter;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;
using Newtonsoft.Json;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Controllers
{
    [LoginAuthorize]
    public class BaseController : Controller
    {
        /// <summary>
        /// 实例化业务处理类
        /// </summary>

        protected PostDataHelper commonHelper = new PostDataHelper();

        /// <summary>
        /// 获取指定服务接口的实例
        /// </summary>
        /// <typeparam name="T">服务接口类型</typeparam>
        /// <returns>指定服务接口的实例</returns>
        protected T GetService<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }


        private ICurrentInfo _currentInfo;

        protected ICurrentInfo CurrentInfo
        {
            get
            {
                if (_currentInfo == null)
                {
                    _currentInfo = GetService<ICurrentInfo>();
                }
                return _currentInfo;
            }
        }


    }
}