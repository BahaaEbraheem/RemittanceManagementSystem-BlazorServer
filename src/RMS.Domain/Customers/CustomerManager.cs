using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using static RMS.Enums.Enums;

namespace RMS.Customers
{
   public class CustomerManager : DomainService
    {
        private readonly ICustomerRepository _CustomerRepository;

        public CustomerManager(ICustomerRepository CustomerRepository)
        {
            _CustomerRepository = CustomerRepository;
        }

        public async Task<Customer> CreateAsync([NotNull] string firstName, [NotNull] string lastName, 
            [NotNull] string fatherName, [NotNull] string motherName,
           [NotNull] DateTime birthDate, [NotNull] string phone, [NotNull] Gender gender)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName));
             Check.NotNullOrWhiteSpace(lastName, nameof(lastName));
             Check.NotNullOrWhiteSpace(fatherName, nameof(fatherName));
            Check.NotNullOrWhiteSpace(motherName, nameof(motherName));

            var existingCustomer = await _CustomerRepository.FindByFullNameAsync(firstName, lastName, fatherName, motherName);
            if (existingCustomer != null)
            {
                throw new CustomerAlreadyExistsException(firstName);
            }
            Check.NotNullOrWhiteSpace(birthDate.ToString(), nameof(birthDate));

            if (birthDate.Date > DateTime.Now.Date)
            {
                throw new CustomerExceptionInHisBirthDate(birthDate);
            }
            return new Customer(
                GuidGenerator.Create(),
                firstName,lastName,fatherName,
                motherName,birthDate,phone,gender);
        }

        public async Task ChangeNameAsync([NotNull] Customer Customer, [NotNull] string newfirstName, [NotNull] string newlastName,
            [NotNull] string newfatherName, [NotNull] string newmotherName)
        {
            Check.NotNull(Customer, nameof(Customer));
            Check.NotNullOrWhiteSpace(newfirstName, nameof(newfirstName));
            Check.NotNullOrWhiteSpace(newlastName, nameof(newlastName));
            Check.NotNullOrWhiteSpace(newfatherName, nameof(newfatherName));
            Check.NotNullOrWhiteSpace(newmotherName, nameof(newmotherName));

            var existingCustomer = await _CustomerRepository.FindByFullNameAsync(newfirstName, newlastName, newfatherName, newmotherName);
            if (existingCustomer != null && existingCustomer.Id != Customer.Id)
            {
                throw new CustomerAlreadyExistsException(newfirstName);
            }

            Customer.ChangeName(newfirstName, newlastName, newfatherName, newmotherName);
        }
    }
}
