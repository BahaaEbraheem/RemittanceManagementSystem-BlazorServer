using Microsoft.EntityFrameworkCore;
using RMS.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using RMS.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;

namespace RMS.Currencies
{
    public class EfCoreCurrencyRepository: EfCoreRepository<RMSDbContext, Currency, Guid>,ICurrencyRepository
    {
        public EfCoreCurrencyRepository(
            IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Currency> FindByNameAndSymbolAsync(string name, string symbol)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(Currency => Currency.Name == name || Currency.Symbol == symbol);
        }

        //public async Task<List<Currency>> GetListAsync(int skipCount,int maxResultCount,string sorting,string filter = null)
        //{
        //    var dbSet = await GetDbSetAsync();
        //    return await dbSet
        //        .WhereIf(
        //            !filter.IsNullOrWhiteSpace(),
        //            Currency => Currency.Name.Contains(filter)
        //         )
        //        .OrderBy(sorting)
        //        .Skip(skipCount)
        //        .Take(maxResultCount)
        //        .ToListAsync();
        //}

        public async Task<List<Currency>> GetListAsync(int skipCount, int maxResultCount, string sorting, Currency filter)
        {
            var dbSet = await GetDbSetAsync();
            var currencies = await dbSet
                //.WhereIf(!filter.Id.Equals(null), 
                //x => x.Id.ToString().Contains(filter.Id.ToString()))
                .WhereIf(!filter.Name.IsNullOrWhiteSpace(),
                x => x.Name.Contains(filter.Name))
                .WhereIf(!filter.Symbol.IsNullOrWhiteSpace(),
                x => x.Symbol.Contains(filter.Symbol))
                .WhereIf(!filter.Code.IsNullOrWhiteSpace(),
                x => x.Code.Contains(filter.Code)).OrderBy(sorting)
                .Skip(skipCount).Take(maxResultCount).ToListAsync();
            return currencies;
        }

        public async Task<int> GetTotalCountAsync(Currency filter)
        {
            var dbSet = await GetDbSetAsync();
            var currencies = await dbSet
                //.WhereIf(!filter.Id.Equals(null),
                //x => x.Id.ToString().Contains(filter.Id.ToString()))
                .WhereIf(!filter.Name.IsNullOrWhiteSpace(),
                x => x.Name.Contains(filter.Name))
                .WhereIf(!filter.Symbol.IsNullOrWhiteSpace(),
                x => x.Symbol.Contains(filter.Symbol))
                .WhereIf(!filter.Code.IsNullOrWhiteSpace()
                , x => x.Code.Contains(filter.Code))
                .ToListAsync();
            return currencies.Count;
        }
    }
}

