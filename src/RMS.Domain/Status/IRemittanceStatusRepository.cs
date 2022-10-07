using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RMS.Enums.Enums;
using Volo.Abp.Domain.Repositories;

namespace RMS.Status
{
   public interface  IRemittanceStatusRepository:IRepository<RemittanceStatus, Guid>
    {
        Task<RemittanceStatus> FindByRemitanceIdAndStateAsync(Guid remitanceId);
        //Task<List<Remittance>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null);
        //Task<bool> IsApprovedRemittanceAsync(Remittance remittance);
    }
}
