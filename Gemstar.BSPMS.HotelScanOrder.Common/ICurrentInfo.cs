using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab;

namespace Gemstar.BSPMS.HotelScanOrder.Common
{
    public interface ICurrentInfo
    {

        /// <summary>
        /// 清空之前的所有信息
        /// </summary>
        void Clear();
        /// <summary>
        /// 从存储中加载值
        /// </summary>
        void LoadValues();
        /// <summary>
        /// 将值保存到存储中
        /// </summary>
        void SaveValues();


        /// <summary>
        /// 用户ID
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// 用户代码
        /// </summary>
        string UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// 是否是集团
        /// </summary>
        bool IsGroup { get; }

        /// <summary>
        /// 集团或酒店id，如果当前是集团时则返回集团id，如果当前是单体酒店时，则返回酒店id
        /// </summary>
        string GroupHotelId { get; }

        /// <summary>
        /// 酒店ID
        /// </summary>
        string HotelId { get; set; }

        /// <summary>
        /// 当前所属的集团id
        /// </summary>
        string GroupId { get; set; }

        /// <summary>
        /// 营业点
        /// </summary>
        string RefeList { get; set; }

        /// <summary>
        /// 餐台ID
        /// </summary>
        string TabId { get; set; }

        /// <summary>
        /// 消费项目
        /// </summary>
        string ItemList { get; set; }

        /// <summary>
        /// 当前营业点ID
        /// </summary>
        string RefeId { get; set; }

        /// <summary>
        /// 要求
        /// </summary>
        string RequestList { get; set; }

        /// <summary>
        /// 机器号
        /// </summary>
        string SNCode { get; set; }


        /// 模块
        /// </summary>
        string Module { get; set; }

        /// <summary>
        /// 运营酒店ID
        /// </summary>
        string GsWxComid { get; set; }

        /// <summary>
        /// openId接口地址
        /// </summary>
        string GsWxOpenidUrl { get; set; }

        /// <summary>
        /// 模板接口地址
        /// </summary>
        string GsWxTemplateMessageUrl { get; set; }

        /// <summary>
        /// 支付下单地址
        /// </summary>
        string GsWxCreatePayOrderUrl { get; set;}

        /// <summary>
        /// 支付地址
        /// </summary>
        string GsWxPayOrderUrl { get; set; }

        /// <summary>
        /// 接口地址（线下程序使用）
        /// </summary> 
        string NotifyUrl { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        string OpenId { get; set; }

        /// <summary>
        ///微信点餐 先付后付模式
        /// </summary>
        string WxPaytype { get; set; }

        /// <summary>
        /// 扫码支付的参数
        /// </summary>
        string PayBillNameStr { get; set; }

        /// <summary>
        /// 扫码支付接口请求参数
        /// </summary>
        string PayBillPostModelStr { get; set; }

        /// <summary>
        /// 餐台编码
        /// </summary>
        string TabNo { get; set; }

        /// <summary>
        /// 餐台名
        /// </summary>
        string TabName { get; set; }

        /// <summary>
        /// 是否线下酒店
        /// </summary>
        string IsCs { get; set; }
    }
}
