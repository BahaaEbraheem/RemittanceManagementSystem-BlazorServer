using System;
using System.Collections.Generic;
using System.Text;
using RMS.Localization;
using Volo.Abp.Application.Services;

namespace RMS
{

    /* Inherit your application services from this class.
     */
    public abstract class RMSAppService : ApplicationService
    {
        protected RMSAppService()
        {
            LocalizationResource = typeof(RMSResource);
        }
    }
}