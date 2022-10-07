using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace RMS.Currencies
{
   public class CurrencyAlreadyExistsException : BusinessException
    {
        public CurrencyAlreadyExistsException(string name)
            : base(RMSDomainErrorCodes.CurrencyAlreadyExists)
        {
            WithData("name", name);
        }
    }

}
