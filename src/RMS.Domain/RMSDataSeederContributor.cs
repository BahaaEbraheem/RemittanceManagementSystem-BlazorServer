using Microsoft.AspNetCore.Identity;
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
using Volo.Abp.Identity;
using Volo.Abp.Users;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace RMS
{
    public class RMSDataSeederContributor
          : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<Currency, Guid> _currencyRepository;
        private readonly IRepository<IdentityRole, Guid> _roleRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IdentityUserManager _identityUserManager;


        public RMSDataSeederContributor(

            IGuidGenerator guidGenerator,
            IdentityUserManager identityUserManager,
            IRepository<IdentityRole, Guid> roleRepository,
            IRepository<Currency, Guid> currencyRepository,
            IRepository<Customer, Guid> customerRepository,
            ICurrentUser currentUser
        )
        {
            _guidGenerator = guidGenerator;
            _customerRepository = customerRepository;
            _identityUserManager = identityUserManager;
                 _currencyRepository = currencyRepository;
            _roleRepository = roleRepository;
            _currentUser = currentUser;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedCurrenciesAsync();
            await SeedRolesAsync();

        }

        private async Task SeedRolesAsync()
        {

            if (await _roleRepository.GetCountAsync() > 1)
            {
                return;
            }
            await _roleRepository.InsertAsync(
               new IdentityRole
               (
                 _guidGenerator.Create(),
                  "Creator"
               ),
               autoSave: true
           );
            await _roleRepository.InsertAsync(
               new IdentityRole
               (
                 _guidGenerator.Create(),
                  "Supervisor"
               ),
               autoSave: true
           );
            await _roleRepository.InsertAsync(
           new IdentityRole
           (
             _guidGenerator.Create(),
              "Releaser"
           ),
           autoSave: true
       );

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
                "Syrian Pound",
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
