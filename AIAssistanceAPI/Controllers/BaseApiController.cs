using APIUtility.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AIAssistanceAPI.Controllers
{  
    public class BaseApiController : ControllerBase
    {
        //protected object ApiSuccess(object result,string responseMethod,string message,HttpStatusCode responseCode)
        //{
        //    return new ResponseModel(message, responseCode, responseMethod, result);
        //}

        //protected object ApiFailure(object result,  string message, HttpStatusCode responseCode, string methodType)
        //{
        //    return new ResponseModel(message, responseCode, methodType, (string)result);
        //}
    }
}
