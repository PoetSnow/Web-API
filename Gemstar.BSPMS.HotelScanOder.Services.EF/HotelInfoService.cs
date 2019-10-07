using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Gemstar.BSPMS.HotelScanOrder.Common.EntityProcedures;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF
{
    public class HotelInfoService 
    {
        private DbCommonContext _db;
        public HotelInfoService(DbCommonContext db) 
        {
            _db = db;
        }
      

      

        public UpQueryHotelInfoByIdResult GetHotelInfo(string hid)
        {
            return _db.Database.SqlQuery<UpQueryHotelInfoByIdResult>("exec up_queryHotelInfoById @hid=@hid", new SqlParameter("@hid", hid)).SingleOrDefault();
        }

    }
}
