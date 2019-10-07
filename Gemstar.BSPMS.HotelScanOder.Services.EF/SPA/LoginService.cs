


using Gemstar.BSPMS.HotelScanOrder.Services.EF;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.SPA.Store;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.SPA
{
    public class LoginService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return postType.SPA_Login;
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<LoginPostModel>(requestData);
            if (postModel == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var reslist = _db.Database.SqlQuery<LoginResultModel>("exec up_pos_sm_login @hid=@hid,@usercode=@usercode,@pwd=@pwd,@pdano=@pdano",
                                                       new SqlParameter("@hid", postModel.hid),
                                                       new SqlParameter("@usercode", postModel.usercode),
                                                       new SqlParameter("@pwd", postModel.pwd),
                                                       new SqlParameter("@pdano", postModel.pdano)
                                                   ).ToList();
                //查询出用户信息，并且返回json
                return "1" + serializer.Serialize(reslist);
            }
            catch (Exception ex)
            {
                return "0" + ex.Message.ToString();
            }
        }
    }
}
