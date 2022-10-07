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

namespace RMS.Status
{
    public class EfCoreRemittanceStatusRepository : EfCoreRepository<RMSDbContext, RemittanceStatus, Guid>, IRemittanceStatusRepository
    {
        public EfCoreRemittanceStatusRepository(
            IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public  async Task<RemittanceStatus> FindByRemitanceIdAndStateAsync(Guid remitanceId)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(remittanceStatus => remittanceStatus.RemittanceId == remitanceId );
        }


    }
}