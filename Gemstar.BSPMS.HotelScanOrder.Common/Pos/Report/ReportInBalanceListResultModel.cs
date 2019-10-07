using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Report
{
    public class ReportInBalanceListResultModel
    {
        public string DeptClassName { get; set; }
        public decimal? SellAmount { get; set; }
        public decimal? GainAmount { get; set; }
        public decimal? DiscAmount { get; set; }
        public decimal? ComAmount { get; set; }
        public decimal? Largess { get; set; }
        public int? Flag { get; set; }
        public int? Seqid { get; set; }
        public int? Bill { get; set; }
        public decimal? AverBill { get; set; }
        public int? Guest { get; set; }
        public decimal? AverGuest { get; set; }
        public decimal? GoTabNate { get; set; }
        public decimal? TurnTabNate { get; set; }
        public string Code { get; set; }

    }
}
