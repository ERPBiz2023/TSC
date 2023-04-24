using GTCore.Connection;
using GTCore.Helper;
using System.Configuration;

namespace GTCore.Config
{
    public class CoreSetting
    {
        public static string QueryPreFix = "GT_";
        public static string SAPDIConnectstring = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056";
        public static SystemType System
        {
            get
            {
                var type = ConfigurationManager.AppSettings["SystemType"].ToString().GetEnumValueByDescription<SystemType>();
                return type;// ConfigurationManager.AppSettings["SystemType"].ToString().GetEnumValueByDescription<SystemType>();
            }
        }
        //SystemType.SAP_SQL_2019;

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
