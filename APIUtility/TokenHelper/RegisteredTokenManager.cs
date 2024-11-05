using APIUtility.Helpers;
using APIUtility.Models;
using System;
using System.Collections.Generic;
using Utility;

namespace APIUtility.TokenHelper
{
    public class RegisteredTokenManager : RegisteredTokenDB
    {
        #region Constructor
        public RegisteredTokenManager()
        {
            // any database can be referenced later on
            new RegisteredTokenDB();
        }
        #endregion

        public static bool SaveRegisteredToken(RegisteredToken model)
        {
            RegisteredTokenDB registeredTokenDB = new RegisteredTokenDB();
            return registeredTokenDB.SaveRegisteredTokenDB(model);
        }

        public static bool GetRegisteredTokenDetail(string deviceID, string userID, string tokenNumber = "")
        {
            bool isExists = false;
            RegisteredTokenDB registeredTokenDB = new RegisteredTokenDB();
            isExists = registeredTokenDB.GetRegisteredTokenDB(deviceID, userID, tokenNumber);
            return isExists;
        }
        
        public static string GenerateUserToken(RegisteredToken model)
        {
            string token = Utility.Constants.EmptyString;
            try
            {
                var payload = new Dictionary<string, object>
                                {
                                    { "DeviceID", model.DeviceID },
                                    { "UserID", model.UserID },
                                    { "AccessDateTime", DateTime.Now.Ticks },
                                    { "DeviceOS", model.DeviceOS },
                                    { "ApplicationID", ApplicationSettings.Instance.AppLocalSetting.ApplicationID },
                                    { "TokenNature",  Utility.Constants.enActive.Y.ToString() }
                                };

                token = DBManagerUtility.encodeJWT(payload);
                model.Token = token;
                RegisteredTokenDB registeredTokenDB = new RegisteredTokenDB();
                registeredTokenDB.SaveRegisteredTokenDB(model);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return token;
        }

    }
}