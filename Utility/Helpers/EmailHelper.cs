//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Mail;

//namespace Utility
//{
//    public class EmailHelper
//    {
//        #region Properties
//        public string EmailFrom { get; set; }
//        public string EmailFromDisplayName { get; set; }
//        public string HostName { get; set; }
//        public List<string> ToEmail { get; set; }
//        public string Message { get; set; }
//        public string Subject { get; set; }
//        public bool AllowHTML { get; set; }
//        public List<string> CCEmail { get; set; }
//        public List<string> BCCEmail { get; set; }

//        public List<string> AttachmentsList { get; set; }
//        #endregion

//        #region Methods
//        public bool SendMail(EmailHelper model)
//        {
//            bool isSent = false;

//            try
//            {
//                var message = new MailMessage();
//                message.From = new MailAddress(EmailFrom, EmailFromDisplayName);

//                foreach(string email in model.ToEmail)
//                {
//                    message.To.Add(new MailAddress(email));
//                }
//                if (model.CCEmail != null)
//                {
//                    foreach (string email in model.CCEmail)
//                    {
//                        message.CC.Add(new MailAddress(email));
//                    }
//                }
//                if (model.BCCEmail != null)
//                {
//                    foreach (string email in model.BCCEmail)
//                    {
//                        message.Bcc.Add(new MailAddress(email));
//                    }
//                }
//                if (UtilityHelper.IsValidList(model.AttachmentsList))
//                {
//                    foreach (var item in model.AttachmentsList)
//                    {
//                        message.Attachments.Add(new Attachment(item));
//                    }
//                }

//                message.Subject = model.Subject;
//                message.Body = model.Message;
//                message.IsBodyHtml = model.AllowHTML;
//                if (message.IsBodyHtml)
//                {
//                  //  message.BodyFormat =
//                }
//                using (var smtp = new SmtpClient(HostName))
//                {
//                    smtp.Send(message);
//                    isSent = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return isSent;
//        }
//        #endregion
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Utility
{
    public class EmailHelper
    {
        #region Properties
        public string EmailFrom { get; set; }
        public string Sender { get; set; }
        public string EmailFromDisplayName { get; set; }
        public string HostName { get; set; }
        public List<string> ToEmail { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public bool AllowHTML { get; set; }
        public List<string> CCEmail { get; set; }
        public List<string> BCCEmail { get; set; }
        #endregion

        #region Methods
        public bool SendMail(EmailHelper model)
        {
            bool isSent = false;

            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(EmailFrom, EmailFromDisplayName);

                foreach (string email in model.ToEmail)
                {
                    message.To.Add(new MailAddress(email));
                }
                if (model.CCEmail != null)
                {
                    foreach (string email in model.CCEmail)
                    {
                        message.CC.Add(new MailAddress(email));
                    }
                }
                if (model.BCCEmail != null)
                {
                    foreach (string email in model.BCCEmail)
                    {
                        message.Bcc.Add(new MailAddress(email));
                    }
                }

                message.Subject = model.Subject;
                message.Body = model.Message;
                message.IsBodyHtml = model.AllowHTML;
                if (message.IsBodyHtml)
                {
                    //  message.BodyFormat =
                }
                using (var smtp = new SmtpClient(HostName))
                {
                    smtp.Send(message);
                    isSent = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSent;
        }
        public bool SendEmail(EmailHelper model)
        {
            bool isSent = false;

            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(EmailFrom, EmailFromDisplayName);


                message.To.Add(new MailAddress(Sender));
                for (int i = 0; i < model.CCEmail.Count; i++)
                {

                    if (!(string.IsNullOrEmpty((model.CCEmail[i]).ToString())))
                    {
                        message.CC.Add(new MailAddress(model.CCEmail[i]));

                    }
                }
                if (model.BCCEmail != null)
                {
                    foreach (string email in model.BCCEmail)
                    {
                        message.Bcc.Add(new MailAddress(email));
                    }
                }

                message.Subject = model.Subject;
                message.Body = model.Message;
                message.IsBodyHtml = model.AllowHTML;

                if (message.IsBodyHtml)
                {
                    //  message.BodyFormat =
                }
                using (var smtp = new SmtpClient(HostName))
                {
                    smtp.Send(message);
                    isSent = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSent;
        }

        public bool SendEmailWithAttachments(EmailHelper model, string attachmentFilename)
        {
            bool isSent = false;

            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(EmailFrom, EmailFromDisplayName);

                foreach (string email in model.ToEmail)
                {
                    message.To.Add(new MailAddress(email));
                }
                if (model.CCEmail != null)
                {
                    foreach (string email in model.CCEmail)
                    {
                        message.CC.Add(new MailAddress(email));
                    }
                }
                if (model.BCCEmail != null)
                {
                    foreach (string email in model.BCCEmail)
                    {
                        message.Bcc.Add(new MailAddress(email));
                    }
                }

                message.Subject = model.Subject;
                message.Body = model.Message;
                message.IsBodyHtml = model.AllowHTML;
                if (message.IsBodyHtml)
                {
                    //  message.BodyFormat =
                }

                Attachment anAttachment = new Attachment(attachmentFilename);
                message.Attachments.Add(anAttachment);

                using (var smtp = new SmtpClient(HostName))
                {
                    smtp.Send(message);
                    isSent = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSent;
        }
        #endregion
    }
}
