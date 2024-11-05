using APIUtility.Helpers;
using APIUtility.Models;
using DBUtility;
using System;
using System.Collections.Generic;
using Utility;

namespace APIUtility.TokenHelper
{
    public class RegisteredTokenDB : APIDBHelper
    {
        public RegisteredTokenDB()
        {
            new APIDBHelper(); 
        }

        public bool SaveRegisteredTokenDB(RegisteredToken model)
        {
            bool isSaved = false;
            try
            {
                if (!string.IsNullOrEmpty(model.DeviceID))
                {
                    var parameters = new List<QueryParameter>
                    {
                        new QueryParameter("USERID", model.UserID),
                        new QueryParameter("DEVICEID", model.DeviceID.Trim()),
                        new QueryParameter("TOKEN", model.Token),
                        new QueryParameter("DEVICEOS", model.DeviceOS),
                        new QueryParameter("ACCESSDTTM", model.AccessDTTM),
                         new QueryParameter("FACILITYID", model.FacilityID)
                    };
                    string query = "";
                    if (GetRegisteredTokenDB(model.DeviceID, model.UserID))
                    {
                        query = @"update PCIREGTOKEN 
                                    set TOKEN=:TOKEN,
                                        ACCESSDTTM=to_date(:ACCESSDTTM,'dd/mm/yyyy hh24:mi:ss'),
                                        DEVICEOS=:DEVICEOS,
                                        FACILITYID = :FACILITYID 
                                    where 
                                        DEVICEID=:DEVICEID and 
                                        UserID=:UserID";
                    }
                    else
                    {
                        query = @" insert into PCIREGTOKEN(UserID,DEVICEID,TOKEN,ACCESSDTTM,DEVICEOS,FACILITYID ) 
                                    Values(:UserID,
                                            :DEVICEID, 
                                            :TOKEN,
                                            to_date(:ACCESSDTTM,'dd/mm/yyyy hh24:mi:ss'),
                                            :DEVICEOS,
                                            :FACILITYID)";
                    }
                    isSaved = _IDBHelper.SaveData(query, parameters);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return isSaved;
        }

        public bool GetRegisteredTokenDB(string DeviceID, string UserID, string TokenNumber = "")
        {
            bool hasData = false;
            try
            {
                string query = @" Select count(*) from PCIREGTOKEN where DEVICEID=:DEVICEID and UserID=:UserID";

                var parameters = new List<QueryParameter>
                {
                    new QueryParameter("DEVICEID", DeviceID),
                    new QueryParameter("UserID", UserID)
                };

                if (!string.IsNullOrEmpty(TokenNumber))
                {
                    query += " and Token=:Token";
                    parameters.Add(new QueryParameter("Token", TokenNumber));
                }

                hasData = _IDBHelper.IsRecordExist(query, parameters);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
                return false;
            }
            return hasData;
        }

        public RegisteredToken GetRegisteredTokenDetail(string DeviceID, string UserID, ref bool hasData,
                                            string TokenNumber = "" )
        {
            RegisteredToken response = new RegisteredToken();

            try
            {
                DataNamesMapper<RegisteredToken> mapper = new DataNamesMapper<RegisteredToken>();

                string query = @" Select DEVICEID,       
                                    USERID,         
                                    TOKEN ,         
                                    DEVICEOS,       
                                    ACCESSDTTM ,    
                                    FACILITYID  as HISFACILITYID
                                from PCIREGTOKEN 
                                where 
                                    DEVICEID=:DEVICEID and 
                                    UserID=:UserID";

                var parameters = new List<QueryParameter>
                {
                    new QueryParameter("DEVICEID", DeviceID),
                    new QueryParameter("USERID", UserID)
                };

                if (!string.IsNullOrEmpty(TokenNumber))
                {
                    query += " and Token=:Token";
                    parameters.Add(new QueryParameter("Token", TokenNumber));
                }

                response = mapper.MapDataObject(_IDBHelper.GetData(query, ref hasData, parameters));

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
               // return false;
            }
            return response;
        }

    }
}
