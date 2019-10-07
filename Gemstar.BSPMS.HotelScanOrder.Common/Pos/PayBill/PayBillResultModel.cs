using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill
{
    public class PayBillResultModel
    {
        /// <summary>
        /// 账单明细Id
        /// </summary>
        public long MId { get; set; }

        /// <summary>
        /// 返回的内部代码 等各个付款方式需要的信息
        /// </summary>
        public string ResultRemak { get; set; }
    }
}
