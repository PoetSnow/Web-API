using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab
{
    public class TabResultModel
    {

        /// <summary>
        /// 餐台ID
        /// </summary>
        public string TabId { get; set; }

        /// <summary>
        /// 餐台编码
        /// </summary>
        public string TabNo { get; set; }

        /// <summary>
        /// 餐台名称
        /// </summary>
        public string TabName { get; set; }

        /// <summary>
        /// 餐台类型 
        /// </summary>
        public string TabTypeNo { get; set; }

        /// <summary>
        /// 营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        /// 营业点编码
        /// </summary>
        public string RefeNo { get; set; }

        /// <summary>
        /// 客账Id
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int? IGuest { get; set; }

        /// <summary>
        /// 可容纳人数
        /// </summary>
        public int? ISeat { get; set; }

        /// <summary>
        /// 餐台状态
        /// </summary>
        public int? TabStatus { get; set; }

        /// <summary>
        /// 开台时间
        /// </summary>
        public DateTime? OpenRecord { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? PayRecord { get; set; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public int? ISeqid { get; set; }

        /// <summary>
        /// 手牌
        /// </summary>
        public string KeyId { get; set; }

        /// <summary>
        /// 微信点餐付款模式（先付，后付）
        /// </summary>
        public byte? WxPaytype { get; set; }

        /// <summary>
        /// 是否开启扫码点餐（0：关闭，1：开启）
        /// </summary>
        public bool? isBrushOrder { get; set; }
    }
}
