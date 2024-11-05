using Oracle.ManagedDataAccess.Client;
using System.Data;
using Utility;

namespace DBUtility
{
    public class DBHelper : IDBHelper
    {
        public IDBCore _IDBCore { get; set; }

        public DBHelper(string connectionName)
        {
            if (string.IsNullOrEmpty(connectionName))
            {
                throw new InvalidOperationException();
            }
            // new DBHelper(new OracleDBManager(connectionName));
            LoadOracleTNS();
            _IDBCore = new OracleDBManager(connectionName);
        }

        public DBHelper(string connectionName, bool isSQLDB)
        {
            if (string.IsNullOrEmpty(connectionName))
            {
                throw new InvalidOperationException();
            }
            //new DBHelper(new SQLDBManager(connectionName));
            _IDBCore = new SQLDBManager(connectionName);
        }


        public DBHelper(IDBCore dbCore)
        {
            this._IDBCore = dbCore;
        }

        public string DLookUp(string fieldName, string tableName, string criteria = "")
        {
            string query = string.Empty;
            string result = string.Empty;

            query = "Select " + fieldName + " From " + tableName;
            if (!string.IsNullOrEmpty(criteria))
            {
                query = query + " Where " + criteria;
            }
            try
            {
                DataSet dataset = new DataSet();
                dataset = _IDBCore.GetData(query);

                if (UtilityHelper.IsDataSetValid(dataset))
                {
                    result = dataset.Tables[Constants.ZERO].Rows[Constants.ZERO][Constants.ZERO].ToString();
                }
            }
            catch (Exception ex) { }
            return result; 
        }

        // Mark : Get Data 
        public DataSet GetData(string query, List<QueryParameter> parameters = null, bool hasLongData = false)
        {
            return _IDBCore.GetData(query, parameters, hasLongData);
        }

        // Mark : Get Data with hasData functionality
        public DataSet GetData(string query, ref bool hasData, List<QueryParameter> parameters = null, bool hasLongData = false)
        {
            return _IDBCore.GetData(query, ref hasData, parameters,  hasLongData);
        }

        // Mark : Get Data from Stored Procedure
        public DataSet GetDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null)
        {
            return _IDBCore.GetDataStoredProcedure(storedProcedure, ref hasData, parameters);
        }

        // Mark: Return Next Sequence Value 
        public int GetNextIDSEQUENCE(string tablename)
        {
            return _IDBCore.GetNextIDSEQUENCE(tablename);
        }

        // Mark: Return Next URN
        public string GetNextURN(string uRNFieldName, string tableName, string whereCriteria = "")
        {
            return _IDBCore.GetNextURN(uRNFieldName, tableName, whereCriteria);
        }

        public int GetRowsCount(string query, List<QueryParameter> parameters = null)
        {
            int resultCount = Constants.ZERO;
            try
            {
                DataSet dataset = new DataSet();
                dataset = _IDBCore.GetData(query, parameters);
                if (UtilityHelper.IsDataSetValid(dataset))
                {
                    var count = dataset.Tables[Constants.ZERO].Rows[Constants.ZERO][Constants.ZERO].ToString();
                    resultCount = Convert.ToInt32(count);
                }
            }
            catch (Exception ) { }
            return resultCount;
        }

        public bool IsRecordExist(string query, List<QueryParameter> parameters = null)
        {
            bool isRecordExist = Constants.FALSE;
            try
            {
                DataSet dataset = new DataSet();
                dataset = _IDBCore.GetData(query, parameters);
                if (UtilityHelper.IsDataSetValid(dataset))
                {
                    var count = dataset.Tables[Constants.ZERO].Rows[Constants.ZERO][Constants.ZERO].ToString();
                    int intCount = Convert.ToInt32(count);
                    if (intCount > Constants.ZERO)
                    {
                        isRecordExist = Constants.TRUE; 
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isRecordExist;
        }

        public bool SaveData(string query, List<QueryParameter> parameters = null)
        {
            return _IDBCore.SaveData(query, parameters);
        }

        // Mark : Get Single Coumn Data 
        public string GetScalarValue(string query, List<QueryParameter> parameters = null)
        {
            string result = string.Empty;
            DataSet ds = new DataSet();
            if (UtilityHelper.IsDataSetValid(ds = _IDBCore.GetData(query, parameters)))
            {
                result = ds.Tables[Constants.ZERO].Rows[Constants.ZERO][Constants.ZERO].ToString();
            }
            return result;
        }
                
        public string GetScalarValueDataStoredProcedure(string query, ref bool hasData, List<QueryParameter> parameters = null, string outputParameterType = "")
        {
            string result = string.Empty;            
            
            result = _IDBCore.GetScalarValueDataStoredProcedure(query, ref hasData, parameters, outputParameterType);
            
            return result;
        }

        public int SetDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null)
        {
            return _IDBCore.SetDataStoredProcedure(storedProcedure, ref hasData, parameters);
        }

        public void LoadOracleTNS()
        {
            Dictionary<string, string> keyValuesOracleTNS = ApplicationSettings.Instance.OracleTNS;
            foreach(KeyValuePair<string,string> keyValuePair in keyValuesOracleTNS)
            {
                string key = keyValuePair.Key;
                string value = keyValuePair.Value;
                if(string.IsNullOrEmpty(OracleConfiguration.OracleDataSources[key]))
                    OracleConfiguration.OracleDataSources.Add(key, value);
            }            
        }
    }
}
