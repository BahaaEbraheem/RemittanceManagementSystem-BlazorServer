using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static RMS.Enums.Enums;

namespace RMS.Status.Dtos
{
    public class CreateUpdateRemittanceStatusDto
    {
        [Required]
        public Guid RemittanceId { get; protected set; }
        [Required]
        public Remittance_Status State { get; set; }
    }
}
