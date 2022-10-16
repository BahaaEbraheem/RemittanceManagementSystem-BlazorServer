namespace RMS
{

    public static class RMSDomainErrorCodes
    {
        public const string CurrencyAlreadyExists = "RMS:00001";
        public const string CustomerAlreadyExists = "RMS:00002";
        public const string BirthDateExeption = "RMS:00003";
        public const string RemittanceAlreadyApproved = "RMS:00004";
        public const string CurrencyAlreadyUsedInRemittance = "RMS:00005";
        public const string CustomerAlreadyUsedInRemittance = "RMS:00006";
        public const string CustomerDontPassBecauseHisAgeSmallerThan18 = "RMS:00007";




        /* You can add your business exception error codes here, as constants */
    }
}