using System.Collections.Generic;
using System.Data;

namespace DBUtility
{
    public interface IDBCore
    {
        DataSet GetData(string query, List<QueryParameter> parameters = null, bool hasLongData = false);
        DataSet GetData(string query, ref bool hasData, List<QueryParameter> parameters = null, bool hasLongData = false);
        DataSet GetDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null);
        int SetDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null);
        bool SaveData(string query, List<QueryParameter> parameters = null);
        int GetNextIDSEQUENCE(string tablename);
        string GetNextURN(string URNField, string tableName, string criteria = "");
        string GetScalarValueDataStoredProcedure(string storedProcedure, ref bool hasData, List<QueryParameter> parameters = null, string outputParameterType = "");
    }
}
