using APIUtility.Constants;
using APIUtility.Helpers;
using APIUtility.Models;
using APIUtility.TokenHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenAI.Models;
using System.Net;
using Utility;

namespace AIAssistanceAPI
{
    public class APIHelper
    {
        //private static readonly IAppLocalSetting? _appSharedSetting

        private readonly IAppLocalSetting _appLocalSetting;
        //private readonly IAppGlobalSetting _appGlobalSetting;
        //public APIHelper() { }
        public APIHelper(IAppLocalSetting appLocalSetting)
        {
            _appLocalSetting = appLocalSetting;
            //_appGlobalSetting = appGlobalSetting;
        }

        internal string TestAppSetting()
        {
            var test = _appLocalSetting.ApplicationEnvironment;         
            return test;
        }

        internal static bool IsValidRequest(RequestModel requestModel)
        {
            return requestModel != null && !string.IsNullOrEmpty(requestModel.RequestMethod) && !string.IsNullOrEmpty(requestModel.RequestMethod);
        }

        internal static bool IsValidToken(RequestModel request)
        {
            bool flag = false;
            if (request.RequestMethod == StringConstants.RequestMethod.GetOneTimeCode || request.RequestMethod == StringConstants.RequestMethod.GetAIToken)
            {
                flag = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(request.TokenHeader))
                {
                    object jsonJWT = DBManagerUtility.decodeJWT(request.TokenHeader);
                    if (jsonJWT.GetType() != typeof(ErrorResponse))
                        flag = ValidateToken(jsonJWT, request.RequestData, request.TokenHeader, request.RequestMethod);
                }                
            }            
            return flag;
        }

        internal static bool ValidateToken(object jsonJWT, object requestData, string token, string requestMethod)
        {
            try
            {                
                if (ApplicationSettings.Instance.AppLocalSetting.ValidateToken)
                {
                    JWTPayload jwtPayload = (JWTPayload)jsonJWT;
                    if (string.IsNullOrEmpty(jwtPayload.TokenNature) || string.IsNullOrEmpty(jwtPayload.ApplicationID) || string.IsNullOrEmpty(jwtPayload.AccessDateTime) || jwtPayload.ApplicationID != ApplicationSettings.Instance.AppLocalSetting.ApplicationID)
                        return false;
                    if (ServiceLists.Instance.IsPatientService(requestMethod)) //Patient Services is defined for validation of MR Number
                    {
                        if (string.IsNullOrEmpty(jwtPayload.DeviceID) || string.IsNullOrEmpty(jwtPayload.DeviceOS) || string.IsNullOrEmpty(jwtPayload.UserID) || !RegisteredTokenManager.GetRegisteredTokenDetail(jwtPayload.DeviceID, jwtPayload.UserID, token))
                            return false;
                        if (jwtPayload.TokenNature == Utility.Constants.enTokenNature.D.ToString() || !APIHelper.IsValidJson(requestData.ToString()))
                            return true;

                        //MR Number validation should be added as per application requirements
                        JWTPayloadValidationModel jWTPayloadValidationModel = JsonConvert.DeserializeObject<JWTPayloadValidationModel>(Convert.ToString(requestData));
                    }
                    else if (ServiceLists.Instance.IsExceptionalService(requestMethod))
                    {
                        if (jwtPayload.TokenNature != Utility.Constants.enTokenNature.E.ToString())
                            return false;

                        if (!ApplicationSettings.Instance.AppLocalSetting.UserList.ContainsKey(jwtPayload.UserID))
                        return false;

                        if (ApplicationSettings.Instance.AppLocalSetting.UserList[jwtPayload.UserID] != jwtPayload.Password)
                            return false;

                        CredentialModel credentialModel = JsonConvert.DeserializeObject<CredentialModel>(Convert.ToString(requestData));
                        
                        if (credentialModel == null)
                            return false;

                        if (credentialModel.UserID != jwtPayload.UserID || credentialModel.Password != jwtPayload.Password)
                            return false;

                        //if (new DateTime(Convert.ToInt64(jwtPayload.AccessDateTime)).AddHours(jwtPayload.TimeDurationInHours) < DateTime.Now)
                        //    return false;
                    }
                    else if (jwtPayload.TokenNature == Utility.Constants.enTokenNature.N.ToString())
                    {
                        if (new DateTime(Convert.ToInt64(jwtPayload.AccessDateTime)).AddMinutes(5.0) < DateTime.Now)
                            return false;
                    }
                    else if (string.IsNullOrEmpty(jwtPayload.DeviceID) || string.IsNullOrEmpty(jwtPayload.DeviceOS) || string.IsNullOrEmpty(jwtPayload.UserID) || !RegisteredTokenManager.GetRegisteredTokenDetail(jwtPayload.DeviceID, jwtPayload.UserID, token))
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((!strInput.StartsWith("{") || !strInput.EndsWith("}")) && (!strInput.StartsWith("[") || !strInput.EndsWith("]")))
                return false;
            try
            {
                JsonConvert.DeserializeObject<JWTPayloadValidationModel>(strInput);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal static bool IsMaliciousRequest(string requestText)
        {
            try
            {
                requestText = requestText.Replace(" ", "").ToUpper();
                foreach (string str in new List<string>()
                        {
                          "HTML",
                          "<SCRIPT",
                          "'--",
                          ";--",
                          ";#",
                          "/*",
                          "1=1",
                          "DBMS_LOCK",
                          "UNIONALL",
                          "INSERTINTO",
                          "VALUES(",
                          "*FROM",
                          "DATABASE",
                          "ALERT(",
                          "ONLOAD="
                        })
                {
                    if (requestText.Contains(str))
                        return true;
                }
            }
            catch (Exception ex)
            {
                Logger._log.Error(ex.Message);
            }
            return false;
        }

        internal static object RouteToRequestManager(RequestModel request)
        {
            try
            {
                string apiProjectName = GetAPIProjectName(request.RequestMethod.Split('.').First());
                string name = (request.RequestMethod.Split('.')).Last();
                Type? typeProject = Type.GetType(string.Format("{0}.{1}, {0}", apiProjectName, "APIManagerDirectory"));
                if (typeProject == null)
                {
                    ErrorResponse errorResponse1 = new ErrorResponse("Proejct is missing", HttpStatusCode.BadRequest);
                    return errorResponse1;
                }
                string? str = typeProject.GetProperty(name).GetValue(null).ToString();
                Type? typeClass = Type.GetType(string.Format("{0}.{1}, {0}", apiProjectName, str));
                if (typeClass == null)
                {
                    ErrorResponse errorResponse2 = new ErrorResponse("Class is missing", HttpStatusCode.BadRequest);
                    return errorResponse2;
                }
                return typeClass.GetMethod(name).Invoke(Activator.CreateInstance(typeClass), new object[1] { request });
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static string GetAPIProjectName(string managerName) => string.Format("{0}.DBManager", managerName.Split("Manager")[0], "Manager");


        internal static ResponseModel ApiSuccess(object result, string responseMethod, string message, HttpStatusCode responseCode)
        {
            return new ResponseModel(message, responseCode, responseMethod, result);
        }

        internal static ResponseModel ApiFailure(object result, string message, HttpStatusCode responseCode, string methodType)
        {
            return new ResponseModel(message, responseCode, methodType, (string)result);
        }
    }
}
