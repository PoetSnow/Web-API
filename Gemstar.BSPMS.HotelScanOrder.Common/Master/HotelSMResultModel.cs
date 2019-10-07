using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Master
{
    /// <summary>
    /// 扫码点餐根据酒店代码获取接口信息
    /// </summary>
    public class HotelSMResultModel
    {
        /// <summary>
        /// 酒店代码 主键
        /// </summary>
        public string HotelCode { get; set; }

        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelName { get; set; }

        /// <summary>
        /// 酒店Id
        /// </summary>
        public string Hid { get; set; }

        /// <summary>
        /// 是否线下程序
        /// </summary>
        public bool? IsCs { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string NotifyURL { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public byte? Status { get; set; }

        /// <summary>
        /// 微信运营酒店Id
        /// </summary>
        public string GsWxComid { get; set; }

        /// <summary>
        /// openId地址
        /// </summary>
        public string GsWxOpenidUrl { get; set; }

        /// <summary>
        /// 模板消息接口
        /// </summary>
        public string GsWxTemplateMessageUrl { get; set; }

        /// <summary>
        /// 支付下单地址
        /// </summary>
        public string GsWxCreatePayOrderUrl { get; set; }

        /// <summary>
        /// 支付地址
        /// </summary>
        public string GsWxPayOrderUrl { get; set; }

    }
}
