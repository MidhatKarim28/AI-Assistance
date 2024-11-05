namespace APIUtility.Models
{
    public class SearchModel :BaseModel
    {
        public SearchModel(string mRNumber, string visitID, string recordID)
        {
            MRNumber = mRNumber;
            VisitID = visitID;
            RecordID = recordID;            
        }

        public SearchModel()
        {
        }

        public string MRNumber { get; set; }
        public string VisitID { get; set; }
        public string VisitStateID { get; set; }
        public string VisitTypeID { get; set; }
        public string RecordID { get; set; }
    }
}
