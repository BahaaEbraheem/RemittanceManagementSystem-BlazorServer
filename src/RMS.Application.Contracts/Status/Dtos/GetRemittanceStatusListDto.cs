using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace RMS.Status.Dtos
{
    public class GetRemittanceStatusListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
