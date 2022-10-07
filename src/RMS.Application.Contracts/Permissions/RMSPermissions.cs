namespace RMS.Permissions
{

    public static class RMSPermissions
    {
        public const string GroupName = "RMS";
        public static class Currencies
        {
            public const string Default = GroupName + ".Currencies";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        public static class Customers
        {
            public const string Default = GroupName + ".Customers";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        // *** ADDED a NEW NESTED CLASS ***
        public static class Remittances
        {
            public const string Default = GroupName + ".Remittances";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";
    }
}