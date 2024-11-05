using System.Configuration;
//using Oracle.DataAccess.Client;

namespace APIUtility.Helpers
{
    public class DBConnectionOracle
    {
        //private static string GetPrimaryConnection(string connectionString) { return ConfigurationManager.AppSettings[connectionString]; }

        /////private static string PrimaryConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        //public static OracleConnection PrimaryConnection(string connectionString)
        //{
        //    //ADO.NET provides connection pooling concept, 
        //    //everytime creates a new instance of the connection 
        //    //and then we call OPEN then ADO.NET will pick the connection 
        //    //from pool if exists with same connection string.

        //    return new OracleConnection(GetPrimaryConnection(connectionString));
        //}
    }
}