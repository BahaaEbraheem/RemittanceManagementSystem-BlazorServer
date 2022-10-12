using Microsoft.EntityFrameworkCore;
using RMS.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using static RMS.Enums.Enums;
using RMS.Customers;
using RMS.Remittances;

namespace RMS.Status
{
    public class EfCoreRemittanceStatusRepository : EfCoreRepository<RMSDbContext, RemittanceStatus, Guid>, IRemittanceStatusRepository
    {
        private readonly IRemittanceRepository _remittanceRepository;

        public EfCoreRemittanceStatusRepository(
            IRemittanceRepository remittanceRepository,
            IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
            _remittanceRepository = remittanceRepository;
        }

        public  async Task<RemittanceStatus> FindLastStateToThisRemitanceAsync(Guid remitanceId)
        {
            var dbSet = await GetDbSetAsync();

            return await dbSet.Where(remittanceStatus => remittanceStatus.RemittanceId == remitanceId)
                .OrderByDescending(a => a.CreationTime).FirstOrDefaultAsync();

        }
        public async Task<List<RemittanceStatus>> GetListAsync(int skipCount, int maxResultCount, string sorting, RemittanceStatus filter)
        {
            var remittance = _remittanceRepository.GetAsync(filter.RemittanceId);

            if (filter.RemittanceId.Equals(null) || remittance==null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            var dbSet = await GetDbSetAsync();
            var customers = await dbSet
       
                .WhereIf(!filter.State.GetType().Name.IsNullOrEmpty(),
                x => x.State.GetType().Name.Contains(filter.State.GetType().Name))       
                .OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();
            return customers;

        }

        public async Task<int> GetTotalCountAsync(RemittanceStatus filter)
        {
            var dbSet = await GetDbSetAsync();
            var customers = await dbSet

                .WhereIf(!filter.State.GetType().Name.IsNullOrEmpty(),
                x => x.State.GetType().Name.Contains(filter.State.GetType().Name))

                .ToListAsync();
            return customers.Count;
        }

    }
}