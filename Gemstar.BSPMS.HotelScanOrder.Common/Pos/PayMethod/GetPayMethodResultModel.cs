using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayMethod
{
    public class GetPayMethodResultModel
    {
        /// <summary>
        /// 付款方式ID
        /// </summary>
        public string PaymodeId { get; set; }

        /// <summary>
        /// 付款方式编码
        /// </summary>
        public string PaymodeNo { get; set; }

        /// <summary>
        /// 名称一
        /// </summary>
        public string PaymodeName { get; set; }

        /// <summary>
        /// 名称二
        /// </summary>
        public string PaymodeName2 { get; set; }

        /// <summary>
        /// 名称三
        /// </summary>
        public string PaymodeName3 { get; set; }

        /// <summary>
        /// 处理方式（0:无;1:信用卡;2:支票;3:挂账:4:房账;5:客房餐券;6:储值卡;7:积分卡积分;8:积分卡挂账;9:赠券;10:订金;
        /// 11:匙牌;12:会所;13:节日商品券;14:套票;15:会员次券;16:会员增值金额;17:挂台账;18:支付宝;19:微信）
        /// </summary>
        public byte? IsTagType { get; set; }

        /// <summary>
        /// {no:无特别处理,credit:信用卡,corp:挂帐,mbrCard:会员储值卡,mbrLargess:会员增值支付,mbrScore:会员积分支付,roomFolio:房账支付,
        /// AliQrcode:支付宝扫码支付,WxQrcode:微信扫码支付}
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        public decimal? XRate { get; set; }

        /// <summary>
        /// 排列序号
        /// </summary>
        public int? OrderBy { get; set; }

        /// <summary>
        /// 接口类型
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string InterFaceadd { get; set; }

        /// <summary>
        /// 微信，支付宝支付参数（微信（子商户公众账号,子商户号））
        /// </summary>
        public string PayReamk { get; set; }
    }
}
