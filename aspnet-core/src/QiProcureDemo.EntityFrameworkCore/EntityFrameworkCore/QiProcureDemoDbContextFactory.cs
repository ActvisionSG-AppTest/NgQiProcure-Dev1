using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using QiProcureDemo.Configuration;
using QiProcureDemo.Web;

namespace QiProcureDemo.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class QiProcureDemoDbContextFactory : IDesignTimeDbContextFactory<QiProcureDemoDbContext>
    {
        public QiProcureDemoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<QiProcureDemoDbContext>();
            var configuration = AppConfigurations.Get(
                WebContentDirectoryFinder.CalculateContentRootFolder(),
                addUserSecrets: true
            );

            QiProcureDemoDbContextConfigurer.Configure(builder, configuration.GetConnectionString(QiProcureDemoConsts.ConnectionStringName));

            return new QiProcureDemoDbContext(builder.Options);
        }
    }
}