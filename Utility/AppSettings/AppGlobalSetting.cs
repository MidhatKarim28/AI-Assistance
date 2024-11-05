using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Utility
{
    public class AppGlobalSetting  : IAppGlobalSetting
    {
        public string EmailHostName { get; set; }
        public string EmailFrom { get; set; }
        public string SMSConnectionString { get; set; }
        public string UserAuthenticationServiceURL { get; set; }
        public string FileStoragePath { get; set; }
        public Dictionary<string, string> TNS { get; set; }

        public AppGlobalSetting() { }       

    } 
}
