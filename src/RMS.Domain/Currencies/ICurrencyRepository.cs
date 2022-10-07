using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace RMS.Currencies
{
   public interface ICurrencyRepository : IRepository<Currency,Guid>
    {
        Task<Currency> FindByNameAndSymbolAsync(string name, string symbol);
       // Task<List<Currency>> GetListAsync(
       //    int skipCount,
       //    int maxResultCount,
       //    string sorting,
       //    string filter = null
       //);
        Task<List<Currency>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            Currency filter
        );
        Task<int> GetTotalCountAsync(Currency filter);
    }
}
