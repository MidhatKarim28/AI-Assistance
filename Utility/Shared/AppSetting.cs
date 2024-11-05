using System.Configuration;
using System.Linq;

namespace Utility
{
    public static class AppSetting
    {
        public static string GetKeyValue(string appkey)
        {
            string value = string.Empty;
            if (ConfigurationManager.AppSettings.AllKeys.Contains(appkey))
            {
                value = ConfigurationManager.AppSettings[appkey].ToString();
            }

            return value;
        }
    }
}
