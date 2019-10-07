using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill
{
    public class PayBillPostModel : BasePostModel
    {
        /// <summary>
        /// 账单号
        /// </summary>
        public string Billno { get; set; }

        /// <summary>
        /// 当前营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        /// 当前餐台ID
        /// </summary>
        public string TabId { get; set; }

        /// <summary>
        /// 手牌ID
        /// </summary>
        public string KeyId { get; set; }

        /// <summary>
        /// 操作员编码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 付款信息
        /// </summary>
        public string PayNames { get; set; }

        /// <summary>
        /// 操作类型（56：待支付，0：支付成功）
        /// </summary>
        public byte operType { get; set; }
    }
}
