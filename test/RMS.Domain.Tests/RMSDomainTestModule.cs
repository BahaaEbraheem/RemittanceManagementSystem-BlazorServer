using RMS.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace RMS
{

    [DependsOn(
        typeof(RMSEntityFrameworkCoreTestModule)
        )]
    public class RMSDomainTestModule : AbpModule
    {

    }
}