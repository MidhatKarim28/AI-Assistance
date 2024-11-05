using AKUSendSMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class SMSHelper
    {
        public List<string> ToNumberList { get; set; }
        public List<string> smsMessageList { get; set; }

        public bool SendSMS(SMSHelper model)
        {
            bool isSent = false;

            for (int index = Constants.ZERO; index < model.ToNumberList.Count; index++)
            {
                SQLDBConnection objConn = new SQLDBConnection();
                objConn.OpenConnection("SMSConnString");
                clsSendSMS objSendSMS = new clsSendSMS();
                objSendSMS.MobileNo = model.ToNumberList[index];
                objSendSMS.SMS = model.smsMessageList[index];
                objSendSMS.SMSType = "CONS";
                objSendSMS.Priority = 1;

                objSendSMS.AKUSendSMS(objConn.dbconn);
                objConn.ReleaseConnection();
                isSent = true;
            }

            return isSent;
        }
    }

  
}
