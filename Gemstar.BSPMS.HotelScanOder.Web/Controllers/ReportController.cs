using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Report;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        //账单列表
        [HttpGet]
        public ActionResult BillList()
        {
            var model = new ReportBillListPostModel
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                As_FindType = "1",
                As_StartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                As_EndDate = DateTime.Now.ToString("yyyy-MM-dd")
            };

            return View(model);
        }

        //账单明细列表(穿透)
        public ActionResult BillDetailList(string billno)
        {
            var billDetailList = GetBillDetailListData(new ReportBillDetailListPostModel { As_BillNo = billno, Hid = CurrentInfo.HotelId ?? "", Module = CurrentInfo.Module ?? "" });
            return View(billDetailList);
        }

        public ActionResult InBalanceList()
        {
            var model = new ReportInBalanceListPostModel
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                As_FindType = "1",
                As_StartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                As_EndDate = DateTime.Now.ToString("yyyy-MM-dd")
            };

            return View(model);
        }

        //销售排行报表
        public ActionResult SellDetailList()
        {
            var model = new ReportSellDetailListPostModel
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                As_FindType = "1",
                As_StartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                As_EndDate = DateTime.Now.ToString("yyyy-MM-dd"),
                As_DeptClassNo = "",
            };

            return View(model);
        }

        public JsonResult GetBillListData(ReportBillListPostModel model)
        {
            model.Refeid = model.Refeid == null ? "" : model.Refeid;
            model.PayModeId = model.PayModeId == null ? "" : model.PayModeId;

            //获取接口数据
            var result = JsonHelp.PostDataResult(model, postType.GetReport_BillList, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                return Json(JsonHelp.Deserialize<List<ReportBillListResultModel>>(result.Msg), JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public List<ReportBillDetailListResultModel> GetBillDetailListData(ReportBillDetailListPostModel model)
        {
            //获取接口数据
            var result = JsonHelp.PostDataResult(model, postType.GetReport_BillDetailList, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                return JsonHelp.Deserialize<List<ReportBillDetailListResultModel>>(result.Msg);
            }

            return new List<ReportBillDetailListResultModel>();
        }

        public JsonResult GetInBalanceListData(ReportInBalanceListPostModel model)
        {
            model.Refeid = model.Refeid == null ? "" : model.Refeid;
            model.Hid = CurrentInfo.HotelId ?? "";
            model.Module = CurrentInfo.Module ?? "";

            //获取接口数据
            var result = JsonHelp.PostDataResult(model, postType.GetReport_InBalanceList, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                return Json(JsonHelp.Deserialize<List<ReportInBalanceListResultModel>>(result.Msg), JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSellDetailList(ReportSellDetailListPostModel model)
        {
            model.Refeid = model.Refeid == null ? "" : model.Refeid;
            model.As_DeptClassNo = model.As_DeptClassNo == null ? "" : model.As_DeptClassNo;
            model.Hid = CurrentInfo.HotelId ?? "";
            model.Module = CurrentInfo.Module ?? "";

            //获取接口数据
            var result = JsonHelp.PostDataResult(model, postType.GetReport_SellDetailList, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                return Json(JsonHelp.Deserialize<List<ReportSellDetailListResultModel>>(result.Msg), JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}