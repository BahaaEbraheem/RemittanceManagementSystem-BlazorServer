using RMS.Currencies;
using RMS.Currencies.Dtos;
using RMS.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using RMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Volo.Abp.ObjectMapping;
using Volo.Abp.DependencyInjection;
using static System.Reflection.Metadata.BlobBuilder;
using Volo.Abp.SettingManagement;

namespace RMS.Currencies
{
    public class CurrencyAppService :
        CrudAppService<
               Currency, //The Currency entity
            CurrencyDto, //Used to show Currencies
            Guid, //Primary key of the Currency entity
            CurrencyPagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateCurrencyDto>, //Used to create/update a Currency
        ICurrencyAppService //implement the ICurrencyAppService
    {
        private readonly ICurrencyRepository _currencyRepository;
        public CurrencyAppService(IRepository<Currency, Guid> repository,
            ICurrencyRepository currencyRepository)
    : base(repository)
        {
            _currencyRepository = currencyRepository;
        }



        public override async Task<PagedResultDto<CurrencyDto>> GetListAsync(CurrencyPagedAndSortedResultRequestDto input)
        {
            var filter = ObjectMapper.Map<CurrencyPagedAndSortedResultRequestDto, Currency>(input);
            var sorting = (string.IsNullOrEmpty(input.Sorting) ? "Name DESC" : input.Sorting).Replace("ShortName", "Name");
            var currencies = await _currencyRepository.GetListAsync(input.SkipCount, input.MaxResultCount, sorting, filter);
            var totalCount = await _currencyRepository.GetTotalCountAsync(filter);
            return new PagedResultDto<CurrencyDto>(totalCount, ObjectMapper.Map<List<Currency>, List<CurrencyDto>>(currencies));
        }

    }
}


//public class CurrencyAppService: RMSAppService,ICurrencyAppService, ITransientDependency //implement the ICurrencyAppService
// {
//     private readonly ICurrencyRepository _currencyRepository;
//     private readonly CurrencyManager _currencyManager;
//     private readonly IObjectMapper<Currency> _objectMapper;

//     public CurrencyAppService(
//         ICurrencyRepository currencyRepository,
//         CurrencyManager currencyManager,
//         IObjectMapper<Currency> objectMapper)
//     {
//         _currencyRepository = currencyRepository;
//         _currencyManager = currencyManager;
//         _objectMapper = objectMapper;
//     }

//     public async Task<CurrencyDto> GetAsync(Guid id)
//     {
//         var Currency = await _currencyRepository.GetAsync(id);
//         return _objectMapper.Map<Currency, CurrencyDto>(Currency);
//     }
//     public async Task<PagedResultDto<CurrencyDto>> GetListAsync(GetCurrencyListDto input)
//     {
//         if (input.Sorting.IsNullOrWhiteSpace())
//         {
//             input.Sorting = nameof(Currency.Name);
//         }

//         var Currencys = await _currencyRepository.GetListAsync(
//             input.SkipCount,
//             input.MaxResultCount,
//             input.Sorting,
//             input.Filter
//         );

//         var totalCount = input.Filter == null
//             ? await _currencyRepository.CountAsync()
//             : await _currencyRepository.CountAsync(
//                 Currency => Currency.Name.Contains(input.Filter));

//         return new PagedResultDto<CurrencyDto>(
//             totalCount,
//             _objectMapper.Map<List<Currency>, List<CurrencyDto>>(Currencys)
//         );
//     }
//     //[Currencyize(BookStorePermissions.Currencys.Create)]
//     public async Task<CurrencyDto> CreateAsync(CreateCurrencyDto input)
//     {
//         var Currency = await _currencyManager.CreateAsync(
//             input.Name,
//             input.Symbol,
//             input.Code
//         );

//         await _currencyRepository.InsertAsync(Currency);
//         return ObjectMapper.Map<Currency, CurrencyDto>(Currency);
//     }

//     //[Currencyize(BookStorePermissions.Currencys.Edit)]
//     public async Task UpdateAsync(Guid id, UpdateCurrencyDto input)
//     {
//         var Currency = await _currencyRepository.GetAsync(id);

//         if (Currency.Name != input.Name)
//         {
//             await _currencyManager.ChangeNameAsync(Currency, input.Name,input.Symbol);
//         }

//         Currency.Symbol = input.Symbol;
//         Currency.Code = input.Code;

//         await _currencyRepository.UpdateAsync(Currency);
//     }
//     //[Currencyize(BookStorePermissions.Currencys.Delete)]
//     public async Task DeleteAsync(Guid id)
//     {
//         await _currencyRepository.DeleteAsync(id);
//     }

// }

