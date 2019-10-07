using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Report
{
    public class ReportBillDetailListResultModel
    {
        public string TabNo { get; set; }
        public string TabName { get; set; }
        public DateTime? PayRecord { get; set; }
        public decimal? DishAmount { get; set; }
        public decimal? ServiceAmt { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotDiscount { get; set; }
        public decimal? TotAmount { get; set; }
        public string DishName { get; set; }
        public string UnitName { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public int? TagCharge { get; set; }
        public string PayMode { get; set; }
        public string Memo { get; set; }
    }
}
