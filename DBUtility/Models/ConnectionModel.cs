namespace DBUtility
{
    public class ConnectionModel
    {
        public ConnectionModel(string connectionName, string connectionDisplayName, string connectionString)
        {
            ConnectionName = connectionName;
            ConnectionDisplayName = connectionDisplayName;
            ConnectionString = connectionString;
        }

        public ConnectionModel() { }
        public string ConnectionName { get; set; }
        public string ConnectionDisplayName { get; set; }
        public string ConnectionString { get; set; }


    }
}
