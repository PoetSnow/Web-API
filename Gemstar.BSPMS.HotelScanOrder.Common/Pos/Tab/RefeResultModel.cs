using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab
{
    public class RefeResultModel
    {
        /// <summary>
        /// 营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        /// 营业点编码
        /// </summary>
        public string RefeNo { get; set; }

        /// <summary>
        /// 营业点名称
        /// </summary>
        public string RefeCname { get; set; }

        /// <summary>
        /// 营业点英文名
        /// </summary>
        public string RefeEname { get; set; }

        /// <summary>
        /// 营业点其他名字
        /// </summary>
        public string RefeOname { get; set; }

        /// <summary>
        /// 营业点类型	tinyint	0：固定餐台1：快餐厅小卖部2：其它
        /// </summary>
        public byte? IsRefeType { get; set; }

        /// <summary>
        /// 是否支持买单（0：不支持，1：支持）
        /// </summary>
        public bool? IsPay { get; set; }
    }
}
