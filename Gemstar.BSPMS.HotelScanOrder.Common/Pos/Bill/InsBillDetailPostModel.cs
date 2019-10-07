using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill
{
    public class InsBillDetailPostModel : BasePostModel
    {
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
        /// 操作类型(1：新增；2：修改)
        /// </summary>
        public string OperType { get; set; }

        /// <summary>
        /// 点菜信息
        /// </summary>

        public string OrderItem { get; set; }

        /// <summary>
        /// 作法
        /// </summary>
        public string OrderAction { get; set; }

        /// <summary>
        /// 账单Id
        /// </summary>
        public string BillId { get; set; }
    }
}
