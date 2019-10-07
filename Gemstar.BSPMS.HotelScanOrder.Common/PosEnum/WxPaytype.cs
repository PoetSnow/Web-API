using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.PosEnum
{
    /// <summary>
    /// 微信点餐支付方式
    /// </summary>
    public enum WxPaytype : byte
    {
        [Description("点菜支付")]
        点菜支付 = 1,
        [Description("餐后付款")]
        餐后付款 = 2,
    }
}
