using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Master;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Login;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Controllers
{
    //扫码点餐中转界面
    //客户扫码先跳转处理扫码的信息以获取入口信息(微信，支付宝等信息)
    public class ScanOrderTransferController : Base_BController
    {
        //扫码信息包括 酒店代码 餐台代码
        public ActionResult Index(string hid, string tabId)
        {
            if (string.IsNullOrEmpty(tabId))
            {
                return SetError("餐台没有设置");
            }
            if (string.IsNullOrEmpty(hid))
            {
                return SetError("酒店代码没有设置，请联系系统管理员");

            }
            HotelInterfacePostModel model = new HotelInterfacePostModel() { Hid = hid };
            try
            {
                var result = JsonHelp.PostDataResult(model, postType.GetHotelSM);
                if (result == null)
                {
                    return SetError();
                }
                else
                {
                    if (result.ErrorNo == "0")
                    {
                        return SetError(result.Msg);
                    }
                    else
                    {

                        var hotelInfo = JsonHelp.Deserialize<HotelSMResultModel>(result.Msg);

                        // 根据返回的信息进行授权
                        var getOpenIdUrl = hotelInfo.GsWxOpenidUrl;
                        var comid = hotelInfo.GsWxComid;
                        if (string.IsNullOrEmpty(getOpenIdUrl) || string.IsNullOrEmpty(comid))
                        {
                            return SetError("微信参数设置出错，请联系系统管理员");
                        }
                        //存储登录信息
                        var currentinfo = GetService<ICurrentInfo>();
                        currentinfo.Clear();
                        currentinfo.GsWxComid = comid;
                        currentinfo.GsWxOpenidUrl = getOpenIdUrl;
                        currentinfo.GsWxCreatePayOrderUrl = hotelInfo.GsWxCreatePayOrderUrl;
                        currentinfo.GsWxPayOrderUrl = hotelInfo.GsWxPayOrderUrl;
                        currentinfo.GsWxTemplateMessageUrl = hotelInfo.GsWxTemplateMessageUrl;

                        if (hotelInfo.IsCs == true)
                        {
                            currentinfo.NotifyUrl = hotelInfo.NotifyURL;
                        }
                        else
                        {
                            currentinfo.NotifyUrl = "";

                        }
                        currentinfo.HotelId = hotelInfo.Hid;
                        currentinfo.SaveValues();

                        //进行微信授权 Hid 为运营平台设置的酒店ID  线下程序为空
                        var backUrl = "http://" + Request.Url.Host.ToString() + "/ScanOrderTransfer/InSingle@hid=" + hotelInfo.Hid + "@tabid=" + tabId;
                        var fullUrl = string.Format("{0}?comid={1}&oauth2=true&url={2}", getOpenIdUrl, Server.UrlEncode(comid), Server.UrlEncode(backUrl));
                        return Redirect(fullUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                return SetError(ex.Message.ToString());
            }
        }

        private ActionResult SetError(string msg = "")
        {
            ViewBag.Title = "错误";
            ViewBag.ErrorInfo = msg == "" ? "酒店信息设置出错" : msg;
            return View("ErrorInfo");
        }


        public ActionResult InSingle(string hid, string tabId, string openId)
        {
            var NotifyUrl = CurrentInfo.NotifyUrl;  //接口地址
            hid = hid ?? "";


            if (string.IsNullOrEmpty(NotifyUrl) && string.IsNullOrEmpty(hid))
            {
                //线上接口
                return SetError();  //线上程序酒店I的必须填写
            }
            //获取酒店信息
            var model = new LoginPostModel() { Hid = hid };
            //调用接口
            var hotelResult = JsonHelp.PostDataResult(model, postType.GetHotelInfo, NotifyUrl);

            if (hotelResult == null)
            {
                return SetError();
            }
            else
            {
                if (hotelResult.ErrorNo == "0")
                {
                    return SetError();
                }
                else
                {
                    var hotelList = JsonHelp.Deserialize<List<LoginResultModel>>(hotelResult.Msg);

                    //获取酒店信息
                    LoginResultModel hotel = new LoginResultModel();
                    if (hotelList.Count > 1)
                    {
                        hotel = hotelList.Where(w => w.Hid == hid).FirstOrDefault();
                    }
                    else
                    {
                        hotel = hotelList.FirstOrDefault();
                    }
                    var currentinfo = GetService<ICurrentInfo>();
                    currentinfo.OpenId = openId;
                    currentinfo.Module = hotel.SysType; //模块
                    currentinfo.SaveValues();

                    //获取餐台信息
                    var tabPostModel = new TabPostModel()
                    {
                        Hid = hid,
                        PosRefeid = "",
                        Module = currentinfo.Module
                    };
                    var tabResult = JsonHelp.PostDataResult(tabPostModel, postType.GetTabList, NotifyUrl);
                    var billId = "";    //账单ID
                    string tabNo = "";  //餐台编码
                    string tabName = "";    //餐台名称
                    if (tabResult == null)
                    {
                        return SetError();
                    }
                    else
                    {
                        if (tabResult.ErrorNo == "0")
                        {
                            return SetError(tabResult.Msg);
                        }
                        else
                        {
                            var tab = JsonHelp.Deserialize<List<TabResultModel>>(tabResult.Msg).Where(w => w.TabId == tabId).FirstOrDefault();    //餐台信息
                            if (tab == null)
                            {
                                return SetError("餐台设置错误！");
                            }
                            if (tab.isBrushOrder == false)
                            {
                                return SetError("店铺已离线，请联系服务员！");
                            }
                            var refeId = tab.RefeId;

                            currentinfo.TabName = tab.TabName;
                            currentinfo.TabNo = tab.TabNo;

                            currentinfo.RefeId = refeId;    //营业点Id
                            currentinfo.TabId = tabId;      //餐台Id
                            currentinfo.WxPaytype = tab.WxPaytype.ToString();

                            currentinfo.SaveValues();

                            if (tab.WxPaytype == (byte)WxPaytype.餐后付款) //后付模式
                            {
                                billId = tab.BillNo;
                            }
                        }
                    }
                    return RedirectToAction("Index", "InSingle_B", new { Tabid = currentinfo.TabId, RefeId = currentinfo.RefeId, BillId = billId });
                    // return Content("");
                }
            }
        }




    }
}