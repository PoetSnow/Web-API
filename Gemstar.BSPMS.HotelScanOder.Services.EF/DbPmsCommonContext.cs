using System.Data.Entity;

namespace Gemstar.BSPMS.HotelScanOrder.Services.EF
{
    /// <summary>
    /// 业务库数据连接
    /// </summary>
    public class DbPmsCommonContext : DbContext
    {
        static DbPmsCommonContext()
        {
            Database.SetInitializer<DbPmsCommonContext>(null);
        }

        public DbPmsCommonContext(string connStr) : base(connStr)
        {
            //设置一些默认值，以优化一些速度
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;//在需要修改和删除时，需要手动的打开此开关
            Configuration.LazyLoadingEnabled = false;
        }

        

    }
}
