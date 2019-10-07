using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models
{
    /// <summary>
    /// 当前登录信息
    /// </summary>
    public class CurrentInfo : ICurrentInfo
    {
        public const string SessionKeyGroupId = "GroupId";
        public const string SessionKeyHotelId = "HotelId";
        //public const string SessionKeySignature = "Signature";
        //public const string SessionKeyHotelName = "HotelName";
        //public const string SessionKeyProductType = "ProductType";

        public const string SessionKeyUserId = "UserId";
        public const string SessionKeyUserCode = "UserCode";
        public const string SessionKeyUserName = "UserName";

        public const string SessionKeyRefeList = "RefeList";    //营业点列表
        public const string SessionKeyTabId = "TabId";    //营业点列表
        public const string SessionKeyItemList = "ItemList";    //营业点列表

        public const string SessionKeyRequestList = "RequestList";  //要求列表
        public const string SessionKeyRefeId = "RefeId";  //营业点

        public const string SessionKeySNCode = "SNCode"; //机器号


        public const string SessionKeyModule = "Module";  //营业点


        public const string SessionKeyWxComid = "WxComid";  //运营酒店ID
        public const string SessionKeyOpenidUrl = "OpenidUrl";  //openId接口地址
        public const string SessionKeyTemplateMessageUrl = "TemplateMessageUrl";  //模板接口地址
        public const string SessionKeyCreatePayOrderUrl = "CreatePayOrderUrl";  //支付下单地址
        public const string SessionKeyPayOrderUrl = "PayOrderUrl";  //支付地址
        public const string SessionKeyNotifuUrl = "NotifuUrl";  //接口地址 线下程序使用

        public const string SessionKeyOpenId = "OpenId";    //

        public const string SessionKeyWxPaytype = "WxPaytype";

        public const string SessionKeyPayBillNameStr = "PayBillNameStr";
        public const string SessionKeyPayBillPostModelStr = "PayBillPostModelStr";

        public const string SessionKeyTabNo = "TabNo";
        public const string SessionKeyTabName = "TabName";

        public const string SessionKeyIsCs = "IsCs";

        private Dictionary<string, string> _values;

        #region 登录信息

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId
        {
            get { return getValue(SessionKeyUserId); }
            set { setValue(SessionKeyUserId, value); }
        }

        /// <summary>
        /// 用户代码
        /// </summary>
        public string UserCode
        {
            get { return getValue(SessionKeyUserCode); }
            set { setValue(SessionKeyUserCode, value); }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return getValue(SessionKeyUserName); }
            set { setValue(SessionKeyUserName, value); }
        }

        public string GroupId
        {
            get { return getValue(SessionKeyGroupId); }
            set { setValue(SessionKeyGroupId, value); }
        }

        /// <summary>
        /// 模块
        /// </summary>
        public string Module
        {
            get { return getValue(SessionKeyModule); }
            set { setValue(SessionKeyModule, value); }
        }

        /// <summary>
        /// 当前是否集团，如果是集团则返回true，如果是单体酒店，则返回false
        /// </summary>
        public bool IsGroup { get { return !string.IsNullOrWhiteSpace(GroupId); } }

        /// <summary>
        /// 集团或酒店id，如果当前是集团时则返回集团id，如果当前是单体酒店时，则返回酒店id
        /// </summary>
        public string GroupHotelId { get { return string.IsNullOrWhiteSpace(GroupId) ? HotelId : GroupId; } }


        public string HotelId
        {
            get { return getValue(SessionKeyHotelId); }
            set { setValue(SessionKeyHotelId, value); }
        }
        #endregion

        #region 业务信息

        /// <summary>
        /// 营业点
        /// </summary>            
        public string RefeList
        {
            get { return getValue(SessionKeyRefeList); }
            set { setValue(SessionKeyRefeList, value); }
        }

        /// <summary>
        /// 餐台ID
        /// </summary>
        public string TabId
        {
            get { return getValue(SessionKeyTabId); }
            set { setValue(SessionKeyTabId, value); }
        }


        /// <summary>
        /// 消费项目
        /// </summary>
        public string ItemList
        {
            get { return getValue(SessionKeyItemList); }
            set { setValue(SessionKeyItemList, value); }
        }

        /// <summary>
        /// 营业点
        /// </summary>
        public string RefeId
        {
            get { return getValue(SessionKeyRefeId); }
            set { setValue(SessionKeyRefeId, value); }
        }

        /// <summary>
        /// 要求
        /// </summary>
        public string RequestList
        {
            get { return getValue(SessionKeyRequestList); }
            set { setValue(SessionKeyRequestList, value); }
        }

        /// <summary>
        /// 机器号
        /// </summary>
        public string SNCode
        {
            get { return getValue(SessionKeySNCode); }
            set { setValue(SessionKeySNCode, value); }
        }

        #endregion

        private void setValue(string key, string value)
        {
            _values[key] = value;
        }

        private string getValue(string key)
        {
            if (_values.ContainsKey(key))
            {
                return _values[key];
            }
            return string.Empty;
        }

        public void Clear()
        {
            //清空会话中的其他信息
            _values.Clear();
            var _session = HttpContext.Current.Session;
            if (_session != null)
            {
                _session.Clear();
            }
        }

        /// <summary>
        /// 从存储中加载值
        /// </summary>
        public void LoadValues()
        {
            _values = null;
            var sessionKey = GetValueKey();
            if (!string.IsNullOrWhiteSpace(sessionKey))
            {
                try
                {
                    var serializer = new JavaScriptSerializer();

                    //if (Setting.IsCS == "true")   //线下程序
                    //{
                    //    var valueStr = HttpContext.Current.Session[sessionKey];
                    //    if (valueStr != null)
                    //    {
                    //        _values = serializer.Deserialize<Dictionary<string, string>>(valueStr.ToString());
                    //    }
                    //}
                    //else
                    //{
                    var redisConnection = MvcApplication.RedisConnection;
                    var db = redisConnection.GetDatabase();
                    var task = db.StringGetAsync(sessionKey);
                    task.Wait();
                    var valueStr = task.Result;
                    if (!string.IsNullOrWhiteSpace(valueStr))
                    {
                        _values = serializer.Deserialize<Dictionary<string, string>>(valueStr);
                    }
                    //  }
                    //

                }
                catch (Exception ex)
                {
                    MvcApplication.WriteRedisException(ex);
                }
            }
            if (_values == null)
            {
                _values = new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// 将值保存到存储中
        /// </summary>
        public void SaveValues()
        {
            var sessionKey = GetValueKey();
            if (!string.IsNullOrWhiteSpace(sessionKey))
            {
                var serializer = new JavaScriptSerializer();
                var valueStr = serializer.Serialize(_values);
                //if (Setting.IsCS == "true")
                //{
                //    try
                //    {
                //        HttpContext.Current.Session[sessionKey] = valueStr;

                //    }
                //    catch
                //    {
                //        HttpContext.Current.Session.Remove(sessionKey);

                //    }
                //}
                //else
                //{
                var redisconnection = MvcApplication.RedisConnection;
                var db = redisconnection.GetDatabase();

                try
                {
                    // HttpContext.Current.Session[sessionKey] = valueStr;
                    db.StringSet(sessionKey, valueStr);
                }
                catch
                {
                    // HttpContext.Current.Session.Remove(sessionKey);
                    db.KeyDelete(sessionKey);
                    db.StringSet(sessionKey, valueStr);
                }
                // }

            }
        }

        private string GetValueKey()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                return string.Format("Session{0}ValueKey", HttpContext.Current.Session.SessionID);
            }
            return string.Empty;
        }

        #region 微信运营基本信息


        /// <summary>
        /// 运营酒店ID
        /// </summary>
        public string GsWxComid
        {
            get { return getValue(SessionKeyWxComid); }
            set { setValue(SessionKeyWxComid, value); }
        }

        /// <summary>
        /// openId接口地址
        /// </summary>
        public string GsWxOpenidUrl
        {
            get { return getValue(SessionKeyOpenidUrl); }
            set { setValue(SessionKeyOpenidUrl, value); }
        }

        /// <summary>
        /// 模板接口地址
        /// </summary>
        public string GsWxTemplateMessageUrl
        {
            get { return getValue(SessionKeyTemplateMessageUrl); }
            set { setValue(SessionKeyTemplateMessageUrl, value); }
        }

        /// <summary>
        /// 支付下单地址
        /// </summary>
        public string GsWxCreatePayOrderUrl
        {
            get { return getValue(SessionKeyCreatePayOrderUrl); }
            set { setValue(SessionKeyCreatePayOrderUrl, value); }
        }

        /// <summary>
        /// 支付地址
        /// </summary>
        public string GsWxPayOrderUrl
        {
            get { return getValue(SessionKeyPayOrderUrl); }
            set { setValue(SessionKeyPayOrderUrl, value); }
        }

        /// <summary>
        /// 接口地址 线下程序使用
        /// </summary>
        public string NotifyUrl
        {
            get { return getValue(SessionKeyNotifuUrl); }
            set { setValue(SessionKeyNotifuUrl, value); }
        }

        public string OpenId
        {
            get { return getValue(SessionKeyOpenId); }
            set { setValue(SessionKeyOpenId, value); }
        }

        public string WxPaytype
        {
            get { return getValue(SessionKeyWxPaytype); }
            set { setValue(SessionKeyWxPaytype, value); }
        }

        public string PayBillNameStr
        {
            get { return getValue(SessionKeyPayBillNameStr); }
            set { setValue(SessionKeyPayBillNameStr, value); }
        }

        public string PayBillPostModelStr
        {
            get { return getValue(SessionKeyPayBillPostModelStr); }
            set { setValue(SessionKeyPayBillPostModelStr, value); }
        }

        #endregion

        public string TabNo
        {
            get { return getValue(SessionKeyTabNo); }
            set { setValue(SessionKeyTabNo, value); }
        }

        public string TabName
        {
            get { return getValue(SessionKeyTabName); }
            set { setValue(SessionKeyTabName, value); }
        }


        public string IsCs
        {
            get { return getValue(SessionKeyIsCs); }
            set { setValue(SessionKeyIsCs, value); }
        }
    }
}