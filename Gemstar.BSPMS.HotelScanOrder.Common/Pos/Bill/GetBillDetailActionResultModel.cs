using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill
{
    public class GetBillDetailActionResultModel
    {
        /// <summary>
        /// 账单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 行号ID
        /// </summary>
        public long? RowId { get; set; }

        /// <summary>
        /// 组号
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// 作法ID
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// 作法编码
        /// </summary>
        public string ActionNo { get; set; }

        /// <summary>
        /// 作法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 作法名称二
        /// </summary>
        public string ActionName2 { get; set; }

        /// <summary>
        /// 作法名称三
        /// </summary>
        public string ActionName3 { get; set; }

        /// <summary>
        /// 加价
        /// </summary>
        public decimal? AddPrice { get; set; }
    }
}
