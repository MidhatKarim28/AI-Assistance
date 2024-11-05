namespace APIUtility.Models
{
    public class JWTPayload
    {
        public string UserID { get; set; }
        public string TokenNature { get; set; }
        public string DeviceID { get; set; }
       // public string RegCardNo { get; set; }
        public string AccessDateTime { get; set; }
        public string DeviceOS { get; set; }
        public string ApplicationID { get; set; }
        public long TimeDurationInHours { get; set; }
        public string Password { get; set; }
    }
}
