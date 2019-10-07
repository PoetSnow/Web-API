using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Login;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    /// <summary>
    /// 获取酒店信息
    /// </summary>
    public class GetHotelInfoService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return postType.GetHotelInfo;
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<BasePostModel>(requestData);
            if (postModel == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            //根据参数进行处理，然后返回值
            //执行具体的处理......
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库连接
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系！");
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
