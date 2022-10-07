using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using static RMS.Enums.Enums;

namespace RMS.Customers
{
    public class Customer : FullAuditedAggregateRoot<Guid>
    {

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        [Display(Name = "Mother Name")]
        public string MotherName { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime BirthDate { get; set; }  


        public string Phone { get; set; }
        public string Address { get; set; }
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        private Customer() { }
        public Customer(Guid id, [NotNull] string firstName, [NotNull] string lastName,
            [NotNull] string fatherName, [NotNull] string motherName,
         [NotNull] DateTime birthDate, [NotNull] string phone, [NotNull] Gender gender) : base(id)
        {
            Id = id;
            SetName(firstName, lastName, fatherName, motherName);
            BirthDate = SetBirthdate(birthDate);
            Phone = phone;
            Gender = gender;
 
        }
        internal Customer ChangeName([NotNull] string firstName, [NotNull] string lastName,
            [NotNull] string fatherName, [NotNull] string motherName)
        {
            SetName(firstName, lastName, fatherName, motherName);
            return this;
        }
        public DateTime SetBirthdate([NotNull]  DateTime birthDate)
        {
           Check.NotNullOrWhiteSpace(birthDate.ToString(), nameof(birthDate));
        
            if (birthDate.Date>DateTime.Now.Date)
            {
                throw new CustomerExceptionInHisBirthDate(birthDate);
            }
            return birthDate;
        }

        private void SetName([NotNull] string firsName, [NotNull] string lastName
            , [NotNull] string fatherName, [NotNull] string motherName)
        {
            FirstName = Check.NotNullOrWhiteSpace(firsName, nameof(firsName));
            LastName = Check.NotNullOrWhiteSpace(lastName, nameof(lastName));
            FatherName = Check.NotNullOrWhiteSpace(fatherName, nameof(fatherName));
            MotherName = Check.NotNullOrWhiteSpace(motherName, nameof(motherName));

        }
    }
}
