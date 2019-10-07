using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Common
{
    public class PostDataResult
    {
        /// <summary>
        /// 请求类型
        /// </summary>
        public string BusinessType { get; set; }
        /// <summary>
        /// 成功或者失败
        /// </summary>
        public string ErrorNo { get; set; }

        /// <summary>
        /// 返回的值
        /// </summary>
        public string Msg { get; set; }
    }
}
