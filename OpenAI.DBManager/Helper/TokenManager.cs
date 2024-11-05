using APIUtility.Helpers;
using APIUtility.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpenAI.Models;
using OpenAI_API;
using OpenAI_API.Chat;
using System.Net;
using Utility;

namespace OpenAI.DBManager
{
    public class TokenManager
    {   
        public object GetAIToken(RequestModel request)
        {
            SuccessResponse successResponseModel = new SuccessResponse();
            try
            {
                TokenRequestModel model = JsonConvert.DeserializeObject<TokenRequestModel>(Convert.ToString(request.RequestData));

                if (model != null && !string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.UserID) && !string.IsNullOrEmpty(model.ApplicationID))
                {
                    var payload = new Dictionary<string, object>
                    {
                        { "AccessDateTime", DateTime.Now.Ticks },
                        { "ApplicationID",ApplicationSettings.Instance.AppLocalSetting.ApplicationID },
                        { "TokenNature",Constants.enTokenNature.E.ToString() },
                        { "TimeDurationInHours",model.TimeDurationInHours },
                        { "UserID",model.UserID },
                        { "Password",model.Password }
                    };
                    string oneTimeCode = DBManagerUtility.encodeJWT(payload);
                    successResponseModel = new SuccessResponse(oneTimeCode, true);
                }
                else
                {
                    successResponseModel = new SuccessResponse(null, false, "Invalid parameters");
                }                
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
