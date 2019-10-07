using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;

namespace Gemstar.BSPMS.HotelScanOrder.Common
{
    public interface ISettingProvider
    {
        /// <summary>
        /// 当前应用程序设置的值
        /// </summary>
        SettingInfo SettingInfo { get; }
    }
}
