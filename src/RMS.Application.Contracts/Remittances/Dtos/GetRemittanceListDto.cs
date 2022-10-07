using Volo.Abp.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.Remittances.Dtos
{
   public class GetRemittanceListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
