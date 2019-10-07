using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Request
{
    /// <summary>
    /// 要求返回结果
    /// </summary>
    public class RequestResultModel
    {
        /// <summary>
        /// 要求ID
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// 要求编码
        /// </summary>
        public string RequestNo { get; set; }

        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称2
        /// </summary>
        public string Name2 { get; set; }

        /// <summary>
        /// 名称3
        /// </summary>
        public string Name3 { get; set; }

        /// <summary>
        /// 要求操作(0：单道 ；1：全单；2：本单)	
        /// </summary>
        public byte? ITagOperator { get; set; }

        /// <summary>
        /// 前提要求
        /// </summary>
        public string ReQuest { get; set; }

        /// <summary>
        /// 要求属性(0：一般；1：追单；2：叫起；3：起菜)
        /// </summary>
        public byte? IsTagProperty { get; set; }

        /// <summary>
        /// 出品状态(0：不出品；1：出品；2：全单出品一张单；3：全单出品传菜部)
        /// </summary>
        public byte? IsProduce { get; set; }

        /// <summary>
        /// 营业点ID
        /// </summary>
        public string Refeid { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderBy { get; set; }
    }
}
