using RMS.Remittances.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace RMS.Remittances
{
   public interface IRemittanceAppService : IApplicationService
    {
        Task<RemittanceDto> GetAsync(Guid id);

        Task<PagedResultDto<RemittanceDto>> GetListAsync(GetRemittanceListDto input);

        Task<RemittanceDto> CreateAsync(CreateRemittanceDto input);

        Task UpdateAsync(Guid id, UpdateRemittanceDto input);

        Task DeleteAsync(Guid id);

        Task<ListResultDto<CurrencyLookupDto>> GetCurrencyLookupAsync();
        Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync();
        Task<ListResultDto<UserLookupDto>> GetUserLookupAsync();
        Task SetReady(RemittanceDto input);

    }
}
