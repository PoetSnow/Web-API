using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Common
{
    public class PostType
    {
        #region 餐饮

        private const string CYKey = "CY";

        /// <summary>
        /// 获取酒店信息
        /// </summary>
        public string GetHotelSM { get { return CYKey + "00"; } }

        /// <summary>
        /// 登录
        /// </summary>
        public string Login { get { return CYKey + "01"; } }

        /// <summary>
        /// 获取营业点
        /// </summary>
        public string GetRefeList { get { return CYKey + "02"; } }

        /// <summary>
        /// 获取餐台资料
        /// </summary>
        public string GetTabList { get { return CYKey + "03"; } }

        /// <summary>
        /// 获取消费项目
        /// </summary>
        public string GetItemList { get { return CYKey + "04"; } }

        /// <summary>
        /// 获取要求
        /// </summary>
        public string GetRequestList { get { return CYKey + "05"; } }

        /// <summary>
        /// 获取作法
        /// </summary>
        public string GetActionList { get { return CYKey + "06"; } }

        /// <summary>
        /// 获取当前收银点和营业点上的营业日、班次和市别等
        /// </summary>
        public string GetSysInfo { get { return CYKey + "07"; } }

        /// <summary>
        /// 获取账单明细
        /// </summary>
        public string GetBillDetail { get { return CYKey + "08"; } }

        /// <summary>
        /// 处理账单明细
        /// </summary>
        public string InBillDetail { get { return CYKey + "09"; } }

        /// <summary>
        /// 开台
        /// </summary>
        public string InBill { get { return CYKey + "10"; } }

        /// <summary>
        /// 处理作法
        /// </summary>
        public string InDetailAction { get { return CYKey + "11"; } }

        /// <summary>
        /// 获取账单明细做法
        /// </summary>
        public string GetBillDetailAction { get { return CYKey + "12"; } }

        /// <summary>
        /// 账单处理
        /// </summary>
        public string BillCmp { get { return CYKey + "13"; } }


        /// <summary>
        /// 获取付款方式列表
        /// </summary>
        public string GetPayMethodList { get { return CYKey + "14"; } }


        /// <summary>
        /// 记录日志
        /// </summary>
        public string OperLog { get { return CYKey + "15"; } }

        /// <summary>
        /// 支付
        /// </summary>
        public string PayBill { get { return CYKey + "16"; } }

        /// <summary>
        /// 支付金额处理
        /// </summary>
        public string PayBillAmount { get { return CYKey + "17"; } }

        /// <summary>
        /// 获取酒店信息
        /// </summary>
        public string GetHotelInfo { get { return CYKey + "18"; } }

        /// <summary>
        /// 获取报表-账单列表
        /// </summary>
        public string GetReport_BillList { get { return CYKey + "19"; } }

        /// <summary>
        /// 获取报表-账单明细表
        /// </summary>
        public string GetReport_BillDetailList { get { return CYKey + "20"; } }

        /// <summary>
        /// 获取报表-收入平衡表
        /// </summary>
        public string GetReport_InBalanceList { get { return CYKey + "21"; } }

        /// <summary>
        /// 获取报表-销售明细
        /// </summary>
        public string GetReport_SellDetailList { get { return CYKey + "22"; } }

        #endregion

        #region 水疗
        private const string SPKey = "SP";

        /// <summary>
        /// 水疗通用登陆
        /// </summary>
        public string SPA_Login { get { return SPKey + "01"; } }

        /// <summary>
        /// 特产店入单
        /// </summary>
        public string SPA_StoreOrder { get { return SPKey + "02"; } }
        #endregion


    }
}
