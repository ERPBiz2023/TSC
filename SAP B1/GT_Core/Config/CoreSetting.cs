﻿using GTCore.Connection;
using GTCore.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTCore.Config
{
    public class CoreSetting
    {
        public static string QueryPreFix = "GT_";
        public static SystemType System = SystemType.SAP_SQL_2019;

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
