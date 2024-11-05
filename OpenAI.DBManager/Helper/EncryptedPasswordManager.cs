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
    public class EncryptedPasswordManager 
    {
        public object GetEncryptedPassword(RequestModel request)
        {
            SuccessResponse successResponseModel = new SuccessResponse();
            try
            {
                CredentialModel model = JsonConvert.DeserializeObject<CredentialModel>(Convert.ToString(request.RequestData));

                string encryptedPassword = UtilityHelper.Encrypt(model.Password);
                successResponseModel = new SuccessResponse(encryptedPassword, true);                
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
