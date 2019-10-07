using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item
{
    /// <summary>
    /// 获取消费项目请求
    /// </summary>
    public class ItemPostModel : BasePostModel
    {
    

        /// <summary>
        /// 营业点ID
        /// </summary>
        public string Posrefeid { get; set; }

        /// <summary>
        /// 用户代码
        /// </summary>
        public string Usercode { get; set; }
    }
}
