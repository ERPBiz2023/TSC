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

        /// <summary>
        /// Call Query by system SAP HANA or SQL
        /// Syntax SQL : EXEC [query] param1, param1;
        /// </summary>: CALL "[schema]"."[query]" (param1, param1);
        /// <param name="query">Store name</param>
        /// <param name="param">Parameter of store</param>
        /// <returns></returns>
        private static string CallStoreBySystem(string query, string param = "")
        {
            if (CoreSetting.System == SystemType.SAP_HANA)
            {
                var schema = ConfigurationManager.AppSettings["Schema"];
                return "CALL \"" + schema + "\".\"" + query + "\" (" + param + ")";
            }
            else
            {
                return "EXEC " + query + " " + param;
            }
        }

        /// <summary>
        /// USP_BS_STOCKOUTREQUEST  "'{0}','{1}','{2}', {3}"
        /// </summary>
        public static string sp_LoadOutStockRequest
        {
           
            get
            {
                return CallStoreBySystem("USP_BS_STOCKOUTREQUEST", "'{0}','{1}','{2}', {3}");
            }
        }

        /// <summary>
        /// USP_BS_STOCKOUTREQUEST2_DELETE "'{0}'"
        /// </summary>
        public static string sp_DeleteOutStockRequest
        {
            get
            {
                return CallStoreBySystem("USP_BS_STOCKOUTREQUEST2_DELETE", "'{0}'");
            }
        }

        /// <summary>
        /// usp_Notification_UpdateStock
        /// </summary>
        public static string sp_NotificationUpdateStock
        {
            get
            {
                return CallStoreBySystem("usp_Notification_UpdateStock");
            }
        }

        /// <summary>
        /// usp_BS_STOCKOUTREQUEST_LoadbyID "'{0}'"
        /// </summary>
        public static string sp_LoadOutStockRequestByID
        {
            get
            {
                return CallStoreBySystem("usp_BS_STOCKOUTREQUEST_LoadbyID", "'{0}'");
            }
        }

        /// <summary>
        /// usp_BS_STOCKOUTREQUEST_DETAIL_LoadbyID "'{0}'"
        /// </summary>
        public static string sp_LoadOutStockRequestDetailByID
        {
            get
            {
                return CallStoreBySystem("usp_BS_STOCKOUTREQUEST_DETAIL_LoadbyID", "'{0}'");
            }
        }

        /// <summary>
        /// USP_BS_STOCKOUTREQUEST_APPLY_SAP "'{0}', {1}"
        /// </summary>
        public static string sp_OutStockRequestApplySAP
        {
            get
            {
                return CallStoreBySystem("USP_BS_STOCKOUTREQUEST_APPLY_SAP", "'{0}', {1}");
            }
        }

        /// <summary>
        /// usp_BS_InventoryTransferReq_Confirm "'{0}', {1}"
        /// </summary>
        public static string sp_OutStockRequestConfirm
        {
            get
            {
                return CallStoreBySystem("usp_BS_InventoryTransferReq_Confirm", "'{0}', {1}");
            }
        }

        /// <summary>
        /// USP_BS_OWHS
        /// </summary>
        public static string sp_GetWarehouses
        {
            get
            {
                return CallStoreBySystem("USP_BS_OWHS", "");
            }
        }

        /// <summary>
        /// USP_BS_BINCODE "'{0}'"
        /// </summary>
        public static string sp_GetBins
        {
            get
            {
                return CallStoreBySystem("USP_BS_BINCODE", "'{0}'");
            }
        }

        /// <summary>
        /// USP_BS_GETMAXT_STCKNO_DOUPLICATE "'{0}'"
        /// </summary>
        public static string sp_GetMaxStockNo
        {
            get
            {
                return CallStoreBySystem("USP_BS_GETMAXT_STCKNO_DOUPLICATE", "'{0}'");
            }
        }

        /// <summary>
        /// USP_BS_LOT_OINM "'{0}', '{1}', {2}"
        /// </summary>
        public static string sp_LoadLotItem
        {
            get
            {
                return CallStoreBySystem("USP_BS_LOT_OINM", "'{0}', '{1}', {2}");
            }
        }

        /// <summary>
        /// USP_BS_STOCKOUTREQUEST_GETAPPLYSAP "'{0}', {1}"
        /// </summary>
        public static string sp_OutStockRequestGetApplySAP
        {
            get
            {
                return CallStoreBySystem("USP_BS_STOCKOUTREQUEST_GETAPPLYSAP", "'{0}', {1}");
            }
        }

        /// <summary>
        /// usp_BS_PO_LoadbyEntry "{0}"
        /// </summary>
        public static string sp_GetPOByDocEntry
        {
            get
            {
                return CallStoreBySystem("usp_BS_PO_LoadbyEntry", "{0}");
            }
        }

        /// <summary>
        /// USP_BS_PO_DateAllocate_LoadItem
        /// </summary>
        public static string sp_GetAllItemToCombobox
        {
            get
            {
                return CallStoreBySystem("USP_BS_PO_DateAllocate_LoadItem", "{0}");
            }
        }

        /// <summary>
        /// USP_BS_PO_DateAllocate_LoadBIN
        /// </summary>
        public static string sp_GetAllBinToCombobox
        {
            get
            {
                return CallStoreBySystem("USP_BS_PO_DateAllocate_LoadBIN", "");
            }
        }

        /// <summary>
        /// USP_BS_PO_DateAllocate_LoadPOLotNobyEntry "'{0}'"
        /// </summary>
        public static string sp_LoadPOAllocate
        {
            get
            {
                return CallStoreBySystem("USP_BS_PO_DateAllocate_LoadPOLotNobyEntry", "'{0}'");
            }
        }

        /// <summary>
        /// usp_BS_PO_DateAllocate_ImportInfo_DeleteBefAdd "{0}"
        /// </summary>
        public static string sp_POAllocateImportInfo_DeleteBefAdd
        {
            get
            {
                return CallStoreBySystem("usp_BS_PO_DateAllocate_ImportInfo_DeleteBefAdd", "{0}");
            }
        }

        /// <summary>
        /// usp_BS_PO_DateAllocate_ImportInfo_Add "{0}, '{1}', '{2}', '{3}', '{4}', {5}"
        /// </summary>
        public static string sp_POAllocateImportInfo_Add
        {
            get
            {
                return CallStoreBySystem("usp_BS_PO_DateAllocate_ImportInfo_Add", "{0}, '{1}', '{2}', '{3}', '{4}', {5}");
            }
        }

        /// <summary>
        /// usp_BS_PO_DateAllocate_ImportInfo_POLotNo_Add "{0}, '{1}'
        /// </summary>
        public static string sp_POAllocateImportInfo_LotNoAdd
        {
            get
            {
                return CallStoreBySystem("usp_BS_PO_DateAllocate_ImportInfo_POLotNo_Add", "{0}, '{1}'");
            }
        }
    }
}
