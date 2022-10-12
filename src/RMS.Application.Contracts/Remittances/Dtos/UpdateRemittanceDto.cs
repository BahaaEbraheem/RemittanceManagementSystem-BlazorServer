using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static RMS.Enums.Enums;

namespace RMS.Remittances.Dtos
{
  public  class UpdateRemittanceDto
    {
        [Required]
        public double Amount { get; set; }
        public string SerialNumber { get;  set; }

        [Required]
        public RemittanceType Type { get; set; }
        //[Required]

        public Guid CreatorId { get; set; }
        public DateTime CreationTime { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public Guid? ReleasedBy { get; set; }
        public DateTime? ReleasedDate { get; set; }
        [Required]
        public Guid SenderBy { get; set; }

        public Guid? ReceiverBy { get; set; }
        public string SenderName { get; set; }

        [Required]
        public string ReceiverFullName { get; set; }
        public string ReceiverName { get; set; }
        
        [Required]
        public Guid CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public Remittance_Status State { get; set; }


    }
}
