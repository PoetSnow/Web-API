using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Bill;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    /// <summary>
    /// 处理账单明细作法
    /// </summary>
    public class InBillDetailActionService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return PostType.InDetailAction;
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<InsBillDetailActionPostData>(requestData);
            if (postModel == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var requestList = _db.Database.SqlQuery<InsBillDetailActionResult>("exec up_pos_sm_BillDetailaction" +
                    " @hid=@hid,@module=@module,@refeid=@refeid,@tabid=@tabid,@keyid=@keyid,@usercode=@usercode," +
                    "@opertype=@opertype,@orderaction=@orderaction",
                                                       new SqlParameter("@hid", postModel.Hid),
                                                       new SqlParameter("@module", postModel.Module),
                                                       new SqlParameter("@refeid", postModel.RefeId),
                                                       new SqlParameter("@tabid", postModel.TabId),
                                                       new SqlParameter("@keyid", postModel.KeyId),
                                                       new SqlParameter("@usercode", postModel.UserCode),
                                                       new SqlParameter("@opertype", postModel.OperType),
                                                       new SqlParameter("@orderaction", postModel.OrderAction)).ToList();
                return "1" + serializer.Serialize(requestList);
            }
            catch (Exception ex)
            {
                AddErrorMsg(postModel.Hid, ex.Message.ToString());
                return "0" + ex.Message.ToString();
            }

        }
    }
}
