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

namespace RMS.Remittances
{
    public class EfCoreRemittanceRepository : EfCoreRepository<RMSDbContext, Remittance, Guid>, IRemittanceRepository
    {
        public EfCoreRemittanceRepository(
            IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Remittance> FindBySerialNumAsync(string serialNum)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(Remittance => Remittance.SerialNumber == serialNum);
        }

        public async Task<bool> IsApprovedRemittanceAsync(Remittance remittance)
        {
            var dbSet = await GetDbSetAsync();
           Remittance checkApprovedRemittance= await dbSet.FirstOrDefaultAsync(Remittance => Remittance.Id == remittance.Id);
            if (checkApprovedRemittance.ApprovedBy.Equals(null) && checkApprovedRemittance.ApprovedDate.Equals(null))
            {
                return true;
            }
            return false;
         
        }

        public async Task<List<Remittance>> GetListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            string filter = null)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    Remittance => Remittance.SerialNumber.Contains(filter)
                 )
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }

        public async Task<Remittance> FindRemittance_StillDraftAsync(double amount, string receiverName)
        {
            var dbSet = await GetDbSetAsync();

            return await dbSet.FirstOrDefaultAsync(Remittance => Remittance.Amount == amount&& 
            Remittance.Status.Any(a=>a.State== Remittance_Status.Draft) &&Remittance.ReceiverFullName== receiverName);
        }
    }
}