using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    /// <summary>
    /// 获取当前收银点和营业点上的营业日、班次和市别
    /// </summary>
    public class GetSysInfoService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return postType.GetSysInfo;
        }

        protected override string HandleData(string requestData)
        {
            var posData = serializer.Deserialize<SysInfoPostModel>(requestData);
            if (posData == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(posData.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var list = _db.Database.SqlQuery<SysInfoResultModel>("exec up_pos_sm_SysInfo @hid=@hid,@module=@module,@posid=@posid,@refeid=@refeid,@usercode=@usercode",
                                                   new SqlParameter("@hid", posData.Hid),
                                                   new SqlParameter("@module", posData.Module),
                                                   new SqlParameter("@posid", posData.PosId),
                                                   new SqlParameter("@refeid", posData.RefeId),
                                                   new SqlParameter("@usercode", posData.UserCode)).ToList();

                return "1" + serializer.Serialize(list);
            }
            catch (Exception ex)
            {
                AddErrorMsg(posData.Hid, ex.Message.ToString());
                return "0" + ex.Message.ToString();
            }

        }
    }
}
