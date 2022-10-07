using Microsoft.EntityFrameworkCore;
using RMS.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace RMS.Customers
{
        public class EfCoreCustomerRepository : EfCoreRepository<RMSDbContext, Customer, Guid>, ICustomerRepository
    {
        public EfCoreCustomerRepository(
            IDbContextProvider<RMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Customer> FindByFullNameAsync(string firstName, string lastName, string fatherName, string motherName)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.FirstOrDefaultAsync(Customer => Customer.FirstName == firstName && Customer.LastName == lastName &&
                                                    Customer.FatherName == fatherName && Customer.MotherName == motherName);
        }

        public async Task<List<Customer>> GetListAsync(int skipCount, int maxResultCount, string sorting, Customer filter)
        {
            var dbSet = await GetDbSetAsync();
            var customers= await dbSet
                .WhereIf(!filter.FirstName.IsNullOrWhiteSpace(), x => x.FirstName.Contains(filter.FirstName))
                .WhereIf(!filter.LastName.IsNullOrWhiteSpace(), x => x.LastName.Contains(filter.LastName))
                .WhereIf(!filter.FatherName.IsNullOrWhiteSpace(), x => x.FatherName.Contains(filter.FatherName))
                .WhereIf(!filter.MotherName.IsNullOrWhiteSpace(), x => x.MotherName.Contains(filter.MotherName))
                .OrderBy(sorting).Skip(skipCount).Take(maxResultCount).ToListAsync();
            return customers;

        }

        public async Task<int> GetTotalCountAsync(Customer filter)
        {
            var dbSet = await GetDbSetAsync();
            var customers = await dbSet
                .WhereIf(!filter.FirstName.IsNullOrWhiteSpace(),
                x => x.FirstName.Contains(filter.FirstName))
                .WhereIf(!filter.LastName.IsNullOrWhiteSpace(),
                x => x.LastName.Contains(filter.LastName))
                .WhereIf(!filter.FatherName.IsNullOrWhiteSpace(),
                x => x.FatherName.Contains(filter.FatherName))
                .WhereIf(!filter.MotherName.IsNullOrWhiteSpace()
                , x => x.MotherName.Contains(filter.MotherName))
                .ToListAsync();
            return customers.Count;
        }

        //public async Task<List<Customer>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
        //{
        //    var dbSet = await GetDbSetAsync();
        //    return await dbSet
        //        .WhereIf(
        //            !filter.IsNullOrWhiteSpace(),
        //            Customer => Customer.FirstName.Contains(filter)
        //         )
        //        .OrderBy(sorting)
        //        .Skip(skipCount)
        //        .Take(maxResultCount)
        //        .ToListAsync();
        //}
    }
}
