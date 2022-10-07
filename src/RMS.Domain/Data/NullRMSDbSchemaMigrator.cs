using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace RMS.Data
{

    /* This is used if database provider does't define
     * IRMSDbSchemaMigrator implementation.
     */
    public class NullRMSDbSchemaMigrator : IRMSDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}