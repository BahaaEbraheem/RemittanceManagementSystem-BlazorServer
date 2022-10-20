using Blazorise.DataGrid;
using RMS.Customers.Dtos;
using RMS.Localization;
using System;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.Ui.Branding;

namespace RMS.Blazor
{

    public abstract class RMSComponentBase : AbpComponentBase
    {
        protected RMSComponentBase()
        {
            LocalizationResource = typeof(RMSResource);
        }
 
    }
}