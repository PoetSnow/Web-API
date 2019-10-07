using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Master;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos;
using System;
using System.Linq;
using System.Text;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Master
{
    public class GetHotelSMService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return PostType.GetHotelSM;
        }

        protected override string HandleData(string requestData)
        {
            //根据参数进行处理，然后返回值
            //执行具体的处理......
            try
            {
                var model = serializer.Deserialize<HotelInterfacePostModel>(requestData);
                var _db = GetDBCommonContext.GetCenterDB(model.Hid); //获取中央数据库连接
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }

                StringBuilder strSql = new StringBuilder("");
                strSql.Append(" select hotelCode,hotelName,hid,isCs,notifyURL,status")
                      .Append(" ,GsWxComid,GsWxOpenidUrl,GsWxTemplateMessageUrl,GsWxCreatePayOrderUrl,GsWxPayOrderUrl")
                      .Append(" from posSmMappingHid where hotelCode='" + model.Hid + "'")
                      .Append(" and status=0");

                var itemList = _db.Database.SqlQuery<HotelSMResultModel>(strSql.ToString()).ToList().FirstOrDefault();
                if (itemList == null)
                {
                    throw new ApplicationException("未找到酒店信息，请联系系统管理员");
                }


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
