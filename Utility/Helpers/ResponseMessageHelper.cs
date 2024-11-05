namespace Utility
{
    public class ResponseMessage
    {
        #region Properties
        public bool Status { get; set; } // Status as boolean 
        public string StrStatus { get; set; } // Status as string 
        public string Title { get; set; } //Custom title  
        public string Message { get; set; } //Custom message  
        public string RecordID { get; set; } // Saved or Updated ID 
        public bool IsValid { get; set; } // Saved or Updated ID 
        #endregion

        #region Constructor 
        public ResponseMessage()
        {
            Status = Constants.FALSE;
            StrStatus = string.Empty;
            Title = string.Empty;
            Message = string.Empty;
            RecordID = string.Empty;
            IsValid = Constants.FALSE;
        }

        public ResponseMessage(bool status, string strStatus, string title, string message, string recordID)
        {
            Status = status;
            StrStatus = strStatus;
            Title = title;
            Message = message;
            RecordID = recordID;
            IsValid = Constants.FALSE;
        }

        #endregion
    }

    public static class ResponseMessageHelper
    {
        public static ResponseMessage SetResponseMessageWithValidity(bool isSaved, bool isValid = false, string recordID = "",
                                                                      string successResponseMessage = "",
                                                                      string failureResponseMessage = "")
        {
            ResponseMessage model = new ResponseMessage();
            string reponseMessage;

            if (isSaved)
            {
                reponseMessage = string.IsNullOrEmpty(successResponseMessage) ? Constants.Default_SaveSuccessMessage : successResponseMessage;
            }
            else
            {
                reponseMessage = string.IsNullOrEmpty(failureResponseMessage) ? Constants.Default_SaveFailureMessage : failureResponseMessage;
            }

            model.Status = isSaved;
            model.StrStatus = isSaved.ToString();
            model.Title = Constants.EmptyString;
            model.RecordID = recordID;
            model.Message = reponseMessage;
            model.IsValid = isValid;

            return model;
        }

        public static ResponseMessage SetResponseMessage(bool isSaved, string recordID = "",
                                                                       string successResponseMessage = "",
                                                                       string failureResponseMessage = "")
        {
            string reponseMessage;

            if (isSaved)
            {
                reponseMessage = string.IsNullOrEmpty(successResponseMessage) ? Constants.Default_SaveSuccessMessage : successResponseMessage;
            }
            else
            {
                reponseMessage = string.IsNullOrEmpty(failureResponseMessage) ? Constants.Default_SaveFailureMessage : failureResponseMessage;
            }

            return new ResponseMessage(isSaved, isSaved.ToString(), string.Empty, reponseMessage, recordID);
        }

        public static ResponseMessage SetResponseTitleAndMessage(bool isSaved, string recordID = "",
                                                                    string successResponseTitle = "",
                                                                    string successResponseMessage = "",
                                                                    string failureResponseTitle = "",
                                                                    string failureResponseMessage = "")
        {
            string reponseTitle;
            string reponseMessage;

            if (isSaved)
            {
                reponseTitle = string.IsNullOrEmpty(successResponseTitle) ? Constants.Default_SaveSuccessTitle : successResponseTitle;
                reponseMessage = string.IsNullOrEmpty(successResponseMessage) ? Constants.Default_SaveSuccessMessage : successResponseMessage;
            }
            else
            {
                reponseTitle = string.IsNullOrEmpty(failureResponseTitle) ? Constants.Default_SaveFailureTitle : failureResponseTitle;
                reponseMessage = string.IsNullOrEmpty(failureResponseMessage) ? Constants.Default_SaveFailureMessage : failureResponseMessage;
            }

            return new ResponseMessage(isSaved, isSaved.ToString(), reponseTitle, reponseMessage, recordID);
        }
    }

}
