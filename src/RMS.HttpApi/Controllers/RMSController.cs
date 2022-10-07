using RMS.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace RMS.Controllers
{

    /* Inherit your controllers from this class.
     */
    public abstract class RMSController : AbpControllerBase
    {
        protected RMSController()
        {
            LocalizationResource = typeof(RMSResource);
        }
    }
}