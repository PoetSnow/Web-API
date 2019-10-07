using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill
{
    /// <summary>
    /// 开台提交信息
    /// </summary>
    public class InsBillPostDataModel : BasePostModel
    {
        /// <summary>
        /// 当前营业点ID
        /// </summary>
        public string RefeId { get; set; }

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
        /// 操作类型(1：新增；2：新增[同时产生开台项目]；3：修改)
        /// </summary>
        public string OperType { get; set; }

        /// <summary>
        /// 开台信息
        /// </summary>
        public string OpenData { get; set; }//	--开台信息
    }
}
