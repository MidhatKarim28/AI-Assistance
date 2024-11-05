using System.Data;
using System.Data.Common;

namespace DBUtility
{
    public class QueryParameter : DbParameter
    {
       
        public QueryParameter(string parameterName, object obj)
        {
            ParameterName = parameterName;
            Value = obj;
        }

        public QueryParameter(string parameterName, object obj, ParameterDirection direction = ParameterDirection.Input)
        {
            ParameterName = parameterName;
            Value = obj;
            Direction = direction;
        }

        //AMQ overide QueryParams
        public QueryParameter(string parameterName, object obj, ParameterDirection direction, DbType type, int size)
        {
            ParameterName = parameterName;
            Value = obj;
            Direction = direction;
            DbType = type;
            Size = size;
        }

        public QueryParameter(string parameterName, ParameterDirection direction, DbType type, int size)
        {
            ParameterName = parameterName;
            Direction = direction;
            DbType = type;
            Size = size;
        }

        public override DbType DbType
        { get; set; }

        public override ParameterDirection Direction { get; set; }
        
        public override bool IsNullable
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string ParameterName { get; set; }

        public override int Size{ get; set; }

        public override string SourceColumn
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool SourceColumnNullMapping
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override DataRowVersion SourceVersion
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override object Value { get; set; }

        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }

        
    }
}
