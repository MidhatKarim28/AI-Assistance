using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Utility
{    
    public class AppLocalSetting  : IAppLocalSetting
    {
        public string ApplicationEnvironment { get; set; }
        public bool ValidateToken { get; set; }
        public string DefaultDataSource { get; set; }
        public string ApplicationID { get; set; }
        public string OpenAIKey { get; set; }
        public Dictionary<string,string> UserList { get; set; }
        public Dictionary<string, string> OpenAIAssistants { get; set; }
        public AppLocalSetting() { }
    }
}
