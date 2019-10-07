using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Logs;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    public class OpeLogService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {

            return postType.OperLog;
        }

        protected override string HandleData(string requestData)
        {
            var posData = serializer.Deserialize<OperLogPostModel>(requestData);
            if (posData == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                if (string.IsNullOrEmpty(posData.Hid))
                {
                    var _db = GetDBCommonContext.GetPmsDB(posData.Hid); //获取数据库
                    if (_db == null)
                    {
                        throw new ApplicationException("连接失败，请与软件供应商联系");
                    }
                    _db.Database.ExecuteSqlCommand("exec up_pos_sm_OperLog @hid=@hid,@opertype=@opertype,@opercontent=@opercontent",
                                                  new SqlParameter("@hid", posData.Hid),
                                                  new SqlParameter("@opertype", posData.OperType),
                                                  new SqlParameter("@opercontent", posData.OperContent));
                }
                else
                {
                    var _db = GetDBCommonContext.GetCenterDB(posData.Hid);  //线上使用运营库

                    _db.Database.ExecuteSqlCommand("exec up_pos_sm_OperLog @hid=@hid,@opertype=@opertype,@opercontent=@opercontent",
                                                  new SqlParameter("@hid", posData.Hid),
                                                  new SqlParameter("@opertype", posData.OperType),
                                                  new SqlParameter("@opercontent", posData.OperContent));
                }
                return "1";
            }
            catch (Exception ex)
            {

                return "0" + ex.Message.ToString();
            }

        }

        
    }
}
