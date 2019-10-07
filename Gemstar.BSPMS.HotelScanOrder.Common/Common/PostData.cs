using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Common
{
    public class PostData
    {
        /// <summary>
        /// 请求类型
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestData { get; set; }
    }
}
