using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill
{
    public class BillDetailResultModel
    {
        /// <summary>
        /// 账单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 行号ID
        /// </summary>
        public long? RowId { get; set; }

        /// <summary>
        /// 消费项目ID
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 消费项目编码
        /// </summary>
        public string ItemNo { get; set; }

        /// <summary>
        /// 消费项目名称 
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 消费项目名称二
        /// </summary>
        public string ItemName2 { get; set; }

        /// <summary>
        /// 消费项目名称3
        /// </summary>
        public string ItemName3 { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Quan { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        public string UnitId { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string UnitNo { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 单位二
        /// </summary>
        public string UnitName2 { get; set; }

        /// <summary>
        /// 单位三
        /// </summary>
        public string UnitName3 { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 消费项目状态（ 0：正常 ,1：例送；2：赠送；3：取酒；4：保存；10：找赎；51：不加回库存取消；
        /// 52：加回库存取消；54：未落单的取消；55：反结付款方式取消；56：付款方式作废）
        /// </summary>
        public int? Tagcharge { get; set; }

        /// <summary>
        /// 要求
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// 作法
        /// </summary>
        public string ActioName { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string OperName { get; set; }

        /// <summary>
        /// 点菜时间
        /// </summary>
        public DateTime? Record { get; set; }

        public bool? Sp { get; set; }// 		--求和套餐

        public bool? Sd { get; set; }     // --求各套餐明细

        public string Upid { get; set; }

        public decimal? TotAmount { get; set; }
    }
}
