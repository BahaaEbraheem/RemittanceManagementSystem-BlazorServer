using Volo.Abp.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using static RMS.Enums.Enums;


namespace RMS.Remittances.Dtos
{
   public class GetRemittanceListPagedAndSortedResultRequestDto : PagedAndSortedResultRequestDto
    {

        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        public RemittanceType Type { get; set; }
        public string SerialNumber { get; set; }
        public string SenderName { get; set; }
        public string ReceiverFullName { get; set; }
        public string CurrencyName { get; set; }
        public Remittance_Status State { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? CreationTime { get; set; }
    }
}
