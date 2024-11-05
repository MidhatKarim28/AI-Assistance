using Utility;

namespace HIS.Models
{
    public class HospitalFacility : CommonDictionaryAttribute
    {
        [DataNames("HISFacilityID")]
        public new string FacilityID { get; set; }
        [DataNames]
        public string Description { get; set; }
    }
    
}
