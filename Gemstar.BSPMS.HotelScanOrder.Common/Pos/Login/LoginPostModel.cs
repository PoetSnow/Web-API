using System.ComponentModel.DataAnnotations;

namespace Gemstar.BSPMS.HotelScanOrder.Common.Pos.Login
{
    public class LoginPostModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>

        [Display(Name = "密码")]
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        /// <summary>
        /// 酒店代码
        /// </summary>
        [Display(Name = "酒店代码")]
        [Required(ErrorMessage = "请输入酒店代码")]
        public string Hid { get; set; }
    }
}
