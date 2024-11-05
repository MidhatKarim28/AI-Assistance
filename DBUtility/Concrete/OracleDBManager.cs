//using Oracle.DataAccess.Client;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Utility;

namespace DBUtility
{
   public class OracleDBManager : IDBCore 
    {
        #region Variables
        // DBConnection Strings
        //private const string CONNECTION_STRING_NAME = "DefaultConnection";
        //private const string DEFAULT_DATA_SOURCE = "DefaultDataSource";
        //private const string SOURCE_DEV = "dev";
        //private const string SOURCE_UAT = "nakuprod";
        //private const string SOURCE_StadiumRoad = "akuprod";
        //private const string SOURCE_Kharadar = "akhsprdk";
        //private const string SOURCE_Garden = "akhsprdg";
        //private const string SOURCE_Hyderabad = "akhsprdh";
        //private const string SOURCE_Karimabad = "akhsprdr";        
        
        private List<ConnectionModel> DBConnectionStrings = new List<ConnectionModel>();        
        #endregion

        #region Constructor        
        // Mark: Constructor which takes the connection string name
        public OracleDBManager(string connectionStringName)
        {            
            if (!string.IsNullOrEmpty(connectionStringName))
            {
                DBConnectionStrings.AddRange(CreateConnections(connectionStringName));
            }            
        }

        public List<ConnectionModel> CreateConnections(string connectionStringName)
        {
            List<ConnectionModel> connections = new List<ConnectionModel>();
            string user = connectionStringName.Split('.').First().ToLower();
            string source = connectionStringName.Split('.').Last().ToLower();

            if (string.IsNullOrEmpty(source))
            {
                source = "UAT";// AppSetting.GetKeyValue("DefaultDataSource");
                connections.Add(GetConnectionModel(source.ToString(), user));
            }
            else if (source.Contains(","))
            {
                string[] sourceArr = source.Split(',');
                foreach (var src in sourceArr)
                {
                    connections.Add(GetConnectionModel(src.ToString(), user));
                }
            }
            // All facilities connections (*)
            else if (source == DBConstants.ALL_HIS_FACILITIES)
            {
                foreach (enHISFacility facility in Enum.GetValues(typeof(enHISFacility)))
                {
                    if(!(facility.ToString().Equals(enHISFacility.DEV.ToString()) || facility.ToString().Equals(enHISFacility.UAT.ToString())))
                    connections.Add(GetConnectionModel(facility.ToString(), user));
                }

                //AMQ remove addition stadium road
                connections = connections.GroupBy(x => x.ConnectionName).Select(x => x.First()).ToList();
            }
            else
            {
                connections.Add(GetConnectionModel(source.ToString(), user));
            }
            return connections;
        }

        // Mark : Get Connection Model from source and user
        public ConnectionModel GetConnectionModel(string source, string dbUserID)
        {
            string sourceID = source.ToUpper();
            string dataSource = Constants.EmptyString; 
         
            if(sourceID == enHISFacility.STADIUM.ToString() || sourceID == enHISFacility.STADIUMROAD.ToString())
            {
                sourceID = AppSetting.GetKeyValue("DefaultDataSource");
                sourceID = sourceID.ToUpper();
            }
            dataSource = UtilityHelper.GetEnumDescription((enHISFacilityDataSource)Enum.Parse(typeof(enHISFacilityDataSource), sourceID));
      
            string dbUser = source == enHISFacility.DEV.ToString() ? dbUserID : dbUserID + "usr";
            string connectionstring = string.Format(@"Data source={0};User ID={1};Password={1}", dataSource, dbUser);
            return new ConnectionModel(sourceID,
                                        UtilityHelper.GetEnumDescription((enHISFacility) Enum.Parse(typeof(enHISFacility), sourceID)),
                                        connectionstring);
            
        }

        #endregion

        #region Get Methods

        // Mark : Get Data 
        public DataSet GetData(string query, List<QueryParameter> parameters = null, bool hasLongData = false)
        {
            List<DataSet> lstDataSet = new List<DataSet>();
            try
            {
                foreach (var conn in DBConnectionStrings)
                {
                    DataSet dataset = new DataSet();
                    using (OracleConnection connection = new OracleConnection(conn.ConnectionString))
                    {
                        connection.Open();
                        using (OracleCommand command = connection.CreateCommand())
                        {
                            command.CommandText = query;
                            if (hasLongData)
                            {
                                command.InitialLONGFetchSize = -1;
                            }

                            if (parameters != null && parameters.Count > Constants.ZERO)
                            {
                                var temp = (from param in parameters
                                            select new OracleParameter()
                                            {
                                                ParameterName = param.ParameterName,
                                                Value = param.Value
                                            }).ToList();

                                command.Parameters.AddRange(temp.ToArray());
                                command.BindByName = true;
                            }

                            using (var dataAdapter = new OracleDataAdapter(command))
                            {
                                try
                                {
                                    dataAdapter.Fill(dataset, conn.ConnectionName);

                                    if (UtilityHelper.IsDataSetValid(dataset))
                                    {
                                        DataColumn columnfacilityid = new DataColumn("FacilityID", typeof(string));
                                        columnfacilityid.DefaultValue = conn.ConnectionName;
                                        columnfacilityid.AllowDBNull = false;
                                        
                                        DataColumn columnfacilityname = new DataColumn("FacilityName", typeof(string));
                                        columnfacilityname.DefaultValue = conn.ConnectionDisplayName;
                                        columnfacilityname.AllowDBNull = false;

                                        dataset.Tables[conn.ConnectionName].Columns.AddRange(new DataColumn[] {
                                                                                        columnfacilityid, columnfacilityname });
                                        lstDataSet.Add(dataset);
                                    }
                                    //else
                                    //{
                                    //    Logger.WriteInfoLog("No Data Found - Invalid Dataset");
                                    //}
                                }
                                catch (Exception ex)
                                {
                                   // If idle time exception
                                    if(ex.Message.Contains("ORA-02396"))
                                    {
                                         dataset = GetData(query, parameters);
                                    }
                                    else
                                    {
                                        Logger.WriteInfoLog(conn.ConnectionString);
                                        Logger.WriteInfoLog(query);
                                        throw ex;
                                    }
                                }
                            } // end of DataAdapter using statement
                        }// end of Oracle Command using statement
                    }// end of Connection using statement
                }// end of foreach
            }

            catch (Exception)
            {
                throw;
            }
            
            if (lstDataSet.Count > 0)
            {
                //Logger.WriteInfoLog("Multiple DataSets If Condition ");
                foreach (DataSet innerDataSet in lstDataSet.Skip(1)) //(int i = 1; i < lstDataSet.Count; i++)
                {
                    foreach (DataRow dr in innerDataSet.Tables[0].Rows)
                    {
                        lstDataSet[0].Tables[0].Rows.Add(dr.ItemArray);
                    }
                }
                return lstDataSet[0];
            }
         
            return new DataSet();
        }

        public DataSet GetDataStoredProcedure(string storedProcedure, List<QueryParameter> parameters = null)
        {
            DataSet dataset = new DataSet();
            try
            {
                int table_urn = 0;
                foreach (var conn in DBConnectionStrings)
                {
                    using (OracleConnection connection = new OracleConnection(conn.ConnectionString))
                    {
                        connection.Open();
                        using (OracleCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = storedProcedure;
                            if (parameters != null && parameters.Count > Constants.ZERO)
                            {
                                var temp = (from param in parameters
                                            select new OracleParameter()
                                            {
                                                ParameterName = (param.Direction == ParameterDirection.Output) ? param.ParameterName + "CURSOR" : param.ParameterName,
                                                Value = param.Value,
                                                Direction = param.Direction,                                                 
                                                OracleDbType = GetOracleDBType(param.Direction, param.Value),
                                            }).ToList();

                                command.Parameters.AddRange(temp.ToArray());
                                //command.BindByName = true;
                            }
                            using (var dataAdapter = new OracleDataAdapter(command))
                            {
                                //try
                                //{
                                    dataAdapter.Fill(dataset);

                                    if (UtilityHelper.IsDataSetValid(dataset))
                                    {
                                        DataColumn columnfacilityid = new DataColumn("FacilityID", typeof(string));
                                        columnfacilityid.DefaultValue = conn.ConnectionName;
                                        columnfacilityid.AllowDBNull = false;


                                        DataColumn columnfacilityname = new DataColumn("FacilityName", typeof(string));
                                        columnfacilityname.DefaultValue = conn.ConnectionDisplayName;
                                        columnfacilityname.AllowDBNull = false;

                                        dataset.Tables[table_urn].Columns.AddRange(new DataColumn[] { columnfacilityid, columnfacilityname });

                                        if (dataset.Tables.Count == 2 && dataset.Tables[conn.ConnectionName].Rows.Count != Constants.ZERO)
                                        {
                                            dataset.Tables[0].Merge(dataset.Tables[conn.ConnectionName]);
                                            dataset.Tables.Remove(dataset.Tables[conn.ConnectionName]);
                                        }

                                        table_urn++;
                                    }

                                //}
                                //catch (Exception ex)
                                //{
                                //    Logger.WriteInfoLog("ex: " + ex);
                                //    if(ex.Message.Contains("ORA-06550"))
                                //    {
                                //        Logger.WriteInfoLog("CAlling SP Again..");
                                //        dataset = GetDataStoredProcedure(storedProcedure, parameters);
                                        
                                //    }
                                //    else
                                //    {
                                //        throw ex;
                                //    }
                                   
                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteInfoLog("Stored Procedure: " + storedProcedure);
                //foreach(var item in parameters)
                //{
                //    Logger.WriteInfoLog("Parameter Name: " + item.ParameterName + " " + item.Value);
                //}
                //Logger.WriteInfoLog("ex: " + ex);

                if (ex.Message.Contains("ORA-06550"))
                {
                    //Logger.WriteInfoLog("CAlling SP Again..");
                    dataset = GetDataStoredProcedure(storedProcedure, parameters);

                }
                else
                {
                    throw ex;
                }
            }

            return dataset;
        }

        public string GetScalarValueDataStoredProcedure(string storedProcedure, List<QueryParameter> parameters = null, string outputParameterType = "")
        {
            DataSet dataset = new DataSet();
            string result = string.Empty;
            try
            {                
                foreach (var conn in DBConnectionStrings)
                {
                    using (OracleConnection connection = new OracleConnection(conn.ConnectionString))
                    {
                        connection.Open();
                        using (OracleCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = storedProcedure;
                            if (parameters != null && parameters.Count > Constants.ZERO)
                            {
                                var temp = (from param in parameters
                                            select new OracleParameter()
                                            {
                                                ParameterName = param.ParameterName,
                                                Value = param.Value,
                                                Direction = param.Direction,
                                                Size = string.IsNullOrEmpty(outputParameterType) ?   param.Size : 100,
                                                OracleDbType = (param.Direction == ParameterDirection.Output) ? GetOracleDBType(param.Direction, param.Value, outputParameterType) : GetOracleDBType(param.Direction, param.Value),
                                            }).ToList();

                                command.Parameters.AddRange(temp.ToArray());                                
                            }

                            command.ExecuteScalar();
                                                                                 
                            foreach (OracleParameter param in command.Parameters)
                            {
                                if (param.Direction == ParameterDirection.Output)
                                {
                                    result = Convert.ToString(param.Value);
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

            return result;
        }
        // Mark : Get Data with has Data feature
        public DataSet GetData(string query, ref bool hasData , List<QueryParameter> parameters = null, bool hasLongData = false)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = GetData(query, parameters, hasLongData);
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

        public DataSet GetDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null)
        {
            DataSet dataset = new DataSet();
            try
            {
                dataset = GetDataStoredProcedure(storedProcedure, parameters);
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
        public string GetScalarValueDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null, string outputparametertype = "")
        {
            string result = string.Empty; 
            try
            {
                result = GetScalarValueDataStoredProcedure(storedProcedure, parameters, outputparametertype);
                if (!string.IsNullOrEmpty(result))
                {
                    hasData = Constants.TRUE;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        
        private OracleDbType GetOracleDBType(ParameterDirection Direction, object ParamValue, string outputParameterType = "")
        {
            if (Direction == ParameterDirection.Output)
            {
                if (outputParameterType == "Int")
                {
                    return OracleDbType.Int64;
                }
                else if(outputParameterType == "Varchar")
                {
                    return OracleDbType.Varchar2;
                }
                else
                {
                    return OracleDbType.RefCursor;
                }                
            }
            else
            {
                if (ParamValue is DateTime)
                {
                    return OracleDbType.Date;
                }
                else if (ParamValue is int)
                {
                    return OracleDbType.Int32;
                }
                else
                {
                    return OracleDbType.Varchar2;
                }
            }
        }
        #endregion

        #region Save Method

        // Mark : Saves data

        public bool SaveData(string query, List<QueryParameter> parameters = null)
        {
            bool isSaved = Constants.FALSE;
            try
            {
                using (OracleConnection connection = new OracleConnection(DBConnectionStrings[0].ConnectionString))
                {
                    connection.Open();
                    using (OracleCommand command = connection.CreateCommand())
                    {
                        int LastInsertedId = Constants.ZERO;
                        command.CommandText = query;
                        if (parameters != null && parameters.Count > Constants.ZERO)
                        {
                            var temp = (from param in parameters
                                        select new OracleParameter()
                                        {
                                            ParameterName = param.ParameterName,
                                            Value = param.Value
                                        }).ToList();

                            command.Parameters.AddRange(temp.ToArray());
                            command.BindByName = Constants.TRUE;
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
            int affectedRows = 0;
            try
            {
                foreach (var conn in DBConnectionStrings)
                {
                    using (OracleConnection connection = new OracleConnection(conn.ConnectionString))
                    {
                        connection.Open();
                        using (OracleCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.BindByName = true;
                            command.CommandText = storedProcedure;
                            if (parameters != null && parameters.Count > Constants.ZERO)
                            {
                                var temp = (from param in parameters
                                            select new OracleParameter()
                                            {
                                                ParameterName = param.ParameterName,
                                                Value = param.Value,
                                                Direction = param.Direction,
                                                OracleDbType = GetOracleDBType(param.Direction, param.Value, "Varchar"),
                                                Size = param.Size
                                            }).ToList();

                                command.Parameters.AddRange(temp.ToArray());
                            }

                            
                            affectedRows = command.ExecuteNonQuery();
                            hasData = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return affectedRows;
        }

        #endregion

        #region Helper Methods 
        // Mark: Return Next Sequence Value 
        public int GetNextIDSEQUENCE(string tablename)
        {
            string query = " select {0}.nextval from dual";
            query = string.Format(query, tablename);
            int result = Constants.ZERO;
            DataSet ds = new DataSet();
            if (UtilityHelper.IsDataSetValid(ds = GetData(query)))
            {
                result = Convert.ToInt32(ds.Tables[Constants.ZERO].Rows[Constants.ZERO][Constants.ZERO].ToString());
            }
            return result;

        }

        // Mark : Get Next URN 
        public string GetNextURN(string URNField, string tableName, string criteria = "")
        {
            string query = " select nvl(max({0}),0)+1 from {1} ";

            query = string.Format(query, URNField, tableName);
            if (!string.IsNullOrEmpty(criteria))
            {
                query = query + " Where " + criteria;
            }

            string URN = string.Empty;
            DataSet ds = new DataSet();
            if (UtilityHelper.IsDataSetValid(ds = GetData(query)))
            {
                URN = ds.Tables[Constants.ZERO].Rows[Constants.ZERO][Constants.ZERO].ToString();
            }
            return URN;
        }
        #endregion
    }
}
