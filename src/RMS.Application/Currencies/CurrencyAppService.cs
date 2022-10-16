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
using Volo.Abp;
using RMS.Remittances;

namespace RMS.Currencies
{
    [Authorize(RMSPermissions.Currencies.Default)]

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
        private readonly IRemittanceAppService _remittanceAppService;
        private readonly CurrencyManager _currencyManager;
        public CurrencyAppService(IRepository<Currency, Guid> repository,
            ICurrencyRepository currencyRepository,
            IRemittanceAppService remittanceAppService,
            CurrencyManager currencyManager)
    : base(repository)
        {
            _currencyRepository = currencyRepository;
            _currencyManager = currencyManager;
            _remittanceAppService = remittanceAppService;
        }



        public override async Task<PagedResultDto<CurrencyDto>> GetListAsync(CurrencyPagedAndSortedResultRequestDto input)
        {
            var filter = ObjectMapper.Map<CurrencyPagedAndSortedResultRequestDto, Currency>(input);
            var sorting = (string.IsNullOrEmpty(input.Sorting) ? "Name DESC" : input.Sorting).Replace("ShortName", "Name");
            var currencies = await _currencyRepository.GetListAsync(input.SkipCount, input.MaxResultCount, sorting, filter);
            var totalCount = await _currencyRepository.GetTotalCountAsync(filter);
            return new PagedResultDto<CurrencyDto>(totalCount, ObjectMapper.Map<List<Currency>, List<CurrencyDto>>(currencies));
        }




        [Authorize(RMSPermissions.Currencies.Create)]
        public override  Task<CurrencyDto> CreateAsync(CreateUpdateCurrencyDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("");
            }
            Check.NotNullOrWhiteSpace(input.Name, nameof(input.Name));
            Check.NotNullOrWhiteSpace(input.Symbol, nameof(input.Symbol));
            var existingCurrency = _currencyRepository.FindByNameAndSymbolAsync(input.Name, input.Symbol).Result;
            if (existingCurrency != null)
            {
                throw new CurrencyAlreadyExistsException(existingCurrency.Name);
            }
            return base.CreateAsync(input);
        }




        [Authorize(RMSPermissions.Currencies.Edit)]
        public override async Task<CurrencyDto> UpdateAsync(Guid id, CreateUpdateCurrencyDto input)
        {

            if (input == null)
            {
                throw new ArgumentNullException("");
            }
            Check.NotNullOrWhiteSpace(id.ToString(), nameof(id));
            //check if currency exist befor in tables
            var existingCurrency = await _currencyRepository.FindByNameAndSymbolAsync(input.Name, input.Symbol);
            if ((existingCurrency != null && !existingCurrency.Name.Contains(input.Name))
               || (existingCurrency != null && !existingCurrency.Symbol.Contains(input.Symbol)))
            {
                throw new CurrencyAlreadyExistsException(existingCurrency.Name);
            }
            return await base.UpdateAsync(id, input);
        }



        [Authorize(RMSPermissions.Currencies.Delete)]
        public override Task DeleteAsync(Guid id)
        {
            //check if this currency using by any remittance
             _currencyManager.IsCurrencyUsedBeforInRemittance(id);
            return base.DeleteAsync(id);
        }

    }
}


