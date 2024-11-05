using log4net;
using System;

namespace Utility
{
    public class Logger
    {
        public static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Mark : Write Info Log
        public static void WriteInfoLog(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                _log.Info(message);
            }
        }

        // Mark : Write Error Log
        public static void WriteErrorLog(Exception ex)
        {
            _log.Error(ex.Message + "\n" + ex.StackTrace);
            if(ex.InnerException!= null)
            {
                _log.Error(ex.InnerException.Message + "\n" + ex.InnerException.StackTrace);
            }
        }

    }

}