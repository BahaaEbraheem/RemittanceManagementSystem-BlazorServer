using RMS.Currencies;
using RMS.Customers;
using RMS.Remittances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace RMS
{
    public class RMSDataSeederContributor
          : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<Currency, Guid> _currencyRepository;
        

        public RMSDataSeederContributor(
            IGuidGenerator guidGenerator,
            IRepository<Currency, Guid> currencyRepository,
            IRepository<Customer, Guid> customerRepository
        )
        {
            _guidGenerator = guidGenerator;
            _customerRepository = customerRepository;
       
            _currencyRepository = currencyRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedCurrenciesAsync();
        }

        private async Task SeedCurrenciesAsync()
        {
            if (await _currencyRepository.GetCountAsync() > 0)
            {
                return;
            }
         

            await _currencyRepository.InsertAsync(
                new Currency
                (
                  _guidGenerator.Create(),
                   "United Kingdom Pound",
                    "GBP",
                   "£"

                ),
                autoSave: true
            );


            await _currencyRepository.InsertAsync(

            new Currency
           (
                  _guidGenerator.Create(),
               "United States Dollar",
               "USD",
                "$"

           )
            );

            await _currencyRepository.InsertAsync(
            new Currency
            (
                  _guidGenerator.Create(),
                "Syria Pound",
              "SYP",
                "£"

            ), autoSave: true
            );

            await _currencyRepository.InsertAsync(
            new Currency(
                _guidGenerator.Create(),
                "Russia Ruble",
                 "RUB",
                 "₽"
                 ), autoSave: true





           ) ;

        }
    }
}
