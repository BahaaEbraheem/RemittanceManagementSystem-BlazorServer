using RMS.Currencies.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace RMS.Currencies
{
    public interface ICurrencyAppService :
         ICrudAppService< //Defines CRUD methods
             CurrencyDto, //Used to show currencies
             Guid, //Primary key of the currency entity
             CurrencyPagedAndSortedResultRequestDto, //Used for paging/sorting
             CreateUpdateCurrencyDto> //Used to create/update a currency
    {

    }
    //public interface ICurrencyAppService : IApplicationService
    //{
    //    Task<CurrencyDto> GetAsync(Guid id);

    //    Task<PagedResultDto<CurrencyDto>> GetListAsync(GetCurrencyListDto input);

    //    Task<CurrencyDto> CreateAsync(CreateCurrencyDto input);

    //    Task UpdateAsync(Guid id, UpdateCurrencyDto input);

    //    Task DeleteAsync(Guid id);
    //}

}
