using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace RMS.Currencies
{
    public class CurrencyManager : DomainService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyManager(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<Currency> CreateAsync([NotNull] string name, [NotNull] string symbol, string code)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(symbol, nameof(symbol));

            var existingCurrency = await _currencyRepository.FindByNameAndSymbolAsync(name, symbol);
            if (existingCurrency != null)
            {
                throw new CurrencyAlreadyExistsException(name);
            }

            return new Currency(
                GuidGenerator.Create(),
                name,
                symbol,
                code
            );
        }

        public async Task ChangeNameAsync([NotNull] Currency Currency,[NotNull] string newName, [NotNull] string newSymbol)
        {
            Check.NotNull(Currency, nameof(Currency));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existingCurrency = await _currencyRepository.FindByNameAndSymbolAsync(newName, newSymbol);
            if (existingCurrency != null && existingCurrency.Id != Currency.Id)
            {
                throw new CurrencyAlreadyExistsException(newName);
            }

            Currency.ChangeName(newName, newSymbol);
        }
    }
}
