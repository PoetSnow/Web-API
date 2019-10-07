using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    public class PayBillService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {

            return postType.PayBill;
        }

        protected override string HandleData(string requestData)
        {
            var posData = serializer.Deserialize<PayBillPostModel>(requestData);
            if (posData == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(posData.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var result = _db.Database.SqlQuery<PayBillResultModel>("exec up_pos_sm_BillPay @hid=@hid,@module=@module,@billno=@billno,@refeid=@refeid,@tabid=@tabid,@keyid=@keyid,@usercode=@usercode,@paynames=@paynames,@operType=@operType",
                                                new SqlParameter("@hid", posData.Hid),
                                                new SqlParameter("@module", posData.Module),
                                                new SqlParameter("@billno", posData.Billno),
                                                new SqlParameter("@refeid", posData.RefeId),
                                                new SqlParameter("@tabid", posData.TabId),
                                                new SqlParameter("@keyid", posData.KeyId),
                                                new SqlParameter("@usercode", posData.UserCode),
                                                new SqlParameter("@paynames", posData.PayNames),
                                                new SqlParameter("@operType", posData.operType)).ToList();

                return "1" + serializer.Serialize(result);
            }
            catch (Exception ex)
            {
                AddErrorMsg(posData.Hid, ex.Message.ToString());
                return "0" + ex.Message.ToString();
            }

        }
    }
}
