using DBUtility;
using Utility;

namespace HIS.DBManager
{
    public class HISDBAccessProvider
    {
        public IDBHelper _IDBHelper;
        const string Default_Schema = "HIS";

        public HISDBAccessProvider(string facilityID = "")
        {
            //if (string.IsNullOrEmpty(facilityID))
            //{
            //    // _IDBHelper = new DBHelper(AppSetting.GetKeyValue(enDatabaseConnections.HISStadiumDataSource.ToString()));
            //    _IDBHelper = new DBHelper(AppLocalSetting.HISStadiumDataSource);
            //    return;
            //}
            //if (facilityID.Contains("."))
            //{
            //    _IDBHelper = new DBHelper(facilityID);
            //    return;
            //}
            _IDBHelper = new DBHelper(enHISDBSchema.HIS.ToString() + "." + facilityID);
            
        }

        public void SetIDBHelper(string facilityID)
        {
            _IDBHelper = new DBHelper(Default_Schema + "." + facilityID);
        }

    }
}
