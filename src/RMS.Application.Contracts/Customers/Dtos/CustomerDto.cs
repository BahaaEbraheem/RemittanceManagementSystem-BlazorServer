using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using static RMS.Enums.Enums;

namespace RMS.Customers.Dtos
{
      public class CustomerDto : AuditedEntityDto<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FatherName { get; set; }

        public string MotherName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }


        public string Phone { get; set; }

        public string Address { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
    }
}
