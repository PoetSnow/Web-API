using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Action
{
    public class ActionPostModel : BasePostModel
    {


        /// <summary>
        /// 消费项目编码
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserCode { get; set; }
    }
}
