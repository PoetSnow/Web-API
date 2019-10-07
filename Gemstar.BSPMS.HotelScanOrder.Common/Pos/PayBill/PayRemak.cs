using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill
{
    /// <summary>
    /// 挂账，房账需要的参数
    /// </summary>
    public class PayRemak
    {
        /// <summary>
        /// 功能代码
        /// </summary>
        public string FuncCode { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string interfaceKey { get; set; }
    }
}
