using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static RMS.Enums.Enums;

namespace RMS.Customers.Dtos
{
  public  class UpdateCustomerDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]

        public string FatherName { get; set; }
        [Required]

        public string MotherName { get; set; }
        [Required]

        public DateTime BirthDate { get; set; }

        [Required]

        public string Phone { get; set; }
        [Required]

        public string Address { get; set; }
        [Required]

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
    }
}
