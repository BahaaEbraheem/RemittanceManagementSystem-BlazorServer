using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RMS.Enums.Enums;
using Volo.Abp.Domain.Repositories;
using RMS.Customers;

namespace RMS.Status
{
    public interface IRemittanceStatusRepository : IRepository<RemittanceStatus, Guid>
    {
        Task<RemittanceStatus> FindLastStateToThisRemitanceAsync(Guid remitanceId);
        Task<List<RemittanceStatus>> GetListAsync(
                                     int skipCount,
                                     int maxResultCount,
                                     string sorting,
                                     RemittanceStatus filter
                                                 );
        Task<int> GetTotalCountAsync(RemittanceStatus filter);

    }
}
