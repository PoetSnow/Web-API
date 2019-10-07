using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Request
{
    public class RequestPostModel : BasePostModel
    {
       

        /// <summary>
        /// 营业点代码
        /// </summary>
        public string Refeid { get; set; }
    }
}
