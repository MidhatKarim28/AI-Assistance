using APIUtility.Helpers;
using APIUtility.Models;
using System.Net;
using Utility;

namespace Shared.DBManager
{
    public class TokenManager
    {   
        public object GetOneTimeCode(RequestModel request)
        {
            SuccessResponse successResponseModel = new SuccessResponse();
            try
            {
                var payload = new Dictionary<string, object>
                {
                    { "AccessDateTime", DateTime.Now.Ticks },
                    { "ApplicationID",ApplicationSettings.Instance.AppLocalSetting.ApplicationID },
                    { "TokenNature",Constants.enActive.N.ToString() }
                };
                string oneTimeCode = DBManagerUtility.encodeJWT(payload);
                successResponseModel = new SuccessResponse(oneTimeCode, true);
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
