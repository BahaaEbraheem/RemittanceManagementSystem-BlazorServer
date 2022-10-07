using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace RMS.Remittances.Dtos
{
   public class CustomerLookupDto : EntityDto<Guid>
    {
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string LastName { get; set; }
    }
}
