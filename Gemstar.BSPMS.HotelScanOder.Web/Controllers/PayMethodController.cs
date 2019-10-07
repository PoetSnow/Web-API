using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayMethod;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;
using Gemstar.BSPMS.HotelScanOrder.Web.Models.Pay;
using Newtonsoft.Json;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Controllers
{
    /// <summary>
    /// 买单界面
    /// </summary>
    public class PayMethodController : BaseController
    {

        public ActionResult Index(string refeId, string tabId, string billId, string amount)
        {
            //获取付款方式
            refeId = refeId ?? CurrentInfo.RefeId;
            var PayMethodList = commonHelper.GetPayMethodList(refeId);

            ViewBag.RefeId = refeId;
            ViewBag.TabId = tabId;
            ViewBag.BillId = billId;
            ViewBag.Amount = Convert.ToDecimal(amount);
            return View(PayMethodList);

        }



        /// <summary>
        /// 切换付款方式金额尾数处理
        /// </summary>
        /// <returns></returns>
        public ActionResult CmpAmount(string refeId, string amount)
        {
            var posData = new PayBillAmountPostModel()
            {
                Hid = CurrentInfo.HotelId ?? "",
                Module = CurrentInfo.Module ?? "",
                RefeId = refeId ?? "",
                Amount = Convert.ToDecimal(amount)
            };
            var result = JsonHelp.PostDataResult(posData, PostType.PayBillAmount, CurrentInfo.NotifyUrl);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    var resultAmount = JsonHelp.Deserialize<List<PayBillAmountResultModel>>(result.Msg).FirstOrDefault();
                    return Json(JsonResultData.Successed(String.Format("{0:N2} ", resultAmount.ReturnAmount)));
                }
            }
            return Json(JsonResultData.Failure(""));
        }

        /// <summary>
        ///  //支付接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Pay(PayBillPostModel model)
        {

            model.Hid = CurrentInfo.HotelId ?? "";
            model.Module = CurrentInfo.Module ?? "";
            model.UserCode = CurrentInfo.UserCode ?? "";
            model.KeyId = model.KeyId ?? "";

            PayBillNamesPostModel payBillNames = null;

            var _settleId = Guid.NewGuid();
            //如果前台提交的付款信息不为空。给付款信息重新赋值
            if (!string.IsNullOrEmpty(model.PayNames))
            {
                payBillNames = JsonHelp.Deserialize<PayBillNamesPostModel>(model.PayNames);
                payBillNames.dueamount = payBillNames.dueamount ?? 0;
                payBillNames.payamount = payBillNames.payamount ?? 0;
                payBillNames.settleid = _settleId;
            }
            if (payBillNames == null)
            {
                return Json(JsonResultData.Failure("未找到付款信息，请联系系统管理员"));
            }

            model.PayNames = JsonHelp.Serialize(payBillNames);
            if (payBillNames.payCode == "no")   //付款方式没有处理动作。直接调用接口生成支付成功的付款数据
            {
                model.operType = (byte)PosbillDetailStatus.正常;
                try
                {
                    var result = JsonHelp.PostDataResult(model, PostType.PayBill, CurrentInfo.NotifyUrl);
                    return cmpPayResult(result);
                }
                catch (Exception ex)
                {

                    return Json(JsonResultData.Failure(ex.Message.ToString()));
                }
            }
            else
            {
                model.operType = (byte)PosbillDetailStatus.付款方式作废;   //生成待支付数据
                try
                {

                    var payActionXmlBuilder = GetService<PayActionXmlBuilder>();
                    var payActionXmlHandler = payActionXmlBuilder.Build(payBillNames.payCode);

                    var s = payActionXmlHandler.DoOperate(model);

                    return Json(s);
                }
                catch (Exception ex)
                {
                    return Json(JsonResultData.Failure(ex.Message.ToString()));
                }



            }

        }

        /// <summary>
        /// 处理接口返回数据
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected JsonResult cmpPayResult(PostDataResult result)
        {
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    return Json(JsonResultData.Successed("支付成功"));

                }
                else
                {
                    return Json(JsonResultData.Failure(result.Msg));
                }
            }
            else
            {
                return Json(JsonResultData.Failure("支付失败！"));
            }
        }

        #region 会员，合约单位，房账接口查询

        /// <summary>
        /// 获取会员卡信息
        /// </summary>
        /// <param name="cardNo">卡号/手机号</param>
        /// <returns></returns>
        public ActionResult GetmbrCardInfo(string payUrl, string mbrCardNo, string payRemark)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(mbrCardNo))
                {
                    string xmlStr = "<?xml version='1.0' encoding='gbk' ?>"
                        + "<RealOperate>"
                        + "<XType>JxdBSPms</XType>"
                        + "<OpType>会员查询</OpType>"
                        + "<MbrQuery>"
                        + "<ProfileID></ProfileID>"
                        + "<NetName></NetName>"
                        + "<NetPwd></NetPwd>"
                        + "<OtherKeyWord></OtherKeyWord>"
                        + "<OtherName></OtherName>"
                        + "<Mobile>" + mbrCardNo + "</Mobile>"
                        + "<MbrCardNo>" + mbrCardNo + "</MbrCardNo>"
                        + "</MbrQuery>"
                        + "</RealOperate>";

                    string hid = CurrentInfo.GroupHotelId ?? "";     //集团Id
                    string channel = payRemark.Split(',')[0];   //渠道代码
                    string interfaceKey = payRemark.Split(',')[1];  //秘钥
                    var sign = CryptHelper.EncryptMd5(string.Format("{0}{1}{2}{3}", interfaceKey, hid, channel, xmlStr));

                    payUrl += "?grpid=" + CurrentInfo.HotelId ?? "";
                    payUrl += "&channel=" + channel;
                    payUrl += "&sign=" + sign;
                    var xmlInfo = JsonHelp.PostCheck(payUrl, xmlStr);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlInfo);
                    Dictionary<string, string> xmls = new Dictionary<string, string>();
                    if (doc != null)
                    {
                        if (doc["MbrQuery"] != null)
                        {
                            if (doc["MbrQuery"]["Rows"] != null)
                            {
                                if (doc["MbrQuery"]["Rows"]["Row"] != null)
                                {
                                    foreach (XmlNode node in doc["MbrQuery"]["Rows"]["Row"])
                                    {
                                        if (node != null && node.Name != null && node.FirstChild != null)
                                        {
                                            if (node.Name == "Status")
                                            {
                                                xmls.Add(node.Name, Convert.ToInt32(node.FirstChild.Value) < 51 ? "正常" : "无效");
                                            }
                                            else
                                            {
                                                xmls.Add(node.Name, node.FirstChild.Value);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            return Json(JsonResultData.Failure("未找到会员信息"));
                        }
                    }

                    string json = JsonConvert.SerializeObject(xmls);
                    return Json(JsonResultData.Successed(json));
                }

                return Json(JsonResultData.Failure("请输入会员卡号或手机号"));
            }
            catch (Exception ex)
            {
                return Json(JsonResultData.Failure(ex.Message.ToString()));
            }
        }

        /// <summary>
        /// 获取房账信息
        /// </summary>
        /// <param name="payUrl"></param>
        /// <param name="roomNo">房号</param>
        /// <param name="payRemark"></param>
        /// <returns></returns>
        public ActionResult RommInfo(string payUrl, string roomNo, string payRemark)
        {

            try
            {
                if (!string.IsNullOrWhiteSpace(roomNo))
                {

                    string xmlStr = "<?xml version='1.0' encoding='gbk' ?>"
                        + "<RealOperate>"
                            + "<XType>" + "JxdBSPms" + "</XType>"
                            + "<OpType>" + "房账客人资料查询" + "</OpType>"
                            + "<RoomFolio>"
                                + "<hid>" + CurrentInfo.HotelId + "</hid>"
                                + "<roomNo>" + roomNo + "</roomNo>"
                                + "<guestCName></guestCName>"
                                + "<Regid></Regid>"
                                + "<Outlet></Outlet>"
                            + "</RoomFolio>"
                        + "</RealOperate> ";

                    string hid = CurrentInfo.GroupHotelId ?? "";     //集团Id
                    string channel = payRemark.Split(',')[0];   //渠道代码
                    string interfaceKey = payRemark.Split(',')[1];  //秘钥
                    var sign = CryptHelper.EncryptMd5(string.Format("{0}{1}{2}{3}", interfaceKey, hid, channel, xmlStr));

                    payUrl += "?grpid=" + CurrentInfo.HotelId ?? "";
                    payUrl += "&channel=" + channel;
                    payUrl += "&sign=" + sign;
                    var xmlInfo = JsonHelp.PostCheck(payUrl, xmlStr);


                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlInfo);
                    Dictionary<string, string> xmls = new Dictionary<string, string>();
                    if (doc != null)
                    {
                        if (doc["RoomFolio"] != null)
                        {
                            if (doc["RoomFolio"]["Rows"] != null)
                            {
                                if (doc["RoomFolio"]["Rows"]["Row"] != null)
                                {
                                    foreach (XmlNode node in doc["RoomFolio"]["Rows"]["Row"])
                                    {
                                        if (node != null && node.Name != null && node.FirstChild != null)
                                        {
                                            xmls.Add(node.Name, node.FirstChild.Value);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    string json = JsonConvert.SerializeObject(xmls);
                    return Json(JsonResultData.Successed(json));
                }

                return Json(JsonResultData.Failure("请输入房号"));
            }
            catch (Exception ex)
            {
                return Json(JsonResultData.Failure(ex));
            }
            return View();
        }
        #endregion
    }




}