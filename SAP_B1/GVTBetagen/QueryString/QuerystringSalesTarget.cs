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

        /// <summary>
        /// Load sales target
        /// 
        /// USP_BS_Customer_SaleTarget_LoadbyUserId "'{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}"
        /// </summary>
        public static string sp_SaleTarget_LoadbyUserId
        {
            get
            {
                return CallStoreBySystem("USP_BS_Customer_SaleTarget_LoadbyUserId", "'{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}");
            }
        }

        /// <summary>
        /// Load approve targetid
        /// usp_BS_SalesTarget_GetTargetID_Approved "{0}, {1}, '{2}'"
        /// </summary>
        public static string sp_SaleTarget_TargetID_Approved
        {
            get
            {
                return CallStoreBySystem("usp_BS_SalesTarget_GetTargetID_Approved", "{0}, {1}, '{2}'");
            }
        }
    }
}
