using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace RMS.Customers
{
    public class CustomerAlreadyExistsException : BusinessException
    {
        public CustomerAlreadyExistsException(string name)
            : base(RMSDomainErrorCodes.CustomerAlreadyExists)
        {
            WithData("name", name);
        }
    }
}
