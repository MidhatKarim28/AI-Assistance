using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class ApplicationSettings
    {
        private static ApplicationSettings instance = new ApplicationSettings();
        public static ApplicationSettings Instance { get { return instance; } }

        public IAppGlobalSetting AppGlobalSetting { get; set; }
        public IAppLocalSetting AppLocalSetting { get; set; }

        public Dictionary<string, string> OracleTNS { get {  
                if(AppGlobalSetting.TNS != null && AppGlobalSetting.TNS.Count > 0)
                    return AppGlobalSetting.TNS;
                return new Dictionary<string, string>(); } }
    }
}
