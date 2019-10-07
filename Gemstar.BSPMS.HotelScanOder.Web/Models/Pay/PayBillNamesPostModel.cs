using Gemstar.BSPMS.HotelScanOrder.Common.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models.Pay
{
    /// <summary>
    /// 支付 的payNames
    /// </summary>
    public class PayBillNamesPostModel 
    {

        /// <summary>
        /// 付款方式Id
        /// </summary>
        public string paymodeid { get; set; }

        /// <summary>
        /// 付款方式处理动作
        /// </summary>
        public string payCode { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal? payamount { get; set; }

        /// <summary>
        /// 支付宝，微信支付成功返回的订单Id
        /// </summary>
        public string prepayID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; }

        /// <summary>
        /// 原位币
        /// </summary>
        public decimal? dueamount { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string padNo { get; set; }

        /// <summary>
        /// 微信，支付宝。付款条码
        /// </summary>
        public string payBarCode { get; set; }

        /// <summary>
        /// 付款接口
        /// </summary>
        public string payUrl { get; set; }

        /// <summary>
        /// 付款接口信息
        /// </summary>
        public string payRemark { get; set; }

        /// <summary>
        /// 接口类型 区分 是否捷云还是其他
        /// </summary>
        public string payType { get; set; }

        /// <summary>
        /// 付款Id
        /// </summary>
        public Guid settleid { get; set; }

        /// <summary>
        /// 用户OpenId
        /// </summary>
        public string OpenId { get; set; }
    }
}