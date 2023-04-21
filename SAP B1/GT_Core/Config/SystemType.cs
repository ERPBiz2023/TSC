﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTCore.Config
{
    public enum SystemType
    {
        [Description("SAP_SQL_2014")]
        SAP_SQL_2014,

        [Description("SAP_SQL_2016")]
        SAP_SQL_2016,

        [Description("SAP_SQL_2017")]
        SAP_SQL_2017,

        [Description("SAP_SQL_2019")]
        SAP_SQL_2019,

        [Description("SAP_HANA")]
        SAP_HANA
    }
}
