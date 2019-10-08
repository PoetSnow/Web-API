using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Action;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayMethod;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Request;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models
{
    public class PostDataHelper
    {
        PostType postType;
        public PostDataHelper(PostType _postType)
        {
            postType = _postType;
        }

        /// <summary>
        /// 获取消费项目（服务员使用）
        /// </summary>
        /// <param name="refeId"></param>
        /// <param name="tabId"></param>
        /// <returns></returns>

        public List<ItemResultModel> SetItemList(string refeId, string tabId)
        {
            ItemPostModel postModel = new ItemPostModel
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                Posrefeid = refeId,
                Usercode = CurrentInfo.UserCode
            };

            //当前餐台Id 不等于缓存的餐台ID，调用接口获取数据
            var currentinfo = GetService<ICurrentInfo>();

            var postItemList = new List<ItemResultModel>();
            var _itemListStr = "";
            if (currentinfo.TabId != tabId)
            {

                //获取接口查询出来的数据
                postItemList = GetItemList(postModel, out _itemListStr);
                //最新的信息存储到缓存中
                currentinfo.TabId = tabId;
                currentinfo.ItemList = _itemListStr;
                currentinfo.SaveValues();
            }
            else
            {
                //相同的餐台则从缓存中取数据
                _itemListStr = currentinfo.ItemList;
                postItemList = JsonHelp.Deserialize<List<ItemResultModel>>(CurrentInfo.ItemList);
            }
            //获取当前市别等信息
            var sysInfo = GetSysInfo(refeId);
            postItemList = postItemList.Where(w => w.Shuffleid.IndexOf(sysInfo.ShuffleId) > -1).ToList();
            return postItemList;
        }

        /// <summary>
        /// 获取消费项目（扫码使用）
        /// </summary>
        /// <param name="refeId"></param>
        /// <param name="tabId"></param>
        /// <returns></returns>
        public List<ItemResultModel> SetItemListB(string refeId, string tabId)
        {
            ItemPostModel postModel = new ItemPostModel
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                Posrefeid = refeId,
                Usercode = CurrentInfo.UserCode
            };

            //当前餐台Id 不等于缓存的餐台ID，调用接口获取数据
            var currentinfo = GetService<ICurrentInfo>();

            var postItemList = new List<ItemResultModel>();

            var _itemListStr = "";
            postItemList = GetItemList(postModel, out _itemListStr);

            //获取当前市别等信息
            var sysInfo = GetSysInfo(refeId);
            postItemList = postItemList.Where(w => w.Shuffleid.IndexOf(sysInfo.ShuffleId) > -1).ToList();
            return postItemList;
        }

        /// <summary>
        /// 调用接口获取消费项目数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<ItemResultModel> GetItemList(ItemPostModel model, out string data)
        {
            var currentinfo = GetService<ICurrentInfo>();
            data = "";
            //获取接口数据
            var result = JsonHelp.PostDataResult(model, postType.GetItemList, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    //  data = result.Msg;
                    var itemList = JsonHelp.Deserialize<List<ItemResultModel>>(result.Msg);
                    if (itemList != null)
                    {
                        //是线下程序 给图片赋值
                        if (currentinfo.IsCs == "true")
                        {
                            SetItemPic(itemList);
                        }
                    }
                    data = JsonHelp.Serialize(itemList);
                    return itemList;

                }
                else
                {

                    return new List<ItemResultModel>();
                }
            }
            return new List<ItemResultModel>();
        }

        private void SetItemPic(List<ItemResultModel> itemList)
        {
            var pinBeginName = CurrentInfo.Module == "CY" ? "cyDish" : "snItem";
            foreach (var item in itemList)
            {
                //  CurrentInfo.Module
                item.PicUrl = string.IsNullOrEmpty(item.PicUrl) ? "/images/noImage.png" : Setting.PicUrl + "/" + item.PicUrl + ".jpg";  //小图
                item.PicUrl2 = string.IsNullOrEmpty(item.PicUrl2) ? "/images/noImage.png" : Setting.PicUrl + "/" + item.PicUrl2 + ".jpg"; //大图
            }
        }

        /// <summary>
        /// 获取当前营业日，市别等信息
        /// </summary>
        /// <returns></returns>
        public SysInfoResultModel GetSysInfo(string refeId)
        {
            SysInfoPostModel postModel = new SysInfoPostModel()
            {
                Hid = CurrentInfo.HotelId,
                Module = CurrentInfo.Module ?? "",
                RefeId = refeId,
                PosId = "",
                UserCode = CurrentInfo.UserCode,
            };
            var resultModel = new SysInfoResultModel();

            var result = JsonHelp.PostDataResult(postModel, postType.GetSysInfo, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    resultModel = JsonHelp.Deserialize<List<SysInfoResultModel>>(result.Msg).FirstOrDefault();
                }
            }
            return resultModel;
        }

        /// <summary>
        /// 根据消费项目Id 获取作法
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public List<ActionResultModel> GetActionListByItemId(string itemId)
        {
            var actionList = new List<ActionResultModel>();

            //请求参数
            var postModel = new ActionPostModel()
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                ItemId = itemId,
                UserCode = CurrentInfo.UserCode ?? ""
            };
            var result = JsonHelp.PostDataResult(postModel, postType.GetActionList, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {

                    actionList = JsonHelp.Deserialize<List<ActionResultModel>>(result.Msg);

                }
            }

            return actionList;
        }


        /// <summary>
        /// 查询账单明细的做法
        /// </summary>
        /// <param name="refeId"></param>
        /// <param name="tabId"></param>
        /// <returns></returns>
        public List<GetBillDetailActionResultModel> GetBillDetailAction(string refeId, string tabId)
        {
            var postData = new GetBillDetailActionPostModel()
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                RefeId = refeId,
                TabId = tabId
            };
            List<GetBillDetailActionResultModel> resultModel = new List<GetBillDetailActionResultModel>();
            var result = JsonHelp.PostDataResult(postData, postType.GetBillDetailAction, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    resultModel = JsonHelp.Deserialize<List<GetBillDetailActionResultModel>>(result.Msg);
                }
            }
            return resultModel;
        }


        /// <summary>
        /// 获取要求列表
        /// </summary>
        /// <param name="refeId"></param>
        /// <returns></returns>
        public List<RequestResultModel> GetRequestList(string refeId)
        {
            var postModel = new RequestPostModel()
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                Refeid = refeId
            };

            //当前操作的营业点与缓存的营业点不一致 。则调用接口查询
            var currentinfo = GetService<ICurrentInfo>();
            var requestList = new List<RequestResultModel>();
            var _requestListStr = "";
            if (currentinfo.RefeId != refeId)
            {
                //获取接口数据
                var result = JsonHelp.PostDataResult(postModel, postType.GetRequestList, CurrentInfo.NotifyUrl);
                if (result != null)
                {
                    if (result.ErrorNo == "1")
                    {
                        _requestListStr = result.Msg;
                        requestList = JsonHelp.Deserialize<List<RequestResultModel>>(result.Msg);

                    }
                }
                //最新的信息存储到缓存中
                currentinfo.RefeId = refeId;
                currentinfo.RequestList = _requestListStr;
                currentinfo.SaveValues();
            }
            else
            {
                //要求列表为空
                if (string.IsNullOrEmpty(currentinfo.RequestList))
                {
                    //获取接口数据
                    var result = JsonHelp.PostDataResult(postModel, postType.GetRequestList, CurrentInfo.NotifyUrl);
                    if (result != null)
                    {
                        if (result.ErrorNo == "1")
                        {
                            _requestListStr = result.Msg;
                            requestList = JsonHelp.Deserialize<List<RequestResultModel>>(result.Msg);

                        }
                    }
                    //最新的信息存储到缓存中
                    currentinfo.RefeId = refeId;
                    currentinfo.RequestList = _requestListStr;
                    currentinfo.SaveValues();

                }
                _requestListStr = currentinfo.RequestList;
                requestList = JsonHelp.Deserialize<List<RequestResultModel>>(CurrentInfo.RequestList);
            }
            return requestList;
        }

        /// <summary>
        /// 对象赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oneT">目标对象</param>
        /// <param name="twoT">源对象</param>
        /// <returns></returns>
        public bool SetModel<T, T1>(T oneT, T1 twoT)
        {
            if (oneT == null || twoT == null) { return false; }//为空直接返回false

            try
            {
                Type typeOne = oneT.GetType();
                Type typeTwo = twoT.GetType();
                PropertyInfo[] pisOne = typeOne.GetProperties(); //获取所有公共属性(Public)
                PropertyInfo[] pisTwo = typeTwo.GetProperties();

                for (int i = 0; i < pisOne.Length; i++)
                {
                    for (int j = 0; j < pisTwo.Length; j++)
                    {
                        if (pisOne[i].Name == pisTwo[j].Name)
                        {
                            pisOne[i].SetValue(oneT, pisTwo[j].GetValue(twoT, null), null);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }


        /// <summary>
        /// 查询餐台上已有的消费项目
        /// </summary>
        /// <param name="refeId"></param>
        /// <param name="tabId"></param>
        /// <returns></returns>

        public List<BillDetailResultModel> GetBillQuery(string refeId, string tabId, string keyId = "", string billId = "")
        {
            var postData = new BillDetailPostModel()
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                RefeId = refeId,
                TabId = tabId,
                KeyId = keyId,
                BillId = billId
            };
            List<BillDetailResultModel> resultModel = new List<BillDetailResultModel>();
            var result = JsonHelp.PostDataResult(postData, postType.GetBillDetail, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    resultModel = JsonHelp.Deserialize<List<BillDetailResultModel>>(result.Msg);
                }
            }
            return resultModel;
        }


        /// <summary>
        /// 拼接Action
        /// </summary>
        /// <returns></returns>
        public string SplicingAction(List<BillDetailActionModel> actionList)
        {
            StringBuilder actionNames = new StringBuilder("");
            if (actionList != null && actionList.Count > 0)
            {
                var actionGroupSplit = "";
                actionList.Sort((a, b) => a.groupid.CompareTo(b.groupid));

                //通过分组ID进行分组查询出需要的数据
                foreach (IGrouping<string, BillDetailActionModel> group in actionList.GroupBy(x => x.groupid))
                {

                    string resultSplit = "";
                    StringBuilder action = new StringBuilder("");
                    foreach (var actionModel in group)
                    {
                        action.Append(resultSplit).Append(actionModel.actionname);
                        resultSplit = ",";
                    }

                    actionNames.Append(actionGroupSplit).Append(action);
                    actionGroupSplit = "/";
                }

            }
            return actionNames.ToString();
        }


        /// <summary>
        /// 获取付款方式列表
        /// </summary>
        /// <param name="refeId"></param>
        /// <returns></returns>
        public List<GetPayMethodResultModel> GetPayMethodList(string refeId)
        {
            var posData = new GetPayMethodPostModel()
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                RefeId = refeId ?? "",
                UserCode = CurrentInfo.UserCode ?? ""

            };
            var list = new List<GetPayMethodResultModel>();
            var result = JsonHelp.PostDataResult(posData, postType.GetPayMethodList, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    list = JsonHelp.Deserialize<List<GetPayMethodResultModel>>(result.Msg);
                }
            }

            return list;
        }

        #region 注册服务

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
        #endregion
    }
}