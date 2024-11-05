using System.Data;
using System.Reflection;

namespace DBUtility
{
    public static class PropertyMapHelper
    {
        // Mark : Map DataRow to property of model
        public static void Map(Type type, DataRow row, PropertyInfo prop, object entity)
        {
            List<string> columnNames = AttributeHelper.GetDataNames(type, prop.Name);

            foreach (var columnName in columnNames)
            {
                if (!string.IsNullOrWhiteSpace(columnName) && row.Table.Columns.Contains(columnName.ToUpper()))
                {
                    var propertyValue = row[columnName];
                    if (propertyValue != DBNull.Value)
                    {
                        ParsePrimitive(prop, entity, row[columnName]);
                        break;
                    }
                }
            }
        }

        // Mark : Cast DataTypes
        private static void ParsePrimitive(PropertyInfo prop, object entity, object value)
        {
            if (prop.Name.Contains("DateTimeDB"))
            {
                prop.SetValue(entity, ParseDisplayDateTimeDB(value), null);
            }
            else if (prop.Name.Contains("DateDB"))
            {
                prop.SetValue(entity, ParseDisplayDateDB(value), null);
            }
            else if (prop.Name.Contains("str") && prop.Name.Contains("DateTime"))
            {
                prop.SetValue(entity, ParseDisplayDateTime(value), null);
            }
            else if (prop.Name.Contains("str") && prop.Name.Contains("Date"))
            {
                prop.SetValue(entity, ParseDisplayDate(value), null);
            }
            else if (prop.Name.Contains("DateTimeDB"))
            {
                prop.SetValue(entity, ParseDisplayDateTimeDB(value), null);
            }
            else if (prop.Name.Contains("DateDB"))
            {
                prop.SetValue(entity, ParseDisplayDateDB(value), null);
            }
            else if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(entity, value.ToString().Trim(), null);
            }
            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, ParseBoolean(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(long))
            {
                prop.SetValue(entity, value.ToString().Trim(), null);
                //prop.SetValue(entity, long.Parse(value.ToString()), null);
            }
            else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, int.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(decimal))
            {
                prop.SetValue(entity, decimal.Parse(value.ToString()), null);
            }
            else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
            {
                double number;
                bool isValid = double.TryParse(value.ToString(), out number);
                if (isValid)
                {
                    prop.SetValue(entity, double.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            {
                DateTime date;
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    date = Convert.ToDateTime(value.ToString());
                    prop.SetValue(entity, date, null);
                }
            }
            else if (prop.PropertyType == typeof(Guid))
            {
                Guid guid;
                bool isValid = Guid.TryParse(value.ToString(), out guid);
                if (isValid)
                {
                    prop.SetValue(entity, guid, null);
                }
                else
                {
                    isValid = Guid.TryParseExact(value.ToString(), "B", out guid);
                    if (isValid)
                    {
                        prop.SetValue(entity, guid, null);
                    }
                }
            }
        }

        // Mark : Cast Boolean Data Types 
        public static bool ParseBoolean(object value)
        {
            if (value == null || value == DBNull.Value) return false;

            switch (value.ToString().ToLowerInvariant())
            {
                case "1":
                case "y":
                case "yes":
                case "true":
                    return true;

                case "0":
                case "n":
                case "no":
                case "false":
                default:
                    return false;
            }
        }

        public static string ParseDisplayDate(object value)
        {
            return Utility.UtilityHelper.FormatDate(value);
        }

        public static string ParseDisplayDateTime(object value)
        {
            return Utility.UtilityHelper.FormatDateAndTime(value);
        }
        public static string ParseDisplayDateDB(object value)
        {
            return Utility.UtilityHelper.FormatDateDB(value);
        }

        public static string ParseDisplayDateTimeDB(object value)
        {
            return Utility.UtilityHelper.FormatDateAndTimeDB(value);
        }
    }
}
