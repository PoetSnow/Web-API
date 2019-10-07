using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Item;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    /// <summary>
    /// 获取消费项目以及分类
    /// </summary>
    public class GetItelListService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return PostType.GetItemList;
            // return "04";
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<ItemPostModel>(requestData);
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
                var itemList = _db.Database.SqlQuery<ItemResultModel>("exec up_pos_sm_ItemList @hid=@hid,@module=@module,@posrefeid=@posrefeid,@usercode =@usercode ",
                                                       new SqlParameter("@hid", postModel.Hid),
                                                       new SqlParameter("@module", postModel.Module),
                                                       new SqlParameter("@posrefeid", postModel.Posrefeid),
                                                       new SqlParameter("@usercode", postModel.Usercode)
                                                   ).ToList().Where(w => w.Itemsubid != null);
                // var info = _db.Database.SqlQuery<LoginInfo>("exec");
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
