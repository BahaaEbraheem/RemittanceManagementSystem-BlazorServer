using Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace RMS.Blazor
{
    [Dependency(ReplaceServices = true)]
    public class RMSBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "RMS";
        public override string LogoUrl => "https://th.bing.com/th/id/R.1ce9665c38057f6ff44ebb5ac278fd27?rik=wnoAvBsvV9q%2fhw&riu=http%3a%2f%2fwww.pnb.com.ph%2frcc%2fimages%2fRemittance+Page+Banner.jpg&ehk=gKkuPHMENWg3rUJFCFO4TDx1Jf4QPnG%2btxNQI70OvEo%3d&risl=&pid=ImgRaw&r=0";

    }
}