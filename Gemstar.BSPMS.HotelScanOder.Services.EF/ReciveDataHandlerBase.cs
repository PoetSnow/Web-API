using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF
{
    /// <summary>
    /// 实现处理数据接口的抽象类，提供处理数据的一些公共方法实现，并且定义需要由子类实现的抽象方法
    /// </summary>
    public abstract class ReciveDataHandlerBase : IReciveDataHandler
    {


        /// <summary>
        /// 设置下一处理数据接口实例
        /// <example>
        /// <code>
        /// IReciveDataHandler handleAll = new HandleAll();
        /// IReciveDataHandler handLog = new HandleLog();
        /// handLog.SetNextHandler(handleAll);
        /// 
        /// //recive Data handle
        /// handLog.HandleReciveData(data);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="handler">下一处理数据接口实例,为null表示此实例位于职责链的末尾</param>
        public void SetNextHandler(IReciveDataHandler handler)
        {
            nextHandler = handler;
        }
        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        /// <param name="requestType">功能参数代码</param>
        /// <param name="hid">酒店代码</param>
        /// <param name="module">功能模块</param>
        /// <param name="requestArgs">接收到的业务信息中的一个请求的请求参数列表</param>
        /// <returns>处理请求后的返回业务信息，如果不需要返回信息，请返回空</returns>
        public string HandleReciveData(string requestType, string requestData)
        {
            string currentHandleType = GetHandleDataType();
            if (requestType == currentHandleType || CanHandleAllData())
            {
                return currentHandleType + HandleData(requestData);
            }
            else
            {
                return GetNextHandler().HandleReciveData(requestType, requestData);
            }
        }
        /// <summary>
        /// 获取下一处理接口实例，返回的实例有可能为null值
        /// <remarks>
        /// <para>一般职责链的末尾实例的下一处理接口实例为null值，所以末尾的实例应该是可以处理所有的业务信息或者必须保证此前的处理实例能够处理全部的请求</para>
        /// </remarks>
        /// </summary>
        /// <returns>下一处理接口实例</returns>
        public IReciveDataHandler GetNextHandler()
        {
            return nextHandler;
        }
        /// <summary>
        /// 处理业务信息
        /// </summary>
        /// <param name="requestType">接收类型</param>
        /// <param name="hid">酒店代码</param>
        /// <param name="module">模块</param>
        /// <param name="requestArgs">当前处理请求中的所有参数信息</param>
        protected abstract string HandleData(string requestData);
        /// <summary>
        /// 获取可以处理的业务类型
        /// </summary>
        /// <returns>可以处理的业务类型</returns>
        public abstract string GetHandleDataType();
        /// <summary>
        /// 是否可以处理全部业务信息，默认为false，只有在创建处理全部请求子类，如日志子类时才需要重写此方法
        /// <remarks>重写了此方法并且返回true的类，在构建职责链时必须位于最末端，否则会导致其后面的实例无法处理请求</remarks>
        /// </summary>
        /// <returns>是否可以处理全部业务信息</returns>
        protected virtual bool CanHandleAllData()
        {
            return false;
        }
        /// <summary>
        /// 下一处理实例
        /// </summary>
        private IReciveDataHandler nextHandler;
        /// <summary>
        /// 安全的从数据组获取值，如果值为空或者不存在则返回默认值
        /// </summary>
        /// <param name="requestArgs">数据组</param>
        /// <param name="index">索引值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public string GetStringSafely(string[] requestArgs, int index, string defaultValue)
        {
            if (requestArgs.Length > index)
            {
                if (!string.IsNullOrWhiteSpace(requestArgs[index]))
                {
                    return requestArgs[index];
                }
            }
            return defaultValue;
        }

    
        public JavaScriptSerializer serializer
        {
            get { return new JavaScriptSerializer(); }
        }

        /// <summary>
        /// bool类型值的转换
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string GetBoolValByString(string val)
        {
            if (val == "True" || val == "true")
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 添加错误日志记录
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="msgContent"></param>
        public void AddErrorMsg(string hid, string msgContent)
        {
            if (string.IsNullOrEmpty(hid))
            {
                var _db = GetDBCommonContext.GetPmsDB(hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                _db.Database.ExecuteSqlCommand("exec up_pos_sm_OperLog @hid=@hid,@opertype=@opertype,@opercontent=@opercontent",
                                              new SqlParameter("@hid", hid),
                                              new SqlParameter("@opertype", "商米点餐"),
                                              new SqlParameter("@opercontent", msgContent));
            }
            else
            {
                var _db = GetDBCommonContext.GetCenterDB(hid);  //线上使用运营库

                _db.Database.ExecuteSqlCommand("exec up_pos_sm_OperLog @hid=@hid,@opertype=@opertype,@opercontent=@opercontent",
                                              new SqlParameter("@hid", hid),
                                              new SqlParameter("@opertype", "商米点餐"),
                                              new SqlParameter("@opercontent", msgContent));
            }
        }
    }
}
