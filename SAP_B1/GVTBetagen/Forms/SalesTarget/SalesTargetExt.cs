using GTCore;
using GVTBetagen.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Forms
{
    public partial class SalesTarget
    {
        private string UserName;

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
        /// Get Week selected in form
        /// </summary>
        private int WeekSelected
        {
            get
            {
                int week;

                if (string.IsNullOrEmpty(cbbWeek.Selected.Value)
                    || cbbWeek.Selected.Value == "All"
                    || !int.TryParse(cbbWeek.Selected.Value, out week))
                {
                    week = -1;
                }
                return week;
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
        /// <summary>
        /// Load data source to combox Sales Managers
        /// Call store from Database 
        /// </summary>
        public void LoadComboboxSalesManagers()
        {
            var query = string.Format(Querystring.sp_GetSaleManagerByUser, UserName);
            Hashtable[] datas;
            using (var connection = Globals.DataConnection)
            {
                datas = connection.ExecQueryToArrayHashtable(query);
                connection.Dispose();
            }
            if (datas == null || datas.Count() <= 0)
            {
                return;
            }

            for (int i = cbbSalesManager.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbSalesManager.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            foreach (var data in datas)
            {
                cbbSalesManager.ValidValues.Add(data["ExtEmpNo"].ToString(), data["FullName"].ToString());
                cbbSalesManager.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
            }
            cbbSalesManager.Select(datas[0]["ExtEmpNo"], SAPbouiCOM.BoSearchKey.psk_ByValue);
        }

        /// <summary>
        /// Load data source for KA/ASM combobox
        /// </summary>
        public void LoadComboboxKAASM()
        {
            var query = string.Format(Querystring.sp_GetKA_ASMByUser, UserName, this.SalesManagerSelected);
            Hashtable[] datas;
            using (var connection = Globals.DataConnection)
            {
                datas = connection.ExecQueryToArrayHashtable(query);
                connection.Dispose();
            }
            if (datas == null || datas.Count() <= 0)
            {
                return;
            }

            for (int i = cbbKA_ASM.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbKA_ASM.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            foreach (var data in datas)
            {
                cbbKA_ASM.ValidValues.Add(data["ExtEmpNo"].ToString(), data["FullName"].ToString());
                cbbKA_ASM.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
            }
            cbbKA_ASM.Select(datas[0]["ExtEmpNo"], SAPbouiCOM.BoSearchKey.psk_ByValue);
        }

        /// <summary>
        /// Load data source for Sales sup combobox
        /// </summary>
        public void LoadComboboxSalesSups()
        {
            var query = string.Format(Querystring.sp_GetSalesSupByUser, UserName, this.SalesManagerSelected, this.KASelected);
            Hashtable[] datas;
            using (var connection = Globals.DataConnection)
            {
                datas = connection.ExecQueryToArrayHashtable(query);
                connection.Dispose();
            }
            if (datas == null || datas.Count() <= 0)
            {
                return;
            }

            for (int i = cbbSalesSup.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbSalesSup.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            foreach (var data in datas)
            {
                cbbSalesSup.ValidValues.Add(data["ExtEmpNo"].ToString(), data["FullName"].ToString());
                cbbSalesSup.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
            }
            cbbSalesSup.Select(datas[0]["ExtEmpNo"], SAPbouiCOM.BoSearchKey.psk_ByValue);
        }

        /// <summary>
        /// Load data source for Team leader combobox
        /// </summary>
        public void LoadComboboxTeamLeaders()
        {
            var query = string.Format(Querystring.sp_GetTeamLeaderByUser, UserName, this.SalesManagerSelected, this.KASelected, this.SalesSupSelected);
            Hashtable[] datas;
            using (var connection = Globals.DataConnection)
            {
                datas = connection.ExecQueryToArrayHashtable(query);
                connection.Dispose();
            }
            if (datas == null || datas.Count() <= 0)
            {
                return;
            }

            for (int i = cbbTeamLeader.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbTeamLeader.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            foreach (var data in datas)
            {
                cbbTeamLeader.ValidValues.Add(data["ExtEmpNo"].ToString(), data["FullName"].ToString());
                cbbTeamLeader.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
            }
            cbbTeamLeader.Select(datas[0]["ExtEmpNo"], SAPbouiCOM.BoSearchKey.psk_ByValue);
        }

        /// <summary>
        /// Enable policygroup
        /// </summary>
        private void EnableGridCol_byGroupPolicy()
        {
        //    if(InitConfig.UserId_Pub == -1)
        //    {
        //        var message = string.Empty;
        //        if (!SystemInformation.CurrentAccountConnectionInfo(ref message))
        //        {
        //            UIHelper.LogMessage(message, UIHelper.MsgType.StatusBar, true);
        //           // System.Windows.Forms.MessageBox.Show(message);
        //        }
        //    }
            var query = string.Format(Querystring.sp_GetIncentiveUser, InitConfig.UserId_Pub);
            Hashtable data;
            using (var connection = Globals.DataConnection)
            {
                data = connection.ExecQueryToHashtable(query);
                connection.Dispose();
            }
            short userApproved = -1;
            if (data != null)
            {
                short.TryParse(data["Result"].ToString(), out userApproved);
            }
            this.btnApprove.Item.Enabled = false;
            this.btnSave.Item.Enabled = true;
            this.btnImportExcel.Item.Enabled = true;

            switch (InitConfig.GroupPolicy)
            {
                case 0:
                    this.grData.Columns.Item("SSAmount").Editable = false;
                    this.grData.Columns.Item("KAAmount").Editable = false;
                    this.grData.Columns.Item("SMAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;
                    this.grData.Columns.Item("GMAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;//this.GridCustomer.RootTable.Columns["GMAmount"].EditType = EditType.TextBox;
                    this.grData.Columns.Item("KSUSSAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;//this.GridCustomer.RootTable.Columns["KSUSSAmount"].EditType = EditType.TextBox;
                    this.grData.Columns.Item("KSUKAAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;//this.GridCustomer.RootTable.Columns["KSUKAAmount"].EditType = EditType.TextBox;
                    this.grData.Columns.Item("KSUSMAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;//this.GridCustomer.RootTable.Columns["KSUSMAmount"].EditType = EditType.TextBox;
                    this.grData.Columns.Item("KSUGMAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText; //this.GridCustomer.RootTable.Columns["KSUGMAmount"].EditType = EditType.TextBox;
                    this.btnCopy.Item.Visible = true;
                    this.btnCopy.Item.Description = "Copy Target from Sale Manager";
                    if (userApproved != (short)1)
                        break;
                    this.btnApprove.Item.Enabled = true;
                    break;
                case 1:
                    this.grData.Columns.Item("SSAmount").Editable = false;
                    this.grData.Columns.Item("KAAmount").Editable = false;
                    this.grData.Columns.Item("SMAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;
                    this.grData.Columns.Item("GMAmount").Editable = false;
                    this.grData.Columns.Item("KSUSSAmount").Editable = false;
                    this.grData.Columns.Item("KSUKAAmount").Editable = false;
                    this.grData.Columns.Item("KSUSMAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;
                    this.grData.Columns.Item("KSUGMAmount").Editable = false;
                    this.btnCopy.Item.Visible = true;
                    this.btnCopy.Item.Description = "Copy Target from  KA/ASM";
                    break;
                case 2:
                    this.grData.Columns.Item("SSAmount").Editable = false;
                    this.grData.Columns.Item("KAAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;
                    this.grData.Columns.Item("SMAmount").Editable = false;
                    this.grData.Columns.Item("GMAmount").Editable = false;
                    this.grData.Columns.Item("KSUSSAmount").Editable = false;
                    this.grData.Columns.Item("KSUKAAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;//this.GridCustomer.RootTable.Columns["KSUKAAmount"].EditType = EditType.TextBox;
                    this.grData.Columns.Item("KSUSMAmount").Editable = false;
                    this.grData.Columns.Item("KSUGMAmount").Editable = false;
                    this.btnCopy.Item.Visible = true;
                    this.btnCopy.Item.Description = "Copy Target from  Sales sup";
                    break;
                case 3:
                    this.grData.Columns.Item("SSAmount").Editable = false;
                    this.grData.Columns.Item("KAAmount").Editable = false;
                    this.grData.Columns.Item("SMAmount").Editable = false;
                    this.grData.Columns.Item("GMAmount").Editable = false;
                    this.grData.Columns.Item("KSUSSAmount").Editable = false;
                    this.grData.Columns.Item("KSUKAAmount").Editable = false;
                    this.grData.Columns.Item("KSUSMAmount").Editable = false;
                    this.grData.Columns.Item("KSUGMAmount").Editable = false;
                    this.btnCopy.Item.Visible = false;
                    this.btnSave.Item.Enabled = false;
                    this.btnImportExcel.Item.Enabled = false;
                    break;
                case 4:
                    this.grData.Columns.Item("SSAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;
                    this.grData.Columns.Item("KAAmount").Editable = false;
                    this.grData.Columns.Item("SMAmount").Editable = false;
                    this.grData.Columns.Item("GMAmount").Editable = false;
                    this.grData.Columns.Item("KSUSSAmount").Type = SAPbouiCOM.BoGridColumnType.gct_EditText;
                    this.grData.Columns.Item("KSUKAAmount").Editable = false;
                    this.grData.Columns.Item("KSUSMAmount").Editable = false;
                    this.grData.Columns.Item("KSUGMAmount").Editable = false;
                    this.btnCopy.Item.Visible = false;
                    break;
                case 5:
                    this.grData.Columns.Item("SSAmount").Editable = false;
                    this.grData.Columns.Item("KAAmount").Editable = false;
                    this.grData.Columns.Item("SMAmount").Editable = false;
                    this.grData.Columns.Item("GMAmount").Editable = false;
                    this.grData.Columns.Item("KSUSSAmount").Editable = false;
                    this.grData.Columns.Item("KSUKAAmount").Editable = false;
                    this.grData.Columns.Item("KSUSMAmount").Editable = false;
                    this.grData.Columns.Item("KSUGMAmount").Editable = false;
                    this.btnCopy.Item.Visible = false;
                    this.btnSave.Item.Enabled = false;
                    this.btnImportExcel.Item.Enabled = false;
                    break;
            }
        }
    }
}
