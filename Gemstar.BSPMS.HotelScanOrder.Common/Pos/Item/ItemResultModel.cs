using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item
{
    public class ItemResultModel
    {
        /// <summary>
        /// 消费项目ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 消费项目代码
        /// </summary>
        public string ItemNo { get; set; }

        /// <summary>
        /// 消费项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 名称2
        /// </summary>
        public string ItemName2 { get; set; }

        /// <summary>
        /// 名称3
        /// </summary>
        public string ItemName3 { get; set; }

        /// <summary>
        /// 营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        /// 营业点代码
        /// </summary>
        public string RefeNo { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public string Itemsubid { get; set; }

        /// <summary>
        /// 分类代码
        /// </summary>
        public string ItemSubNo { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ItemSubname { get; set; }

        /// <summary>
        /// 分类名称2
        /// </summary>
        public string ItemSubName2 { get; set; }

        /// <summary>
        /// 分类名称3
        /// </summary>
        public string ItemSubName3 { get; set; }

        /// <summary>
        /// 可用市别ID
        /// </summary>
        public string Shuffleid { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        ///  单位代码
        /// </summary>
        public string UnitNo { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 单位名称2
        /// </summary>
        public string UnitName2 { get; set; }

        /// <summary>
        /// 单位名称3
        /// </summary>
        public string UnitName3 { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 会员价
        /// </summary>
        public decimal? PriceCclub { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public decimal? Multiple { get; set; }

        /// <summary>
        /// 是否是默认单位
        /// </summary>
        public bool? IsDefault { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        public string IndexNo { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        public string WbNo { get; set; }

        /// <summary>
        /// 不可赠送
        /// </summary>
        public bool? IsLargess { get; set; }

        /// <summary>
        /// 是否沽清
        /// </summary>
        public bool? IsSellout { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderBy { get; set; }


        /// <summary>
        /// 是否有多单位
        /// </summary>
        public bool? IsMultiUnit { get; set; }

        /// <summary>
        /// 是否显示作法
        /// </summary>
        public bool? IsAutoaction { get; set; }

        /// <summary>
        /// 消费项目类型(0：一般消费项目；1:手写单；2：计时项目；3：技师项目；4：小费...)
        /// </summary>
        public byte? IsItemType { get; set; }

        /// <summary>
        /// 消费项目小图路径
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 消费项目大图路径
        /// </summary>
        public string PicUrl2 { get; set; }
        
        /// <summary>
        ///是否时价菜
        /// </summary>
        public bool ItemIsCurrent { get; set; }

        /// <summary>
        /// 是否手写单
        /// </summary>
        public bool ItemIsHandWrite { get; set; }

        /// <summary>
        /// 条形码
        /// </summary>
        public string barCode { get; set; }
    }
}
