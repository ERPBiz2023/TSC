using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Forms
{
    public partial class SalesTargetActual
    {
        private string UserName;
        private System.Data.DataTable DataLoad;

        /// <summary>
        /// Current selected Sales Manager
        /// </summary>
        private string SalesManagerSelected
        {
            get
            {
                if (cbbSalesManager.Selected != null && cbbSalesManager.Selected?.Value != "All")
                    return cbbSalesManager.Selected?.Value;
                return "";
            }
        }

        /// <summary>
        /// Current selected KA/ASM
        /// </summary>
        private string KASelected
        {
            get
            {
                if (cbbKA_ASM.Selected != null && cbbKA_ASM.Selected?.Value != "All")
                    return cbbKA_ASM.Selected?.Value;
                return "";
            }
        }


        /// <summary>
        /// Current selected Sales Supervizer
        /// </summary>
        private string SalesSupSelected
        {
            get
            {
                if (cbbSalesSup.Selected != null && cbbSalesSup.Selected?.Value != "All")
                    return cbbSalesSup.Selected?.Value;
                return "";
            }
        }

        /// <summary>
        /// Current selected Team Leader
        /// </summary>
        private string TeamleaderSelected
        {
            get
            {
                if (cbbTeamLeader.Selected != null && cbbTeamLeader.Selected?.Value != "All")
                    return cbbTeamLeader.Selected?.Value;
                return "";
            }
        }

        
        /// <summary>
        ///  Get month selected in form
        /// </summary>
        private int MonthSelected
        {
            get
            {
                int month;
                if (!int.TryParse(cbbMon.Selected.Value, out month))
                {
                    month = DateTime.Now.Month;
                }
                return month;
            }
        }

        /// <summary>
        ///  get year selected in form
        /// </summary>
        private int YearSelected
        {
            get
            {
                int year;
                if (!int.TryParse(cbbYear.Selected.Value, out year))
                {
                    year = DateTime.Now.Year;
                }
                return year;
            }
        }

        public void LoadDataGrid()
        {
            var query = string.Format(Querystring.sp_SaleTarget_Actual_LoadbyUserId,
                            UserName,
                            SalesManagerSelected,
                            KASelected,
                            SalesSupSelected,
                            TeamleaderSelected,
                            MonthSelected,
                            YearSelected);

            this.grData.DataTable.ExecuteQuery(query);
            this.grData.Columns.Item("TargetDID").Visible = false;
            this.grData.Columns.Item("TargetId").Visible = false;
            this.grData.Columns.Item("CustCode").TitleObject.Caption = "Code";
            this.grData.Columns.Item("CustName").TitleObject.Caption = "Name";
            this.grData.Columns.Item("Channel").Visible = false;
            this.grData.Columns.Item("GroupName").TitleObject.Caption = "Group";
            this.grData.Columns.Item("SaleRepEmpid").TitleObject.Caption = "PG/SR ID";
            this.grData.Columns.Item("SaleRepfullName").TitleObject.Caption = "PG/Sale Rep";
            this.grData.Columns.Item("SMEmpId").TitleObject.Caption = "Sales Manager";
            this.grData.Columns.Item("SMEmpFullName").Visible = false;
            this.grData.Columns.Item("KAEmpId").TitleObject.Caption = "KA/ASM";
            this.grData.Columns.Item("KAEmpFullName").Visible = false;
            this.grData.Columns.Item("SSEmpId").TitleObject.Caption = "Sales sup";
            this.grData.Columns.Item("SSEmpFullName").Visible = false;
            this.grData.Columns.Item("TeamLeadID").TitleObject.Caption = "Team Leader";
            this.grData.Columns.Item("TeamLeadFullName").Visible = false;

            this.grData.Columns.Item("SSAmount").Visible = false;
            this.grData.Columns.Item("KAAmount").Visible = false;
            this.grData.Columns.Item("SMAmount").Visible = false;
            this.grData.Columns.Item("DocTotal").TitleObject.Caption = "Actual";
            this.grData.Columns.Item("GMAmount").TitleObject.Caption = "Target";
            this.grData.Columns.Item("TotalPercent").TitleObject.Caption = "% Target - Actual";
            this.grData.Columns.Item("OP").Visible = false;
            this.grData.Columns.Item("SaleActual").Visible = false;

            this.grData.Columns.Item("Descriptions").Visible = false;
            this.grData.Columns.Item("SS_SKUAmt").Visible = false;
            this.grData.Columns.Item("KA_SKUAmt").Visible = false;
            this.grData.Columns.Item("GM_SKUAmt").Visible = false;
            this.grData.Columns.Item("GM_SKUAmt").TitleObject.Caption = "SKU Target";
            this.grData.Columns.Item("SKU_DocTotal").TitleObject.Caption = "SKU Actual";
            this.grData.Columns.Item("SKU_TotalPercent").TitleObject.Caption = "SKU Percent";

            this.grData.Columns.Item("SKU_SaleActual").Visible = false;
            this.grData.Columns.Item("SKU_OB").Visible = false;
            this.grData.Columns.Item("SKUSaleActual").Visible = false;
            this.grData.Columns.Item("SKUDesc").Visible = false;

            this.grData.AutoResizeColumns();
        }
    }
}
