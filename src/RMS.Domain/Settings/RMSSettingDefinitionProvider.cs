using Volo.Abp.Settings;

namespace RMS.Settings
{

    public class RMSSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(RMSSettings.MySetting1));
        }
    }
}