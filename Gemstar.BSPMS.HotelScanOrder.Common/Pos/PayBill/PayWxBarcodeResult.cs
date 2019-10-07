using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill
{
    /// <summary>
    /// 微信条码支付返回值
    /// </summary>
    public class PayWxBarcodeResult
    {
        /// <summary>
        /// 子商户公众账号
        /// </summary>
        public string SubAppid { get; set; } 

        /// <summary>
        /// 子商户号
        /// </summary>
        public string SubMchId { get; set; }

        /// <summary>
        /// 商品或支付单简要描述
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 商户系统内部的订单号
        /// </summary>
        public Guid? OutTradeNo { get; set; }

        /// <summary>
        /// 需要支付的金额，以元为单位
        /// </summary>
        public string OrderAmount { get; set; }

        /// <summary>
        /// 微信条码
        /// </summary>
        public string AuthCode { get; set; }
    }
}
