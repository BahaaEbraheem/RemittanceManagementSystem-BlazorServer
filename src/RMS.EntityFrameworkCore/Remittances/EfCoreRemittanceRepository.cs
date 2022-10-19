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

        public async Task<List<Remittance>> GetListRemittancesStatusAsync(int skipCount, int maxResultCount, string sorting, Remittance filter)
        {
            var dbSet = await GetDbSetAsync();

            var remittances = await dbSet
                .WhereIf(!filter.ReceiverFullName.IsNullOrWhiteSpace(), x => x.ReceiverFullName.Contains(filter.ReceiverFullName))
                .WhereIf(!filter.Amount.Equals(0), x => x.Amount.ToString().Contains(filter.Amount.ToString()))
                .WhereIf(!filter.TotalAmount.Equals(0), x => x.TotalAmount.ToString().Contains(filter.TotalAmount.ToString()))
                .WhereIf(!filter.SerialNumber.IsNullOrWhiteSpace(), x => x.SerialNumber.Contains(filter.SerialNumber))
                .OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();
            return remittances;

        }

        public async Task<int> GetTotalCountAsync(Remittance filter)
        {
            var dbSet = await GetDbSetAsync();
            var remittances = await dbSet
             .WhereIf(!filter.ReceiverFullName.IsNullOrWhiteSpace(), x => x.ReceiverFullName.Contains(filter.ReceiverFullName))
                .WhereIf(!filter.Amount.Equals(0), x => x.Amount.ToString().Contains(filter.Amount.ToString()))
                .WhereIf(!filter.TotalAmount.Equals(0), x => x.TotalAmount.ToString().Contains(filter.TotalAmount.ToString()))
                .WhereIf(!filter.SerialNumber.IsNullOrWhiteSpace(), x => x.SerialNumber.Contains(filter.SerialNumber))
                .ToListAsync();
            return remittances.Count;
        }

        public async Task<Remittance> FindRemittance_StillDraftAsync(double amount, string receiverName)
        {
            var dbSet = await GetDbSetAsync();

            return await dbSet.FirstOrDefaultAsync(Remittance => Remittance.Amount == amount&& 
            Remittance.Status.Any(a=>a.State== Remittance_Status.Draft) &&Remittance.ReceiverFullName== receiverName);
        }
    }
}