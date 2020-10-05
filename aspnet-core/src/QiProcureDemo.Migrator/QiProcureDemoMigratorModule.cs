using Abp.AspNetZeroCore;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using QiProcureDemo.Configuration;
using QiProcureDemo.EntityFrameworkCore;
using QiProcureDemo.Migrator.DependencyInjection;

namespace QiProcureDemo.Migrator
{
    [DependsOn(typeof(QiProcureDemoEntityFrameworkCoreModule))]
    public class QiProcureDemoMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public QiProcureDemoMigratorModule(QiProcureDemoEntityFrameworkCoreModule abpZeroTemplateEntityFrameworkCoreModule)
        {
            abpZeroTemplateEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(QiProcureDemoMigratorModule).GetAssembly().GetDirectoryPathOrNull(),
                addUserSecrets: true
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                QiProcureDemoConsts.ConnectionStringName
                );
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(QiProcureDemoMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}