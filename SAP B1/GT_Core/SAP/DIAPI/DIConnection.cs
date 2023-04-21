using GTCore.Config;
using GTCore.Helper;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTCore.SAP.DIAPI
{
    public class DIConnection
    {
        private static DIConnection instance;
        public static DIConnection Instance
        {
            get
            {
                if (instance == null)
                    instance = new DIConnection();
                return instance;
            }
        }
        public bool IsConnected = false;
        //private string ServerName { get; set; }
        //private string DBCode { get; set; }
        //private string DBUserName { get; set; }
        //private string DBUserPass { get; set; }
        //private string UserName { get; set; }
        //private string UserPass { get; set; }
        //private string LicenseServer { get; set; }
        //private string SLDServer { get; set; }

        public Company Company { get; set; }
        public SAPbouiCOM.Application SBO_Application;
       
        private int ConnectDI(ref string message)
        {
            int ret, lErrCode;
            try
            {
                Company = new SAPbobsCOM.Company();
                var sCookie = Company.GetContextCookie();
                var sConnectionContext = SBO_Application.Company.GetConnectionContext(sCookie);
                if (Company.Connected == true)
                {
                    Company.Disconnect();
                }
                ret = Company.SetSboLoginContext(sConnectionContext);
                if (ret != 0)
                    Company.GetLastError(out lErrCode, out message);
            }
            catch (Exception ex)
            {
                ret = -1;
                message = string.Format("Error ConnectDI : {0}", ex.Message);
            }

            return ret;
        }

        private int ConnectToCompany(ref string message)
        {
            int ret, lErrCode;
            try
            {
                ret = Company.Connect();
                if(ret != 0)
                    Company.GetLastError(out lErrCode, out message);
            }
            catch (Exception ex)
            {
                ret = -1;
                message = string.Format("Error ConnectToCompany : ",  ex.Message);
            }
            return ret;
        }
        public bool Connect(ref string message)
        {
            if(ConnectDI(ref message) != 0)
            {
                return false;
            }
            if(ConnectToCompany(ref message) != 0)
            {
                return false;
            }
            return true;
        }
        private DIConnection()
        {
            var message = string.Empty;
            try
            {

                SAPbouiCOM.SboGuiApi SboGuiApi = null;
                string sConnectionString = null;

                SboGuiApi = new SAPbouiCOM.SboGuiApi();
                sConnectionString = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056";

                SboGuiApi.Connect(sConnectionString);
                SBO_Application = SboGuiApi.GetApplication();
                //if(ConnectDI(ref message) == 0)
                //{

                //}

                //ServerName = ConfigurationManager.AppSettings["SvName"];
                //DBCode = ConfigurationManager.AppSettings["DbName"];
                //DBUserName = ConfigurationManager.AppSettings["DbU"];
                //DBUserPass = StringUtils.DecryptString(ConfigurationManager.AppSettings["DbP"]);
                //UserName = ConfigurationManager.AppSettings["SapU"];
                //UserPass = StringUtils.DecryptString(ConfigurationManager.AppSettings["SapP"]);
                //LicenseServer = ConfigurationManager.AppSettings["SldServer"];
                //SLDServer = "https://" + LicenseServer;
                //var message = string.Empty;
                //SetApplication(ref message);
                //Company = new Company
                //{
                //    // SQL Server
                //    Server = ConfigurationManager.AppSettings["dataSource"],
                //    // SQL Database Name
                //    CompanyDB = DBCode,
                //    UseTrusted = false,
                //    // SAP Account
                //    UserName = UserName,
                //    Password = UserPass,
                //    // SAP License Server
                //    LicenseServer = LicenseServer,
                //    SLDServer = "https://" + LicenseServer,
                //    language = BoSuppLangs.ln_English,
                //    // SQL Account
                //    DbUserName = DBUserName,
                //    DbPassword = DBUserPass// StringUtils.DecryptString(DBUserPass)
                //};
                //switch (CoreSetting.System)
                //{
                //    case SystemType.SAP_HANA:
                //        Company.DbServerType = BoDataServerTypes.dst_HANADB; break;
                //    case SystemType.SAP_SQL_2014:
                //        Company.DbServerType = BoDataServerTypes.dst_MSSQL2014; break;
                //    case SystemType.SAP_SQL_2016:
                //        Company.DbServerType = BoDataServerTypes.dst_MSSQL2016; break;
                //    case SystemType.SAP_SQL_2017:
                //        Company.DbServerType = BoDataServerTypes.dst_MSSQL2017; break;
                //    default:
                //        Company.DbServerType = BoDataServerTypes.dst_MSSQL2019; break;
                //}
            }
            catch (Exception ex)
            {

            }
        }

        //public bool DIConnect(ref string message)
        //{
        //    var ret = ConnectDI(ref message);
        //    //message = "";
        //    return ret != -1;
        //    int lRetCode = -1, lErrCode;
        //    try
        //    {
        //        lRetCode = Company.Connect();
        //        //// Check for errors
        //        if (lRetCode != 0)
        //        {
        //            Company.GetLastError(out lErrCode, out message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = string.Format("Connect fail - {0}", ex.Message);
        //        return false;
        //    }
        //    return (lRetCode == 0);
        //}
        //public bool DIDisconnect(ref string message)
        //{
        //    try
        //    {
        //        Company.Disconnect();
        //    }
        //    catch (Exception ex)
        //    {
        //        message = string.Format("Disconnect fail - {0}", ex.Message);
        //        return false;
        //    }
        //    return true;
        //}
        public bool DIDisconnect()
        {
            try
            {
                Company.Disconnect();
            }
            catch (Exception ex)
            {
                // message = string.Format("Disconnect fail - {0}", ex.Message);
                return false;
            }
            return true;
        }

        public int GetFieldID(string tableName, string fieldName)
        {
            int index = -1;
            try
            {

            }
            catch (Exception ex)
            {

            }
            return index;
        }
    }
}
