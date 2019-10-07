using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab
{
    /// <summary>
    /// 获取餐台列表请求参数
    /// </summary>
    public class TabPostModel : BasePostModel
    {
        


        /// <summary>
        /// 营业点ID
        /// </summary>
        public string PosRefeid { get; set; }
    }
}
