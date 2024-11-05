
using System.Data;
using Utility;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace DBUtility
{
   public class SQLDBManager : IDBCore
    {
        #region Variables
        // DBConnection Strings
        private List<ConnectionModel> DBConnectionStrings = new List<ConnectionModel>();
        #endregion

        #region Constructor        
        
        // Mark: Constructor which takes the connection string name
        public SQLDBManager(string connectionStringName)
        {   
            DBConnectionStrings.AddRange(CreateConnectionString(connectionStringName));            
        }

        public List<ConnectionModel> CreateConnectionString(string source)
        {
            List<ConnectionModel> connections = new List<ConnectionModel>();
            switch (source.ToLower())
            {
                case "ncms":
                    connections.Add(new ConnectionModel("ncms", "NCMS", "Persist Security Info=True;User ID=ncms;Initial Catalog=NCMSII;Data Source= vqaysi"));
                   // connections.Add(new ConnectionModel("ncms", "NCMS", AppSetting.GetKeyValue("NCMS_DB")));
                    break;
                case "ncmspz":
                    connections.Add(new ConnectionModel("ncmspz", "NCMSPZ", AppSetting.GetKeyValue("NCMSPZ_DB")));
                    break;
                case "ncmsuat":
                    connections.Add(new ConnectionModel("ncms", "NCMS", AppSetting.GetKeyValue("NCMS_DB_UAT")));
                    break;
                case "absuat":
                    connections.Add(new ConnectionModel("abs", "ABS", AppSetting.GetKeyValue("ABS_UAT")));
                    break;
            }
            return connections;
        }
        
        #endregion

        #region Get Methods

        // Mark : Get Data 
        public DataSet GetData(string query, List<QueryParameter> parameters = null, bool hasLongData = false)
        {
            DataSet dataset = new DataSet();
            try
            {
                foreach (var conn in DBConnectionStrings)
                {
                    using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = query;
                            if (parameters != null && parameters.Count > Constants.ZERO)
                            {
                                var temp = (from param in parameters
                                            select new SqlParameter()
                                            {
                                                ParameterName = param.ParameterName,
                                                Value = param.Value
                                            }).ToList();

                                command.Parameters.AddRange(temp.ToArray());
                               
                            }
                            using (var dataAdapter = new SqlDataAdapter(command))
                            {
                                try
                                {
                                    dataAdapter.Fill(dataset,conn.ConnectionName);
                                    if (UtilityHelper.IsDataSetValid(dataset))
                                    {
                                        DataColumn columnfacilityid = new DataColumn("FacilityID", typeof(string));                                        
                                        columnfacilityid.DefaultValue = conn.ConnectionName;
                                        columnfacilityid.AllowDBNull = false;
                                        

                                        DataColumn columnfacilityname = new DataColumn("FacilityName", typeof(string));
                                        columnfacilityname.DefaultValue = conn.ConnectionDisplayName;
                                        columnfacilityname.AllowDBNull = false;

                                        dataset.Tables[conn.ConnectionName].Columns.AddRange(new DataColumn[] { columnfacilityid, columnfacilityname });
                                        
                                        if (dataset.Tables.Count == 2 && dataset.Tables[conn.ConnectionName].Rows.Count != Constants.ZERO)
                                        {
                                            dataset.Tables[0].Merge(dataset.Tables[conn.ConnectionName]);
                                            dataset.Tables.Remove(dataset.Tables[conn.ConnectionName]);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // If idle time exception
                                    if (ex.HResult == -2147467259)
                                    {
                                        dataset = GetData(query, parameters);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception)
            {
                throw;
            }

            return dataset;
        }

        // Mark : Get Data with has Data feature
        public DataSet GetData(string query, ref bool hasData , List<QueryParameter> parameters = null, bool hasLongData = false)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = GetData(query, parameters);
                if (UtilityHelper.IsDataSetValid(dataset))
                {
                    hasData = Constants.TRUE;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dataset;
        }


        #endregion
        
        #region Save Method

        // Mark : Saves data
        public bool SaveData(string query, List<QueryParameter> parameters = null)
        {
            bool isSaved = Constants.FALSE;
            try
            {   
                using (SqlConnection connection = new SqlConnection(DBConnectionStrings[0].ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        int LastInsertedId = Constants.ZERO;
                        command.CommandText = query;
                        if (parameters != null && parameters.Count > Constants.ZERO)
                        {
                            var temp = (from param in parameters
                                        select new SqlParameter()
                                        {
                                            ParameterName = param.ParameterName,
                                            Value = param.Value
                                        }).ToList();

                            command.Parameters.AddRange(temp.ToArray());                            
                        }

                        if (command != null)
                        {
                            LastInsertedId = command.ExecuteNonQuery();
                            if (LastInsertedId > Constants.ZERO)
                            {
                                isSaved = Constants.TRUE;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isSaved;
        }

        public int SetDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Helper Methods 
        // Mark: Return Next Sequence Value 
        public int GetNextIDSEQUENCE(string tablename)
        {
            throw new NotImplementedException();
        }

        // Mark : Get Next URN 
        public string GetNextURN(string URNField, string tableName, string criteria = "")
        {
            throw new NotImplementedException();
        }

        

        public string GetScalarValueDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null, string outputParameterType = "")
        {
            throw new NotImplementedException();
        }

        public DataSet GetDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
