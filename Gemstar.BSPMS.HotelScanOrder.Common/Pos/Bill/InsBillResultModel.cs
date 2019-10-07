using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill
{
    /// <summary>
    /// 开台返回信息
    /// </summary>
    public class InsBillResultModel
    {
        public int? Rc { get; set; }
        public string Msg { get; set; }

        public string Billno { get; set; }
    }
}
