﻿using System.Threading.Tasks;
using RMS.Localization;
using RMS.MultiTenancy;
using RMS.Permissions;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace RMS.Blazor.Menus
{

    public class RMSMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<RMSResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    RMSMenus.Home,
                    l["Menu:Home"],
                    "/",
                    icon: "fas fa-home",
                    order: 0
                )
            );
            context.Menu.AddItem(
            new ApplicationMenuItem(
                 RMSMenus.Home,
                l["Menu:RMS"],
                icon: "fa fa-book"
            ).AddItem(
                new ApplicationMenuItem(
                    RMSMenus.Currencies,
                    l["Menu:Currencies"],
                    url: "/currencies"
                )
            ).AddItem(
                new ApplicationMenuItem(
                    RMSMenus.Customers,
                    l["Menu:Customers"],
                    url: "/customers"
                )
            ).AddItem(
                new ApplicationMenuItem(
                    RMSMenus.Remittances,
                    l["Menu:Remittances"],
                    url: "/remittances"
                    )
                )
        );
           
            if (MultiTenancyConsts.IsEnabled)
            {
                administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
            }
            else
            {
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
            administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

            return Task.CompletedTask;
        }
    }
}