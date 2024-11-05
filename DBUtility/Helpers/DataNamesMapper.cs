using System.Data;
using Utility;

namespace DBUtility
{
    public class DataNamesMapper<TEntity> where TEntity : class, new()
    {
        public TEntity Map(DataRow row)
        {
            TEntity entity = new TEntity();
            return Map(row, entity);
        }

        public TEntity Map(DataRow row, TEntity entity)
        {
            var columnNames = row.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var properties = typeof(TEntity).GetProperties()
                                              .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                              .ToList();
            foreach (var prop in properties)
            {
                PropertyMapHelper.Map(typeof(TEntity), row, prop, entity);
            }
            return entity;
        }

        // Mark : Map Data from datatable to model
        public IEnumerable<TEntity> MapData(DataSet dataset)
        {
            List<TEntity> entities = new List<TEntity>();
            try
            {
                DataTable datatable = dataset.Tables[Constants.ZERO];
                var columnNames = datatable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

                var properties = typeof(TEntity).GetProperties()
                                                    .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                                    .ToList();

                foreach (DataRow row in datatable.Rows)
                {
                    TEntity entity = new TEntity();
                    foreach (var prop in properties)
                    {
                        PropertyMapHelper.Map(typeof(TEntity), row, prop, entity);
                    }
                    entities.Add(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return entities;
        }

        public TEntity MapDataObject(DataSet dataset)
        {
            TEntity entity = new TEntity();
            try
            {
                if (UtilityHelper.IsDataSetValid(dataset))
                {
                    DataTable datatable = dataset.Tables[Constants.ZERO];
                    var columnNames = datatable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

                    var properties = typeof(TEntity).GetProperties()
                                                        .Where(x => x.GetCustomAttributes(typeof(DataNamesAttribute), true).Any())
                                                        .ToList();

                    foreach (DataRow row in datatable.Rows)
                    {
                        foreach (var prop in properties)
                        {
                            PropertyMapHelper.Map(typeof(TEntity), row, prop, entity);
                        }
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }


        // Mark : Get list from dataset and map to SuccessResponse model for API 
        //public SuccessResponse GetAutoServiceData(IDBHelper iDBHelper, string query, List<QueryParameter> queryParameters = null)
        //{
        //    bool hasData = Constants.FALSE;
        //    try
        //    {
        //        DataSet dataset = iDBHelper.GetData(query, ref hasData, queryParameters);

        //        if (hasData)
        //        {
        //            IEnumerable<TEntity> responseList = new List<TEntity>();
        //            responseList = MapData(dataset);
        //            return new SuccessResponse(responseList, hasData);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return new SuccessResponse(null, hasData);
        //}

        // Mark : Get list from dataset and map to SuccessResponse model for API 
        public IEnumerable<TEntity> GetDBResponseAuto(IDBHelper iDBHelper, string query, ref bool hasData,
                                                      List<QueryParameter> queryParameters = null, bool hasLongData = false)
        {
            try
            {
                DataSet dataset = iDBHelper.GetData(query, ref hasData, queryParameters, hasLongData);
                if (hasData)
                {
                    IEnumerable<TEntity> responseList = new List<TEntity>();
                    responseList = MapData(dataset);
                    return responseList;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public IEnumerable<TEntity> GetDBResponseStoredProcedureAuto(IDBHelper iDBHelper, string storedProcedure, ref bool hasData,
                                                      List<QueryParameter> queryParameters = null)
        {
            try
            {
                DataSet dataset = iDBHelper.GetDataStoredProcedure(storedProcedure, ref hasData, queryParameters);
                if (hasData)
                {
                    IEnumerable<TEntity> responseList = new List<TEntity>();
                    responseList = MapData(dataset);
                    return responseList;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}
