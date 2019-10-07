using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.PosEnum;
using Gemstar.BSPMS.HotelScanOrder.Common.SPA.Store;
using Gemstar.BSPMS.HotelScanOrder.Common.SPAEnum;
using Gemstar.BSPMS.HotelScanOrder.Web.Areas.SpecialtyStore.Models;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Areas.SpecialtyStore.Controllers
{
    public class LoginController : Controller
    {
        // GET: SpecialtyStore/Login
        public ActionResult Index()
        {
            return View();
        }

        [JsonException]
        [HttpPost]
        public JsonResult Index(string usercode="",string pwd="",string snno="")
        {
            if (string.IsNullOrEmpty(usercode))
            {
                return Json( JsonResultData.Failure(Resources.GlobalResource.ResourceManager, Thread.CurrentThread.CurrentUICulture,"请输入用户名") );
            }
            if (string.IsNullOrEmpty(pwd))
            {
                return Json(JsonResultData.Failure(Resources.GlobalResource.ResourceManager, Thread.CurrentThread.CurrentUICulture, "请输入密码"));
            }

            var sendata = new LoginPostModel { hid="",pdano=snno,pwd=pwd,usercode =usercode};
            var res = GetPostData<LoginResultModel>(sendata, PostType.SPA_Login, out PostErrorModel postErrorModel);
            if (res == null || res.Count == 0)  //出现错误
            {
                if (postErrorModel != null)
                {
                    return Json(JsonResultData.Failure(Resources.GlobalResource.ResourceManager, Thread.CurrentThread.CurrentUICulture, postErrorModel.Msg));
                }
                else
                {
                    return Json(JsonResultData.Failure(Resources.GlobalResource.ResourceManager, Thread.CurrentThread.CurrentUICulture, "接口访问错误"));
                }
            }
            var _data = res.First();
            if (_data.rc == (int)PosPostStatus.Success)
            {
                //存储登录信息
                var currentinfo = GetService<ICurrentInfo>();
                currentinfo.Clear();                              
                currentinfo.UserCode = usercode;          //用户编码
                currentinfo.SNCode = snno;  //机器号

                currentinfo.SaveValues();
                CreateLoginCookie(new { usercode,snno }, usercode);
                return Json(JsonResultData.Successed());
            }
            else
            {
                return Json(JsonResultData.Failure(Resources.GlobalResource.ResourceManager, Thread.CurrentThread.CurrentUICulture, _data.msg));
            }
        }

        /// <summary>
        /// 通用获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setdata">发送信息</param>
        /// <param name="posttype">服务类型</param>
        /// <param name="postErrorModel">错误信息回传</param>
        /// <param name="hid">酒店ID</param>
        /// <returns></returns>
        protected List<T> GetPostData<T>(object setdata, string posttype, out PostErrorModel postErrorModel, string hid = "")
        {
            postErrorModel = null;
            var data = "";
            List<T> storeOrderResultModels;
            //获取接口数据
            var result = JsonHelp.PostDataResult(setdata, posttype);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {
                    data = result.Msg;
                    return storeOrderResultModels = JsonHelp.Deserialize<List<T>>(result.Msg);
                }
                else
                {
                    postErrorModel = new PostErrorModel() { Code = -1, Msg = result.Msg };
                }
            }
            else
            {
                postErrorModel = new PostErrorModel() { Code = -1, Msg = "接口数据转换错误" };
            }
            return null;
        }

        public void CreateLoginCookie(object userdata, string loginName)
        {
            var serializer = new JavaScriptSerializer();

            // 1. 把需要保存的用户数据转成一个字符串。
            string data = null;
            if (userdata != null)
                data = serializer.Serialize(userdata);

            // 2. 创建一个FormsAuthenticationTicket，它包含登录名以及额外的用户数据。
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, loginName, DateTime.Now, DateTime.Now.AddDays(1), true, data);
            // 3. 加密Ticket，变成一个加密的字符串。
            string cookieValue = FormsAuthentication.Encrypt(ticket);
            // 4. 根据加密结果创建登录Cookie
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;

            cookie.Path = FormsAuthentication.FormsCookiePath;
            cookie.Expires = DateTime.Now.AddDays(1);
            // 5. 写登录Cookie
            Response.Cookies.Remove(cookie.Name);
            Response.Cookies.Add(cookie);
        }
        protected T GetService<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }

    }
}