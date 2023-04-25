using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTCore;
using GTCore.Forms;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon.Forms
{
    [FormAttribute("BetagenSBOAddon.Forms.POAllocationBatch", "Forms/POAllocationBatch.b1f")]
    class POAllocationBatch : UserFormBase
    {
        public string PONo { get; set; }
        public string POEntry { get; set; }
        public string POStatus { get; set; }
        public string POConfirm { get; set; }
        public POAllocationBatch()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.edFile = ((SAPbouiCOM.EditText)(this.GetItem("edFile").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.btnFile = ((SAPbouiCOM.Button)(this.GetItem("btnFile").Specific));
            this.btnFile.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnFile_ClickBefore);
            this.btnImport = ((SAPbouiCOM.Button)(this.GetItem("btnImport").Specific));
            this.btnImport.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnImport_ClickBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.edPOEn = ((SAPbouiCOM.EditText)(this.GetItem("edPOEn").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.edPONo = ((SAPbouiCOM.EditText)(this.GetItem("edPONo").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.btnSave.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnSave_ClickBefore);
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.btnCancel.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCancel_ClickBefore);
            this.mtData = ((SAPbouiCOM.Matrix)(this.GetItem("mtData").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);

        }

        private static POAllocationBatch instance;

        public static bool IsFormOpen = false;
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new POAllocationBatch();
                instance.Show();
                IsFormOpen = true;
            }
        }
        public static void ShowForm(string poNo)
        {
            if (instance == null)
            {
                instance = new POAllocationBatch();
                instance.POEntry = poNo;
                instance.LoadData();

                instance.Show();
                IsFormOpen = true;
                // instance.edPOEn.Value = poNo;
            }
        }

        public void LoadData()
        {
            this.Freeze(true);
            if (string.IsNullOrEmpty(this.POEntry))
            {
                UIHelper.LogMessage("Please select Purchase Order to Allocate", UIHelper.MsgType.StatusBar, true);
                this.Freeze(false);
                return;
            }

            int po;
            if (!int.TryParse(this.POEntry, out po))
            {
                this.Freeze(false);
                return;
            }

            Hashtable data;
            var query = string.Format(Querystring.sp_GetPOByDocEntry, po);
            using (var connection = Globals.DataConnection)
            {
                data = connection.ExecQueryToHashtable(query);
                connection.Dispose();
            }

            if (data == null)
            {
                this.Freeze(false);

                return;
            }
            this.PONo = data["PONo"].ToString();
            this.POStatus = data["DocStatus"].ToString();
            this.edPOEn.Value = this.POEntry;
            this.edPONo.Value = this.PONo;

            LoadDataToGrid();
            this.Freeze(false);
        }

        private void LoadComboboxItem()
        {
            var query = Querystring.sp_GetAllItemToCombobox;
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

            for (int i = this.mtData.Columns.Item("clItemCode").ValidValues.Count - 1; i >= 0; i--)
            {
                this.mtData.Columns.Item("clItemCode").ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            // this.mtData.Columns.Item("clItemCode").ValidValues.a
            foreach (var data in datas)
            {
                this.mtData.Columns.Item("clItemCode").ValidValues.Add(data["ItemCode"].ToString(), data["ItemName"].ToString());
                this.mtData.Columns.Item("clItemCode").ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            }
            this.mtData.Columns.Item("clItemCode").DisplayDesc = true;
            //this.mtData.LoadFromDataSource();
        }
        private void LoadDataToGrid()
        {
            try
            {
                LoadComboboxItem();

                var query = string.Format(Querystring.sp_LoadPOAllocate, this.POEntry);
                Hashtable[] datas;
                using (var connection = Globals.DataConnection)
                {
                    datas = connection.ExecQueryToArrayHashtable(query);
                    connection.Dispose();
                }
                if (datas == null || datas.Count() <= 0)
                {
                    UIHelper.LogMessage(string.Format("Data is empty"), UIHelper.MsgType.StatusBar, false);
                    return;
                }
                foreach (var data in datas)
                {
                    this.mtData.AddRow();
                    int lastRow = this.mtData.VisualRowCount;
                    var oEdit = (SAPbouiCOM.ComboBox)this.mtData.GetCellSpecific("clItemCode", lastRow);
                    
                    oEdit.Select(data["ItemCode"].ToString(), SAPbouiCOM.BoSearchKey.psk_ByDescription);
                }
                
                this.mtData.SelectRow(this.mtData.RowCount, true, false);
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Load data error {0}", ex.Message), UIHelper.MsgType.StatusBar, false);
            }
        }

        private void Freeze(bool freeze)
        {
            this.UIAPIRawForm.Freeze(freeze);
        }
        private static AddonUserForm information;
        private SAPbouiCOM.EditText edFile;

        public static AddonUserForm Information
        {
            get
            {
                if (information == null)
                {
                    information = new AddonUserForm();
                    information.FormID = "POAllocationBatch_F";
                    information.MenuID = "";
                    information.MenuName = "";
                    information.ParentID = "";
                }
                return information;
            }
        }

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.Button btnFile;
        private SAPbouiCOM.Button btnImport;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText edPOEn;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText edPONo;
        private SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.Button btnCancel;

        private void btnFile_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void btnImport_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void btnSave_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void btnCancel_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            instance = null;
            IsFormOpen = false;
        }

        private SAPbouiCOM.Matrix mtData;
    }
}
