using GTCore.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Forms
{
    public class GlobalFormsInformations
    {
        #region GLPosting
        private static AddonUserForm glPostingInformation;
        private static void InitGLPostingInformation()
        {
            glPostingInformation = new AddonUserForm();
            glPostingInformation.FormID = "GLAllPosting_F";
            glPostingInformation.MenuID = "GLAlPosting_M";
            glPostingInformation.MenuName = "Postting/Re-Allocate GL";
            glPostingInformation.ParentID = "1536";
        }
        public static AddonUserForm GLPostingInformation
        {
            get
            {
                if (glPostingInformation == null)
                {
                    InitGLPostingInformation();
                }
                return glPostingInformation;
            }
        }
        public static string GLPostingMenuID
        {
            get
            {
                if (glPostingInformation == null)
                {
                    InitGLPostingInformation();
                }
                return glPostingInformation.MenuID;
            }
        }
        #endregion

        #region SalesTarget
        private static void InitSalesTargetInformation()
        {
            salesTargetInformation = new AddonUserForm();
            salesTargetInformation.FormID = "SalesTarget_F";
            salesTargetInformation.MenuID = "SalesTarget_M";
            salesTargetInformation.MenuName = "Sales Target";
            salesTargetInformation.ParentID = "2048"; // Inventory Transactions
        }
        private static AddonUserForm salesTargetInformation;
        public static AddonUserForm SalesTargetInformation
        {
            get
            {
                if (salesTargetInformation == null)
                {
                    InitSalesTargetInformation();
                }
                return salesTargetInformation;
            }
        }
        public static string SalesTargetMenuID
        {
            get
            {
                if (salesTargetInformation == null)
                {
                    InitSalesTargetInformation();
                }
                return salesTargetInformation.MenuID;
            }
        }
        #endregion

        #region SalesTargetActual
        private static void InitSalesTargetActualInformation()
        {
            salesTargetActualInformation = new AddonUserForm();
            salesTargetActualInformation.FormID = "SalesTargetActual_F";
            salesTargetActualInformation.MenuID = "SalesTargetActual_M";
            salesTargetActualInformation.MenuName = "Sales Target Actual";
            salesTargetActualInformation.ParentID = "2048"; // Inventory Transactions
        }
        private static AddonUserForm salesTargetActualInformation;
        public static AddonUserForm SalesTargetActualInformation
        {
            get
            {
                if (salesTargetActualInformation == null)
                {
                    InitSalesTargetActualInformation();
                }
                return salesTargetActualInformation;
            }
        }
        public static string SalesTargetActualMenuID
        {
            get
            {
                if (salesTargetActualInformation == null)
                {
                    InitSalesTargetActualInformation();
                }
                return salesTargetActualInformation.MenuID;
            }
        }
        #endregion

        #region Import && Export
        private static void InitImExpInformation()
        {
            importExportInformation = new AddonUserForm();
            importExportInformation.FormID = "Import_Export_F";
            importExportInformation.MenuID = "Import_Export_M";
            importExportInformation.MenuName = "Import & Export Excel";
            importExportInformation.ParentID = "GTV_Addon"; // Inventory Transactions
        }
        private static AddonUserForm importExportInformation;
        public static AddonUserForm ImportExportInformation
        {
            get
            {
                if (importExportInformation == null)
                {
                    InitImExpInformation();
                }
                return importExportInformation;
            }
        }
        public static string ImportExportMenuID
        {
            get
            {
                if (importExportInformation == null)
                {
                    InitImExpInformation();
                }
                return importExportInformation.MenuID;
            }
        }
        #endregion
    }
}
