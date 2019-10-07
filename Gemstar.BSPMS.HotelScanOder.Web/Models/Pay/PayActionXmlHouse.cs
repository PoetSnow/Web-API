using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill;
using System.Text;
using System.Net;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models.Pay
{
    /// <summary>
    /// 房账
    /// </summary>
    public class PayActionXmlHouse : PayActionXmlBase
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

                    var xmlStrBuilder = new StringBuilder();
                    AppendBeforeBusinessXmlStr("房账挂账", xmlStrBuilder);
                    xmlStrBuilder.Append("<RoomFolio>")
                        .Append("<hid>").Append(model.Hid).Append("</hid>")
                        .Append("<isCheck>0</isCheck>")
                        .Append("<refNo>" + mid + "</refNo>")
                        .Append("<RoomNo>" + payBillNames.payBarCode + "</RoomNo>")
                        .Append("<Regid>" + model.Billno + "</Regid>")
                        .Append("<OutletCode>" + codeIn + "</OutletCode>")
                        .Append("<PosCode>" + Dns.GetHostName() + "</PosCode>")
                        .Append("<Amount>" + payBillNames.payamount + "</Amount>")
                        .Append("<Invno>" + model.Billno + "</Invno>")
                        .Append("<Remark>[POS客账号：" + model.Billno + ";金额：" + payBillNames.payamount + ";酒店ID：" + model.Hid + "]</Remark>")
                        .Append("<Operator>" + model.UserCode + "</Operator>")
                        .Append("<GuestCname></GuestCname>")
                        .Append("<LockCardNo></LockCardNo>")
                        .Append("</RoomFolio>");
                    AppendAfterBusinessXmlStr(xmlStrBuilder);

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
                return JsonResultData.Failure("买单失败！" + result.Msg);
            }

        }
    }
}