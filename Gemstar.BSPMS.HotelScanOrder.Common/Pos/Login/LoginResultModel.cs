using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Login
{
    /// <summary>
    /// 登录成功之后获取用户的信息
    /// </summary>
    public class LoginResultModel
    {
        /// <summary>
        /// 酒店ID
        /// </summary>
        public string Hid { get; set; }

        /// <summary>
        /// 名字1
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 名字2
        /// </summary>
        public string EName { get; set; }

        /// <summary>
        /// 名字3
        /// </summary>
        public string OName { get; set; }

        /// <summary>
        /// 模块(CY：餐饮；CL：会所；POS：收银点；SN：桑拿；WQ：温泉；YT：游艇)
        /// </summary>
        public string SysType { get; set; }
    }
}
