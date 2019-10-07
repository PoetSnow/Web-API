using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill
{
    public class BillDetailPostModel : BasePostModel
    {
        /// <summary>
        /// 营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        ///餐台ID
        /// </summary>
        public string TabId { get; set; }

        /// <summary>
        /// 手牌
        /// </summary>
        public string KeyId { get; set; }

        /// <summary>
        /// 账单Id
        /// </summary>
        public string BillId { get; set; }
    }
}
