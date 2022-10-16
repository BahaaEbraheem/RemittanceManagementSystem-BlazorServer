using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace RMS.Customers
{
    public class CustomerDontPassBecauseHisAgeSmallerThan18Exception : BusinessException
    {
        public CustomerDontPassBecauseHisAgeSmallerThan18Exception(string customerName)
            : base(RMSDomainErrorCodes.CustomerDontPassBecauseHisAgeSmallerThan18)
        {
            WithData("customerName", customerName);
        }
    
    }
}
