using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.SPA;
using Gemstar.BSPMS.HotelScanOrder.Web.Areas.SpecialtyStore.Models;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Resources;
using System.Threading;
using Gemstar.BSPMS.HotelScanOrder.Common.SPAEnum;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Areas.SpecialtyStore.Controllers
{

    public class HomeController : BaseController
    {
        // GET: SpecialtyStore/Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Order(decimal price=0,string handcode="")
        {
            var sendata = new StoreOrderPostModel { price = price, handcode = handcode, computer = CurrentInfo.SNCode };
            var res = base.GetPostData<StoreOrderResultModel>(sendata, PostType.SPA_StoreOrder, out PostErrorModel postErrorModel);
            if (res == null || res.Count ==0)  //出现错误
            {
                if (postErrorModel != null)
                {
                    return Json(JsonResultData.Failure(Resources.GlobalResource.ResourceManager, Thread.CurrentThread.CurrentUICulture, postErrorModel.Msg));
                }
                else
                {
                    return Json(JsonResultData.Failure(Resources.GlobalResource.ResourceManager, Thread.CurrentThread.CurrentUICulture, "接口访问错误"));
                }              
            }
            var _data = res.First();
            if (_data.Code == (int)SPAPostStatus.Success)
            {
                return Json(JsonResultData.Successed());
            }
            else
            {
                return Json(JsonResultData.Failure(Resources.GlobalResource.ResourceManager, Thread.CurrentThread.CurrentUICulture, _data.Msg));
            }           
        }



        public ActionResult Success()
        {
            return View();
        }

    }
}