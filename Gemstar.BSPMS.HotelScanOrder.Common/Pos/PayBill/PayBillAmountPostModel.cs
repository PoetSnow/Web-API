using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill
{
    /// <summary>
    /// 付款金额尾数处理
    /// </summary>
    public class PayBillAmountPostModel : BasePostModel
    {
        /// <summary>
        /// 营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }
    }
}
