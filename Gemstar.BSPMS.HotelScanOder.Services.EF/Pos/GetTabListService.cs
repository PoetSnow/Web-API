using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Tab;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    /// <summary>
    /// 获取餐台信息
    /// </summary>
    public class GetTabListService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return PostType.GetTabList;
            //return "03";
        }

        protected override string HandleData(string requestData)
        {

            var posData = serializer.Deserialize<TabPostModel>(requestData);
            if (posData == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(posData.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var list = _db.Database.SqlQuery<TabResultModel>("exec up_pos_sm_TabList @hid=@hid,@posrefeid=@posrefeid,@module=@module",
                                                   new SqlParameter("@hid", posData.Hid),
                                                   new SqlParameter("@posrefeid", posData.PosRefeid),
                                                   new SqlParameter("@module", posData.Module)).ToList();

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
