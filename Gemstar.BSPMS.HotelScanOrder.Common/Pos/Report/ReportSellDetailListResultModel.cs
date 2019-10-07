using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Report
{
    public class ReportSellDetailListResultModel
    {
        public string DishNo { get; set; }
        public string DishName { get; set; }
        public string UnitName { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public int TagCharge { get; set; }
        public decimal? NPercent { get; set; }
    }
}
