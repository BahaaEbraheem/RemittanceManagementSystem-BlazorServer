using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace RMS.Blazor
{

    [Dependency(ReplaceServices = true)]
    public class RMSBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "RMS";
    }
}