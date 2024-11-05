using System.Collections.Generic;
using System.Configuration;
namespace APIUtility.Constants
{
    public static class ServiceConstants
    {
        public const string Login              = "Login";
        public const string Register           = "Register";
    }

    public class ServiceLists
    {
        private static ServiceLists instance = new ServiceLists();
        private List<string> patientServiceList;
        private List<string> exceptionalServiceList;
        object mutex;
        object mutexExceptionalService;

        public static ServiceLists Instance
        {
            get
            {
                return instance;
            }
        }

        private ServiceLists()
        {
            patientServiceList = new List<string>();
            mutex = new object();

            exceptionalServiceList = new List<string>();
            mutexExceptionalService = new object();


            patientServiceList.AddRange(new string[] {
                "MyPatientsManager.GetPatientDetailsByMR"
            });

            exceptionalServiceList.AddRange(new string[]
            {
                "OpenAI.GetAIAssistance",
                "OpenAI.GetAIRefinedText",
                "OpenAI.GetAIClinicalNote"
            });
            
        }
        public bool IsPatientService(string Method)
        {
            lock (mutex)
            {
                return patientServiceList.Contains(Method);
            }
        }
        public bool IsExceptionalService(string Method)
        {
            lock (mutex)
            {
                return exceptionalServiceList.Contains(Method);
            }
        }
    }

}