using GTCore.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTCore.SAP
{
    public  class Query
    {
        public static string GetFieldID
        {
            get
            {
                if (CoreSetting.System == SystemType.SAP_HANA)
                    return "SELECT \"FieldID\" FROM \"CUFD\" WHERE \"TableID\" = '{0}' and \"AliasID\" = '{1}';";
                else
                    return @"SELECT FieldID FROM CUFD WHERE TableID = '{0}' and AliasID = '{1}'";
            }
        } 
    }
}
