using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.SPA.Store
{
   public class LoginPostModel
    {

        public string hid { get; set; }  //酒店ID

        public string usercode { get; set; } //用户编码

        public string pwd { get; set; } //密码

        public string pdano { get; set; } //机器号


    }
}
