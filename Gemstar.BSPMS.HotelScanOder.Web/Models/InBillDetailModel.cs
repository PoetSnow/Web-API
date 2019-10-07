using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models
{
    /// <summary>
    /// 用于插入到数据库中的实体
    /// </summary>
    public class InBillDetailModel
    {
        public string rowid { get; set; }

        public string itemid { get; set; }

        public string itemname { get; set; }

        public string unitid { get; set; }

        public decimal? quan { get; set; }

        public decimal? quan2 { get; set; }

        public decimal? price { get; set; }

        public int? status { get; set; }

        public string dcFlag { get; set; }

        public string request { get; set; }

        public string actions { get; set; }

        public string sp { get; set; }

        public string sd { get; set; }

        public string upid { get; set; }

        public string place { get; set; }     //客位

        /// <summary>
        /// 推销员
        /// </summary>
        public string salename { get; set; }

        public string pdano { get; set; }

        public string canReason { get; set; }

        public string memo { get; set; }

        public string OperType { get; set; }

       
    }
}