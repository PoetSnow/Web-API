using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos
{
    /// <summary>
    /// 获取基本信息设置
    /// </summary>
    public class HotelInterfacePostModel : BasePostModel
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        public string SecretKey { get; set; }

    }
}
