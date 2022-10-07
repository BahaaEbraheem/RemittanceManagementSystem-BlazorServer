using RMS.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace RMS.Permissions
{

    public class RMSPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(RMSPermissions.GroupName);

            var currenciesPermission = myGroup.AddPermission(RMSPermissions.Currencies.Default, L("Permission:Currencies"));
            currenciesPermission.AddChild(RMSPermissions.Currencies.Create, L("Permission:Currencies.Create"));
            currenciesPermission.AddChild(RMSPermissions.Currencies.Edit, L("Permission:Currencies.Edit"));
            currenciesPermission.AddChild(RMSPermissions.Currencies.Delete, L("Permission:Currencies.Delete"));
            //Define your own permissions here. Example:
            //myGroup.AddPermission(RMSPermissions.MyPermission1, L("Permission:MyPermission1"));

            var customersPermission = myGroup.AddPermission(RMSPermissions.Customers.Default, L("Permission:Customers"));
            customersPermission.AddChild(RMSPermissions.Customers.Create, L("Permission:Customers.Create"));
            customersPermission.AddChild(RMSPermissions.Customers.Edit, L("Permission:Customers.Edit"));
            customersPermission.AddChild(RMSPermissions.Customers.Delete, L("Permission:Customers.Delete"));


            var remittancesPermission = myGroup.AddPermission(RMSPermissions.Remittances.Default, L("Permission:Remittances"));
            remittancesPermission.AddChild(RMSPermissions.Remittances.Create, L("Permission:Remittances.Create"));
            remittancesPermission.AddChild(RMSPermissions.Remittances.Edit, L("Permission:Remittances.Edit"));
            remittancesPermission.AddChild(RMSPermissions.Remittances.Delete, L("Permission:Remittances.Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<RMSResource>(name);
        }
    }
}