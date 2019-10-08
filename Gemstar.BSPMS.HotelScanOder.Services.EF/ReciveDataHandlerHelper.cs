using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF
{
    public static class ReciveDataHandlerHelper
    {
        /// <summary>
        /// 处理接收到的业务信息
        /// </summary>
        /// <param name="data">接收到的业务信息</param>
        /// <param name="dataHandler">负责处理具体业务信息的职责链首对象</param>
        /// <param name="send">处理完业务信息后，发送回的响应信息是否发送成功</param>
        /// <param name="errorMsg">发送失败的原因</param>
        public static string HandleReciveData(List<PostData> requestData, Dictionary<string, IReciveDataHandler> dataHandler)
        {
            //提交过来的数据集


            StringBuilder result = new StringBuilder();

            string resultSplit = "";
            try
            {
                //请求类型
                foreach (var data in requestData)
                {
                    var businessType = data.BusinessType;
                    var businessHandler = dataHandler.ContainsKey(businessType) ? dataHandler[businessType] : dataHandler[""];
                    result.Append(resultSplit).Append(businessHandler.HandleReciveData(businessType, data.RequestData));
                    resultSplit = "|";
                }
            }
            catch (Exception ex)
            {

                var errorMsg = ex.Message;
                result.Remove(0, result.Length);
                result.Append("990执行错误，原因：").Append(ex.Message);
            }



            return JsonStr(result.ToString());
            //return "33";
        }

        /// <summary>
        /// 处理返回结果，用json 传递
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string JsonStr(string data)
        {
            string[] dataArr = data.Split('|');
            List<PostDataResult> results = new List<PostDataResult>();
            foreach (var item in dataArr)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                var DataResult = new PostDataResult()
                {
                    BusinessType = GetBusinessType(item),
                    ErrorNo = GetErrorNo(item),
                    Msg = GetMsg(item)
                };
                results.Add(DataResult);
            }
            var serializer = new JavaScriptSerializer();
            var jsonStr = serializer.Serialize(results);// <List<PostData>>(requestData);
            return jsonStr;
        }

        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetBusinessType(string data)
        {
            return data.Substring(0, 4);
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetErrorNo(string data)
        {
            return data.Substring(4, 1);
        }

        /// <summary>
        /// 返回结果说明
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetMsg(string data)
        {
            return data.Substring(5);
        }
    }
}
