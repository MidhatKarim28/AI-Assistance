using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Utility
{

    public class SQLDBConnection
    {
        public SqlConnection dbconn;
        public string strConnString;

        // Mark : Open SQL Connection
        public void OpenConnection()
        {
            try
            {
                if (strConnString == null || strConnString.Trim() == string.Empty)
                    strConnString = ConfigurationManager.AppSettings["ABSCon"];

                if (dbconn == null)
                    dbconn = new SqlConnection(strConnString);
                else
                {
                    if (dbconn.State == ConnectionState.Open)
                        return;
                    else
                    {
                        try
                        {
                            dbconn.Close();
                            dbconn = null;
                            dbconn = new SqlConnection(strConnString);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                dbconn.ConnectionString = strConnString;
                dbconn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Mark : Open SQL Connection with connection name
        public void OpenConnection(string connectionName)
        {
            try
            {
                if (strConnString == null || strConnString.Trim() == string.Empty)
                    strConnString = ConfigurationManager.AppSettings[connectionName];

                if (dbconn == null)
                    dbconn = new SqlConnection(strConnString);
                else
                {
                    if (dbconn.State == ConnectionState.Open)
                        return;
                    else
                    {
                        try
                        {
                            dbconn.Close();
                            dbconn = null;
                            dbconn = new SqlConnection(strConnString);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                dbconn.ConnectionString = strConnString;
                dbconn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Mark : Release SQL Connection
        public void ReleaseConnection()
        {
            try
            {
                if (dbconn != null)
                {
                    if (dbconn.State == ConnectionState.Connecting || dbconn.State == ConnectionState.Fetching || dbconn.State == ConnectionState.Executing || dbconn.State == ConnectionState.Open)
                    {
                        dbconn.Close();
                        dbconn.Dispose();
                        dbconn = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
