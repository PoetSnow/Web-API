using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayMethod
{
    /// <summary>
    /// 付款方式请求参数
    /// </summary>
    public class GetPayMethodPostModel : BasePostModel
    {
        /// <summary>
        /// 营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserCode { get; set; }
    }
}
