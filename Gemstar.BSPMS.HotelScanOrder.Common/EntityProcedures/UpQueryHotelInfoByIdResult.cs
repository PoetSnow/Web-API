using System;
using Gemstar.BSPMS.HotelScanOrder.Common.Common;

namespace Gemstar.BSPMS.HotelScanOrder.Common.EntityProcedures
{
    public class UpQueryHotelInfoByIdResult
    {
        public string Grpid { get; set; }
        public string Hid { get; set; }
        public string Name { get; set; }
        public string ServerAddress { get; set; }
        public string DbServer { get; set; }
        public string DbName { get; set; }
        public string Logid { get; set; }
        public string LogPwd { get; set; }
        public string ReadonlyDbServer { get; set; }
        public string ReadonlyDbName { get; set; }
        public string ReadonlyLogId { get; set; }
        public string ReadonlyLogPwd { get; set; }
        public EntityStatus Status { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string versionId { get; set; }
        public string customerStatus { get; set; }
    }
}
