using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTCore.Forms;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon.Forms
{
    [FormAttribute("BetagenSBOAddon.Forms.SalesTarget", "Forms/SalesTarget.b1f")]
    class SalesTarget : UserFormBase
    {
        private string UserName;
        private SalesTarget()
        {
            UserName = Application.SBO_Application.Company.UserName;
        }
        //private static AddonUserForm information;
        //public static AddonUserForm Information
        //{
        //    get
        //    {
        //        if (information == null)
        //        {
        //            information = new AddonUserForm();
        //            information.FormID = "SalesTarget_F";
        //            information.MenuID = "SalesTarget_M";
        //            information.MenuName = "Sales Target";
        //            information.ParentID = "2048"; // Inventory Transactions
        //        }
        //        return information;
        //    }
        //}
        private static SalesTarget instance;

        public static bool IsFormOpen = false;
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

        private void InitControl()
        {
            this.cbbYear.Select(DateTime.Now.Year.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbYear.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

            this.cbbMon.Select(DateTime.Now.Month.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbMon.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

            this.cbbWeek.Select("1", SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbWeek.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;
           // this.cbbWeek.Item.DisplayDesc = true;
        }

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
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));
            this.cbbKA_ASM = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbKA").Specific));
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_16").Specific));
            this.cbbSaesSup = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbSSu").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_18").Specific));
            this.cbbTeamLeader = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbTLe").Specific));
            this.btnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSear").Specific));
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
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {

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
        private SAPbouiCOM.ComboBox cbbSaesSup;
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

        private void Freeze(bool freeze)
        {
            this.UIAPIRawForm.Freeze(freeze);
        }
    }
}
