using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Report
{
    public class ReportBillListPostModel : BasePostModel
    {
        public string As_FindType { get; set; }
        public string As_StartDate { get; set; }
        public string As_EndDate { get; set; }
        public string Refeid { get; set; }
        public string PayModeId { get; set; }
    }
}
