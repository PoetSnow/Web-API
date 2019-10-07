using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models
{
    /// <summary>
    /// 用于传递到前端的实体
    /// </summary>
    public class billDetailViewModel
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Quan { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Tagcharge { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public string itemClassId { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public long? RowId { get; set; }

        /// <summary>
        /// 作法ID
        /// </summary>
        public string ActioId { get; set; }

        /// <summary>
        /// 作法
        /// </summary>
        public string ActioName { get; set; }

        /// <summary>
        /// 要求
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// 是否可赠送
        /// </summary>

        public string IsLargess { get; set; }

        /// <summary>
        /// 是否多单位
        /// </summary>
        public string IsMultiUnit { get; set; }

        /// <summary>
        /// 是否显示作法
        /// </summary>
        public string IsAutoaction { get; set; }

        /// <summary>
        /// 消费项目名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal? TotAmount { get; set; }
    }
}