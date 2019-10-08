using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Gemstar.BSPMS.HotelScanOrder.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Master;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Login;
using Gemstar.BSPMS.HotelScanOrder.Web.Models;

namespace Gemstar.BSPMS.HotelScanOrder.Web.Controllers
{
    public class HomeController : Controller
    {
        PostType postType = new PostType();

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost, JsonException]
        public ActionResult Index(LoginPostModel model)
        {
            
            //存储登录信息
            var currentinfo = GetService<ICurrentInfo>();
            currentinfo.Clear();

            //从运营库取出酒店信息 这个Hid为运营系统 扫码点餐酒店设置的HotelCode 
            //线下程序取出接口地址。用设置的接口地址进行访问
            SetCacheHotel(model.Hid, currentinfo);


            if (currentinfo.IsCs == "false")
            {
                //密码需要进行加密进行传递
                model.Password = PasswordHelper.GetEncryptedPassword(model.Username, model.Password);
            }

            model.Hid = currentinfo.HotelId;    //设置酒店ID            
            ////判断用户是否登录成功
            var result = JsonHelp.PostDataResult(model, postType.Login, currentinfo.NotifyUrl);
            if (result != null)
            {
                if (result.ErrorNo == "1")
                {

                    //返回酒店列表
                    var ResortList = JsonHelp.Deserialize<List<LoginResultModel>>(result.Msg);
                    if (ResortList.Count == 1)
                    {
                        //如果只有一个酒店，取默认
                        var info = ResortList.FirstOrDefault();
                        currentinfo.Module = info.SysType;  //模块
                    }

                    // currentinfo.UserName = model.Username;
                    currentinfo.UserCode = model.Username;          //用户编码
                                                                    //  currentinfo.HotelId = model.Hid;
                    CreateLoginCookie(model, model.Username);
                    currentinfo.SaveValues();


                    return Json(JsonResultData.Successed());
                }
                else
                {

                    return Json(JsonResultData.Failure(result.Msg));
                }
            }
            else
            {
                return Json(JsonResultData.Failure("登录失败"));
            }
        }

        private void SetCacheHotel(string hid, ICurrentInfo currentinfo)
        {
            HotelInterfacePostModel hotelmodel = new HotelInterfacePostModel() { Hid = hid };
            //PostType.GetHotelSM 获取线上酒店的不用修改
            var hotelresult = JsonHelp.PostDataResult(hotelmodel, postType.GetHotelSM);
            if (hotelresult != null)
            {
                if (hotelresult.ErrorNo != "0")
                {
                    var hotelInfo = JsonHelp.Deserialize<HotelSMResultModel>(hotelresult.Msg);
                    // 根据返回的信息进行授权
                    var getOpenIdUrl = hotelInfo.GsWxOpenidUrl;
                    var comid = hotelInfo.GsWxComid;
                    //现在不用加判断，以后需要微信的时候加上
                    //if (string.IsNullOrEmpty(getOpenIdUrl) || string.IsNullOrEmpty(comid))
                    //{
                    //    return;
                    //}
                    currentinfo.GsWxCreatePayOrderUrl = hotelInfo.GsWxCreatePayOrderUrl;
                    currentinfo.GsWxPayOrderUrl = hotelInfo.GsWxPayOrderUrl;
                    currentinfo.GsWxTemplateMessageUrl = hotelInfo.GsWxTemplateMessageUrl;
                    if (hotelInfo.IsCs == true)
                    {
                        currentinfo.IsCs = "true";
                        currentinfo.NotifyUrl = hotelInfo.NotifyURL;
                    }
                    else
                    {
                        currentinfo.IsCs = "false";
                        currentinfo.NotifyUrl = "";

                    }
                    currentinfo.HotelId = hotelInfo.Hid;
                    
                    currentinfo.SaveValues();
                }
            }
        }

        public ActionResult Main()
        {
            return View();
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
        private ActionResult SetError(string msg = "")
        {
            ViewBag.Title = "错误";
            ViewBag.ErrorInfo = msg == "" ? "酒店信息设置出错" : msg;
            return View("ErrorInfo");
        }

    }
}