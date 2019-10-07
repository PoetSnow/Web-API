using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models.Pay
{
    /// <summary>
    /// 付款处理动作调用接口的xml格式处理基类
    /// </summary>
    public class PayActionXmlBase
    {
        /// <summary>
        /// 检查指定接口是否能够正常调用
        /// </summary>
        /// <param name="para">接口参数</param>
        /// <param name="businessPara">其他业务参数</param>
        /// <returns>默认直接返回能够正常调用</returns>
        public virtual JsonResultData DoCheck(PayBillPostModel model)
        {
            return JsonResultData.Successed();
        }
        /// <summary>
        /// 执行真实的接口调用
        /// </summary>
        /// <param name="para">接口参数</param>
        /// <param name="businessPara">其他业务参数</param>
        /// <returns>默认不做任何接口调用，直接返回成功，具体的接口调用由子类重写来实现</returns>
        public virtual JsonResultData DoOperate(PayBillPostModel model)
        {
            return JsonResultData.Successed();
        }
        /// <summary>
        /// 当前处理动作需要返回的默认账务状态
        /// </summary>
        public virtual PosbillDetailStatus DefaultDetailStatus => PosbillDetailStatus.正常;
        /// <summary>
        /// 给xml增加业务信息之前的通用信息
        /// </summary>
        /// <param name="businessTitle">接口名称</param>
        /// <param name="xmlStrBuilder">xml生成器</param>
        protected void AppendBeforeBusinessXmlStr(string businessTitle, StringBuilder xmlStrBuilder)
        {
            xmlStrBuilder.Append("<?xml version='1.0' encoding='gbk' ?>")
                       .Append("<RealOperate>")
                       .Append("<XType>JxdBSPms</XType>")
                       .Append("<OpType>").Append(businessTitle).Append("</OpType>");
        }
        /// <summary>
        /// 给xml增加业务信息之后的通用信息
        /// </summary>
        /// <param name="xmlStrBuilder">xml生成器</param>
        protected void AppendAfterBusinessXmlStr(StringBuilder xmlStrBuilder)
        {
            xmlStrBuilder.Append("</RealOperate> ");
        }

        /// <summary>
        /// 买单调用捷云处理返回结果
        /// </summary>
        /// <param name="xmlInfo"></param>
        /// <returns></returns>
        protected JsonResultData CmpXmlResult(string xmlInfo)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlInfo);
            Dictionary<string, string> xmls = new Dictionary<string, string>();
            if (doc != null)
            {
                if (doc["ReturnMessage"] != null)
                {
                    foreach (XmlNode node in doc["ReturnMessage"])
                    {
                        if (node != null && node.Name != null && node.FirstChild != null)
                        {
                            if (node.Name == "MessageNo")
                            {
                                if (Convert.ToInt32(node.FirstChild.Value) == 1)
                                {
                                    return JsonResultData.Successed("");
                                }
                            }
                        }
                    }
                }
                else if (doc["ErrorMessage"] != null)
                {
                    if (doc["ErrorMessage"]["Message"] != null)
                    {
                        return JsonResultData.Failure(doc["ErrorMessage"]["Message"].InnerText);
                    }
                }
            }
            return JsonResultData.Failure("买单失败");
        }

        /// <summary>
        /// 买单处理返回结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected JsonResultData cmpPostApiResult(PostDataResult result)
        {
            if (result != null)
            {
                if (result.ErrorNo != "1")
                {
                    return JsonResultData.Failure("买单失败！" + result.Msg);
                }
                return JsonResultData.Successed();
            }
            else
            {
                return JsonResultData.Failure("买单失败！");
            }
        }
    }
}