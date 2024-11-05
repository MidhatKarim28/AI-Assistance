using Utility;

namespace APIUtility.Models
{
    public class RegisteredToken
    {
        [DataNames]
        public string UserID { get; set; }
        [DataNames]
        public string DeviceID { get; set; }
        [DataNames]
        public string Token { get; set; }
        [DataNames]
        public string AccessDTTM { get; set; }
        [DataNames]
        public string DeviceOS { get; set; }
        [DataNames("HISFacilityID")]
        public string FacilityID { get; set; }
    }
}
