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
    public class BillCmpService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {           
            return postType.BillCmp;
        }

   
        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<BillCmpPostModel>(requestData);
            if (postModel==null) throw new ApplicationException("接口参数错误");

            //根据参数进行处理，然后返回值
            //执行具体的处理......
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var itemList = _db.Database.SqlQuery<BillCmpResultModel>("exec up_pos_sm_BillCmp @hid=@hid,@module=@module,@refeid=@refeid,@tabid=@tabid,@keyid=@keyid,@usercode=@usercode,@isSave=@isSave,@billId=@billId ",
                                                       new SqlParameter("@hid", postModel.Hid),
                                                       new SqlParameter("@module", postModel.Module),
                                                       new SqlParameter("@refeid", postModel.RefeId),
                                                       new SqlParameter("@tabid", postModel.TabId),
                                                       new SqlParameter("@keyid", postModel.KeyId),
                                                       new SqlParameter("@usercode", postModel.UserCode),
                                                       new SqlParameter("@isSave", postModel.IsSave),
                                                       new SqlParameter("@billId", postModel.BillId)
                                                   ).ToList();
                //查询出用户信息，并且返回json
                return "1" + serializer.Serialize(itemList);
            }
            catch (Exception ex)
            {
                AddErrorMsg(postModel.Hid, ex.Message.ToString());
                return "0" + ex.Message.ToString();
            }

        }
    }
}
