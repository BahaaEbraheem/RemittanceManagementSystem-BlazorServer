using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using static RMS.Enums.Enums;

namespace RMS.Status.Dtos
{
    public class RemittanceStatusDto : AuditedEntityDto<Guid>
    {
        public Guid RemittanceId { get; protected set; }
        public Remittance_Status State { get; set; }


    }
}
