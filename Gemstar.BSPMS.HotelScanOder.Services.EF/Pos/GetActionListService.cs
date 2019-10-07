using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Action;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    /// <summary>
    /// 获取作法列表服务
    /// </summary>
    public class GetActionListService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {

            return PostType.GetActionList;
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<ActionPostModel>(requestData);
            if (postModel == null) throw new ApplicationException("接口参数错误");
            //根据参数进行处理，然后返回值
            //执行具体的处理......
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var itemList = _db.Database.SqlQuery<ActionResultModel>("exec up_pos_sm_ItemAction @hid=@hid,@module=@module,@itemid=@itemid,@usercode =@usercode ",
                                                       new SqlParameter("@hid", postModel.Hid),
                                                       new SqlParameter("@module", postModel.Module),
                                                       new SqlParameter("@itemid", postModel.ItemId),
                                                       new SqlParameter("@usercode", postModel.UserCode)
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
