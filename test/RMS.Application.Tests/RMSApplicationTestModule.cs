using Volo.Abp.Modularity;

namespace RMS
{

    [DependsOn(
        typeof(RMSApplicationModule),
        typeof(RMSDomainTestModule)
        )]
    public class RMSApplicationTestModule : AbpModule
    {

    }
}