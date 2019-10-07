﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Login;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Filter
{
    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //if (!filterContext.HttpContext.Request.IsAuthenticated)
            //{

            //    logout(filterContext);
            //    return;
            //}

            //验证Session是否存在
            var currentinfo = GetService<ICurrentInfo>();
            currentinfo.LoadValues();
            //Log.WriteLog("测试日志", "", currentinfo.UserCode);
            if (string.IsNullOrWhiteSpace(currentinfo.UserCode))
            {
                logout(filterContext);
                //try
                //{
                //    //session信息丢失，读取存储在登录cokkie中的信息重新登录，不验证密码
                //    HttpCookie cookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                //    if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                //    {
                //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                //        var serializer = new JavaScriptSerializer();
                //        //  serializer.
                //        var loginmodel = JsonHelp.Deserialize<LoginPostModel>(ticket.UserData);
                //        //验证登录标识
                //        if (string.IsNullOrEmpty(loginmodel.))
                //        {
                //            //登出
                //           // logout(filterContext);
                //        }



                //    }
                //    else
                //    {
                //        //登出
                //        // logout(filterContext);
                //    }

                //}
                //    catch
                //    {
                //        //logout(filterContext);
                //    }
                //filterContext.Result = new JsonResult() { Data = JsonResultData.Failure("登录超时，请重新登录", 1), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }

        /// <summary>
        /// 获取指定服务接口的实例
        /// </summary>
        /// <typeparam name="T">服务接口类型</typeparam>
        /// <returns>指定服务接口的实例</returns>
        protected T GetService<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }

        public void logout(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult() { Data = JsonResultData.Failure("登录超时，请重新登录", 1) };
            }
            else
            {
                //登出
                FormsAuthentication.SignOut();
                filterContext.Result = new RedirectResult("/Home/index");
            }
        }
    }
}