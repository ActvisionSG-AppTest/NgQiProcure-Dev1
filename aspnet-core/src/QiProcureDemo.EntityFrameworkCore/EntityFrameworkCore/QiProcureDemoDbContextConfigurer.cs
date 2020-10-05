using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace QiProcureDemo.EntityFrameworkCore
{
    public static class QiProcureDemoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<QiProcureDemoDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<QiProcureDemoDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}