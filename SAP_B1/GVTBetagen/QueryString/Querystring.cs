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
        // functions
        // UF_BS_GETQUANTITYSTOCKOUTREQUEST
        // UF_BS_SETCOLOR_ITEMCODE
        // uv_Empoyee_Salary_Accounting
        //uv_SaleDetail_ByInvoice_SaleIncentive_SalesTarget
        //uv_SaleDetail_ByInvoice_SaleIncentive_Item

        // UDF Item U_Volumn decimal
        // UDF Line U_OrgTotal

        // FUNCTION: UF_BS_Customer_Incharge_DocDate

        /// <summary>
        /// Call Query by system SAP HANA or SQL
        /// Syntax SQL : EXEC [query] param1, param1;
        /// Syntax HANA: CALL "[schema]"."[query]" (param1, param1);
        /// </summary>: 
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
        /*
          "USP_BS_STOCKOUTREQUEST"
             ( IN StockType char,
              IN FromDate DATE,
              IN ToDate DATE,
              IN User INTEGER
            )
            CALL "USP_BS_STOCKOUTREQUEST" (1,'2022-4-17','2023-4-18',1)
         */
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
        /// usp_BS_Discount_SaleQuotation "{0}, '{1}'
        /// </summary>
        public static string sp_Discount_SaleQuotation
        {
            get
            {
                return CallStoreBySystem("usp_BS_Discount_SaleQuotation", "{0}, '{1}'");
            }
        }
        
        /// <summary>
        /// USP_BS_PurchaseOrder_LoadbyEntry "{0}"
        /// </summary>
        public static string sp_PurchaseOrder_LoadbyEntry
        {
            get
            {
                return CallStoreBySystem("USP_BS_PurchaseOrder_LoadbyEntry", "{0}");
            }
        }

        /// <summary>
        /// USP_BS_PurchaseOrderDetail_LoadbyEntry "{0}"
        /// </summary>
        public static string sp_PurchaseOrderDetail_LoadbyEntry
        {
            get
            {
                return CallStoreBySystem("USP_BS_PurchaseOrderDetail_LoadbyEntry", "{0}");
            }
        }

        /// <summary>
        /// USP_BS_PO_LotNo_LoadbyEntry "{0}, '{1}'
        /// </summary>
        public static string sp_PurchaseOrderDetail_LoadLotNo
        {
            get
            {
                return CallStoreBySystem("USP_BS_PO_LotNo_LoadbyEntry", "{0}, '{1}'");
            }
        }
        public static string sp_GetUserFromUserCode
        {
            get
            {
                return CallStoreBySystem("USP_BS_GETUSERID_FROMUSERCODE", "'{0}'");
            }
        }
        public static string sp_GetIncentiveUser
        {
            get
            {
                return CallStoreBySystem("usp_BS_Incentive_User", "{0}");
            }
        }
    }
}
