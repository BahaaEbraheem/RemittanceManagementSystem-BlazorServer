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
using Volo.Abp.DependencyInjection;
using RMS.Customers;
using RMS.Customers.Dtos;
using Volo.Abp.ObjectMapping;
using RMS.Remittances.Dtos;
using RMS.Remittances;
using RMS.Status;
using Volo.Abp.Users;

namespace RMS.Currencies
{
    [Authorize(RMSPermissions.Customers.Default)]

    public class CustomerAppService :
        CrudAppService<
               Customer, //The customer entity
            CustomerDto, //Used to show customers
            Guid, //Primary key of the customer entity
            CustomerPagedAndSortedResultRequestDto, //Used for paging/sorting
            CreateUpdateCustomerDto>, //Used to create/update a customer
        ICustomerAppService //implement the IcustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRemittanceRepository _remittanceRepository;
        private readonly CustomerManager _customerManager;
        public CustomerAppService(IRepository<Customer, Guid> repository,
            CustomerManager customerManager,
            ICustomerRepository customerRepository,
            IRemittanceRepository remittanceRepository)
            : base(repository)
        {
            _customerManager = customerManager;
            _customerRepository = customerRepository;
            _remittanceRepository = remittanceRepository;
        }




        public override async Task<PagedResultDto<CustomerDto>> GetListAsync(CustomerPagedAndSortedResultRequestDto input)
        {

            var filter = ObjectMapper.Map<CustomerPagedAndSortedResultRequestDto, Customer>(input);
            var sorting = (string.IsNullOrEmpty(input.Sorting) ? "FirstName DESC" : input.Sorting).Replace("ShortName", "FirstName");
            var customers = await _customerRepository.GetListAsync(input.SkipCount, input.MaxResultCount, sorting, filter);
            var totalCount = await _customerRepository.GetTotalCountAsync(filter);
            return new PagedResultDto<CustomerDto>(totalCount, ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers));
        }

        [Authorize(RMSPermissions.Customers.Create)]
        public override Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
        {
            try
            {
                if (input!=null)
                {
                    return base.CreateAsync(input);

                }
                throw new ArgumentNullException(nameof(input));

            }
            catch (Exception)
            {

                throw;
            }
        }



        [Authorize(RMSPermissions.Customers.Edit)]
        public override Task<CustomerDto> UpdateAsync(Guid id, CreateUpdateCustomerDto input)
        {
            try
            {
                if (input != null)
                {
                    return base.UpdateAsync(id, input);

                }
                    throw new ArgumentNullException(nameof(input));
            }
            catch (Exception)
            {

                throw;
            }
        }


        [Authorize(RMSPermissions.Customers.Delete)]
        public override Task DeleteAsync(Guid id)
        {
            try
            {
                if (!id.Equals(null))
                {
                    _customerManager.IsCustomerUsedBeforInRemittance(id);

                    return base.DeleteAsync(id);
                }
                throw new ArgumentNullException(nameof(id));
            }
            catch (Exception)
            {

                throw;
            }
          
        }


    }
}






