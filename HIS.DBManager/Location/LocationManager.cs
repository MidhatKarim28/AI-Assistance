using APIUtility.Models;
using HIS.Models;
using System.Net;
using Utility;

namespace HIS.DBManager
{
    public class LocationManager
    {

        // Get Hospital Facilities List
        public object GetHospitalFacilityList(RequestModel request)
        {
            Logger.WriteInfoLog("This method is called");
            SuccessResponse successResponseModel = new SuccessResponse();
            bool hasData = Constants.FALSE;

            try
            {
                IEnumerable<HospitalFacility> response = new List<HospitalFacility>();
                LocationDB locationDB = new LocationDB();
                response = locationDB.GetHospitalFacilityList(null, ref hasData);

                successResponseModel = new SuccessResponse(response, hasData);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }
            return successResponseModel;
        }

    }

   
}
