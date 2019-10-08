using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Controllers
{

    public class TabController : BaseController
    {

        /// <summary>
        /// 餐台列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            RefePostModel model = new RefePostModel
            {
                Hid = CurrentInfo.HotelId ?? "",
                UserCode = CurrentInfo.UserCode,
                Module = CurrentInfo.Module ?? ""
            };
            if (string.IsNullOrEmpty(CurrentInfo.RefeList))
            {
                GetRefeList(model); //获取营业点

            }
            var refeList = JsonHelp.Deserialize<List<RefeResultModel>>(CurrentInfo.RefeList);
            if (refeList != null)
            {
                //如果用户只对应营业点。判断是否进入餐台
                if (refeList.Count == 1)
                {
                    var refe = refeList.FirstOrDefault();
                    if (refe.IsRefeType == 1)
                    {
                        RedirectToRoute(new { controller = "InSingle", action = "Index" });
                    }
                }
                ViewBag.RefeList = JsonHelp.Deserialize<List<RefeResultModel>>(CurrentInfo.RefeList);    //营业点
                var refeId = "";
                var split = "";
                foreach (var refe in refeList)
                {
                    refeId += split + refe.RefeId;
                    split = ",";
                }

                string TabSrt = "";
                //获取餐台
                ViewBag.TabList = GetTabList(new TabPostModel { Hid = CurrentInfo.HotelId ?? "", PosRefeid = refeId, Module = CurrentInfo.Module ?? "" }, out TabSrt);
            }

            //   ViewBag.TabSrt = TabSrt;
            return View();

        }

        #region 加载数据

        /// <summary>
        /// 获取分厅列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private void GetRefeList(RefePostModel model)
        {
            var result = JsonHelp.PostDataResult(model, postType.GetRefeList, CurrentInfo.NotifyUrl);

            if (result != null)
            {
                if (result.ErrorNo == "1")
                {

                    //var serializer = new JavaScriptSerializer();
                    //var results = serializer.Deserialize<List<RefeResultModel>>(result.Msg);

                    //把营业点json字符串存储到session 中
                    var currentinfo = GetService<ICurrentInfo>();
                    currentinfo.RefeList = result.Msg;
                    currentinfo.SaveValues();
                    // return results;
                }
            }
        }

        /// <summary>
        /// 获取餐台数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="data"></param>
        /// <returns>返回餐台列表以及餐台列表json 字符串</returns>
        private List<TabResultModel> GetTabList(TabPostModel model, out string data)
        {
            data = "";
            //获取接口数据
            var result = JsonHelp.PostDataResult(model, postType.GetTabList, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    data = result.Msg;
                    return JsonHelp.Deserialize<List<TabResultModel>>(result.Msg);

                }
                else
                {

                    return new List<TabResultModel>();
                }
            }
            return new List<TabResultModel>();

        }
        #endregion


        /// <summary>
        /// 获取餐台状态
        /// </summary>
        /// <param name="refeId"></param>
        /// <returns></returns>
        public PartialViewResult _TabStatus(string refeId)
        {
            string tabList = "";


            //营业点为空 取当前用户对应的全部营业点
            if (string.IsNullOrEmpty(refeId))
            {
                var refeList = JsonHelp.Deserialize<List<RefeResultModel>>(CurrentInfo.RefeList);
                if (refeList != null)
                {

                    var split = "";
                    foreach (var refe in refeList)
                    {
                        refeId += split + refe.RefeId;
                        split = ",";
                    }
                }
            }
            var list = GetTabList(new TabPostModel { Hid = CurrentInfo.HotelId ?? "", PosRefeid = refeId, Module = CurrentInfo.Module ?? "" }, out tabList);
            return PartialView("_TabStatus", list);
        }

    }
}