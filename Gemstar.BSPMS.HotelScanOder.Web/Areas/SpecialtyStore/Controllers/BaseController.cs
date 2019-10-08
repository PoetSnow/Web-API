using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Web.Areas.SpecialtyStore.Models;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Areas.SpecialtyStore.Controllers
{
    public class BaseController : Controller
    {

        public PostType postType { get { return new PostType(); } }

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


        /// <summary>
        /// 通用获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setdata">发送信息</param>
        /// <param name="posttype">服务类型</param>
        /// <param name="postErrorModel">错误信息回传</param>
        /// <param name="hid">酒店ID</param>
        /// <returns></returns>
        protected List<T> GetPostData<T>(object setdata, string posttype, out PostErrorModel postErrorModel, string hid = "")
        {
            postErrorModel = null;
            var data = "";
            List<T> storeOrderResultModels;
            //获取接口数据
            var result = JsonHelp.PostDataResult(setdata, posttype);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    data = result.Msg;
                    return storeOrderResultModels = JsonHelp.Deserialize<List<T>>(result.Msg);
                }
                else
                {
                    postErrorModel = new PostErrorModel() { Code = -1, Msg = result.Msg };
                }
            }
            else
            {
                postErrorModel = new PostErrorModel() { Code = -1, Msg = "接口数据转换错误" };
            }
            return null;
        }

    }
}