using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos
{
    public class BasePostModel
    {
        /// <summary>
        /// 酒店代码
        /// </summary>
        public string Hid { get; set; }

        /// <summary>
        /// 功能模块
        /// </summary>
        public string Module { get; set; }
    }
}
