using Gemstar.BSPMS.HotelScanOrder.Common.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.SPA
{
    public class StoreOrderPostModel:BasePostModel
    {
        //录单金额
        public decimal price { get; set; }  
        //机器号
        public string computer { get; set; }
        //操作员
        public string createor { get; set; }
        //手牌号
        public string handcode { get; set; }

    }
}
