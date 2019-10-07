using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemstar.BSPMS.HotelScanOrder.Common
{
    /// <summary>
    /// 接收到数据后的处理接口
    /// <remarks>
    /// <para>在main函数中需要将此接口实例组装成职责链，然后提供一个访问点，接收到数据后会直接调用此访问点来进行数据处理。</para>
    /// <para>每个子类只需要处理与自己处理类型相关的业务信息即可，与自己无关的直接调用获取下一处理实例，交给他进行处理即可</para>
    /// </remarks>
    /// </summary>
    public interface IReciveDataHandler
    {
        /// <summary>
        /// 处理接收到的数据，每个子类只需要处理与自己处理类型相关的业务信息即可，与自己无关的直接调用获取下一处理实例，交给他进行处理即可
        /// </summary>
        
        /// <param name="requestType">接收到的业务信息中的一个请求的请求类型</param>
        /// <param name="requestArgs">接收到的业务信息中的一个请求的请求参数列表</param>
        /// <returns>处理请求后的返回业务信息，如果不需要返回信息，请返回空</returns>
        string HandleReciveData(string requestType, string requestData);
        /// <summary>
        /// 获取下一处理实例
        /// </summary>
        /// <returns>下一处理实例</returns>
        IReciveDataHandler GetNextHandler();

    }
}
