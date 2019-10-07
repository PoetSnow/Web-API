namespace Gemstar.BSPMS.HotelScanOrder.Common.SPA.Store
{
    public class LoginResultModel
    {
        //rc 是否登陆成功  int	0：失败
        //1：成功
        //msg 登陆提示信息 varchar(1000)   0失败：”用户名或密码不正确”
        //1成功：””

        /// <summary>
        /// 是否登陆成功  int 	0：失败   1：成功
        /// </summary>
        public int rc { get; set; }

        /// <summary>
        /// 登陆提示信息  0失败：”用户名或密码不正确” 1成功
        /// </summary>
        public string msg { get; set; }



    }
}
