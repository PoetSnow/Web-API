using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models.Pay
{
    /// <summary>
    /// 会员增值金额支付
    /// </summary>
    public class PayActionXmlMbrLargess : PayActionXmlBase
    {
        public override JsonResultData DoOperate(PayBillPostModel model)
        {
            var payBillNames = JsonHelp.Deserialize<PayBillNamesPostModel>(model.PayNames);
            if (payBillNames == null)
            {
                return JsonResultData.Failure("买单失败！");
            }

            var result = JsonHelp.PostDataResult(model, PostType.PayBill);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    var payResult = JsonHelp.Deserialize<List<PayBillResultModel>>(result.Msg).FirstOrDefault();
                    if (payResult == null)
                    {
                        return JsonResultData.Failure("买单失败！未对应内部代码");
                    }
                    var codeIn = payResult.ResultRemak; //内部代码，调用接口需要使用
                    var mid = payResult.MId;    //账单明细ID

                    #region 处理XML

                    StringBuilder xmlStrBuilder = new StringBuilder();
                    AppendBeforeBusinessXmlStr("会员交易", xmlStrBuilder);
                    xmlStrBuilder.Append("<ProfileCa>")
                                 .Append("<ProfileId>").Append(payBillNames.payBarCode).Append("</ProfileId>")
                                 .Append("<OutletCode>").Append(codeIn).Append("</OutletCode>")
                                 .Append("<HotelCode>").Append(model.Hid ?? "").Append("</HotelCode>")
                                 .Append("<BalanceType>").Append(((byte)BalanceType.赠送金额).ToString("D2")).Append("</BalanceType>")
                                 .Append("<PaymentDesc>").Append(((byte)PaymentDesc.扣款).ToString("D2")).Append("</PaymentDesc>")
                                 .Append("<Amount>").Append(0 - payBillNames.payamount).Append("</Amount>")
                                 .Append("<RefNo>").Append(mid).Append("</RefNo>")
                                 .Append("<Remark>[POS客账号：").Append(model.Billno).Append(";营业点：").Append(model.RefeId).Append(";金额：").Append(payBillNames.payamount).Append(";酒店ID：").Append(model.Hid).Append("]</Remark>")
                                 .Append("<Creator>").Append(model.UserCode).Append("</Creator>")
                                 .Append("<PayTypeId>").Append(payBillNames.paymodeid).Append("</PayTypeId>")

                                 .Append("</ProfileCa>");
                    AppendAfterBusinessXmlStr(xmlStrBuilder);
                    #endregion

                    var channel = payBillNames.payRemark.Split(',')[0]; //功能代码
                    var interfaceKey = payBillNames.payRemark.Split(',')[1]; //秘钥
                    var sign = CryptHelper.EncryptMd5(string.Format("{0}{1}{2}{3}", interfaceKey, model.Hid, channel, xmlStrBuilder.ToString()));
                    var payUrl = payBillNames.payUrl;
                    payUrl += "?grpid=" + model.Hid;
                    payUrl += "&channel=" + channel;
                    payUrl += "&sign=" + sign;
                    var xmlInfo = JsonHelp.PostCheck(payUrl, xmlStrBuilder.ToString());
                    var returnResult = CmpXmlResult(xmlInfo);
                    if (returnResult.Success)
                    {
                        //再次调用处理账单
                        payBillNames.prepayID = mid.ToString();
                        model.PayNames = JsonHelp.Serialize(payBillNames);
                        model.operType = (byte)PosbillDetailStatus.正常;
                        result = JsonHelp.PostDataResult(model, PostType.PayBill);
                        return cmpPostApiResult(result);

                    }
                    else
                    {
                        return JsonResultData.Failure(returnResult.Data.ToString() == "" ? "买单失败" : returnResult.Data);
                    }

                }
                else
                {
                    return JsonResultData.Failure("买单失败！" + result.Msg);
                }
            }
            else
            {
                return JsonResultData.Failure("买单失败！");
            }
        }
    }
}