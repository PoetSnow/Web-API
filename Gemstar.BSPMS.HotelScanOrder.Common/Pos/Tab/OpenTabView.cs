using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab
{
    /// <summary>
    /// 开台信息
    /// </summary>
    public class OpenTabView
    {
        /// <summary>
        /// 人数
        /// </summary>
        public int? IGuest { get; set; }

        /// <summary>
        /// 餐台ID
        /// </summary>
        public string Tabid { get; set; }

        /// <summary>
        /// 餐台号
        /// </summary>
        public string TabNo { get; set; }

        /// <summary>
        /// 营业经理
        /// </summary>
        public string Sale { get; set; }

        /// <summary>
        /// 营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        /// 账单ID
        /// </summary>
        public string BillId { get; set; }
        /// <summary>
        /// 餐台名
        /// </summary>
        public string TabName { get; set; }
    }
}
