using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Web.App_Start;
using RedisSessionProvider.Config;
using StackExchange.Redis;

namespace Gemstar.BSPMS.HotelScanOrder.Web
{
    public class MvcApplication : System.Web.HttpApplication, ISettingProvider
    {

        private static SettingInfo _settingInfo;
        /// <summary>
        /// 配置信息
        /// </summary>

        private static ConfigurationOptions _redisConfigOpts;
        private static ConnectionMultiplexer _redisConnection;

        protected void Application_Start()
        {
            GetSettingInfoFromRemoteServer();
            SetConfig();
            SetRedisConfig();
            AutofacConfig.Config(this);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public static void WriteRedisException(Exception ex)
        {
            try
            {
                var file = HttpContext.Current.Server.MapPath("~/Logs");
                if (!Directory.Exists(file))
                {
                    Directory.CreateDirectory(file);
                }
                file = Path.Combine(file, string.Format("{0}.log", DateTime.Today.ToString("yyyyMMdd")));
                using (var fs = File.AppendText(file))
                {
                    fs.WriteLine("--------------------------");
                    fs.WriteLine(string.Format("{0} 错误信息:{1}", DateTime.Now.ToString(), ex.Message));
                    fs.WriteLine(string.Format("调用堆栈:{0}", ex.StackTrace));
                    if (ex.InnerException != null)
                    {
                        var inner = ex.InnerException;
                        while (inner.InnerException != null)
                        {
                            inner = inner.InnerException;
                        }
                        fs.WriteLine(string.Format("内部异常错误信息：{0}", inner.Message));
                    }
                    fs.Close();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 设置配置信息
        /// </summary>
        private void SetConfig()
        {

            var appSettings = ConfigurationManager.AppSettings;

            Setting.InterfaceUrl = appSettings["InterfaceUrl"];
            Setting.PicUrl = appSettings["PicUrl"];


        }
        public static void GetSettingInfoFromRemoteServer()
        {
            var settingUrl = ConfigurationManager.AppSettings["pmsSettingUrl"];
            var setting2Url = ConfigurationManager.AppSettings["pmsSetting2Url"];
            _settingInfo = SettingHelper.GetSetting(settingUrl, setting2Url);
        }
        public static ConnectionMultiplexer RedisConnection
        {
            get
            {
                return _redisConnection;
            }
        }

        public SettingInfo SettingInfo
        {
            get
            {
                return _settingInfo;
            }
        }

        public static void SetRedisConfig()
        {
            RedisSessionConfig.SessionTimeout = TimeSpan.FromHours(4);
            var redisServerIp = _settingInfo.ProductSettingInfo.RedisServerIp;
            var redisServerPort = _settingInfo.ProductSettingInfo.RedisServerPort;
            var redisPassword = _settingInfo.ProductSettingInfo.RedisServerPassword;

            var redisServer2Ip = _settingInfo.ProductSettingInfo.RedisServer2Ip;
            var redisServer2Port = _settingInfo.ProductSettingInfo.RedisServer2Port;
            var redis2Password = _settingInfo.ProductSettingInfo.RedisServer2Password;
            //if (IsTestEnv)
            //{
            //    redisServerIp = _settingInfo.TestSettingInfo.RedisServerIp;
            //    redisServerPort = _settingInfo.TestSettingInfo.RedisServerPort;
            //    redisPassword = _settingInfo.TestSettingInfo.RedisServerPassword;

            //    redisServer2Ip = _settingInfo.TestSettingInfo.RedisServer2Ip;
            //    redisServer2Port = _settingInfo.TestSettingInfo.RedisServer2Port;
            //    redis2Password = _settingInfo.TestSettingInfo.RedisServer2Password;
            //}
            redisPassword = CryptHelper.DecryptDES(redisPassword);
            redis2Password = CryptHelper.DecryptDES(redis2Password);

            //测试主redis是否能连接成功，可以则使用主的，否则使用备用的
            try
            {
                _redisConfigOpts = ConfigurationOptions.Parse(string.Format("{0}:{1}", redisServerIp, redisServerPort));
                _redisConfigOpts.Password = redisPassword;
                _redisConnection = ConnectionMultiplexer.Connect(_redisConfigOpts);
                var db = _redisConnection.GetDatabase();
                db.StringSet("testConnect", "testConnect");
                db.KeyDelete("testConnect");
            }
            catch
            {
                //主redis无效，直接使用备用的
                redisServerIp = redisServer2Ip;
                redisServerPort = redisServer2Port;
                redisPassword = redis2Password;
                _redisConfigOpts = ConfigurationOptions.Parse(string.Format("{0}:{1}", redisServerIp, redisServerPort));
                _redisConfigOpts.Password = redisPassword;
                _redisConnection = ConnectionMultiplexer.Connect(_redisConfigOpts);
            }

            RedisConnectionConfig.GetSERedisServerConfig = (HttpContextBase context) =>
            {
                return new KeyValuePair<string, ConfigurationOptions>(
                    "DefaultConnection",
                    _redisConfigOpts);
            };
            RedisSessionConfig.SessionExceptionLoggingDel = (Exception e) =>
            {
                WriteRedisException(e);
            };
        }
    }
}
