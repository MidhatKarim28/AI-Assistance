using APIUtility.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace APIUtility.Models
{
    public static class ModelHelper
    {
        public static string ServiceURL { get; set; }
        public static FileModel ReadFile(string filePath)
        {
            byte[] buffer;
            int length = 0;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }

            return new FileModel(Path.GetFileName(filePath), filePath, length, buffer);
        }

        /// <summary>
        /// Async Method to get the ResponseModel Object from Service based on Requested Method and Requested Data
        /// </summary>
        /// <param name="requestMethod">Request Method e.g IIBManager.GetRequisition </param>
        /// <param name="requestData">Any Object which will use in the Request Method</param>
        /// <param name="tokenHeader">Token to process the request</param>
        /// <param name="uploadedFiles">List of Files to upload into the system</param>
        /// <returns>ResponseModel</returns>
        public static async Task<ResponseModel> GetResponseFromService(string requestMethod, object requestData, string tokenHeader, List<FileModel> uploadedFiles = null)
        {
            string responseBody = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if(string.IsNullOrEmpty(tokenHeader))
                    {
                        ResponseModel responseModel1 = await GetOneTimeCode();
                        if (responseModel1.ResponseCode == HttpStatusCode.OK)
                        {
                            ResponseMessage code = responseModel1.ParseData<ResponseMessage>();
                            tokenHeader = code.RecordID;
                        }
                    }

                    client.Timeout = TimeSpan.FromMinutes(30);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("_token", tokenHeader);

                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(DateTime.Now.ToString(UtilityHelper.DATETIME_FORMAT_ddMMyyyyHHmmss)), "RequestDateTime");
                    content.Add(new StringContent(requestMethod), "RequestMethod");
                    content.Add(new StringContent(JsonConvert.SerializeObject(requestData).ToString(), Encoding.UTF8, "application/json"), "RequestData");

                    if (uploadedFiles != null && uploadedFiles.Count > Utility.Constants.ZERO )
                    {
                        foreach (FileModel fm in uploadedFiles)
                        {
                            if (fm.Content != null)
                            {
                                var fileContent = new ByteArrayContent(fm.Content);
                                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(StringConstants.General.ContentTypeAttachment)
                                {
                                    FileName = fm.FileName
                                };
                                content.Add(fileContent);
                            }
                        }
                    }    

                    HttpResponseMessage response = await client.PostAsync(ServiceURL, content);
                    //response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }

                ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseBody);
                var res = responseModel.ResponseResult; 

                return responseModel;
            }
            catch (Exception e)
            {
                return new ResponseModel(e.Message, System.Net.HttpStatusCode.BadRequest);
            }
        }
        private static async Task<ResponseModel> GetOneTimeCode()
        {
            string responseBody = "";
            object requestData = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("_token", "");

                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(DateTime.Now.ToString(UtilityHelper.DATETIME_FORMAT_ddMMyyyyHHmmss)), "RequestDateTime");
                    content.Add(new StringContent("SharedManager.GetOneTimeCode"), "RequestMethod");
                    // TODO - Remove these temp vars
                    var s = JsonConvert.SerializeObject(requestData).ToString();
                    var temp = new StringContent(JsonConvert.SerializeObject(requestData).ToString(), Encoding.UTF8, "application/json");
                    content.Add(new StringContent(JsonConvert.SerializeObject(requestData).ToString(), Encoding.UTF8, "application/json"), "RequestData");                    

                    HttpResponseMessage response = await client.PostAsync(ServiceURL, content);
                    //response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }

                ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseBody);
                var res = responseModel.ResponseResult;

                return responseModel;
            }
            catch (Exception e)
            {
                return new ResponseModel(e.Message, System.Net.HttpStatusCode.BadRequest);
            }
        }
        /// <summary>
        /// Generic Method which parse the Response Result into your given object
        /// </summary>
        /// <typeparam name="T">Any object or List of Objects</typeparam>
        /// <param name="responseModel"></param>
        /// <returns>Return the Object or List of Objects</returns>
        public static T ParseData<T>(this ResponseModel responseModel)
        {
            object parsedValue = default(T);
            try
            {
                if (responseModel.ResponseCode == HttpStatusCode.OK)
                {
                    parsedValue = Convert.ChangeType(JsonConvert.DeserializeObject<T>(responseModel.ResponseResult.ToString()), typeof(T));
                }
            }
            catch (Exception)
            {
                //parsedValue = null;
            }

            return (T)parsedValue;
        }

        public static List<T> ParseDataToList<T>(this ResponseModel responseModel)
        {
            List<T> parsedList = new List<T>();
            object parsedValue = default(T);
            try
            {
                if (responseModel.ResponseCode == HttpStatusCode.OK)
                {
                    parsedList = JsonConvert.DeserializeObject<List<T>>(responseModel.ResponseResult.ToString());
                }
            }
            catch (Exception)
            {
                //parsedValue = null;
            }

            return (List<T>)parsedList;
        }
    }
}
