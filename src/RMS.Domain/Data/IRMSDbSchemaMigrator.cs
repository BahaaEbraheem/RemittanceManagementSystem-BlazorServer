using System.Threading.Tasks;

namespace RMS.Data
{

    public interface IRMSDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}