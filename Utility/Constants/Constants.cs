using System.ComponentModel;

namespace Utility
{
    public static class Constants
    {
        #region Date Constants
        // DateFormats 
        public static string DATE_FORMAT_DISPLAY = "MMM dd, yyyy";
        public static string DATE_FORMAT_MONTH_YEAR = "MMMM yyyy";
        public static string DATE_FORMAT_COMPLETE = "MMMM dd yyyy";
        public static string DATE_FORMAT_YEAR = "yyyy";
        public static string DATE_FORMAT_MONTH = "MMMM";
        public static string DATETIME_FORMAT_DISPLAY = "MMM dd, yyyy HH:mm";
        public const string DATE_FORMAT = "dd/MM/yyyy";
        public const string DATETIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
        public const string DATE_FORMAT_ORACLE = "To_date('{0}','dd/mm/yyyy hh24:mi:ss')";
        public const string DATETIME_FORMAT_CYBERSOURCE = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        public static string DATETIME_FORMAT_FOR_Physcian = "ddd, MMM dd, yyyy";
        public static string TIME_FORMAT_FOR_CLINICS = "HH:mm tt";
        public static string TIME_FORMAT_FOR_CLINICS_24Hour = "HH:mm";
        // return Current DateTime
        public static string strCurrentDate { get { return System.DateTime.Now.ToString(DATE_FORMAT); } }
        public static string strCurrentDateTime { get { return System.DateTime.Now.ToString(DATETIME_FORMAT); } }
        public static string strCurrentUniversalDateTimeCYBERSOURCE { get { return System.DateTime.Now.ToUniversalTime().ToString(DATETIME_FORMAT_CYBERSOURCE); } }
        #endregion

        #region Integer Constants
        public const int ZERO = 0;
        public const string strZERO = "0";
        public const int ONE = 1;
        public const string strONE = "1";
        public const string USDValue = "150";
        #endregion

        #region Boolean Constants 
        public const bool TRUE = true;
        public const string strTRUE = "true";
        public const bool FALSE = false;
        public const string strFALSE = "false";
        #endregion

        #region Enums Constants 

        public enum enSignInStatus
        {
            Success = 0, //     Sign in was successful
            LockedOut = 1,  //     User is locked out
            AccessDenied = 2, //    User does not have rights
            Failure = 3, //     Sign in failed
        }
        public enum enCyberSourceReasonCodes
        {
            [Description("Successful transaction")]
            SOK100 = 100,
            [Description("Declined - The request is missing one or more fields")]
            DMISSINGFIELD101 = 101,
            [Description("Declined - One or more fields in the request contains invalid data")]
            DINVALIDDATA102 = 102,
            [Description("Declined - ThemerchantReferenceCode sent with this authorization request matches the merchantReferenceCode of another authorization request that you sent in the last 15 minutes.")]
            DDUPLICATE104 = 104,
            [Description("Partial amount was approved")]
            SPARTIALAPPROVA110L = 110,
            [Description("Error - General system failure.")]
            ESYSTEM150 = 150,
            [Description("Error - The request was received but there was a server timeout. This error does not include timeouts between the client and the server.")]
            ETIMEOUT151 = 151,
            [Description("Soft Decline - The authorization request was approved by the issuing bank but declined by CyberSource because it did not pass the Address Verification Service (AVS) check.")]
            DAVSNO200 = 200,
            [Description("Decline - The issuing bank has questions about the request. You do not receive an authorization code programmatically, but you might receive one verbally by calling the processor.")]
            DCALL201 = 201,
            [Description("Decline - Expired card. You might also receive this if the expiration date you provided does not match the date the issuing bank has on file.")]
            DCARDEXPIRED202 = 202,
            [Description("Decline - General decline of the card. No other information provided by the issuing bank.")]
            DCARDREFUSED203 = 203,
            [Description("Decline - Insufficient funds in the account.")]
            DCARDREFUSED204 = 204,
            [Description("Decline - Issuing bank unavailable.")]
            DCARDREFUSED207 = 207,
            [Description("Decline - Inactive card or card not authorized for card-not-present transactions.")]
            DCARDREFUSED208 = 208,
            [Description("Decline - American Express Card Identification Digits (CID) did not match.")]
            DCARDREFUSED209 = 209,
            [Description("Decline - The card has reached the credit limit.")]
            DCARDREFUSED210 = 210,
            [Description("Decline - Invalid Card Verification Number (CVN).")]
            DCARDREFUSED211 = 211,
            [Description("Decline - Generic Decline.")]
            DCHECKREFUSED220 = 220,
            [Description("Decline - The customer matched an entry on the processor's negative file.")]
            DCHECKREFUSED221 = 221,
            [Description("Decline - customer's account is frozen")]
            DCHECKREFUSED222 = 222,
            [Description("Soft Decline - The authorization request was approved by the issuing bank but declined by CyberSource because it did not pass the card verification number (CVN) check.")]
            DCV230 = 230,
            [Description("Decline - Invalid account number")]
            DINVALIDCARD231 = 231,
            [Description("Decline - The card type is not accepted by the payment processor.")]
            DINVALIDDATA232 = 232,
            [Description("Decline - General decline by the processor.")]
            DINVALIDDATA233 = 233,
            [Description("Decline - There is a problem with your CyberSource merchant configuration.")]
            DINVALIDDATA234 = 234,
            [Description("Decline - The requested amount exceeds the originally authorized amount. Occurs, for example, if you try to capture an amount larger than the original authorization amount.")]
            DINVALIDDATA235 = 235,
            [Description("Decline - Processor failure.")]
            DINVALIDDATA236 = 236,
            [Description("Decline - The authorization has already been reversed.")]
            DINVALIDDATA237 = 237,
            [Description("Decline - The transaction has already been settled.")]
            DINVALIDDATA238 = 238,
            [Description("Decline - The requested transaction amount must match the previous transaction amount.")]
            DINVALIDDATA239 = 239,
            [Description("Decline - The card type sent is invalid or does not correlate with the credit card number.")]
            DINVALIDDATA240 = 240,
            [Description("Decline - The referenced request id is invalid for all follow-on transactions.")]
            DINVALIDDATA241 = 241,
            [Description("Decline - The request ID is invalid.")]
            DNOAUTH242 = 242,
            [Description("Decline - The transaction has already been settled or reversed.")]
            DINVALIDDATA243 = 243,
            [Description("Decline - The capture or credit is not voidable because the capture or credit information has already been submitted to your processor. Or, you requested a void for a type of transaction that cannot be voided.")]
            DNOTVOIDABLE246 = 246,
            [Description("Decline - You requested a credit for a capture that was previously voided.")]
            DINVALIDDATA247 = 247,
            [Description("Decline - The boleto request was declined by your processor.")]
            DBOLETODECLINED248 = 248,
            [Description("Error - The request was received, but there was a timeout at the payment processor.")]
            ETIMEOUT250 = 250,
            [Description("Decline - The Pinless Debit card's use frequency or maximum amount per use has been exceeded.")]
            DCARDREFUSED251 = 251,
            [Description("Decline - Account is prohibited from processing stand-alone refunds.")]
            DINVALIDDATA254 = 254,
            [Description("Soft Decline - Fraud score exceeds threshold.")]
            DSCORE400 = 400,
            [Description("Apartment number missing or not found.")]
            DINVALIDADDRESS450 = 450,
            [Description("Insufficient address information.")]
            DINVALIDADDRESS451 = 451,
            [Description("House/Box number not found on street.")]
            DINVALIDADDRESS452 = 452,
            [Description("Multiple address matches were found.")]
            DINVALIDADDRESS453 = 453,
            [Description("P.O. Box identifier not found or out of range.")]
            DINVALIDADDRESS454 = 454,
            [Description("Route service identifier not found or out of range.")]
            DINVALIDADDRESS455 = 455,
            [Description("Street name not found in Postal code.")]
            DINVALIDADDRESS456 = 456,
            [Description("Postal code not found in database")]
            DINVALIDADDRESS457 = 457,
            [Description("Unable to verify or correct address.")]
            DINVALIDADDRESS458 = 458,
            [Description("Multiple addres matches were found (international)")]
            DINVALIDADDRESS459 = 459,
            [Description("Address match not found (no reason given)")]
            DINVALIDADDRESS460 = 460,
            [Description("Unsupported character set")]
            DINVALIDADDRESS461 = 461,
            [Description("The cardholder is enrolled in Payer Authentication. Please authenticate the cardholder before continuing with the transaction.")]
            DAUTHENTICATE475 = 475,
            [Description("Encountered a Payer Authentication problem. Payer could not be authenticated.")]
            DAUTHENTICATIONFAILED476 = 476,
            [Description("The order is marked for review by Decision Manager")]
            DREVIEW480 = 480,
            [Description("The order has been rejected by Decision Manager")]
            DREJECT481 = 481,
            [Description("Soft Decline - The authorization request was approved by the issuing bank but declined by CyberSource based on your Smart Authorization settings.")]
            DSETTINGS520 = 520,
            [Description("The customer matched the Denied Parties List")]
            DRESTRICTED700 = 700,
            [Description("Export bill_country/ship_country match")]
            DRESTRICTED701 = 701,
            [Description("Export email_country match")]
            DRESTRICTED702 = 702,
            [Description("Export hostname_country/ip_country match")]
            DRESTRICTED703 = 703
        }
        public enum enYesNoDesc
        {
            YES,
            NO
        }

        public enum enActive
        {
            Y,
            N
        }

        public enum enHISApps
        {
            MYPATIENTS
        }

        public enum enTokenNature
        {
            Y,
            N,
            D,
            E
        }
        #endregion

        #region string
        public const string Unknown = "U";
        #endregion

        #region String Constants

        public const string NullValue = null;
        public const string EmptyString = "";
        public const string NotApplicable = "N/A";

        public const string Default_SaveSuccessTitle = "Saved";
        public const string Default_SaveFailureTitle = "Failed";

        public const string Default_SaveSuccessMessage = "Record has been saved successfully.";
        public const string Default_SaveFailureMessage = "Something went wrong. Please try again.";

        public const string Default_NoRecordFoundMessage = "Record Not Found.";
        public const string Default_NoReportFoundMessage = "No report found.";

        #endregion

        #region Char Constants
        public const char DotCharacter = '.';
        #endregion

     

        public enum enVBStatusConstants
        {
            DIRTY_RECORD = 0,
            INCOMPLETE_RECORD = 1,
            CLEAN_RECORD = 2,
            RECORD_FOUND = 3,
            NO_RECORD_FOUND = 4,
            RECORD_SAVED = 5,
            DATABASE_ERROR = 6,
            LOGICAL_ERROR = 7,
            EXPORT_FAILED = 8,
            EXPORT_SUCCESS = 9,
            EXPORT_UNKNOWN = 10,
            VBCONN_ERROR = 11,
            PARAM_ERROR = 12
        }
    }

    public static class enCyberSourceReasonCodesExtensions
    {
        public static string ToDescriptionString(this Constants.enCyberSourceReasonCodes val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
