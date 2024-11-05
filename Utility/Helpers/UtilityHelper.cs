using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Net.Http;
using System.Drawing;
using System.Net.Http.Headers;
using System.Drawing.Imaging;
using System.Net;
using System.Xml;
using Utility;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Utility
{
    public class UtilityHelper
    {
        public static string DATETIME_FORMAT_ddMMyyyyHHmm = "dd/MM/yyyy HH:mm";
        public static string DATETIME_FORMAT_ddMMyyyyHHmmss = "dd/MM/yyyy HH:mm:ss";
        public static string DATETIME_FORMAT_ddMMyyyy = "dd/MM/yyyy";
        public static string DATETIME_FORMAT_ddMMMyyyy = "dd MMM yyyy";
        public static string DATETIME_FORMAT_HHmm = "HH:mm";
        public static string DATETIME_FORMAT_ddMMMyyyyHHmm = "dd MMM yyyy HH:mm";

        public static string INTEGER_FORMAT = "N0";
        public static string DECIMAL_FORMAT_TWO = "N2";

        private MarkupConverter.MarkupConverter markupConverter = new MarkupConverter.MarkupConverter();

        public static string IsValidString(object obj)
        {
            if (obj is DBNull || obj == null)
                return "";
            else
                return obj.ToString();
        }
        public static DateTime? IsValidDateTime(object obj)
        {
            if (obj is DBNull || obj == null)
                return null;
            else
                return Convert.ToDateTime(obj.ToString());
        }
        public static long IsValidLong(object obj)
        {
            if (obj is DBNull || obj == null)
                return 0;
            else
                return Convert.ToInt64(obj.ToString());
        }
        public static double IsValidDouble(object obj)
        {
            if (obj is DBNull || obj == null)
                return 0;
            else
                return Convert.ToDouble(obj.ToString());
        }
        public static int IsValidInt(object obj)
        {
            if (obj is DBNull || obj == null)
                return 0;
            else
                return Convert.ToInt32(obj.ToString());
        }

        public static string GetTimeFormat(TimeSpan ts)
        {
            string format = "";
            //if(ts.Days != 0)
            //{
            //    if (ts.Days == 1 || ts.Days == -1)
            //        format += ts.Days + "<sub>day</sub> ";
            //    else
            //        format += ts.Days + "<sub>days</sub> ";
            //}
            if (ts.Hours != 0 || ts.Days != 0)
            {
                //if (ts.Hours == 1 || ts.Hours == -1)
                format += Math.Truncate(ts.TotalHours) + "<sub>hr</sub> ";

                //else
                //    format += ts.Hours + "<sub>hrs</sub> ";
            }
            if (ts.Minutes != 0)
            {
                //if (ts.Minutes == 1 || ts.Minutes == -1)
                format += ts.Minutes + "<sub>min</sub> ";
                //else
                //    format += ts.Minutes + "<sub>mins</sub> ";
            }

            if (format == "")
                format = "0<sub>min</sub>";

            return format;

        }

        public static string GetTimeStringfromTimeSpan(TimeSpan ts)
        {
            string time = "";
        
            if (ts.Hours != 0 || ts.Days != 0)
            {
      
                time += Math.Truncate(ts.TotalHours) + " h ";

                
            }
            if (ts.Minutes != 0)
            { 
                time += ts.Minutes + " m";

            }

            if (time == "")
                time = "0 min";

            return time;

        }

        public static string CalculateHoursAndMinutesByDecimalTime(string input)
        {
            double erlos, iHours, iMinutes = 0;
            string result = string.Empty;
            if (!string.IsNullOrEmpty(input))
            {

                erlos = Convert.ToDouble(input);

                if (erlos < 0)
                {
                    result = erlos.ToString();
                }
                else
                {
                    input = Math.Round(erlos, 2).ToString();
                    string[] arr = input.Split('.');

                    // Round Off values 
                    for (int i = 0; i < arr.Length; i++)
                    {
                        double temp = Convert.ToDouble(arr[i].ToString());
                        temp = Math.Round(temp, 2);
                        arr[i] = temp.ToString();
                    }

                    if (arr.GetUpperBound(0) > Constants.ZERO)
                    {
                        iHours = Convert.ToDouble(arr[0]);
                        arr[1] = "0." + arr[1];

                        iMinutes = Math.Round((Convert.ToDouble(arr[1])) * 60, 0);
                    }

                    else
                    {
                        iHours = Convert.ToDouble(arr[0]);
                        iMinutes = 0;
                    }
                    result = result + UtilityHelper.Pad2(iHours) + ":" + UtilityHelper.Pad2(iMinutes);
                }
            }
            return result;
        }

        public static string Pad2(double input)
        {
            if (input > 9)
            {
                return input.ToString();
            }
            else
            {
                return "0" + input;
            }
        }

        public static string GetEnumDescription(object enumValue, string defDesc = "")
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return defDesc;
        }

        public static string ReplaceLastOccurrenceOfString(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        public static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static string GetNameProperCase(string firstName, string MiddleName, string lastName)
        {
            MiddleName = string.Empty;
            firstName = FirstCharToUpper(firstName) + " ";
            lastName = FirstCharToUpper(lastName);
            if (!string.IsNullOrEmpty(MiddleName))
            {
                MiddleName = FirstCharToUpper(MiddleName) + " ";
            }
            return firstName + MiddleName + lastName;
        }
        
        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
        }

        public static string GetProperCase(string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;

            string[] words = input.Split(',', ' ');

            if (words.Length == 1)
            {
                input = FirstCharToUpper(input);
            }

            else if (words.Length > 1)
            {
                for (int index = 0; index < words.Length; index++)
                {
                    words[index] = FirstCharToUpper(words[index]);
                }
                input = String.Join(" ", words);
            }

            return input;
        }

        public static string GetDefaultText(object text)
        {
            return IsValidString(text) == "" ? Constants.NotApplicable : UtilityHelper.IsValidString(text);
        }

        // Mark: Check whether dataset has datatable or not 
        public static bool IsDataSetValid(DataSet dataset)
        {
            bool isValid = false;
            if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
            {
                isValid = true;
            }
            return isValid;
        }

        // Mark : Return formatted date
        public static string FormatDate(object inputDate, string format = "")
        {
            string date = IsValidString(inputDate);
            // Set Default Date Format if no format given
            if (string.IsNullOrEmpty(format))
            {
                format = Constants.DATE_FORMAT_DISPLAY;
            }
            return (date != string.Empty) ? Convert.ToDateTime(date).ToString(format) : "";
        }

        // Mark : Return formatted datetime
        public static string FormatDateAndTime(object inputDate, string format = "")
        {
            string date = IsValidString(inputDate);
            // Set Default DateTime Format if no format given
            if (string.IsNullOrEmpty(format))
            {
                format = Constants.DATETIME_FORMAT_DISPLAY;
            }
            return (date != string.Empty) ? Convert.ToDateTime(date).ToString(format) : "";
        }

        // Mark : Return formatted date
        public static string FormatDateDB(object inputDate, string format = "")
        {
            string date = IsValidString(inputDate);
            // Set Default Date Format if no format given
            if (string.IsNullOrEmpty(format))
            {
                format = Constants.DATE_FORMAT;
            }
            return (date != string.Empty) ? Convert.ToDateTime(date).ToString(format) : "";
        }

        // Mark : Return formatted datetime
        public static string FormatDateAndTimeDB(object inputDate, string format = "")
        {
            string date = IsValidString(inputDate);
            // Set Default DateTime Format if no format given
            if (string.IsNullOrEmpty(format))
            {
                format = Constants.DATETIME_FORMAT;
            }
            return (date != string.Empty) ? Convert.ToDateTime(date).ToString(format) : "";
        }

        public static string AddYearsToCurrentDate(int years)
        {
            return DateTime.Now.AddYears(years).ToString(Constants.DATE_FORMAT);
        }

        public static string AddYearsToCurrentDateTime(int years)
        {
            return DateTime.Now.AddYears(years).ToString(Constants.DATETIME_FORMAT);
        }
        
        // Mark : Check if list has items or not 
        public static bool IsValidList<T>(IEnumerable<T> list)
        {
            bool isValid = false;
            if (list != null )
            {
                if(list.ToList().Count > 0)
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        public static bool IsValidObject<T>(T obj)
        {
            bool isValid = false;
            if (obj != null)
            {   
               isValid = true;             
            }
            return isValid;
        }


        public static bool IsValidIENumerableList<T>(IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        // Mark : Check if list has items or not 
        public static T GetFirstFromList<T>(IEnumerable<T> list)
        {
            T model = default(T) ;
            if (list != null )
            {
                var tempList = list.ToList();
                if(IsValidList(tempList))
                {
                    model = tempList.FirstOrDefault();
                }
            }
            return model;
        }


        // Mark : Check and return unique file nanme if exists
        public static string GetUniqueFileName(string filepath, bool splitName=true)
        {

            string filename = string.Empty;
            //string temppath = System.Web.Hosting.HostingEnvironment.MapPath(filepath);
            //string temppath2 = filepath.Replace("\\\\", "\\");
            //temppath2 = "\\" + temppath2;
            string folder = Path.GetDirectoryName(filepath);
            string tempfilename = Path.GetFileNameWithoutExtension(filepath);

            // Validate length 
            if (splitName)
            {
                //int tempfilenameLength = tempfilename.Length;

                //if (tempfilenameLength > 5)
                //{
                //    tempfilename = tempfilename.Trim().Substring(0, 5);
                //}
            }
        
            string extension = Path.GetExtension(filepath);

            if (File.Exists(filepath))
            {
                int number = 0;
                Match regex = Regex.Match(filepath, @"(.+) \((\d+)\)\.\w+");
                if (regex.Success)
                {
                    tempfilename = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }
                do
                {
                    number++;
                    filename = string.Format("{0}_{1}{2}", tempfilename, number, extension);
                }
                while (File.Exists(Path.Combine(folder, filename)));
            }
            else
            {
                filename = string.Format("{0}{1}", tempfilename, extension);
            }

            return filename;
        }

        // Mark : Check if content type of is valid mime
        public static bool IsValidMimeType(string contentType)
        {
            bool isValid = true;
            contentType = contentType.ToLower();

            if (contentType != "image/jpg" && contentType != "image/jpeg" &&
                    contentType != "image/pjpeg" && contentType != "image/gif" &&
                        contentType != "image/x-png" && contentType != "image/png")
            {
                isValid = false;
            }            
            return isValid;
        }

        public static int GenerateRandomNumber()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        public static int GenerateRandomNumberSixDigit()
        {
            int _min = 100000;
            int _max = 999999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static List<string> GetMultipleConStrings(string file_path)
        {
            //TODO
            //string fi = HttpContextHelper.Current.Server.MapPath(file_path);
            string fi = "";
            StreamReader file = new StreamReader(fi);
            string line;
            List<string> lines = new List<string>();
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines;
        }
       
        // Mark: Make Select List from Dataset
        public static List<SelectListItem> MakeListFromDataSet(DataSet dataset, string IDColName, string DescriptionColName)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (IsDataSetValid(dataset))
            {
                list = (from DataRow dr in dataset.Tables[Constants.ZERO].Rows
                        select new SelectListItem()
                        {
                            Value = IsValidString(dr[IDColName]),
                            Text = IsValidString(dr[DescriptionColName])
                        }).ToList();
            }
            return list;
        }

        //// Mark: Get Default Text
        //public static List<SelectListItem> MakeListFromDataSet(DataSet dataset, string IDColName, string DescriptionColName)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    if (IsDataSetValid(dataset))
        //    {
        //        list = (from DataRow dr in dataset.Tables[Constants.ZERO].Rows
        //                select new SelectListItem()
        //                {
        //                    Value = UtilityHelper.IsValidString(dr[IDColName]),
        //                    Text = UtilityHelper.IsValidString(dr[DescriptionColName])
        //                }).ToList();
        //    }
        //    return list;
        //}

        // Mark : Send Response 
        public static string SendMessageResponseString(bool isSaved, string successResponseMessage = "", string failureResponseMessage = "")
        {
            string result;
            string reponseMessage;
            string successMessage = "Record has been saved successfully.";
            string failureMessage = "Something went wrong. Please try again.";

            if (isSaved)
            {
                reponseMessage = successMessage;
                if (!string.IsNullOrEmpty(successResponseMessage))
                {
                    reponseMessage = successResponseMessage;
                }
            }
            else
            {
                reponseMessage = failureMessage;
                if (!string.IsNullOrEmpty(failureResponseMessage))
                {
                    reponseMessage = failureResponseMessage;
                }
            }
            result = isSaved.ToString() + "-" + reponseMessage;
            return result;
        }
        #region :: RTF to HTML conversion ::

        public string ConvertRtfToHtml(string rtfText)
        {
            var thread = new Thread(ConvertRtfInSTAThread);
            var threadData = new ConvertRtfThreadData { RtfText = rtfText };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(threadData);
            thread.Join();
            return threadData.HtmlText;
        }

        private void ConvertRtfInSTAThread(object rtf)
        {
            var threadData = rtf as ConvertRtfThreadData;
            threadData.HtmlText = markupConverter.ConvertRtfToHtml(threadData.RtfText);
        }

        private class ConvertRtfThreadData
        {
            public string RtfText { get; set; }
            public string HtmlText { get; set; }
        }

        #endregion

        public static DateTime GetDateForComparison(string inputDate)
        {
            DateTime comparisonDate = new DateTime();
            if (!string.IsNullOrEmpty(inputDate))
            {
                comparisonDate = DateTime.ParseExact(inputDate, Constants.DATETIME_FORMAT, null);
            }
            return comparisonDate;
        }

        public static bool IsTodaysDate(string inputDate)
        {
            bool isTodaysDate = Constants.FALSE;
            if (!string.IsNullOrEmpty(inputDate))
            {
                DateTime comparisonDate = new DateTime();
                comparisonDate = DateTime.ParseExact(inputDate, Constants.DATETIME_FORMAT, null);
                if(comparisonDate.Date  == DateTime.Now.Date)
                {
                    isTodaysDate = Constants.TRUE; 
                }
            }
            return isTodaysDate;
        }

        public static string ParseDisplayDBDate(string date)
        {
            try
            {
               return  DateTime.ParseExact(date, DATETIME_FORMAT_ddMMyyyy, CultureInfo.InvariantCulture).ToString(Constants.DATE_FORMAT_DISPLAY);
            }
            catch(Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return ""; 
        }

        public static string ParseDisplayDBDateTime(string date)
        {
            try
            {
                return DateTime.ParseExact(date, DATETIME_FORMAT_ddMMyyyyHHmmss, CultureInfo.InvariantCulture).ToString(Constants.DATETIME_FORMAT_DISPLAY);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return "";
        }

        public static DateTime? ParseToDBDateTime(string date)
        {
            try
            {
                return DateTime.ParseExact(date, DATETIME_FORMAT_ddMMyyyyHHmmss, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return null;
        }

        public static object GetImage(string filePath)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.BadRequest);
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Bitmap bmp = new Bitmap((Stream)fs);
                Stream streamContent = new MemoryStream();
                bmp.Save(streamContent, ImageFormat.Jpeg);
                streamContent.Seek(0, SeekOrigin.Begin);

                MediaTypeHeaderValue header = new MediaTypeHeaderValue("image/jpeg");
                result.StatusCode = HttpStatusCode.OK;
                result.Content = new StreamContent(streamContent);
                result.Content.Headers.ContentType = header;
            }
            return result;
        }

        public static int CalculateAge(string birthdate)
        {
            DateTime dtBirthDate = DateTime.ParseExact(birthdate, DATETIME_FORMAT_ddMMyyyy,CultureInfo.InvariantCulture);
            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - dtBirthDate.Year;
            // Go back to the year the person was born in case of a leap year
            if (dtBirthDate > today.AddYears(-age)) age--;
            return age;
        }

        public static string CalculateAgeDetail(string input, bool isPtExpired, string DischargeDttm)// Date of Birth
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            DateTime inputDate = Convert.ToDateTime(input);

            int diffYear = 0, diffMonth = 0;
            DateTime currentDate = inputDate;

            //AMQ edits
            DateTime date2 = new DateTime();
            if (isPtExpired)
            {
                if(!string.IsNullOrEmpty(DischargeDttm))
                {
                    date2 = DateTime.ParseExact(DischargeDttm, Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new Exception("Expired patient's discharge date time should not be null");
                }
            }
            else
            {
                date2 = DateTime.Today;
            }
            
            diffYear = (date2.Year - inputDate.Year - 1);

            currentDate = currentDate.AddYears(diffYear);

            if (currentDate.AddYears(1) <= date2)
            {
                currentDate = currentDate.AddYears(1);
                diffYear++;
            }

            while (currentDate.AddMonths(1) <= date2)
            {
                currentDate = currentDate.AddMonths(1);
                diffMonth++;
            }

            TimeSpan diff = date2.Subtract(currentDate);
            if (diffYear > 5)
            {
                return ("" + diffYear + " Y ");
            }
            else if (diffYear >= 1 && diffYear <= 5)
            {
                return ("" + diffYear + " Y " + diffMonth + " M ");
            }
            else
            {
                if (diffMonth > 5)
                { return ("" + diffMonth + " M "); }
                else if (diffMonth >= 1)
                { return ("" + diffMonth + " M " + diff.Days + " D "); }
                else
                { return ("" + diff.Days + " D "); }
            }
        }

        // Mark : Calculate Time Ago 
        public static string TimeAgo(string inputDateTime)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(inputDateTime)) { return result; }
            DateTime tempDateTime = DateTime.ParseExact(inputDateTime, DATETIME_FORMAT_ddMMyyyyHHmmss, CultureInfo.InvariantCulture);
            DateTime today = DateTime.ParseExact(Constants.strCurrentDateTime, DATETIME_FORMAT_ddMMyyyyHHmmss, CultureInfo.InvariantCulture);
            
            var temp = today.Subtract(tempDateTime);

            if (temp.Days > Constants.ZERO)
            {
                result = temp.Days.ToString() + " d";
            }

            if (temp.Hours > Constants.ZERO)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result = result + " ";
                }
                result = result + temp.Hours.ToString() + " h";
            }

            if (temp.Minutes > Constants.ZERO)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result = result + " ";
                }
                result = result + temp.Minutes.ToString() + " m";
            }
            return result;
        }

        public static int CalculateTimeDifference(DateTime startTime, DateTime endTime)
        {
            //DateTime dtstartTime = DateTime.ParseExact(startTime, DATETIME_FORMAT_ddMMyyyy, CultureInfo.InvariantCulture);
            //DateTime dtendTime = DateTime.ParseExact(endTime, DATETIME_FORMAT_ddMMyyyy, CultureInfo.InvariantCulture);
            TimeSpan span = endTime.Subtract(startTime);
            //span.Seconds, span.Minutes, span.Hours

            return span.Hours;
        }

        public static bool ComppareAppointmentTimeWithServer(string InputDateStr)
        {

            if (string.IsNullOrEmpty(InputDateStr))
            {
                return false;
            }
            DateTime FInputDate = Convert.ToDateTime(InputDateStr); // InputDateStr //Apr 22, 2020 10:30
            DateTime ServeDateNow = DateTime.Now;//Convert.ToDateTime("Apr 23, 2020 19:00");//DateTime.Today;

            TimeSpan span = ServeDateNow.Subtract(FInputDate);
            if (span.Days == 0)
            {
                if (span.Hours >= 0 && span.Hours <= 24)
                {
                    if (span.Minutes >= -5)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsTodaysDate(DateTime inputDate)
        {
            throw new NotImplementedException();
        }

        public static string GetTimeAgoForDates(string startDateTime, string stopDateTime)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(startDateTime) || string.IsNullOrEmpty(stopDateTime)) { return result; }
            DateTime tempDateTime = DateTime.ParseExact(startDateTime, DATETIME_FORMAT_ddMMyyyyHHmmss, CultureInfo.InvariantCulture);
            DateTime today = DateTime.ParseExact(stopDateTime, DATETIME_FORMAT_ddMMyyyyHHmmss, CultureInfo.InvariantCulture);

            var temp = today.Subtract(tempDateTime);

            if (temp.Days > Constants.ZERO)
            {
                result = temp.Days.ToString() + " d";
            }

            if (temp.Hours > Constants.ZERO)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result = result + " ";
                }
                result = result + temp.Hours.ToString() + " h";
            }

            if (temp.Minutes > Constants.ZERO)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result = result + " ";
                }
                result = result + temp.Minutes.ToString() + " m";
            }
            return result;
        }


        public static string GetELOS(string inputDateTime)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(inputDateTime)) { return result; }

            DateTime tempDateTime = DateTime.ParseExact(inputDateTime, DATETIME_FORMAT_ddMMyyyy, 
                                                        CultureInfo.InvariantCulture);

            DateTime today        = DateTime.ParseExact(Constants.strCurrentDate , DATETIME_FORMAT_ddMMyyyy, 
                                                        CultureInfo.InvariantCulture);


            var temp = today.Subtract(tempDateTime);

            if (temp.Days <= 1)
            {
                result ="1 Day";
            }
            else
            {
                result = temp.Days + " Days";
            }
            return result;

        }

        public static string GetELOSWithTime(string inputDateTime)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(inputDateTime)) { return result; }

            DateTime tempDateTime = DateTime.ParseExact(inputDateTime, DATETIME_FORMAT_ddMMyyyyHHmmss,
                                                        CultureInfo.InvariantCulture);

            DateTime today = DateTime.ParseExact(Constants.strCurrentDateTime, DATETIME_FORMAT_ddMMyyyyHHmmss,
                                                        CultureInfo.InvariantCulture);


            var temp = today.Subtract(tempDateTime);

            if (temp.Days <= 1)
            {
                result = "1 Day";
            }
            else
            {
                result = temp.Days + " Days";
            }
            return result;
        }


        // Mark : Get XML Value by Key 
        public static string GetXMLValue(string XML, string searchTerm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(XML);
                XmlNodeList nodes = doc.SelectNodes("root/key");
                foreach (XmlNode node in nodes)
                {
                    XmlAttributeCollection nodeAtt = node.Attributes;
                    if (nodeAtt["name"].Value.ToString() == searchTerm)
                    {
                        XmlDocument childNode = new XmlDocument();
                        childNode.LoadXml(node.OuterXml);
                        return childNode.SelectSingleNode("key/value").InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return Constants.EmptyString;
        }

        //AMQ add
        public static string AddDoctorInString(string str) {
            if (!string.IsNullOrEmpty(str))
            {
                string tmpstr = str.Replace(" ", string.Empty).ToLower().Substring(0, 3);
                if (tmpstr.Equals("dr."))
                {
                    return "Dr" + str.Substring(3);
                }
                return "Dr " + str;
            }
            return "";
        }
        public static string Ordinal(int number)
        {
            var work = number.ToString();
            if ((number % 100) == 11 || (number % 100) == 12 || (number % 100) == 13)
                return work + "th";
            switch (number % 10)
            {
                case 1: work += "st"; break;
                case 2: work += "nd"; break;
                case 3: work += "rd"; break;
                default: work += "th"; break;
            }
            return work;
        }
    }
}