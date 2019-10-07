using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Action
{
    public class ActionResultModel
    {
        /// <summary>
        /// 消费项目Id
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 消费项目代码
        /// </summary>
        public string ItemNo { get; set; }

        /// <summary>
        /// 作法ID
        /// </summary>
        public string ActionId { get; set; }

        /// <summary>
        /// 作法代码
        /// </summary>
        public string ActionNo { get; set; }

        /// <summary>
        /// 作法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 作法名称2
        /// </summary>
        public string ActionName2 { get; set; }

        /// <summary>
        /// 作法名称3
        /// </summary>
        public string ActionName3 { get; set; }

        /// <summary>
        /// 是否按数量计算
        /// </summary>
        public bool? IsbyQuan { get; set; }

        /// <summary>
        /// 是否按人数计算
        /// </summary>
        public bool? IsbyGuest { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsOrderAction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsCommmon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProdPrinter { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderBy { get; set; }
    }
}
