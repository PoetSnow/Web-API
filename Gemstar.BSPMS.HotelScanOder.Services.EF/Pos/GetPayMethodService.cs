using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayBill;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.PayMethod;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{

    /// <summary>
    /// 获取付款方式
    /// </summary>
    public class GetPayMethodService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {

            return PostType.GetPayMethodList;
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<GetPayMethodPostModel>(requestData);
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
                var itemList = _db.Database.SqlQuery<GetPayMethodResultModel>("exec up_pos_sm_PayModeList @hid=@hid,@module=@module,@refeid=@refeid,@usercode =@usercode ",
                                                       new SqlParameter("@hid", postModel.Hid),
                                                       new SqlParameter("@module", postModel.Module),
                                                       new SqlParameter("@refeid", postModel.RefeId),
                                                       new SqlParameter("@usercode", postModel.UserCode)
                                                   ).ToList();
           
               SetItemPayRemark(postModel.Hid, itemList);

                //成功 返回付款方式列表json
                return "1" + serializer.Serialize(itemList);
            }
            catch (Exception ex)
            {
                AddErrorMsg(postModel.Hid, ex.Message.ToString());
                return "0" + ex.Message.ToString();
            }

        }

        private void SetItemPayRemark(string hid, List<GetPayMethodResultModel> list)
        {

            if (!string.IsNullOrEmpty(hid)) //云产品 取中央数据库的信息
            {
                var _centerDb = GetDBCommonContext.GetCenterDB(hid);
                var funcodeList = _centerDb.Database.SqlQuery<PayRemak>("select FuncCode,interfaceKey from HotelFunctions where hid='" + hid + "' and isValid=1").ToList();
                foreach (var item in list)
                {
                    if (item.Code == "house")   //房账
                    {
                        var s = funcodeList.Where(w => w.FuncCode == "FuncRoomFolio").FirstOrDefault();
                        if (s!=null)
                        {
                            item.PayReamk = s.FuncCode + "," + s.interfaceKey;
                        }
                       
                    }
                    if (item.Code == "mbrCardAndLargess" || item.Code == "mbrLargess" || item.Code == "mbrCard")   //会员
                    {
                        var s = funcodeList.Where(w => w.FuncCode == "FuncMember").FirstOrDefault();
                        if (s!=null)
                        {
                            item.PayReamk = s.FuncCode + "," + s.interfaceKey;
                        }
                        
                    }
                }
            }
        }
    }
}
