using System;
using System.Collections.Generic;
using System.Text;
using RMS.Localization;
using Volo.Abp.Application.Services;
using Volo.Abp.Ui.Branding;

namespace RMS
{

    /* Inherit your application services from this class.
     */
    public abstract class RMSAppService : ApplicationService, IBrandingProvider
    {
        protected RMSAppService()
        {
            LocalizationResource = typeof(RMSResource);
        }

        public string AppName => "RMS";
        public string LogoUrl => "https://th.bing.com/th/id/R.1ce9665c38057f6ff44ebb5ac278fd27?rik=wnoAvBsvV9q%2fhw&riu=http%3a%2f%2fwww.pnb.com.ph%2frcc%2fimages%2fRemittance+Page+Banner.jpg&ehk=gKkuPHMENWg3rUJFCFO4TDx1Jf4QPnG%2btxNQI70OvEo%3d&risl=&pid=ImgRaw&r=0";
        public string LogoReverseUrl => throw new NotImplementedException();

    }
}