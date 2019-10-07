using System;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Common
{
    /// <summary>
    /// 连接字符串辅助类
    /// </summary>
    public static class ConnStrHelper
    {
        /// <summary>
        /// 将连接字符串的各配置项组装成连接字符串
        /// </summary>
        /// <param name="dbServerIp">数据库服务器ip</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="uid">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="appName">应用名称</param>
        /// <param name="isPwdEncrypted">是否密码已经加密,默认为true</param>
        /// <returns></returns>
        public static string GetConnStr(string dbServerIp, string dbName, string uid, string pwd, string appName, bool isPwdEncrypted = true)
        {
            if (string.IsNullOrWhiteSpace(dbServerIp) || string.IsNullOrWhiteSpace(dbName) || string.IsNullOrWhiteSpace(uid))
            {
                throw new ApplicationException("登录信息已经失效，请重新登录!");
            }
            if (isPwdEncrypted && !string.IsNullOrWhiteSpace(pwd))
            {
                pwd = CryptHelper.DecryptDES(pwd);
            }
            return string.Format("data source={0};initial catalog={1};user id={2};password={3};MultipleActiveResultSets=True;App={4}", dbServerIp, dbName, uid, pwd, appName);
        }
    }
}