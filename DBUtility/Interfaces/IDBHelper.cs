namespace DBUtility
{
    public interface IDBHelper : IDBCore
    {
        bool IsRecordExist(string query, List<QueryParameter> parameters = null);
        int GetRowsCount(string query, List<QueryParameter> parameters = null);
        string DLookUp(string fieldName, string tableName, string criteria = "");
        string GetScalarValue(string query, List<QueryParameter> parameters = null);
        void LoadOracleTNS();
       // string GetScalarValueStoredProcedure(string query, List<QueryParameter> parameters = null);
    }
}
