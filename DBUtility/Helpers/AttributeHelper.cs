using Utility;

namespace DBUtility
{
    public static class AttributeHelper
    {
        public static List<string> GetDataNames(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName).GetCustomAttributes(false).Where(x => x.GetType().Name == "DataNamesAttribute").FirstOrDefault();
            if (property != null)
            {
                var list = ((DataNamesAttribute)property).ValueNames;

                if (UtilityHelper.IsValidList(list))
                {
                    return ((DataNamesAttribute)property).ValueNames;
                }
                else
                {
                    return new List<string>() { propertyName };
                }
                // MM Changes 
                //return ((DataNamesAttribute)property).ValueNames;
            }
            return new List<string>();
        }
    }
}
