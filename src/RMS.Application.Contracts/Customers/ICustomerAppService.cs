using RMS.Customers.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace RMS.Customers
{
    public interface ICustomerAppService :
         ICrudAppService< //Defines CRUD methods
             CustomerDto, //Used to show currencies
             Guid, //Primary key of the currency entity
             CustomerPagedAndSortedResultRequestDto, //Used for paging/sorting
             CreateUpdateCustomerDto> //Used to create/update a currency
    {
        //Task<PagedResultDto<CustomerDto>> GetListAsync(GetCustomerListDto input);

    }

}
