using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Action;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Request;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models
{
    public class OrderListModel
    {
        /// <summary>
        /// 消费项目明细
        /// </summary>
        public billDetailViewModel BillDetail { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public List<ItemResultModel> UnitList { get; set; }

        /// <summary>
        /// 要求
        /// </summary>
        public List<RequestResultModel> RequestList { get; set; }

        /// <summary>
        /// 作法
        /// </summary>
        public List<ActionResultModel> ActionList { get; set; }

        /// <summary>
        /// 作法分组
        /// </summary>
        public List<BillDetailActionGroup> ActionGroupList { get; set; }

        /// <summary>
        /// 作法Json
        /// </summary>
        public string ActionListStr { get; set; }
    }
}