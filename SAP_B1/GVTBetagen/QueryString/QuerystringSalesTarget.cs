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
        /// Load sales target to excel
        /// 
        /// USP_BS_Customer_SaleTarget_LoadExportExcel "'{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}"
        /// </summary>
        public static string sp_SaleTarget_LoadExportExcel
        {
            get
            {
                return CallStoreBySystem("USP_BS_Customer_SaleTarget_LoadExportExcel", "'{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}");
            }
        }

        /// <summary>
        /// Load sales target actual 
        /// USP_BS_Customer_SaleTarget_Actual_LoadbyUserId "'{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}"
        /// </summary>
        public static string sp_SaleTarget_Actual_LoadbyUserId
        {
            get
            {
                return CallStoreBySystem("USP_BS_Customer_SaleTarget_Actual_LoadbyUserId", "'{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}");
            }
        }

        /// <summary>
        /// Load sales target actual to excel file
        /// USP_BS_Customer_SaleTarget_Actual_LoadExportExcel "'{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}"
        /// </summary>
        public static string sp_SaleTarget_Actual_LoadExportExcel
        {
            get
            {
                return CallStoreBySystem("USP_BS_Customer_SaleTarget_Actual_LoadExportExcel", "'{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}");
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

        /// <summary>
        /// Load Target ID to approve
        /// </summary>
        public static string sp_SaleTarget_TargetID
        {
            get
            {
                return CallStoreBySystem("usp_BS_SalesTarget_GetTargetID", "{0}, {1}, '{2}'");
            }
        }

        /// <summary>
        /// usp_BS_SalesTarget_Add "{0}, {1}, '{2}', '{3}', '{4}'"
        /// </summary>
        public static string usp_SalesTarget_Add
        {
            get
            {
                return CallStoreBySystem("usp_BS_SalesTarget_Add", "{0}, {1}, '{2}', '{3}', '{4}'");
            }
        }

        public static string usp_SalesTarget_Add_V1
        {
            get
            {
                return CallStoreBySystem(@"usp_BS_SalesTarget_Detail_Add_V2", "{0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}'," +
                                        "'{8}', {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}");
            }
        }

        /// <summary>
        /// Approve sales target
        /// </summary>
        public static string BS_SalesTarget_Approve
        {
            get
            {
                return CallStoreBySystem("usp_BS_SalesTarget_Approve", "{0}");
            }
        }

        /// <summary>
        /// "SP_BS_SalesManager_Division "'{0}'"
        /// </summary>
        public static string Update_SalesManager_Division
        {
            get
            {
                return CallStoreBySystem("SP_BS_SalesManager_Division", "'{0}'");
            }
        }
        /// <summary>
        /// Sales tagrget update
        /// </summary>
        public static string SalesTarget_Detail_Update
        {
            get
            {
                if (CoreSetting.System == SystemType.SAP_HANA)
                {
                    var schema = ConfigurationManager.AppSettings["Schema"];
                    // return "CALL \"" + schema + "\".\"" + query + "\" (" + param + ")";

                    return "UPDATE \"" + schema + "\".\"BS_SalesTarget_Detail\"" +
                              "SET \"SSAmount\" = '{0}', " +
                                    "\"KAAmount\" = '{1}', " +
                                    "\"SMAmount\" = '{2}', " +
                                    "\"GMAmount\" = '{3}',  " +
                                    "\"KSUSSAmount\" = '{4}', " +
                                    "\"KSUKAAmount\" = '{5}', " +
                                    "\"KSUSMAmount\" = '{6}', " +
                                    "\"KSUGMAmount\" = '{7}' " +
                            " WHERE \"TargetDID\" = '{8}' " +
                              "AND \"TargetId\" = '{9}' ";
                }
                else
                {
                    return @"update BS_SalesTarget_Detail
		                        set SSAmount = '{0}',
			                        KAAmount = '{1}',
			                        SMAmount = '{2}',
			                        GMAmount = '{3}',
			                        KSUSSAmount = '{4}',
			                        KSUKAAmount = '{5}',
			                        KSUSMAmount = '{6}',
			                        KSUGMAmount = '{7}'
		                    where TargetDID = '{8}' and TargetId = '{9}'
                        ";
                }                
            }
        }
    }
}
