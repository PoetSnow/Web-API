using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Login;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    /// <summary>
    /// 登录验证
    /// </summary>
    public class LoginService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {

            return postType.Login;
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<LoginPostModel>(requestData);
            if (postModel == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var result = _db.Database.SqlQuery<LoginCheckResult>("exec up_pos_sm_login @hid=@hid,@usercode=@usercode,@pwd=@pwd",
                                                     new SqlParameter("@hid", postModel.Hid),
                                                     new SqlParameter("@usercode", postModel.Username),
                                                     new SqlParameter("@pwd", postModel.Password)).FirstOrDefault();
                if (result.rc == 0)
                {
                    return "0" + result.msg;
                }
                var info = _db.Database.SqlQuery<LoginResultModel>("exec up_pos_sm_resortlist @hid=@hid", new SqlParameter("@hid", postModel.Hid)).ToList();
                //查询出用户信息，并且返回json
                return "1" + serializer.Serialize(info);
            }
            catch (Exception ex)
            {
                AddErrorMsg(postModel.Hid, ex.Message.ToString());
                return "0" + ex.Message.ToString();
            }

        }
    }
}
