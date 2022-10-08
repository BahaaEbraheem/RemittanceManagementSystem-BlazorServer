using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;
using static RMS.Enums.Enums;
using System.Linq;
using System.Threading.Tasks;
using RMS.Remittances.Dtos;
using RMS.Remittances;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.Abp.Users;
namespace RMS.Remittances.Dtos
{
    public class CreateRemittanceDto /*: IValidatableObject*/
    {
        [Required]
        public double Amount { get; set; }

        [Required]
        public RemittanceType Type { get; set; }
        public string SerialNumber { get;  set; }

        //[Required]

        public Guid? CreatorId { get; set; }
        public DateTime CreationTime { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public Guid? ReleasedBy { get; set; }
        public DateTime? ReleasedDate { get; set; }
        [Required]
        public Guid SenderBy { get; set; }
        public string SenderName { get; set; }
        public Guid? ReceiverBy { get; set; }
        [Required]
        public string ReceiverFullName { get; set; }
        [Required]
        public Guid CurrencyId { get; set; }


        public string CurrencyName { get; set; }
        public Remittance_Status State { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{

        //    //if (Amount<=0)
        //    //{
        //    //    yield return new ValidationResult(
        //    //        "Fill Amount greater than 0!",
        //    //        new[] { "Amount" }
        //    //    );
        //    //}
        //}
    }
}
