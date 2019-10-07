using System;
using System.Configuration;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF
{


    public static class GetDBCommonContext
    {
        private static SettingInfo _settingInfo;
        /// <summary>
        /// 从配置文件中的设置url地址处读取配置信息
        /// 在程序第一次启动或数据库连接不成功时调用此方法来重新读取配置信息
        /// 使用此方法可以避免在此程序部署多个实例后，修改配置信息需要修改好多台服务器的问题
        /// </summary>
        private static void GetSettingInfoFromRemoteServer()
        {
            var settingUrl = ConfigurationManager.AppSettings["pmsSettingUrl"];
            var setting2Url = ConfigurationManager.AppSettings["pmsSetting2Url"];
            _settingInfo = SettingHelper.GetSetting(settingUrl, setting2Url);
        }

        /// <summary>
        /// 获取中央数据库的连接字符串
        /// </summary>
        /// <param name="isEnvTest">是否测试环境</param>
        /// <returns>设置中的项构成的中央数据库的连接字符串</returns>
        private static string GetCenterDBConnStr(bool isEnvTest)
        {
            try
            {
                if (_settingInfo == null)
                {
                    GetSettingInfoFromRemoteServer();
                }

                var dbServerIp = _settingInfo.ProductSettingInfo.DatabaseServerIp;
                var dbName = _settingInfo.ProductSettingInfo.DatabaseName;
                var dbUser = _settingInfo.ProductSettingInfo.DatabaseUserName;
                var pwd = _settingInfo.ProductSettingInfo.DatabasePassword;
                if (isEnvTest)
                {
                    dbServerIp = _settingInfo.TestSettingInfo.DatabaseServerIp;
                    dbName = _settingInfo.TestSettingInfo.DatabaseName;
                    dbUser = _settingInfo.TestSettingInfo.DatabaseUserName;
                    pwd = _settingInfo.TestSettingInfo.DatabasePassword;
                    //要求数据库名称必须包含test
                    if (dbName.IndexOf("test", StringComparison.OrdinalIgnoreCase) < 0 && dbName.IndexOf("dev", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        throw new ApplicationException("测试环境下连接的数据库名称中必须包含test或dev字样");
                    }
                }

                return ConnStrHelper.GetConnStr(dbServerIp, dbName, dbUser, pwd, _settingInfo.ApplicationName);
            }
            catch (Exception ex)
            {
                //出现异常时，重新从远端服务器上取一次配置信息，因为是配置信息中的数据库信息丢失了
                //GetSettingInfoFromRemoteServer();
                //return GetCenterDBConnStr(isEnvTest);
                return "";
            }
        }

        private static string GetHotelDbConnStr(DbCommonContext centerDb, string hid, bool isEnvTest)
        {
            try
            {
                if (_settingInfo == null)
                {
                    GetSettingInfoFromRemoteServer();
                }
                var hotelInfoServices = new HotelInfoService(centerDb);
                var hotelInfos = hotelInfoServices.GetHotelInfo(hid);
                if (hotelInfos == null)
                {
                    throw new ApplicationException("指定酒店没有开通此产品，请与软件供应商联系");
                }
                if (isEnvTest)
                {
                    //要求数据库名称必须包含test
                    if (hotelInfos.DbName.IndexOf("test", StringComparison.OrdinalIgnoreCase) < 0 && hotelInfos.DbName.IndexOf("dev", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        throw new ApplicationException("测试环境下连接的数据库名称中必须包含test或dev字样");
                    }
                }
                return ConnStrHelper.GetConnStr(hotelInfos.DbServer, hotelInfos.DbName, hotelInfos.Logid, hotelInfos.LogPwd, _settingInfo.ApplicationName);
            }
            catch (Exception ex)
            {
                //出现异常时，重新从远端服务器上取一次配置信息，因为是配置信息中的数据库信息丢失了
                //GetSettingInfoFromRemoteServer();
                //return GetHotelDbConnStr(centerDb, hid, isEnvTest);
                return "";
            }
        }

        public static DbPmsCommonContext GetPmsDB(string hid)
        {
            var isEnvTest = ConfigurationManager.AppSettings["isEnvTest"] == "1" ? true : false;
            //数据库字符串连接为空，取线上业务库的数据连接，
            //反之直接连接线下数据库
            if (string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString))
            {
                if (!string.IsNullOrEmpty(GetCenterDBConnStr(isEnvTest)))
                {
                    var centerDB = new DbCommonContext(GetCenterDBConnStr(isEnvTest));

                    if (!string.IsNullOrEmpty(GetHotelDbConnStr(centerDB, hid, isEnvTest)))
                    {
                        var pmsDB = new DbPmsCommonContext(GetHotelDbConnStr(centerDB, hid, isEnvTest));
                        return pmsDB;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
            else
            {
                var pmsDB = new DbPmsCommonContext(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
                return pmsDB;
            }
        }

        public static DbCommonContext GetCenterDB(string hid)
        {
            var isEnvTest = ConfigurationManager.AppSettings["isEnvTest"] == "1" ? true : false;
            var centerDB = new DbCommonContext(GetCenterDBConnStr(isEnvTest));
            return centerDB;
        }
    }
}
