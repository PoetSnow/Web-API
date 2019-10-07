﻿using Gemstar.BSPMS.HotelScanOrder.Common.Common;
using Gemstar.BSPMS.HotelScanOrder.Common.Pos.Report;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF.Pos
{
    public class GetReportInBalanceListService : ReciveDataHandlerBase
    {
        public override string GetHandleDataType()
        {
            return PostType.GetReport_InBalanceList;
        }

        protected override string HandleData(string requestData)
        {
            var postModel = serializer.Deserialize<ReportInBalanceListPostModel>(requestData);
            if (postModel == null) throw new ApplicationException("接口参数错误,请联系软件供应商！");
            try
            {
                var _db = GetDBCommonContext.GetPmsDB(postModel.Hid); //获取数据库
                if (_db == null)
                {
                    throw new ApplicationException("连接失败，请与软件供应商联系");
                }
                var data = _db.Database.SqlQuery<ReportInBalanceListResultModel>("exec up_pos_sm_RptInBalance @hid=@hid,@module=@module,@as_findtype=@as_findtype,@as_startDate=@as_startDate,@as_endDate=@as_endDate,@refeid=@refeid",
                                                   new SqlParameter("@hid", postModel.Hid),
                                                   new SqlParameter("@module", postModel.Module),
                                                   new SqlParameter("@as_findtype", postModel.As_FindType),
                                                   new SqlParameter("@as_startDate", postModel.As_StartDate),
                                                   new SqlParameter("@as_endDate", postModel.As_EndDate),
                                                   new SqlParameter("@refeid", postModel.Refeid)).ToList();
                //查询出收入平衡表信息，并且返回json
                return "1" + serializer.Serialize(data);
            }
            catch (Exception ex)
            {
                AddErrorMsg(postModel.Hid, ex.Message.ToString());
                return "0" + ex.Message.ToString();
            }
        }
    }
}
