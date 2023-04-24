using GTCore.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetagenSBOAddon
{
    public class Querystring
    {
        // functions
        // UF_BS_GETQUANTITYSTOCKOUTREQUEST
        // UF_BS_SETCOLOR_ITEMCODE
        private static string CallStoreBySystem(string query, string param = "")
        {
            if (CoreSetting.System == SystemType.SAP_HANA)
            {
                var schema = ConfigurationManager.AppSettings["Schema"];
                return "CALL \"" + schema + "\".\"" + query + "\" (" + param + ")";
            }
            else
            {
                return "EXEC " + query + " " + param;//, query, param);
            }
        }
        public static string sp_LoadOutStockRequest
        {
           
            get
            {
                return CallStoreBySystem("USP_BS_STOCKOUTREQUEST", "'{0}','{1}','{2}', {3}");
                //if (CoreSetting.System == SystemType.SAP_HANA)
                //    return "CALL \"USP_BS_STOCKOUTREQUEST\" ('{0}','{1}','{2}', {3})";
                //else
                //    return "EXEC USP_BS_STOCKOUTREQUEST '{0}','{1}','{2}', {3}";
            }
        }

        public static string sp_DeleteOutStockRequest
        {
            get
            {
                return CallStoreBySystem("USP_BS_STOCKOUTREQUEST2_DELETE", "'{0}'");
                //if (CoreSetting.System == SystemType.SAP_HANA)
                //    return "CALL \"USP_BS_STOCKOUTREQUEST2_DELETE\" ('{0}')";
                //else
                //    return "EXEC USP_BS_STOCKOUTREQUEST2_DELETE '{0}'";
            }
        }
        public static string sp_NotificationUpdateStock
        {
            get
            {
                return CallStoreBySystem("usp_Notification_UpdateStock");
                //if (CoreSetting.System == SystemType.SAP_HANA)
                //    return "CALL \"usp_Notification_UpdateStock\" ()";
                //else
                //    return "EXEC usp_Notification_UpdateStock ";
            }
        }

        public static string sp_LoadOutStockRequestByID
        {
            get
            {
                return CallStoreBySystem("usp_BS_STOCKOUTREQUEST_LoadbyID", "'{0}'");
                //if (CoreSetting.System == SystemType.SAP_HANA)
                //    return "CALL \"usp_BS_STOCKOUTREQUEST_LoadbyID\" ('{0}')";
                //else
                //    return "EXEC usp_BS_STOCKOUTREQUEST_LoadbyID '{0}'";
            }
        }
        public static string sp_LoadOutStockRequestDetailByID
        {
            get
            {
                return CallStoreBySystem("usp_BS_STOCKOUTREQUEST_DETAIL_LoadbyID", "'{0}'");
                //if (CoreSetting.System == SystemType.SAP_HANA)
                //    return "CALL \"usp_BS_STOCKOUTREQUEST_DETAIL_LoadbyID\" ('{0}')";
                //else
                // return "EXEC usp_BS_STOCKOUTREQUEST_DETAIL_LoadbyID '{0}'";
            }
        }

        public static string sp_OutStockRequestApplySAP
        {
            get
            {
                return CallStoreBySystem("USP_BS_STOCKOUTREQUEST_APPLY_SAP", "'{0}', {1}");
            }
        }
        public static string sp_OutStockRequestConfirm
        {
            get
            {
                return CallStoreBySystem("usp_BS_InventoryTransferReq_Confirm", "'{0}', {1}");
            }
        }

        public static string sp_GetWarehouses
        {
            get
            {
                return CallStoreBySystem("USP_BS_OWHS", "");
            }
        }
        public static string sp_GetBins
        {
            get
            {
                return CallStoreBySystem("USP_BS_BINCODE", "'{0}'");
            }
        }

        public static string sp_GetMaxStockNo
        {
            get
            {
                return CallStoreBySystem("USP_BS_GETMAXT_STCKNO_DOUPLICATE", "'{0}'");
            }
        }

        public static string sp_LoadLotItem
        {
            get
            {
                return CallStoreBySystem("USP_BS_LOT_OINM", "'{0}', '{1}', {2}");
            }
        }

        public static string sp_OutStockRequestGetApplySAP
        {
            get
            {
                return CallStoreBySystem("USP_BS_STOCKOUTREQUEST_GETAPPLYSAP", "'{0}', {1}");
            }
        }
    }
}
