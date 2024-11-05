using DBUtility;
using HIS.Models;


namespace HIS.DBManager
{
    public class LocationDB : HISDBAccessProvider
    {
        public LocationDB(string facilityID = "") : base(facilityID)
        {
            new HISDBAccessProvider(facilityID);
        }

        #region Get Methods

        // Mark : Get Hospital Facility
        public IEnumerable<HospitalFacility> GetHospitalFacilityList(HospitalFacility model, ref bool hasData)
        {
            IEnumerable<HospitalFacility> response = new List<HospitalFacility>();
            model = model == null ? new HospitalFacility() : model;
            try
            {
                DataNamesMapper<HospitalFacility> mapper = new DataNamesMapper<HospitalFacility>();

                string query = @"Select 
                                    FACILITYID as HISFacilityID, 
                                    DESCRIPTION, ACTIVE  
                                From HISFACILITY 
                                where 
                                    (:FACILITYID is null or FACILITYID = :FACILITYID )
                                Order By SEQUENCENUMBER  ";

                List<QueryParameter> queryParameters = new List<QueryParameter>
                {
                    new QueryParameter("FACILITYID", model.FacilityID)
                };
            
                 response = mapper.GetDBResponseAuto(this._IDBHelper, query, ref hasData, queryParameters);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        #endregion
    }
}
