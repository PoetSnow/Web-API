using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Logs;
using Newtonsoft.Json;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models
{
    public static class JsonHelp
    {
        /// <summary>
        /// 调用接口处理请求
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Post(string jsonStr)
        {
            string result;
            HttpClient client;
            StringContent content;

            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            result = client.PostAsync(Setting.InterfaceUrl, content).Result.Content.ReadAsStringAsync().Result;
            return result;

        }

        /// <summary>
        /// 将对象转换成json字符串
        /// </summary>
        /// <typeparam name="T">请求参数类型</typeparam>
        /// <param name="model">请求参数</param>
        /// <param name="type">请求类型</param>
        /// <param name="hid">请求酒店ID</param>
        /// <returns></returns>
        public static string Serialize<T>(T model, string type)
        {

            var serializer = new JavaScriptSerializer();
            var requestData = serializer.Serialize(model);

            List<PostData> list = new List<PostData>();

            PostData data = new PostData
            {
                BusinessType = type,
                RequestData = requestData
            };
            list.Add(data);
            //AddPostDataByLog(type, requestData, hid, module, list);
            return serializer.Serialize(list);
        }

        /// <summary>
        /// 每次接口请求同时添加日志记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="hid"></param>
        /// <param name="module"></param>
        /// <param name="list"></param>
        private static void AddPostDataByLog(string type, string content, string hid, string module, List<PostData> list)
        {
            PostType postType = new PostType();
            var serializer = new JavaScriptSerializer();
            OperLogPostModel model = new OperLogPostModel()
            {
                Hid = hid,
                OperType = postType.OperLog,
                OperContent = type + ":" +content
            };
            var requestData = serializer.Serialize(model);
            PostData data = new PostData
            {              
                BusinessType = postType.OperLog,
                RequestData = requestData
            };
            list.Add(data);

        }

        /// <summary>
        /// 字符串转模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string data)
        {
            var serializer = new JavaScriptSerializer();
            var requestData = serializer.Deserialize<T>(data);
            return requestData;
        }


        public static List<PostDataResult> PostDataResult(string jsonData)
        {
            JsonReader reader = new JsonTextReader(new StringReader(jsonData));
            while (reader.Read())
            {
                jsonData = reader.Value.ToString();
            }
            var serializer = new JavaScriptSerializer();
            var results = serializer.Deserialize<List<PostDataResult>>(jsonData);
            return results;
            //foreach (var item in results)
            //{
            //    if (item.ErrorNo == "0")
            //    {
            //        return Json(JsonResultData.Failure(item.Msg));
            //    }
            //    else
            //    {
            //        return Json(JsonResultData.Successed());
            //    }
            //}
            //return Json(JsonResultData.Successed());
        }

        /// <summary>
        /// 调用接口获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="hid"></param>
        /// <param name="postType"></param>
        /// <returns></returns>
        public static PostDataResult PostDataResult<T>(T model, string postType, string notifyUrl = "")
        {
           

            //把接口请求参数封装传递
            var jsonStr = Serialize(model, postType.ToString());
            //调用接口
            string jsonResult = "";
            if (string.IsNullOrEmpty(notifyUrl))    //为空表示是线上程序
            {
                jsonResult = JsonHelp.Post(jsonStr);
            }
            else
            {
                jsonResult = JsonHelp.Post(jsonStr, notifyUrl);
            }

            PostDataResult result = JsonHelp.PostDataResult(jsonResult).FirstOrDefault();

            return result;
        }




        public static string Serialize<T>(T model)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(model);
        }

        /// <summary>
        /// 支付接口请求方法
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string PostPay(string jsonStr, string Url)
        {
            HttpClient client;

            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(Url + jsonStr).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        public static string PostCheck(string url, string queryString)
        {
            string result;
            HttpClient client;
            StringContent content;

            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

            content = new StringContent(queryString, Encoding.UTF8, "text/xml");
            result = client.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
            return result;
        }



        public static string Post(string jsonStr, string notifyUrl)
        {
            string result;
            HttpClient client;
            StringContent content;

            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            result = client.PostAsync(notifyUrl, content).Result.Content.ReadAsStringAsync().Result;
            return result;
        }

    }
}