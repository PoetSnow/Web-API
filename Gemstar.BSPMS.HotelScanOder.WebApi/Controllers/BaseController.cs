using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Gemstar.BSPMS.HotelScanOrder.Services.EF;
using Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos;
using Gemstar.BSPMS.HotelScanOrder.Common;

namespace Gemstar.BSPMS.HotelScanOrder.WebApi.Controllers
{
    public class BaseController : ApiController
    {
        public Dictionary<string, IReciveDataHandler> ReciveDataHandler
        {
            get { return initBusinessHandlers(); }

        }


        public static Dictionary<string, IReciveDataHandler> initBusinessHandlers()
        {
            //初始化接收处理的职责链对象
            List<Type> TypeItemList = new List<Type>();
            Assembly[] AssbyCustmList = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assItem in AssbyCustmList)
            {
                TypeItemList.AddRange(assItem.GetTypes().Where(x => x.BaseType == typeof(ReciveDataHandlerBase)).ToList());
            }
            ReciveDataHandlerBase[] handlerList = new ReciveDataHandlerBase[TypeItemList.Count];
            for (int i = 0; i < TypeItemList.Count; i++)
            {
                handlerList[i] = (ReciveDataHandlerBase)Activator.CreateInstance(TypeItemList[i]);
            }

            var handlers = initDataHandlerLink(handlerList);
            return handlers;
        }

        /// <summary>
        /// 将指定的所有请求处理类实例初始化成职责链对象，其在职责链中的位置跟其在参数中的位置相同
        /// </summary>
        /// <param name="handlers">所有可以处理请求的类实例，注意，处理全部的处理类实例必须位于末尾</param>
        /// <returns>职责链对象的入口</returns>
        private static Dictionary<string, IReciveDataHandler> initDataHandlerLink(params ReciveDataHandlerBase[] handlers)
        {
            var dic = new Dictionary<string, IReciveDataHandler>();
            var currentHandler = handlers[0];
            dic.Add(currentHandler.GetHandleDataType(), currentHandler);
            for (var i = 1; i < handlers.Length; i++)
            {
                var nextHandler = handlers[i];
                currentHandler.SetNextHandler(nextHandler);
                currentHandler = nextHandler;
                dic.Add(nextHandler.GetHandleDataType(), nextHandler);
            }
            return dic;
        }
    }
}