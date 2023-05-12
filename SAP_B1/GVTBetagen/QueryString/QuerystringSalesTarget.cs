using GTCore.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen
{
    public partial class Querystring
    {

        /// <summary>
        /// USP_BS_SalesManager_LoadbyUserId "'{0}'"
        /// </summary>
        public static string sp_GetSaleManagerByUser
        {
            get
            {
                return CallStoreBySystem("USP_BS_SalesManager_LoadbyUserId", "'{0}'");
            }
        }

        /// <summary>
        /// "USP_BS_KAASM_LoadbyUserId "'{0}', '{1}'"
        /// </summary>
        public static string sp_GetKA_ASMByUser
        {
            get
            {
                return CallStoreBySystem("USP_BS_KAASM_LoadbyUserId", "'{0}', '{1}'");
            }
        }

        /// <summary>
        /// USP_BS_SalesSup_LoadbyUserId "'{0}', '{1}', '{2}'"
        /// </summary>
        public static string sp_GetSalesSupByUser
        {
            get
            {
                return CallStoreBySystem("USP_BS_SalesSup_LoadbyUserId", "'{0}', '{1}', '{2}'");
            }
        }
        public static string sp_GetTeamLeaderByUser
        {
            get
            {
                return CallStoreBySystem("USP_BS_TeamLead_LoadbyUserId", "'{0}', '{1}', '{2}', '{3}'");
            }
        }
    }
}
