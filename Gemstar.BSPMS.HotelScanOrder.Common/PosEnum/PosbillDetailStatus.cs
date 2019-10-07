using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.PosEnum
{
    public enum PosbillDetailStatus : byte
    {
        正常 = 0,
        例送 = 1,
        赠送 = 2,
        取酒 = 3,
        保存 = 4,
        找赎 = 10,
        不加回库存取消 = 51,
        加回库存取消 = 52,
        未落单的取消 = 54,
        反结付款方式取消 = 55,
        付款方式作废 = 56
    }
}
