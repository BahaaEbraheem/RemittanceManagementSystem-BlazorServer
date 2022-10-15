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
        public CustomerAppService(IRepository<Customer, Guid> repository, ICustomerRepository customerRepository,
            IRemittanceRepository remittanceRepository)
            : base(repository)
        {
            _customerRepository = customerRepository;
            _remittanceRepository = remittanceRepository;
        }


 

        //public async Task<PagedResultDto<CustomerDto>> GetListAsync(CustomerPagedAndSortedResultRequestDto input)
        //{
        //    var filter = ObjectMapper.Map<CustomerPagedAndSortedResultRequestDto, Customer>(input);
        //    var sorting = (string.IsNullOrEmpty(input.Sorting) ? "FirstName DESC" : input.Sorting).Replace("ShortName", "FirstName");
        //    var customers = await _customerRepository.GetListAsync(input.SkipCount, input.MaxResultCount, sorting, filter);
        //    var totalCount = await _customerRepository.GetTotalCountAsync(filter);
        //    return new PagedResultDto<CustomerDto>(totalCount, ObjectMapper.Map<List<Customer>, List<CustomerDto>>(customers));
        //}




        //public async Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerListDto input)
        //{
        //    //Get the IQueryable<remittance> from the repository

        //    var customerqueryable = await _customerRepository.GetQueryableAsync();

        //    //Prepare a query to join remittances and currencies
        //    var query = from customer in customerqueryable 
        //                select new {  customer };

        //    //Paging
        //    query = query
        //        .OrderBy(x => x.customer.FirstName).ThenBy(x => x.customer.FatherName)
        //        .Skip(input.SkipCount)
        //        .Take(input.MaxResultCount);

        //    //Execute the query and get a list
        //    var queryResult = await AsyncExecuter.ToListAsync(query);

        //    //Convert the query result to a list of RemittanceDto objects
        //    var customerDtos = queryResult.Select(x =>
        //    {
        //        var customerDto = ObjectMapper.Map<Customer, CustomerDto>(x.customer);
        //        return customerDto;
        //    }).ToList();

        //    //Get the total count with another query
        //    var totalCount = await _customerRepository.GetCountAsync();

        //    return new PagedResultDto<CustomerDto>(
        //        totalCount,
        //        customerDtos
        //    );
        //}
    }
}






