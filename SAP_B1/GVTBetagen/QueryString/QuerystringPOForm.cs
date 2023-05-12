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
