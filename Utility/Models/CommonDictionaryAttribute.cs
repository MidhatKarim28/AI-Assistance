namespace Utility
{
  public class CommonDictionaryAttribute : BaseModel
    {
        [DataNames]
        public string LastFileDttm { get; set; }
        [DataNames]
        public string LastFileUser { get; set; }
        [DataNames]
        public string LastFileTerminal { get; set; }
        [DataNames]
        public string Active { get; set; }
    }
}
