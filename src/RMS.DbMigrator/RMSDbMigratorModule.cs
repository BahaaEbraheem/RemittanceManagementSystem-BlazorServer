using RMS.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace RMS.DbMigrator
{

    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(RMSEntityFrameworkCoreModule),
        typeof(RMSApplicationContractsModule)
        )]
    public class RMSDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}