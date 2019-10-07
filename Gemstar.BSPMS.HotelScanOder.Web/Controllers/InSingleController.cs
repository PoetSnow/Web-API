using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Controllers
{

    public class InSingleController : BaseController
    {
        /// <summary>
        /// 点菜主界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(OpenTabView model)
        {

            //当前餐台Id 不等于缓存的餐台ID，调用接口获取数据
            var currentinfo = GetService<ICurrentInfo>();
            currentinfo.RefeId = model.RefeId; //当前操作的营业点ID存储到session中
            currentinfo.TabNo = model.TabNo;
            currentinfo.TabName = model.TabName;
            currentinfo.SaveValues();

            var postItemList = commonHelper.SetItemList(model.RefeId, model.Tabid);
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
            ViewBag.TabNo = currentinfo.TabNo;      //餐台编码
            ViewBag.TabName = currentinfo.TabName;      //餐台明细

            return View();

        }

        /// <summary>
        /// 强制刷新 获取菜式信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRefreshItem(string subClassId, string refeId, string tabId)
        {
            ItemPostModel postModel = new ItemPostModel
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                Posrefeid = refeId,
                Usercode = CurrentInfo.UserCode
            };

            //强制调用接口获取菜式信息
            var currentinfo = GetService<ICurrentInfo>();

            var postItemList = new List<ItemResultModel>();
            var _itemListStr = "";

            //获取接口查询出来的数据
            postItemList = commonHelper.GetItemList(postModel, out _itemListStr);
            //最新的信息存储到缓存中
            currentinfo.TabId = tabId;
            currentinfo.ItemList = _itemListStr;
            currentinfo.SaveValues();

            //获取当前市别等信息
            var sysInfo = commonHelper.GetSysInfo(refeId);
            postItemList = postItemList.Where(w => w.Shuffleid.IndexOf(sysInfo.ShuffleId) > -1).ToList();
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
        #region 获取消费项目信息


        #endregion



        /// <summary>
        /// 分部视图，根据分类ID 获取消费项目
        /// </summary>
        /// <param name="subClassId">分类ID</param>
        /// <param name="tabId">餐台ID</param>
        /// <param name="refeId">营业点ID</param>
        /// <returns></returns>
        public PartialViewResult _ItemList(string subClassId, string tabId, string refeId)
        {

            var postItemList = commonHelper.SetItemList(refeId, tabId);
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
            var postItemList = commonHelper.SetItemList(refeId, tabId);
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

            //调用接口查询已经落单的账单明细
            var detail = commonHelper.GetBillQuery(refeId, tabId);
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
                var postItemList = commonHelper.SetItemList(refeId, tabId);
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
                        // actionList.Sort((a, b) => a.GroupId.CompareTo(b.GroupId));
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
            var refeList = JsonHelp.Deserialize<List<RefeResultModel>>(CurrentInfo.RefeList);
            if (refeList != null && refeList.Count > 0)
            {
                //如果用户只对应营业点。判断是否进入餐台
                var refeModel = refeList.Where(w => w.RefeId == refeId).FirstOrDefault();

                ViewBag.IsPay = refeModel.IsPay == true ? 1 : 0;
            }
            ViewBag.TotAmount = totAmount;

            return PartialView("_OrderList", result);
        }

        #region 处理账单明细
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
                var result = JsonHelp.PostDataResult(postModel, PostType.InBill, CurrentInfo.NotifyUrl);
                if (result != null)
                {
                    if (result.ErrorNo == "1")
                    {

                        var s = JsonHelp.Deserialize<InsBillResultModel>(result.Msg);
                        billId = s.Billno;
                    }
                    else
                    {
                        return Json(JsonResultData.Failure(result.Msg));
                    }
                }
                else
                {
                    return Json(JsonResultData.Failure("开台失败！"));
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
                BillId = ""
            };
            try
            {
                var detailResult = JsonHelp.PostDataResult(billDetailModel, PostType.InBillDetail, CurrentInfo.NotifyUrl);
                if (detailResult != null)
                {
                    if (detailResult.ErrorNo == "1")
                    {
                        CmpBill(refeId, tabId, keyId);
                        return Json(JsonResultData.Successed(billId));
                    }
                    else
                    {
                        return Json(JsonResultData.Failure(detailResult.Msg));
                    }
                }
                else
                {
                    return Json(JsonResultData.Failure("落单失败，请重试"));
                }
            }
            catch (Exception ex)
            {

                return Json(JsonResultData.Failure(ex.Message.ToString()));
            }

        }




        #endregion

        /// <summary>
        /// 重算账单
        /// </summary>
        /// <param name="refeId"></param>
        /// <param name="tabId"></param>
        /// <param name="keyid"></param>
        public void CmpBill(string refeId, string tabId, string keyid)
        {
            BillCmpPostModel postModel = new BillCmpPostModel()
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                RefeId = refeId,
                TabId = tabId,
                UserCode = CurrentInfo.UserCode,
                IsSave = 0,
                KeyId = keyid ?? "",
                BillId = ""
            };
            var result = JsonHelp.PostDataResult(postModel, PostType.BillCmp, CurrentInfo.NotifyUrl);

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
            var postItemList = commonHelper.SetItemList(refeId, tabId);
            List<ItemResultModel> itemList = new List<ItemResultModel>();
            itemList = postItemList.Where(s => (s.ItemName.Contains(searchName)|| s.IndexNo.ToLower().Contains(searchName.ToLower())|| s.WbNo.ToLower().Contains(searchName.ToLower()) || s.barCode.Contains(searchName)) && s.IsDefault == true).ToList();
            ViewBag.specFlag = "0";
            return PartialView("_ItemList", itemList);
        }
    }
}