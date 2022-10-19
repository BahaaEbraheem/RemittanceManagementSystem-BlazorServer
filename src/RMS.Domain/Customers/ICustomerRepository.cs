using RMS.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace RMS.Customers
{
   public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer> FindByFullNameAsync(string firstName, string lastName,string fatherName,string motherName);

        Task<List<Customer>> GetListAsync(
     int skipCount,
     int maxResultCount,
     string sorting,
     Customer filter
 );
        Task<int> GetTotalCountAsync(Customer filter);
    }
}
