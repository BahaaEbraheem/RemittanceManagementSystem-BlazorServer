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
using AutoMapper.Internal.Mappers;
using RMS.Customers;
using RMS.Customers.Dtos;
using RMS.Status.Dtos;
using RMS.Remittances;

namespace RMS.Status
{
    public class RemittanceStatusAppService :
        CrudAppService<
               RemittanceStatus, //The customer entity
            RemittanceStatusDto, //Used to show customers
            Guid, //Primary key of the customer entity
            RemittanceStatusPagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateRemittanceStatusDto>, //Used to create/update a customer
        IRemittanceStatusAppService //implement the IcustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRemittanceRepository _remittanceRepository;
        private readonly IRemittanceStatusRepository _remittanceStatusRepository;
        public RemittanceStatusAppService(IRepository<RemittanceStatus, Guid> repository,
            ICustomerRepository customerRepository,
            IRemittanceRepository remittanceRepository,
            IRemittanceStatusRepository remittanceStatusRepository)
            : base(repository)
        {
            _customerRepository = customerRepository;
            _remittanceRepository = remittanceRepository;
            _remittanceStatusRepository = remittanceStatusRepository;
        }
        //public async Task<PagedResultDto<RemittanceStatusDto>> GetListAsync(RemittanceStatusPagedAndSortedResultRequestDto input)
        //{
        //    var filter = ObjectMapper.Map<RemittanceStatusPagedAndSortedResultRequestDto, RemittanceStatus>(input);
        //    var sorting = (string.IsNullOrEmpty(input.Sorting) ? "State DESC" : input.Sorting).Replace("ShortName", "State");
        //    var remittancesStatus = await _remittanceStatusRepository.GetListAsync(input.SkipCount, input.MaxResultCount, sorting, filter);
        //    var totalCount = await _remittanceStatusRepository.GetTotalCountAsync(filter);
        //    return  new PagedResultDto<RemittanceStatusDto>(totalCount, ObjectMapper.Map<List<RemittanceStatus>, List<RemittanceStatusDto>>(remittancesStatus));
        //}
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

