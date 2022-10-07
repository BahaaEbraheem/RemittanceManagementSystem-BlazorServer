using RMS.Customers.Dtos;
using RMS.Status.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace RMS.Status
{
    public interface IRemittanceStatusAppService : ICrudAppService< //Defines CRUD methods
             RemittanceStatusDto, //Used to show currencies
             Guid, //Primary key of the currency entity
             PagedAndSortedResultRequestDto, //Used for paging/sorting
             CreateUpdateRemittanceStatusDto> //Used to create/update a currency
    {

    }
}
