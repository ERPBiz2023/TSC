using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTCore;
using GTCore.Forms;
using SAPbouiCOM.Framework;

namespace GVTBetagen.Forms
{
    [FormAttribute("GVTBetagen.Forms.SalesTarget", "Forms/SalesTarget/SalesTarget.b1f")]
    public partial class SalesTarget : UserFormBase
    {
        private SalesTarget()
        {
            UserName = Application.SBO_Application.Company.UserName;
        }

        /// <summary>
        /// 
        /// </summary>
        private static SalesTarget instance;

        public static bool IsFormOpen = false;

        /// <summary>
        /// Show form, only instance to be created and user for system
        /// </summary>
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new SalesTarget();
                instance.InitControl();
                instance.Show();
                IsFormOpen = true;
            }
        }

        /// <summary>
        /// Init control and bind data source to control when open form
        /// </summary>
        private void InitControl()
        {
            this.cbbYear.Select(DateTime.Now.Year.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbYear.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

            this.cbbMon.Select(DateTime.Now.Month.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbMon.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

            this.cbbWeek.Select("1", SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbWeek.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

            this.LoadComboboxSalesManagers();
            this.cbbSalesManager.Item.DisplayDesc = true;

            this.LoadComboboxKAASM();
            this.cbbKA_ASM.Item.DisplayDesc = true;

            this.LoadComboboxSalesSups();
            this.cbbSalesSup.Item.DisplayDesc = true;

            this.LoadComboboxTeamLeaders();
            this.cbbTeamLeader.Item.DisplayDesc = true;
        }

        

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.cbbMon = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbMon").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.cbbYear = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbYea").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.cbbWeek = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbWee").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.cbbSalesManager = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbSMa").Specific));
            this.cbbSalesManager.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cbbSalesManager_ComboSelectAfter);
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));
            this.cbbKA_ASM = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbKA").Specific));
            this.cbbKA_ASM.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cbbKA_ASM_ComboSelectAfter);
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_16").Specific));
            this.cbbSalesSup = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbSSu").Specific));
            this.cbbSalesSup.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cbbSalesSup_ComboSelectAfter);
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_18").Specific));
            this.cbbTeamLeader = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbTLe").Specific));
            this.btnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSear").Specific));
            this.btnSearch.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnSearch_ClickBefore);
            this.grData = ((SAPbouiCOM.Grid)(this.GetItem("grData").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCan").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_24").Specific));
            this.edFile = ((SAPbouiCOM.EditText)(this.GetItem("edFil").Specific));
            this.btnFindFile = ((SAPbouiCOM.Button)(this.GetItem("btnFind").Specific));
            this.cbkAllSKU = ((SAPbouiCOM.CheckBox)(this.GetItem("cbkAll").Specific));
            this.cbkFocusSKU = ((SAPbouiCOM.CheckBox)(this.GetItem("cbkFoc").Specific));
            this.btnImportExcel = ((SAPbouiCOM.Button)(this.GetItem("btnIEx").Specific));
            this.btnExportExcel = ((SAPbouiCOM.Button)(this.GetItem("btnEEx").Specific));
            this.btnApprove = ((SAPbouiCOM.Button)(this.GetItem("btnApr").Specific));
            this.btnCopy = ((SAPbouiCOM.Button)(this.GetItem("btnCop").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.CloseAfter += new SAPbouiCOM.Framework.FormBase.CloseAfterHandler(this.Form_CloseAfter);
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {
            SetControlLocation();
        }

        private SAPbouiCOM.ComboBox cbbMon;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.ComboBox cbbYear;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.ComboBox cbbWeek;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.ComboBox cbbSalesManager;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.ComboBox cbbKA_ASM;
        private SAPbouiCOM.StaticText StaticText8;
        private SAPbouiCOM.ComboBox cbbSalesSup;
        private SAPbouiCOM.StaticText StaticText9;
        private SAPbouiCOM.ComboBox cbbTeamLeader;
        private SAPbouiCOM.Button btnSearch;
        private SAPbouiCOM.Grid grData;
        private SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.Button btnCancel;
        private SAPbouiCOM.StaticText StaticText10;
        private SAPbouiCOM.EditText edFile;
        private SAPbouiCOM.Button btnFindFile;
        private SAPbouiCOM.CheckBox cbkAllSKU;
        private SAPbouiCOM.CheckBox cbkFocusSKU;
        private SAPbouiCOM.Button btnImportExcel;
        private SAPbouiCOM.Button btnExportExcel;
        private SAPbouiCOM.Button btnApprove;
        private SAPbouiCOM.Button btnCopy;

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            instance = null;
            IsFormOpen = false;

        }
        
        /// <summary>
        ///  Freeze and Unfreeze from while execute processing
        ///  Avoid the form to be lag
        /// </summary>
        /// <param name="freeze"></param>
        private void Freeze(bool freeze)
        {
            if(freeze)
                UIHelper.Freeze(this.UIAPIRawForm);
            else
                UIHelper.UnFreeze(this.UIAPIRawForm);
        }

        private void cbbSalesManager_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.Freeze(true);
            this.LoadComboboxKAASM();
            this.Freeze(false);
        }

        private void cbbKA_ASM_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.Freeze(true);
            this.LoadComboboxSalesSups();
            this.Freeze(false);
        }

        private void cbbSalesSup_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.Freeze(true);
            this.LoadComboboxTeamLeaders();
            this.Freeze(false);
        }

        private void btnSearch_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            try
            {
                if(string.IsNullOrEmpty(this.SalesManagerSelected) || this.SalesManagerSelected == "All")
                {
                    UIHelper.LogMessage(string.Format("Please select sale manager."), UIHelper.MsgType.StatusBar, true);
                    this.Freeze(false);
                    return;
                }
                int week;

                if(string.IsNullOrEmpty(cbbWeek.Selected.Value ) 
                    || cbbWeek.Selected.Value == "All"
                    || !int.TryParse(cbbWeek.Selected.Value, out week))
                {
                    week = -1;
                }

                int month;
                if(!int.TryParse(cbbMon.Selected.Value, out month))
                {
                    month = DateTime.Now.Month;
                }

                int year;
                if (!int.TryParse(cbbYear.Selected.Value, out year))
                {
                    year = DateTime.Now.Year;
                }

                var query = string.Format(Querystring.sp_SaleTarget_LoadbyUserId, 
                            UserName,
                            SalesManagerSelected,
                            KASelected,
                            SalesSupSelected,
                            TeamleaderSelected,
                            month, year, week);
                this.grData.DataTable.ExecuteQuery(query);
                //Hashtable[] datas;
                //using (var connection = Globals.DataConnection)
                //{
                //    datas = connection.ExecQueryToArrayHashtable(query);
                //    connection.Dispose();
                //}
                //if (datas == null || datas.Count() <= 0)
                //{
                //    return;
                //}



                query = string.Format(Querystring.sp_SaleTarget_TargetID_Approved, month, year, SalesManagerSelected);
                Hashtable data;
                using (var connection = Globals.DataConnection)
                {
                    data = connection.ExecQueryToHashtable(query);
                    connection.Dispose();
                }
                var result = "-1";
                if (data != null)
                {
                    result = data["Result"].ToString();                   
                }
                if (result != "-1")
                {
                    UIHelper.LogMessage("This target is approved.", UIHelper.MsgType.StatusBar, false);

                    this.grData.Columns.Item("SSAmount").Editable = false;
                    this.grData.Columns.Item("KAAmount").Editable = false;
                    this.grData.Columns.Item("SMAmount").Editable = false;
                    this.grData.Columns.Item("GMAmount").Editable = false;
                    this.grData.Columns.Item("KSUSSAmount").Editable = false;
                    this.grData.Columns.Item("KSUKAAmount").Editable = false;
                    this.grData.Columns.Item("KSUSMAmount").Editable = false;
                    this.grData.Columns.Item("KSUGMAmount").Editable = false;
                }
                else
                {
                    this.EnableGridCol_byGroupPolicy();
                }
           
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Load data error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
            }
            this.Freeze(false);
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            SetControlLocation();
        }

        private void SetControlLocation()
        {
            //var max = ischange && !isFirst ? this.UIAPIRawForm.Height : this.UIAPIRawForm.MaxHeight;// : this.UIAPIRawForm.Height;
            var max = this.UIAPIRawForm.ClientHeight;
            var maxw = this.UIAPIRawForm.ClientWidth;

            this.btnSave.Item.Top = max - 30;
            this.btnCancel.Item.Top = this.btnSave.Item.Top;
            this.StaticText10.Item.Top = this.btnSave.Item.Top + 6;
            this.edFile.Item.Top = this.btnSave.Item.Top + 6;
            this.btnFindFile.Item.Top = this.btnSave.Item.Top;
            this.cbkAllSKU.Item.Top = this.btnSave.Item.Top + 6;
            this.cbkFocusSKU.Item.Top = this.btnSave.Item.Top + 6;
            this.cbkFocusSKU.Item.Left = this.cbkAllSKU.Item.Left + this.cbkAllSKU.Item.Width + 10;

            this.btnImportExcel.Item.Top = this.btnSave.Item.Top;
            this.btnExportExcel.Item.Top = this.btnSave.Item.Top;
            this.btnApprove.Item.Top = this.btnSave.Item.Top;
            this.btnCopy.Item.Top = this.btnSave.Item.Top;

            this.btnCopy.Item.Left = maxw - 20 - this.btnCopy.Item.Width;
            this.btnApprove.Item.Left = this.btnCopy.Item.Left - 10 - this.btnApprove.Item.Width;
            this.btnExportExcel.Item.Left = this.btnApprove.Item.Left - 10 - this.btnExportExcel.Item.Width;
            this.btnImportExcel.Item.Left = this.btnExportExcel.Item.Left - 10 - this.btnImportExcel.Item.Width;

            this.btnSearch.Item.Left = maxw - 20 - this.btnSearch.Item.Width;
            this.cbbTeamLeader.Item.Left = this.btnSearch.Item.Left - this.cbbTeamLeader.Item.Width - 10;
            this.cbbSalesSup.Item.Left = this.btnSearch.Item.Left - this.cbbSalesSup.Item.Width - 10;
            this.StaticText8.Item.Left = this.cbbSalesSup.Item.Left - this.StaticText8.Item.Width - 10;
            this.StaticText9.Item.Left = this.cbbTeamLeader.Item.Left - this.StaticText9.Item.Width - 10;

            this.cbbSalesManager.Item.Left = this.StaticText8.Item.Left - this.cbbSalesManager.Item.Width - 20;
            this.cbbKA_ASM.Item.Left = this.StaticText9.Item.Left - this.cbbKA_ASM.Item.Width - 20;
            this.StaticText4.Item.Left = this.cbbSalesManager.Item.Left - this.StaticText4.Item.Width - 10;
            this.StaticText5.Item.Left = this.cbbKA_ASM.Item.Left - this.StaticText5.Item.Width - 10;


            this.grData.Item.Top = 65;
            this.grData.Item.Left = 20;
            this.grData.Item.Height = this.btnSave.Item.Top - this.grData.Item.Top - 20;// this.btnSave.Item.Top - 20 - this.grData.Item.Top;
            this.grData.Item.Width = maxw - this.grData.Item.Left - 20;
        }
    }
}
