using DBUtility;
using Utility;

namespace APIUtility.Helpers
{
   public class APIDBHelper
    {
        protected IDBHelper _IDBHelper;
        const string Default_Schema = "PCI";

        public APIDBHelper(string facilityID = "")
        {
            if (string.IsNullOrEmpty(facilityID))
            {
                _IDBHelper = new DBHelper(AppSetting.GetKeyValue(enDatabaseConnections.PCIDefaultDataSource.ToString()));
                return;
            }
            if (facilityID.Contains("."))
            {
                _IDBHelper = new DBHelper(facilityID);
                return;
            }
            _IDBHelper = new DBHelper(enHISDBSchema.PCI.ToString() + "." + facilityID);
        }

    }
}
