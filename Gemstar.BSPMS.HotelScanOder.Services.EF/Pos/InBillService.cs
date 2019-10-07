using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    public class InBillService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return postType.InBill;
        }
        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<InsBillPostDataModel>(requestData);
            if (postModel == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }

                var list = _db.Database.SqlQuery<InsBillResultModel>("exec up_pos_sm_opentab @hid=@hid,@module=@module,@refeid=@refeid,@tabid=@tabid,@keyid=@keyid,@usercode=@usercode,@opertype=@opertype,@opendata=@opendata",
                                                   new SqlParameter("@hid", postModel.Hid),
                                                   new SqlParameter("@module", postModel.Module),
                                                   new SqlParameter("@refeid", postModel.RefeId),
                                                   new SqlParameter("@tabid", postModel.TabId),
                                                   new SqlParameter("@keyid ", postModel.KeyId),
                                                   new SqlParameter("@usercode  ", postModel.UserCode),
                                                   new SqlParameter("@opertype ", postModel.OperType),
                                                   new SqlParameter("@opendata ", postModel.OpenData)).FirstOrDefault();

                return "1" + serializer.Serialize(list);
            }
            catch (Exception ex)
            {
                AddErrorMsg(postModel.Hid, ex.Message.ToString());
                return "0" + ex.Message.ToString();
            }

        }
    }
}
