using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Logs
{
    /// <summary>
    /// 写入日志请求
    /// </summary>
    public class OperLogPostModel
    {
        /// <summary>
        /// 酒店代码
        /// </summary>
        public string Hid { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperType { get; set; }

        /// <summary>
        /// 操作日志记录
        /// </summary>
        public string OperContent { get; set; }
    }
}
