using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Report
{
    public class ReportBillListResultModel
    {
        public string BillNo { get; set; }
        public string PaidNo { get; set; }
        public string TabNo { get; set; }
        public string TabName { get; set; }
        public DateTime? OpenRecord { get; set; }
        public DateTime? PayRecord { get; set; }
        public decimal? Amount { get; set; }
    }
}
