using APIUtility.Constants;
using APIUtility.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text.Json.Serialization;
using Utility;
using static APIUtility.Constants.StringConstants;

namespace AIAssistanceAPI.Controllers
{
    [Route("api/")]
    [ApiController]    
    public class AIAssistanceAPIController : ControllerBase 
    {        
        public AIAssistanceAPIController(IAppGlobalSetting _appGlobalSetting, IAppLocalSetting _appLocalSetting)        
        {
            ApplicationSettings.Instance.AppGlobalSetting = _appGlobalSetting;
            ApplicationSettings.Instance.AppLocalSetting = _appLocalSetting;
        }

        [HttpGet]
        public object Get()
        {           
            return Content("API is working", "application/json; charset=utf-8");
        }
                
        [HttpPost]
        public object Post()
        {
            ResponseModel? apiResponseModel = null;
            try
            {
                if (!Request.ContentType.IsNormalized() || !this.Request.HasFormContentType)
                {
                   // return ApiFailure(StringConstants.Message.BadRequest, StringConstants.Message.FailureMessage, HttpStatusCode.BadRequest, string.Empty);
                }

                string tokenHeader = UtilityHelper.IsValidString(Request.Headers[StringConstants.Value.TokenKey]);
                string requestMethod = UtilityHelper.IsValidString(Request.Form[StringConstants.Value.RequestMethodKey]);
                string requestData = UtilityHelper.IsValidString(Request.Form[StringConstants.Value.RequestDataKey]);
                string requestDateTime = UtilityHelper.IsValidString(Request.Form[StringConstants.Value.RequestDateTimeKey]);

                RequestModel requestModel = new RequestModel(requestMethod, (object)requestData, requestDateTime, tokenHeader);

                //Validate Token
                if (!APIHelper.IsValidToken(requestModel))
                {
                    apiResponseModel = APIHelper.ApiFailure(StringConstants.Message.TokenInvalidSignature, StringConstants.Message.FailureMessage, HttpStatusCode.BadRequest,
                                        requestMethod);
                    return Content(JsonConvert.SerializeObject(apiResponseModel, Formatting.Indented), "application/json; charset=utf-8");
                }

                // Validate Request Model 
                if (!APIHelper.IsValidRequest(requestModel))
                {
                    apiResponseModel = APIHelper.ApiFailure(StringConstants.Message.InvalidRequest, StringConstants.Message.FailureMessage, HttpStatusCode.BadRequest,
                                        requestMethod);
                    return Content(JsonConvert.SerializeObject(apiResponseModel, Formatting.Indented), "application/json; charset=utf-8");
                }

                // Check if Request Data is safe
                if (APIHelper.IsMaliciousRequest(tokenHeader))
                {
                    apiResponseModel = APIHelper.ApiFailure(StringConstants.Message.UnsafeRequest, StringConstants.Message.FailureMessage, HttpStatusCode.BadRequest,
                                        requestMethod);
                    return Content(JsonConvert.SerializeObject(apiResponseModel, Formatting.Indented), "application/json; charset=utf-8");
                }

                // Call Route to Manager to get data
                object responseData = APIHelper.RouteToRequestManager(requestModel);
              
                if (responseData != null)
                {
                    if (responseData.GetType() == typeof(SuccessResponse))
                    {
                        SuccessResponse response = (SuccessResponse)responseData;
                        if (requestModel.IsPostRequest)
                        {
                            apiResponseModel = APIHelper.ApiSuccess(response.Data, requestMethod, response.ResponseMessage, response.ResponseCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponseModel = APIHelper.ApiFailure(ex.Message, StringConstants.Message.Failure, HttpStatusCode.BadRequest, Constants.EmptyString);
            }
            return Content(JsonConvert.SerializeObject(apiResponseModel, Formatting.Indented), "application/json; charset=utf-8");            
        }
    }
}
