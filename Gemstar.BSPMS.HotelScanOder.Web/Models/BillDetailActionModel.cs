using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models
{
    public class BillDetailActionModel
    {
        public string rowid { get; set; }
        public string groupid { get; set; }
        public string actionid { get; set; }
        public string actionname { get; set; }
        public string addprice { get; set; }
        public string multiple { get; set; }
        public string deptclassno { get; set; }
        public string pdano { get; set; }
        public string status { get; set; }

        public string opertype { get; set; }
    }
}