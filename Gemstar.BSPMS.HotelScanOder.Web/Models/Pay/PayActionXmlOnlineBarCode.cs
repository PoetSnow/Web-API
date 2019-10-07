using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;
using System;
using System.Text;
namespace Gemstar.BSPMS.HotelScanOrder.Web.Models.Pay
{
    /// <summary>
    /// 微信条码支付或者支付宝条码支付
    /// </summary>
    public class PayActionXmlOnlineBarCode : PayActionXmlBase
    {
        /// <summary>
        /// 支付逻辑 首先插入一条取消的付款记录，再次调用接口进行支付，支付成功再修改付款记录的状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

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
                if (result.ErrorNo == "1")  //生成待支付数据成功。
                {
                    StringBuilder payStr = new StringBuilder();
                    payBillNames.prepayID = Guid.NewGuid().ToString("N");   //付款记录Id
                    var paysplit = "|";
                    payStr.Append("WxProviderBarcodePay").Append(paysplit);
                    payStr.Append(payBillNames.payRemark.Split(',')[0]).Append(paysplit);
                    payStr.Append(payBillNames.payRemark.Split(',')[1]).Append(paysplit);
                    payStr.Append("餐饮微信支付").Append(paysplit);
                    payStr.Append(payBillNames.prepayID).Append(paysplit);
                    payStr.Append(payBillNames.payamount).Append(paysplit);
                    payStr.Append(payBillNames.payBarCode);

                    var payResult = JsonHelp.PostPay("?payStr=" + payStr.ToString(), payBillNames.payUrl);
                    if (payResult.StartsWith("0"))  //支付成功
                    {
                        //var payResultArr = payResult.Split('|');
                        //var settleTransno = payResultArr[0].Substring(1);   //支付成功返回的交易单号
                        //payBillNames.prepayID = settleTransno;
                        model.PayNames = JsonHelp.Serialize(payBillNames);
                        model.operType = (byte)PosbillDetailStatus.正常;      //再次调用接口
                        result = JsonHelp.PostDataResult(model, PostType.PayBill);
                        return cmpPostApiResult(result);

                    }
                    else
                    {
                        return JsonResultData.Failure(payResult.Substring(1));
                    }
                }
                else
                {
                    return JsonResultData.Failure("买单失败！" + result.Msg);
                }

            }
            else
            {
                return JsonResultData.Failure("买单失败！网络连接错误");
            }

        }
    }
}