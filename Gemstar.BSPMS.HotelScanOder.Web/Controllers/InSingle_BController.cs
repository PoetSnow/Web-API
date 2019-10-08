using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Action;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayMethod;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Request;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;
using Gemstar.BSPMS.HotelScanOrder.Web.Models.Pay;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Controllers
{
    /// <summary>
    /// 扫码点餐用户使用的控制器
    /// </summary>
    public class InSingle_BController : Base_BController
    {
        public ActionResult Index(OpenTabView model)
        {

            //当前餐台Id 不等于缓存的餐台ID，调用接口获取数据
            var currentinfo = GetService<ICurrentInfo>();
            currentinfo.RefeId = model.RefeId; //当前操作的营业点ID存储到session中
            currentinfo.SaveValues();

            var postItemList = commonHelper.SetItemListB(model.RefeId, model.Tabid);

            //消费项目大类
            var itemSubClass = postItemList.MyDistinct(w => w.Itemsubid).ToList();

            //取第一个分类下的菜式
            var itemList = new List<ItemResultModel>();
            foreach (var item in postItemList)
            {
                var s = postItemList.Where(w => w.Itemsubid == item.Itemsubid && w.IsDefault == true).ToList();
                itemList.AddRange(s);
                break;
            }

            ViewBag.itemSubClass = itemSubClass;    //分类
            ViewBag.itemList = itemList;            //消费项目
            ViewBag.OpenTabView = model;
            ViewBag.OpenId = CurrentInfo.OpenId;    //openId
            ViewBag.WxPaytype = currentinfo.WxPaytype;      //扫码点菜支付方式
            ViewBag.TabNo = currentinfo.TabNo;      //餐台编码
            ViewBag.TabName = currentinfo.TabName;      //餐台明细

            return View();
        }
        /// <summary>
        /// 分部视图，根据分类ID 获取消费项目
        /// </summary>
        /// <param name="subClassId">分类ID</param>
        /// <param name="tabId">餐台ID</param>
        /// <param name="refeId">营业点ID</param>
        /// <returns></returns>
        public PartialViewResult _ItemList(string subClassId, string tabId, string refeId)
        {
            var postItemList = commonHelper.SetItemListB(refeId, tabId);
            ViewBag.specFlag = "1";
            //筛选分类下的消费项目
            var itemList = new List<ItemResultModel>();
            foreach (var item in postItemList)
            {
                var s = postItemList.Where(w => w.Itemsubid == subClassId && w.IsDefault == true).ToList();
                itemList.AddRange(s);
                break;
            }

            return PartialView("_ItemList", itemList);
        }

        #region 规格业务代码

        /// <summary>
        /// 有规格的消费项目查询出单位以及作法列表
        /// </summary>
        /// <param name="itemId">消费项目ID</param>
        /// <returns></returns>
        public JsonResult SpecList(string itemId, string tabId, string refeId)
        {
            var postItemList = commonHelper.SetItemListB(refeId, tabId);
            //单位列表
            var unitList = postItemList.Where(w => w.ItemId == itemId).ToList();

            //要求列表
            var requestList = commonHelper.GetRequestList(refeId);

            //作法列表
            var actionList = commonHelper.GetActionListByItemId(itemId);

            //返回结果
            var result = new
            {
                Unit = unitList,
                Request = requestList,
                Action = actionList
            };
            //查询消费项目
            return Json(JsonResultData.Successed(result));
        }


        #endregion


        /// <summary>
        /// 已点菜式
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _OrderList(string tabId, string refeId, string detailStr)
        {
            var currentinfo = GetService<ICurrentInfo>();
            List<OrderListModel> result = new List<OrderListModel>();
            //要求列表
            var request = commonHelper.GetRequestList(refeId);

            //账单明细
            List<billDetailViewModel> detailList = new List<billDetailViewModel>();

            //还未落单的数据
            if (!string.IsNullOrEmpty(detailStr))
            {
                detailList = JsonHelp.Deserialize<List<billDetailViewModel>>(detailStr);
            }

            List<BillDetailResultModel> detail = new List<BillDetailResultModel>();
            //调用接口查询已经落单的账单明细
            if (Convert.ToByte(currentinfo.WxPaytype) == (byte)WxPaytype.餐后付款) //如果是餐后付款 需要查询出之前的账单以及账单明细信息
            {
                detail = commonHelper.GetBillQuery(refeId, tabId);
            }
            foreach (var item in detail)
            {
                var billDetail = new billDetailViewModel();
                commonHelper.SetModel(billDetail, item);
                //已经存在的数据查询出来
                detailList.Add(billDetail);

            }
            decimal? totAmount = 0;
            //查询做法
            foreach (var item in detailList)
            {
                if (item.Tagcharge != (int)PosbillDetailStatus.保存)
                {
                    totAmount = item.TotAmount;

                }
                OrderListModel model = new OrderListModel();
                model.RequestList = request;    //要求

                //单位列表
                var postItemList = commonHelper.SetItemListB(refeId, tabId);
                model.UnitList = postItemList.Where(w => w.ItemId == item.ItemId).ToList();

                //做法
                if (item.IsAutoaction != "False")
                    model.ActionList = commonHelper.GetActionListByItemId(item.ItemId);
                else
                    model.ActionList = null;

                var actionListB = new List<BillDetailActionModel>();
                //作法分组
                var actionGroupList = new List<BillDetailActionGroup>();
                if (item.Tagcharge == (int)PosbillDetailStatus.保存)
                {
                    var actionList = JsonHelp.Deserialize<List<BillDetailActionModel>>(item.ActioName ?? "");
                    if (actionList != null && actionList.Count > 0)
                    {
                        //  actionList.OrderBy(w => w.groupid);
                        actionList.Sort((a, b) => a.groupid.CompareTo(b.groupid));
                        //通过分组ID进行分组查询出需要的数据
                        foreach (IGrouping<string, BillDetailActionModel> group in actionList.GroupBy(x => x.groupid))
                        {
                            var s = new BillDetailActionGroup();
                            string resultSplit = "";
                            s.groupid = group.Key;
                            foreach (var actionModel in group)
                            {
                                s.ActionIds += resultSplit + actionModel.actionid;
                                s.ActionNames += resultSplit + actionModel.actionname;
                                resultSplit = ",";
                            }
                            actionGroupList.Add(s);
                        }

                    }

                }
                else
                {
                    //调用接口查询做法
                    var actionList = commonHelper.GetBillDetailAction(refeId, tabId).Where(w => w.RowId == item.RowId);
                    //数据库的做法根据前端规格赋值
                    foreach (var action in actionList)
                    {
                        BillDetailActionModel actionModel = new BillDetailActionModel()
                        {
                            rowid = action.RowId.ToString(),
                            groupid = action.GroupId.ToString(),
                            actionid = action.ActionId,
                            actionname = action.ActionName,
                            addprice = action.AddPrice.ToString(),
                            multiple = "",
                            deptclassno = "",
                            pdano = "",
                            status = "0"

                        };
                        actionListB.Add(actionModel);
                    }
                    if (actionList != null && actionListB.Count > 0)
                    {
                        actionList = actionList.OrderBy(w => w.GroupId).ToList();
                        foreach (IGrouping<int?, GetBillDetailActionResultModel> group in actionList.GroupBy(x => x.GroupId))
                        {
                            var s = new BillDetailActionGroup();
                            string resultSplit = "";
                            s.groupid = group.Key.ToString();
                            foreach (var actionModel in group)
                            {
                                s.ActionIds += resultSplit + actionModel.ActionId;
                                s.ActionNames += resultSplit + actionModel.ActionName;
                                resultSplit = ",";
                            }
                            actionGroupList.Add(s);
                        }
                    }
                }
                model.ActionGroupList = actionGroupList;

                //账单明细
                model.BillDetail = item;
                model.ActionListStr = JsonHelp.Serialize(actionListB);
                result.Add(model);

            }

            //是否显示买单

            ViewBag.WxPaytype = currentinfo.WxPaytype;  //点菜支付，餐后支付
            ViewBag.TotAmount = totAmount;

            return PartialView("_OrderList", result);
        }


        /// <summary>
        /// 处理账单明细数据
        /// </summary>
        /// <param name="tabId">餐台ID</param>
        /// <param name="refeId">营业点ID</param>
        /// <param name="billDetailStr">账单明细</param>
        /// <param name="keyId">锁牌ID</param>
        /// <returns></returns>
        public ActionResult InBillDetail(string billId, string openTabStr, string tabId, string refeId, string billDetailStr, string keyId)
        {
            var result = AddBillDetail(billId, openTabStr, tabId, refeId, billDetailStr, keyId);
            return Json(result);
        }

        private JsonResultData AddBillDetail(string billId, string openTabStr, string tabId, string refeId, string billDetailStr, string keyId)
        {
            #region 开台
            //判断餐台是否已经开台
            if (string.IsNullOrEmpty(billId))
            {
                //餐台未开台 调用服务程序开台 再进行操作消费项目
                InsBillPostDataModel postModel = new InsBillPostDataModel()
                {
                    Hid = CurrentInfo.HotelId ?? "",
                    Module = CurrentInfo.Module ?? "",
                    RefeId = refeId,
                    TabId = tabId,
                    UserCode = CurrentInfo.UserCode,
                    OperType = "2",         //默认传2 代表新增同时产生开台项目
                    OpenData = openTabStr,
                    KeyId = keyId ?? ""
                };
                var result = JsonHelp.PostDataResult(postModel, postType.InBill, CurrentInfo.NotifyUrl);
                if (result != null)
                {
                    if (result.ErrorNo == "1")
                    {

                        var s = JsonHelp.Deserialize<InsBillResultModel>(result.Msg);
                        billId = s.Billno;
                    }
                    else
                    {
                        return JsonResultData.Failure(result.Msg);
                    }
                }
                else
                {
                    return JsonResultData.Failure("开台失败！");
                }
            }
            #endregion

            List<InBillDetailModel> detailList = new List<InBillDetailModel>();

            //还未落单的数据
            if (!string.IsNullOrEmpty(billDetailStr))
            {
                detailList = JsonHelp.Deserialize<List<InBillDetailModel>>(billDetailStr);
            }

            var actionList = new List<BillDetailActionModel>(); //作法集合
            foreach (var item in detailList)
            {
                //作法
                if (!string.IsNullOrEmpty(item.actions))
                {
                    var s = JsonHelp.Deserialize<List<BillDetailActionModel>>(item.actions);
                    if (s != null)
                    {
                        foreach (var action in s)
                        {
                            action.rowid = item.rowid.ToString();
                            action.opertype = action.status;
                            actionList.Add(action);
                        }

                    }
                }

                item.actions = commonHelper.SplicingAction(actionList);
                if (item.status == (int)PosbillDetailStatus.保存)
                {
                    item.status = (int)PosbillDetailStatus.正常;
                }

            }

            InsBillDetailPostModel billDetailModel = new InsBillDetailPostModel()
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                RefeId = refeId,
                TabId = tabId,
                UserCode = CurrentInfo.UserCode,
                OperType = "1",
                OrderItem = JsonHelp.Serialize(detailList).Replace("\\", ""),
                OrderAction = JsonHelp.Serialize(actionList).Replace("\\", ""),
                KeyId = keyId ?? "",
                BillId = billId ?? ""
            };
            try
            {
                var detailResult = JsonHelp.PostDataResult(billDetailModel, postType.InBillDetail, CurrentInfo.NotifyUrl);
                if (detailResult != null)
                {
                    if (detailResult.ErrorNo == "1")
                    {
                        //重算金额
                        CmpBill(refeId, tabId, keyId, billId);
                        return JsonResultData.Successed(billId);
                    }
                    else
                    {
                        return JsonResultData.Failure(detailResult.Msg);
                    }
                }
                else
                {
                    return JsonResultData.Failure("落单失败，请重试");
                }
            }
            catch (Exception ex)
            {

                return JsonResultData.Failure(ex.Message.ToString());
            }
        }


        /// <summary>
        /// 调用接口重算金额 并且返回金额用于支付
        /// </summary>
        /// <param name="refeId"></param>
        /// <param name="tabId"></param>
        /// <param name="keyid"></param>
        public void CmpBill(string refeId, string tabId, string keyid, string billId)
        {
            BillCmpPostModel postModel = new BillCmpPostModel()
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                RefeId = refeId,
                TabId = tabId,
                UserCode = CurrentInfo.UserCode,
                IsSave = CurrentInfo.WxPaytype == "1" ? 1 : 0,  //点菜支付不需要出品，餐后支付需要出品
                KeyId = keyid ?? "",
                BillId = billId
            };
            var result = JsonHelp.PostDataResult(postModel, postType.BillCmp, CurrentInfo.NotifyUrl);

        }


        /// <summary>
        /// 扫码点餐微信支付，直接调用接口
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult PayByWX(string billId, string openTabStr, string tabId, string refeId, string billDetailStr, string keyId, string WxPaytype)
        {
            //点菜先付模式 先开台 插入消费项目
            if (WxPaytype == "1")
            {
                var result = AddBillDetail(billId, openTabStr, tabId, refeId, billDetailStr, keyId);
                if (result.Success == true)
                {
                    billId = result.Data.ToString();
                    return SetPayModel(refeId, billId, tabId, keyId);
                }

            }
            else
            {
                return SetPayModel(refeId, billId, tabId, keyId);
            }

            return Json(JsonResultData.Failure("买单失败"));
        }


        private JsonResult SetPayModel(string refeId, string billId, string tabId, string keyId)
        {
            //获取付款方式
            var payMethodList = commonHelper.GetPayMethodList(refeId);


            var amount = commonHelper.GetBillQuery(refeId, tabId, "", billId).FirstOrDefault().TotAmount;

            GetPayMethodResultModel payMethod = null;

            if (payMethodList != null && payMethodList.Count > 0)
            {
                payMethod = payMethodList.Where(w => w.Code == "WxBarcode").FirstOrDefault();
            }
            var _settleid = Guid.NewGuid();

            var payBillNamesModel = new PayBillNamesPostModel()
            {
                paymodeid = payMethod.PaymodeId,
                payCode = payMethod.Code,
                payamount = Convert.ToDecimal(amount),
                dueamount = Convert.ToDecimal(amount),
                memo = "扫码点餐微信支付",
                settleid = _settleid,
                prepayID = _settleid.ToString("N"),
                OpenId = CurrentInfo.OpenId

            };
            var model = new PayBillPostModel()
            {
                Billno = billId,
                RefeId = refeId,
                TabId = tabId ?? "",
                operType = (byte)PosbillDetailStatus.付款方式作废,
                PayNames = JsonHelp.Serialize(payBillNamesModel),
                KeyId = keyId ?? "",
                Hid = CurrentInfo.HotelId,
                Module = CurrentInfo.Module,
                UserCode = "扫码点餐微信支付",
            };
            //调用接口产生待支付数据
            var result = JsonHelp.PostDataResult(model, postType.PayBill, CurrentInfo.NotifyUrl);
            if (result.ErrorNo == "1")
            {
                // var payResult = JsonHelp.Deserialize<List<PayBillResultModel>>(result.Msg).ToList().FirstOrDefault();
                return PayWx(payBillNamesModel, model, CurrentInfo.NotifyUrl);
            }
            else
            {
                return Json(JsonResultData.Failure("买单失败！" + result.Msg));
            }

        }

        /// <summary>
        /// 微信买单
        /// </summary>
        /// <param name="payBillNamesModel">买单参数</param>
        /// <param name="model">接口调用参数</param>
        /// <param name="postNotifyUrl">接口</param>
        /// <returns></returns>

        private JsonResult PayWx(PayBillNamesPostModel payBillNamesModel, PayBillPostModel model, string postNotifyUrl)
        {
            //调用支付接口
            var comid = CurrentInfo.GsWxComid;
            var createOrderUrl = CurrentInfo.GsWxCreatePayOrderUrl;
            var payUrl = CurrentInfo.GsWxPayOrderUrl;
            if (string.IsNullOrEmpty(comid) || string.IsNullOrEmpty(createOrderUrl) || string.IsNullOrEmpty(payUrl))
            {
                return Json(JsonResultData.Failure("缺少必要支付参数"));
            }

            var notifyUrl = new StringBuilder().Append("http://" + Request.Url.Host.ToString() + "/InSingle_B/PaymentSuccess");

            CurrentInfo.PayBillNameStr = JsonHelp.Serialize(payBillNamesModel);
            CurrentInfo.PayBillPostModelStr = JsonHelp.Serialize(model);
            CurrentInfo.SaveValues();

            // 2 组建相关参数预下单
            //  var notifyUrl = Url.Encode(string.Format("http://" + Request.Url.Host.ToString() + "/InSingle_B/PaymentSuccess?hid={0}&tabid={1}&billid={2}&billDetailId={3}", CurrentInfo.HotelId ?? "", tabId, billId, mId));
            // var returnUrl = Url.Encode(string.Format("http://{0}/ScanOrder/Order/_OrderInfo?hid={1}&tabid={2}&openid={3}&billid={4}", Request.Url.Host.ToString(), hid, tabid, openid, billid));

            var hotelCode = CurrentInfo.HotelId;

            var orderSn = payBillNamesModel.settleid.ToString("N");
            var prem = new Dictionary<string, string>
                        {
                            { "name", "扫码点餐" },
                            { "businessId", model.Billno },
                            { "totalFee", (decimal.Round(Convert.ToDecimal(payBillNamesModel.payamount) * 100)).ToString("0")},
                            { "orderSn",orderSn },
                            { "hotelCode",hotelCode},
                            { "payMode", "PMS" },
                            { "notifyUrl", notifyUrl.ToString()},
                            { "returnUrl", notifyUrl.ToString()}
                        };

            string res = HttpHelper.Post(createOrderUrl, prem);
            if (!res.Contains("200"))
            {
                return Json(JsonResultData.Failure("创建订单失败：" + res));
            }
            //返回支付链接
            return Json(JsonResultData.Successed(new { Parameters = payUrl + "?comid=" + comid + "&orderSn=" + orderSn, type = notifyUrl.ToString() }), JsonRequestBehavior.AllowGet);


        }

        public ActionResult PaymentSuccess(string payBillNames, string PayBillPostStr, string postNotifyUrl)
        {
            //修改账单状态
            var PayBillNameStr = CurrentInfo.PayBillNameStr;
            var PayBillPostModelStr = CurrentInfo.PayBillPostModelStr;

            var payBillNamesModel = JsonHelp.Deserialize<PayBillNamesPostModel>(PayBillNameStr);
            var postPayBillPostModel = JsonHelp.Deserialize<PayBillPostModel>(PayBillPostModelStr);
            var notifyUrl = CurrentInfo.NotifyUrl;


            ViewBag.RefeId = postPayBillPostModel.RefeId; //营业点Id
            ViewBag.TabId = postPayBillPostModel.TabId; //餐台Id

            payBillNamesModel.prepayID = payBillNamesModel.settleid.ToString("N");
            postPayBillPostModel.operType = (byte)PosbillDetailStatus.正常;      //再次调用接口

            postPayBillPostModel.PayNames = JsonHelp.Serialize(payBillNamesModel);
            var result = JsonHelp.PostDataResult(postPayBillPostModel, postType.PayBill, notifyUrl);
            //return cmpPostApiResult(result);
            return View();
        }
        /// <summary>
        /// 查询菜式界面
        /// </summary>
        /// <returns></returns>
        public ActionResult _QueryItemList(string title)
        {
            ViewBag.Title = title;
            return PartialView("_QueryItemList");
        }

        public ActionResult _QueryItemResultList(string searchName, string tabId, string refeId)
        {
            var postItemList = commonHelper.SetItemListB(refeId, tabId);
            List<ItemResultModel> itemList = new List<ItemResultModel>();
            itemList = postItemList.Where(s => (s.ItemName.Contains(searchName) || s.IndexNo.ToLower().Contains(searchName.ToLower()) || s.WbNo.ToLower().Contains(searchName.ToLower()) || s.barCode.Contains(searchName)) && s.IsDefault == true).ToList();
            ViewBag.specFlag = "0";
            return PartialView("_ItemList", itemList);
        }
    }
}