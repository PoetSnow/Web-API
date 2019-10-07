using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Models
{
    public  static class CommonHelper
    {
        /// <summary>
        /// 自定义Distinct扩展方法
        /// </summary>
        /// <typeparam name="T">要去重的对象类</typeparam>
        /// <typeparam name="C">自定义去重的字段类型</typeparam>
        /// <param name="source">要去重的对象</param>
        /// <param name="getfield">获取自定义去重字段的委托</param>
        /// <returns></returns>
        public static IEnumerable<T> MyDistinct<T, C>(this IEnumerable<T> source, Func<T, C> getfield)
        {
            return source.Distinct(new Compare<T, C>(getfield));
        }
    }
}