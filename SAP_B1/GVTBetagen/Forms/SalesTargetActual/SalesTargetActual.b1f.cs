using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTCore;
using GTCore.Forms;
using GTCore.Helper;
using SAPbouiCOM.Framework;

namespace GVTBetagen.Forms
{
    [FormAttribute("GVTBetagen.Forms.SalesTargetActual", "Forms/SalesTargetActual/SalesTargetActual.b1f")]
    public partial class SalesTargetActual : UserFormBase
    {
        private SalesTargetActual()
        {
            UserName = Application.SBO_Application.Company.UserName;
        }
        private static SalesTargetActual instance;

        public static bool IsFormOpen = false;
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new SalesTargetActual();
                instance.InitControl();
                instance.Show();
                IsFormOpen = true;
            }
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.stMon = ((SAPbouiCOM.StaticText)(this.GetItem("stMon").Specific));
            this.cbbMon = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbMon").Specific));
            this.stYear = ((SAPbouiCOM.StaticText)(this.GetItem("stYear").Specific));
            this.cbbYear = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbYea").Specific));
            this.stSaM = ((SAPbouiCOM.StaticText)(this.GetItem("stSaM").Specific));
            this.cbbSalesManager = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbSMa").Specific));
            this.stKA = ((SAPbouiCOM.StaticText)(this.GetItem("stKA").Specific));
            this.cbbKA_ASM = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbKA").Specific));
            this.stSS = ((SAPbouiCOM.StaticText)(this.GetItem("stSS").Specific));
            this.cbbSalesSup = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbSSu").Specific));
            this.stTL = ((SAPbouiCOM.StaticText)(this.GetItem("stTL").Specific));
            this.cbbTeamLeader = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbTLe").Specific));
            this.btnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.btnSearch.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnSearch_ClickBefore);
            this.grData = ((SAPbouiCOM.Grid)(this.GetItem("grData").Specific));
            this.grData.ClickAfter += new SAPbouiCOM._IGridEvents_ClickAfterEventHandler(this.Grid0_ClickAfter);
            this.btnEEX = ((SAPbouiCOM.Button)(this.GetItem("btnEEX").Specific));
            this.btnEEX.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnEEX_ClickBefore);
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.btnCancel.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCancel_ClickBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.LoadAfter += new SAPbouiCOM.Framework.FormBase.LoadAfterHandler(this.Form_LoadAfter);
            this.CloseAfter += new SAPbouiCOM.Framework.FormBase.CloseAfterHandler(this.Form_CloseAfter);
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private SAPbouiCOM.StaticText stMon;

        private void OnCustomInitialize()
        {
            SetControlLocation();
        }

        private void InitControl()
        {
            this.cbbYear.Select(DateTime.Now.Year.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbYear.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

            this.cbbMon.Select(DateTime.Now.Month.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbMon.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

            UIHandler.LoadComboboxSalesManagers(this.cbbSalesManager, UserName);
            //this.LoadComboboxSalesManagers();
            this.cbbSalesManager.Item.DisplayDesc = true;

            UIHandler.LoadComboboxKAASM(this.cbbKA_ASM, UserName, this.SalesManagerSelected);
            // this.LoadComboboxKAASM();
            this.cbbKA_ASM.Item.DisplayDesc = true;

            UIHandler.LoadComboboxSalesSups(this.cbbSalesSup, UserName, this.SalesManagerSelected, this.KASelected);
            // this.LoadComboboxSalesSups();
            this.cbbSalesSup.Item.DisplayDesc = true;

            UIHandler.LoadComboboxTeamLeaders(this.cbbTeamLeader, UserName, this.SalesManagerSelected, this.KASelected, this.SalesSupSelected);
            //this.LoadComboboxTeamLeaders();
            this.cbbTeamLeader.Item.DisplayDesc = true;
        }


        private SAPbouiCOM.ComboBox cbbMon;
        private SAPbouiCOM.StaticText stYear;
        private SAPbouiCOM.ComboBox cbbYear;
        private SAPbouiCOM.StaticText stSaM;
        private SAPbouiCOM.ComboBox cbbSalesManager;
        private SAPbouiCOM.StaticText stKA;
        private SAPbouiCOM.ComboBox cbbKA_ASM;
        private SAPbouiCOM.StaticText stSS;
        private SAPbouiCOM.ComboBox cbbSalesSup;
        private SAPbouiCOM.StaticText stTL;
        private SAPbouiCOM.ComboBox cbbTeamLeader;
        private SAPbouiCOM.Button btnSearch;

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
           

        }

        private SAPbouiCOM.Grid grData;
        private SAPbouiCOM.Button btnEEX;
        private SAPbouiCOM.Button btnCancel;

        private void Grid0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
        }

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            instance = null;
            IsFormOpen = false;

        }
        private void Freeze(bool freeze)
        {
            this.UIAPIRawForm.Freeze(freeze);
        }

        private void SetControlLocation()
        {
            var max = this.UIAPIRawForm.ClientHeight;
            var maxw = this.UIAPIRawForm.ClientWidth;

            this.btnEEX.Item.Top = max - 30;
            this.btnCancel.Item.Left = maxw - 20 - this.btnCancel.Item.Width;

            this.btnSearch.Item.Left = maxw - 20 - this.btnSearch.Item.Width;
            this.cbbTeamLeader.Item.Left = this.btnSearch.Item.Left - this.cbbTeamLeader.Item.Width - 10;
            this.cbbSalesSup.Item.Left = this.btnSearch.Item.Left - this.cbbSalesSup.Item.Width - 10;
            this.stSS.Item.Left = this.cbbSalesSup.Item.Left - this.stSS.Item.Width - 10;
            this.stTL.Item.Left = this.cbbTeamLeader.Item.Left - this.stTL.Item.Width - 10;

            this.cbbSalesManager.Item.Left = this.stSS.Item.Left - this.cbbSalesManager.Item.Width - 20;
            this.cbbKA_ASM.Item.Left = this.stTL.Item.Left - this.cbbKA_ASM.Item.Width - 20;
            this.stSaM.Item.Left = this.cbbSalesManager.Item.Left - this.stSaM.Item.Width - 10;
            this.stKA.Item.Left = this.cbbKA_ASM.Item.Left - this.stKA.Item.Width - 10;


            this.grData.Item.Top = 65;
            this.grData.Item.Left = 20;
            this.grData.Item.Height = this.btnEEX.Item.Top - this.grData.Item.Top - 20;// this.btnSave.Item.Top - 20 - this.grData.Item.Top;
            this.grData.Item.Width = maxw - this.grData.Item.Left - 20;
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            SetControlLocation();
        }

        private void btnSearch_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
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

                query = string.Format(Querystring.Update_SalesManager_Division, this.SalesManagerSelected);
                Hashtable data;
                using (var connection = Globals.DataConnection)
                {
                    data = connection.ExecQueryToHashtable(query);
                    connection.Dispose();
                }
                var result = string.Empty;
                if (data != null)
                {
                    result = data["Result"].ToString();                    
                }

                if(result == "GT")
                {

                }
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(ex.Message, UIHelper.MsgType.StatusBar, true);
            }
        }

        private void btnCancel_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.UIAPIRawForm.Close();
        }

        private void btnEEX_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            try
            {
                if (this.grData.DataTable.Rows.Count <= 0)
                {
                    UIHelper.LogMessage(string.Format("Data is empty"), UIHelper.MsgType.StatusBar, true);
                    this.Freeze(false);
                    return;
                }

                var message = string.Empty;
                this.grData.ExportToExcel(string.Format("SalesTargetActual_{0}_{1}", UserName, DateTime.Now.ToString("yyyyMMdd")), ref message);
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(ex.Message, UIHelper.MsgType.StatusBar, true);
            }
            this.Freeze(false);

        }
    }
}
