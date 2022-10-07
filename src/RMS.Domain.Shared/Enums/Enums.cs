using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RMS.Enums
{
   public class Enums
    {
        public enum Gender
        {
            Male = 1,
            Female = 2
        }
        public enum RemittanceType
        {
            Internal = 1,
            External = 2
        }
        public enum Remittance_Status
        {

            Draft = 1,
            Ready = 2,
            Approved = 3,
            Release = 4,


        }



    }
}
