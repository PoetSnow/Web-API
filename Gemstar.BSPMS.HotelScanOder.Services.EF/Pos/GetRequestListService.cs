using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Request;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    /// <summary>
    /// 获取要求服务
    /// </summary>
    public class GetRequestListService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return PostType.GetRequestList;
            //return "05";
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<RequestPostModel>(requestData);
            if (postModel == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var requestList = _db.Database.SqlQuery<RequestResultModel>("exec up_pos_sm_Quest @hid=@hid,@refeid=@refeid,@module=@module",
                                                       new SqlParameter("@hid", postModel.Hid),
                                                       new SqlParameter("@refeid", postModel.Refeid),
                                                       new SqlParameter("@module", postModel.Module)
                                                   ).ToList();
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
