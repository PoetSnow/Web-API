using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item
{
    public class SysInfoResultModel
    {
        /// <summary>
        /// 营业日
        /// </summary>
        public DateTime? DBusiness { get; set; }


        /// <summary>
        /// 收银点ID
        /// </summary>
        public string PosId { get; set; }
        /// <summary>
        /// 收银点名称
        /// </summary>
        public string PosName { get; set; }

        /// <summary>
        /// 收银点名称2
        /// </summary>
        public string PosName2 { get; set; }

        /// <summary>
        /// 收银点名称3
        /// </summary>
        public string PosName3 { get; set; }

        /// <summary>
        /// 营业点ID
        /// </summary>
        public string RefeId { get; set; }

        /// <summary>
        /// 营业点名称
        /// </summary>
        public string RefeName { get; set; }

        /// <summary>
        /// 营业点名称2
        /// </summary>
        public string RefeName2 { get; set; }

        /// <summary>
        /// 营业点名称3
        /// </summary>
        public string RefeName3 { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        public string ShiftId { get; set; }

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName { get; set; }

        /// <summary>
        /// 班次名称2
        /// </summary>
        public string ShiftName2 { get; set; }

        /// <summary>
        /// 班次名称3
        /// </summary>
        public string ShiftName3 { get; set; }

        /// <summary>
        /// 市别ID
        /// </summary>
        public string ShuffleId { get; set; }

        /// <summary>
        /// 市别名称
        /// </summary>
        public string ShuffleName { get; set; }

        /// <summary>
        /// 市别名称2
        /// </summary>

        public string ShuffleName2 { get; set; }

        /// <summary>
        /// 市别名称3
        /// </summary>
        public string ShuffleName3 { get; set; }
    }
}
