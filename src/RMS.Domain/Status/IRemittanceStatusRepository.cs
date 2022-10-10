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
        Task<RemittanceStatus> FindLastStateToThisRemitanceAsync(Guid remitanceId);

    }
}
