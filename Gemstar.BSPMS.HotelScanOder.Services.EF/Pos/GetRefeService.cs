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
    /// 获取营业点列表
    /// </summary>
    /// <returns></returns>
    public class GetRefeService : ReciveDataHandlerBase
    {

        public override string GetHandleDataType()
        {
            return postType.GetRefeList;
            // return "02";
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<RefePostModel>(requestData);
            if (postModel == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            //根据参数进行处理，然后返回值
            //执行具体的处理......
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var refeList = _db.Database.SqlQuery<RefeResultModel>("exec up_pos_sm_refebyuser @hid=@hid,@usercode=@usercode,@module=@module",
                                                       new SqlParameter("@hid", postModel.Hid),
                                                       new SqlParameter("@usercode", postModel.UserCode),
                                                       new SqlParameter("@module", postModel.Module)
                                                   ).ToList();
                return "1" + serializer.Serialize(refeList);
            }
            catch (Exception ex)
            {
                AddErrorMsg(postModel.Hid, ex.Message.ToString());
                return "0" + ex.Message.ToString();
            }
                                                                                                                                                                                                                              
        }
    }
}
