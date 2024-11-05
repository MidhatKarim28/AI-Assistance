using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Models
{
    public class TokenRequestModel : CredentialModel
    {
        public long TimeDurationInHours { get; set; }
        public string ApplicationID { get; set; }
    }
}
