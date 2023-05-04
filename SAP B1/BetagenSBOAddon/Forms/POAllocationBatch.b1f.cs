using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
        public string POStatus { get; set; } = "";
        public string POConfirm { get; set; }

        private bool DataChange = false;
        private string DataBefore = string.Empty;

        public POAllocationBatch()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            //     this.btnImport.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnImport_ClickBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.edPOEn = ((SAPbouiCOM.EditText)(this.GetItem("edPOEn").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.edPONo = ((SAPbouiCOM.EditText)(this.GetItem("edPONo").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.btnSave.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnSave_ClickBefore);
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.btnCancel.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCancel_ClickBefore);
            this.grData = ((SAPbouiCOM.Grid)(this.GetItem("grMain").Specific));
            this.grData.ClickBefore += this.GrData_ClickBefore;
            this.grData.LostFocusAfter += this.GrData_LostFocusAfter;
            this.btnAdd = ((SAPbouiCOM.Button)(this.GetItem("btnAdd").Specific));
            this.btnAdd.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAdd_ClickBefore);
            this.btnRemo = ((SAPbouiCOM.Button)(this.GetItem("btnRemo").Specific));
            this.btnRemo.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnRemo_ClickBefore);
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("edFocus").Specific));
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

                instance.SetControl();
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

        private void LoadComboboxItem(SAPbouiCOM.ComboBoxColumn column)
        {
            var query = string.Format(Querystring.sp_GetAllItemToCombobox, this.POEntry);
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
            for (int i = column.ValidValues.Count - 1; i >= 0; i--)
            {
                column.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            foreach (var data in datas)
            {
                column.ValidValues.Add(data["ItemCode"].ToString(), data["ItemName"].ToString());
                column.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
            }
        }
        private void LoadComboboxTeam(SAPbouiCOM.ComboBoxColumn column)
        {
            var query = string.Format(Querystring.sp_GetAllBinToCombobox);
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
                for (int i = column.ValidValues.Count - 1; i >= 0; i--)
                {
                    column.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
                }
                foreach (var data in datas)
                {
                    column.ValidValues.Add(data["BinCode"].ToString(), data["BinName"].ToString());
                    column.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
                }
        }
        private void SetControl()
        {
          //  this.UIAPIRawForm.Items.Item("edFocus").Click();
          ////  this.btnCancel.Item.
          //  this.edPOEn.Item.Enabled = false;
          //  this.edPONo.Item.Enabled = false;

            if (this.POStatus != "O")
            {
                this.btnSave.Item.Enabled = false;
                this.btnAdd.Item.Enabled = false;
                this.btnRemo.Item.Enabled = false;
            }
            else
            {
                this.btnSave.Item.Enabled = true;
                this.btnAdd.Item.Enabled = true;
                this.btnRemo.Item.Enabled = true;
            }
        }
        private void LoadDataToGrid()
        {
            try
            {
                this.grData.DataTable.Clear();
                var query = string.Format(Querystring.sp_LoadPOAllocate, this.POEntry);
                this.grData.DataTable.ExecuteQuery(query);

                this.grData.Columns.Item("ItemCode").TitleObject.Caption = "Item Code";
                this.grData.Columns.Item("ItemCode").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox;
                LoadComboboxItem((SAPbouiCOM.ComboBoxColumn)this.grData.Columns.Item("ItemCode"));
                this.grData.Columns.Item("ItemCode").Editable = (this.POStatus == "O");

                this.grData.Columns.Item("ExpDate").TitleObject.Caption = "Exp. Date";
                this.grData.Columns.Item("ExpDate").Editable = (this.POStatus == "O");

                this.grData.Columns.Item("Team").TitleObject.Caption = "Team";
                this.grData.Columns.Item("Team").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox;
                LoadComboboxTeam((SAPbouiCOM.ComboBoxColumn)this.grData.Columns.Item("Team"));
                this.grData.Columns.Item("Team").Editable = (this.POStatus == "O");

                this.grData.Columns.Item("Quantity").Editable = (this.POStatus == "O");

                this.grData.Columns.Item("MyID").Visible = false;
                this.grData.Columns.Item("PODocEntry").Visible = false;
                
                this.grData.AutoResizeColumns();
              

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
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText edPOEn;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText edPONo;
        private SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.Button btnCancel;
        private SAPbouiCOM.Grid grData;
        private SAPbouiCOM.Button btnAdd;
        private SAPbouiCOM.Button btnRemo;
        
        private void btnImport_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            try
            {

            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Import Error: {0}", ex.Message), UIHelper.MsgType.StatusBar, false);

                this.Freeze(false);
            }

            this.Freeze(false);
        }

        private void btnSave_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            try
            {
                if (this.grData.DataTable == null ||
                    this.grData.DataTable.Rows.Count <= 0)
                {
                    UIHelper.LogMessage(string.Format("Do not data to save"), UIHelper.MsgType.StatusBar, false);

                    this.Freeze(false);
                    return;
                }

                var query = string.Format(Querystring.sp_POAllocateImportInfo_DeleteBefAdd, this.POEntry);
                using (var connection = Globals.DataConnection)
                {
                    connection.ExecuteWithOpenClose(query);
                    connection.Dispose();
                }

                for(var index = 0; index < this.grData.DataTable.Rows.Count; index ++)
                {
                    var itemcode = this.grData.DataTable.GetValue("ItemCode", index).ToString();
                    var expDate = this.grData.DataTable.GetValue("ExpDate", index).ToString();
                    var team = this.grData.DataTable.GetValue("Team", index).ToString();
                    var qty = this.grData.DataTable.GetValue("Quantity", index).ToString();
                    decimal Qty;
                    if (!decimal.TryParse(qty, out Qty))
                    {
                        Qty = 0;
                    }

                    query = string.Format(Querystring.sp_POAllocateImportInfo_Add, this.POEntry, this.PONo, itemcode, expDate, team, Qty);

                    using (var connection = Globals.DataConnection)
                    {
                        connection.ExecuteWithOpenClose(query);
                        connection.Dispose();
                    }

                    Hashtable data;
                    query = string.Format(Querystring.sp_POAllocateImportInfo_LotNoAdd, this.POEntry, this.PONo);
                    using (var connection = Globals.DataConnection)
                    {
                        data = connection.ExecQueryToHashtable(query);
                        connection.Dispose();
                    }
                    var result = string.Empty;
                    if (data!= null)
                    {
                        result = data["Result"].ToString();
                    }
                    if(string.IsNullOrEmpty(result))
                    {
                        UIHelper.LogMessage(string.Format("Saved Successfully"), UIHelper.MsgType.StatusBar, false);
                    }
                    else
                    {
                        UIHelper.LogMessage(string.Format("Please check {0}", result), UIHelper.MsgType.Msgbox);
                    }
                }

            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Save Error: {0}", ex.Message), UIHelper.MsgType.StatusBar, false);

                this.Freeze(false);
                return;
            }
            this.Freeze(false);
        }

        private void btnCancel_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.UIAPIRawForm.Close();
        }

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            instance = null;
            IsFormOpen = false;
        }



        private void GrData_LostFocusAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!this.DataChange)
            {
                if (pVal.ColUID == "ExpDate")
                {
                    var data = this.grData.DataTable.GetValue("ExpDate", pVal.Row).ToString();
                    if (this.DataBefore != data)
                    {
                        this.DataChange = true;
                    }
                }
            }
            SetControl();
        }

        private void GrData_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!this.DataChange)
            {
                if (pVal.ColUID == "ExpDate")
                {
                    this.DataBefore = this.grData.DataTable.GetValue("ExpDate", pVal.Row).ToString();
                }
            }
        }

        private void btnAdd_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.grData.DataTable.Rows.Add();
        }

        private void btnRemo_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            var index = this.grData.Rows.SelectedRows.Item(0, SAPbouiCOM.BoOrderType.ot_RowOrder);
            if(index >= 0)
                this.grData.DataTable.Rows.Remove(index);
        }

        private SAPbouiCOM.EditText EditText0;
    }
}
