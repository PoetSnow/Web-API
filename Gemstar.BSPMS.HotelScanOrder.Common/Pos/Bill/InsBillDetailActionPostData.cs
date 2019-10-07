using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill
{
    public class InsBillDetailActionPostData : BasePostModel
    {
        /// <summary>
        /// 当前营业点ID
        /// </summary>
        public string RefeId { get; set; } //		--

        /// <summary>
        /// 当前餐台ID
        /// </summary>
        public string TabId { get; set; }

        /// <summary>
        /// 手牌ID
        /// </summary>

        public string KeyId { get; set; }

        /// <summary>
        /// 操作员编码
        /// </summary>

        public string UserCode { get; set; }

        /// <summary>
        /// 操作类型(1：新增；2：修改；3：删除)
        /// </summary>

        public string OperType { get; set; }

        /// <summary>
        /// 点菜作法
        /// </summary>

        public string OrderAction { get; set; }
    }
}
