using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace RMS.Remittances.Dtos
{
   public class CurrencyLookupDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
