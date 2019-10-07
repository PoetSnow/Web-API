using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Common
{
    public static class PostType
    {

        /// <summary>
        /// 获取酒店信息
        /// </summary>
        public const string GetHotelSM = "00";

        /// <summary>
        /// 登录
        /// </summary>
        public const string Login = "01";

        /// <summary>
        /// 获取营业点
        /// </summary>
        public const string GetRefeList = "02";

        /// <summary>
        /// 获取餐台资料
        /// </summary>
        public const string GetTabList = "03";

        /// <summary>
        /// 获取消费项目
        /// </summary>
        public const string GetItemList = "04";

        /// <summary>
        /// 获取要求
        /// </summary>
        public const string GetRequestList = "05";

        /// <summary>
        /// 获取作法
        /// </summary>
        public const string GetActionList = "06";

        /// <summary>
        /// 获取当前收银点和营业点上的营业日、班次和市别等
        /// </summary>
        public const string GetSysInfo = "07";

        /// <summary>
        /// 获取账单明细
        /// </summary>
        public const string GetBillDetail = "08";

        /// <summary>
        /// 处理账单明细
        /// </summary>
        public const string InBillDetail = "09";

        /// <summary>
        /// 开台
        /// </summary>
        public const string InBill = "10";

        /// <summary>
        /// 处理作法
        /// </summary>
        public const string InDetailAction = "11";

        /// <summary>
        /// 获取账单明细做法
        /// </summary>
        public const string GetBillDetailAction = "12";

        /// <summary>
        /// 账单处理
        /// </summary>
        public const string BillCmp = "13";


        /// <summary>
        /// 获取付款方式列表
        /// </summary>
        public const string GetPayMethodList = "14";


        /// <summary>
        /// 记录日志
        /// </summary>
        public const string OperLog = "15";

        /// <summary>
        /// 支付
        /// </summary>
        public const string PayBill = "16";

        /// <summary>
        /// 支付金额处理
        /// </summary>
        public const string PayBillAmount = "17";

        /// <summary>
        /// 获取酒店信息
        /// </summary>
        public const string GetHotelInfo = "18";

        /// <summary>
        /// 获取报表-账单列表
        /// </summary>
        public const string GetReport_BillList = "19";

        /// <summary>
        /// 获取报表-账单明细表
        /// </summary>
        public const string GetReport_BillDetailList = "20";

        /// <summary>
        /// 获取报表-收入平衡表
        /// </summary>
        public const string GetReport_InBalanceList = "21";

        /// <summary>
        /// 获取报表-销售明细
        /// </summary>
        public const string GetReport_SellDetailList = "22";

        //////////////////水疗相关数据编码 50--->99 /////////////////////////////////

        /// <summary>
        /// 水疗通用登陆
        /// </summary>
        public const string SPA_Login = "50";

        /// <summary>
        /// 特产店入单
        /// </summary>
        public const string SPA_StoreOrder = "51";

    }
}
