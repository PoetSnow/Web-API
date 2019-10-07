using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item
{
    public class SysInfoPostModel : BasePostModel
    {
        /// <summary>
        /// 收银点ID
        /// </summary>
        public string PosId { get; set; }

        /// <summary>
        /// 营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        /// 用户代码
        /// </summary>
        public string UserCode { get; set; }
    }
}
