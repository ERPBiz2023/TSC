using GTCore.Config;
using GTCore.Connection;
using GTCore.Helper;
//using log4net;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetagenSBOAddon
{
    public class Globals
    {
        // 

        //public static string QueryPreFix = "GT_";
        //public static string System = "SAP_SQL"; // SAP_HANA
        // try read from config
        public static List<string> MenuList = new List<string>();
       // public static ILog LogInstance = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static Application MainApplication;
        public static SAPbouiCOM.Application Application;

        public static DateTime NullDate = new DateTime(2099, 01, 01);
        public static string DateShowFormat = "dd.MM.yyyy";
        public static string DateQueryFormat = "yyyy-MM-dd";
        public static string DateParseFormat = "dd'.'MM'.'yyyy";

        public static string WarehouseNull = "Select warehouse";
        public static string BinNull = "Select team code";
        public static string UserName;
        public static BaseConnection DataConnection
        {
            get
            {
                if (CoreSetting.System == SystemType.SAP_HANA)
                    return new HanaDBConnection(SAPHanaConnectionString);

                return new SQLDBConnection(SAPSQLConnectionString);
            }
        }
        public static string SAPSQLConnectionString
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["SAPConnection"].ConnectionString;
                var pass = StringUtils.DecryptString(ConfigurationManager.AppSettings["HASH"]);

                return string.Format(connectionString, pass);
            }
        }
        public static string SAPHanaConnectionString
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["SAPHanaConnection"].ConnectionString;
                var pass = StringUtils.DecryptString(ConfigurationManager.AppSettings["HASH"]);

                return string.Format(connectionString, pass);
            }
        }
    }
}
