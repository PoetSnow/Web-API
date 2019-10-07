
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.SPA;
using Gemstar.BSPMS.HotelScanOrder.Services.EF;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.SPA
{
    public class StoreOrderService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return postType.SPA_StoreOrder;
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<StoreOrderPostModel>(requestData);
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var itemList = _db.Database.SqlQuery<StoreOrderResultModel>("exec up_pos_SpecialtyStoreOrder @price=@price,@computer=@computer,@createor=@createor,@handcode=@handcode",
                                                       new SqlParameter("@price", postModel.price),
                                                       new SqlParameter("@computer", postModel.computer),
                                                       new SqlParameter("@createor", postModel.createor),
                                                       new SqlParameter("@handcode", postModel.handcode)
                                                   ).ToList();
                //查询出用户信息，并且返回json
                return "1" + serializer.Serialize(itemList);
            }
            catch (Exception ex)
            {
                return "0" + ex.Message.ToString();
            }
        }

    }
}
