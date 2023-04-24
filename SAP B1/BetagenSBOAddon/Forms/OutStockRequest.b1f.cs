using BetagenSBOAddon.AccessSAP;
using GTCore;
using GTCore.Forms;
using SAPbouiCOM.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BetagenSBOAddon.Forms
{
    [FormAttribute("BetagenSBOAddon.Forms.OutStockRequest", "Forms/OutStockRequest.b1f")]
    public class OutStockRequest : UserFormBase
    {
        // field data 
        private DateTime fromDate;
        private DateTime toDate;
        private DateTime stockDate;// = DateTime.Today.AddDays(1);
        private bool checkNullDate;
        private List<int> ListSelect;
        private OutStockRequestDAL DALAccess;
        private FormMode Mode;

        private string AbsEntry
        {
            get
            {
                return this.cbbFromBin.Value;
            }
        }
        private string AbsEntry1
        {
            get
            {
                return this.cbbToBin.Value;
            }
        }

        private string BinCode
        {
            get
            {
                return this.cbbFromBin.Selected.Description;
            }
        }

        private string BinCode1
        {
            get
            {
                return this.cbbToBin.Selected.Description;
            }
        }
        private string StockNo
        {
            get
            {
                return edNo.Value;
            }
        }
        private string FromWarehouseCode
        {
            get
            {
                return cbbFmWh.Value;
            }
        }
        private string ToWarehouseCode
        {
            get
            {
                return cbbToWh.Value;
            }
        }
        public DateTime FromDate
        {
            get => fromDate;
            set
            {
                fromDate = value;
                if (this.fromDate == Globals.NullDate)
                    this.edFromDateList.Value = string.Empty;
                else if (this.edFromDateList.Value != fromDate.ToString(Globals.DateShowFormat))
                    this.edFromDateList.Value = fromDate.ToString(Globals.DateShowFormat);
            }
        }
        public DateTime ToDate
        {
            get => toDate;
            set
            {
                toDate = value;
                if (this.toDate == Globals.NullDate)
                    this.edToDateList.Value = string.Empty;
                else if (this.edToDateList.Value != toDate.ToString(Globals.DateShowFormat))
                    this.edToDateList.Value = toDate.ToString(Globals.DateShowFormat);
            }
        }
        public DateTime StockDate
        {
            get => stockDate;
            set
            {
                stockDate = value;
                if (this.stockDate == Globals.NullDate)
                    this.edDateAdd.Value = string.Empty;
                else if (this.edDateAdd.Value != stockDate.ToString(Globals.DateShowFormat))
                    this.edDateAdd.Value = stockDate.ToString(Globals.DateShowFormat);
            }
        }
        public bool CheckNullDate
        {
            get => checkNullDate;
            set
            {
                checkNullDate = value;
                this.cbNullDate.Item.Enabled = true;
            }
        }


        private static OutStockRequest instance;

        public static bool IsFormOpen = false;

        private static AddonUserForm information;
        public static AddonUserForm Information
        {
            get
            {
                if (information == null)
                {
                    information = new AddonUserForm();
                    information.FormID = "OutStockRequest_F";
                    information.MenuID = "OutStockRequest_M";
                    information.MenuName = "Inventory Transfer Req. Form";
                    information.ParentID = "43540"; // Inventory Transactions
                }
                return information;
            }
        }

        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new OutStockRequest();
                instance.Show();
                IsFormOpen = true;
            }
        }

        private OutStockRequest()
        {
            this.tagPage1.Select();
            this.FromDate = DateTime.Today.AddDays(-1);
            this.ToDate = DateTime.Today.AddDays(-1);
            this.StockDate = DateTime.Today.AddDays(1);
            this.CheckNullDate = true;
            Mode = FormMode.Add;
            ListSelect = new List<int>();
            DALAccess = new OutStockRequestDAL();

            LoadMainGrid();
            LoadComboboxWarehouse();

            this.EnabledControl();
            this.btnAddNew.Item.Enabled = false;
            this.btnAppSapAdd.Item.Enabled = false;
            this.tagPage1.Select();
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.tagPage1 = ((SAPbouiCOM.Folder)(this.GetItem("tagPage1").Specific));
            this.tagPage2 = ((SAPbouiCOM.Folder)(this.GetItem("tagPage2").Specific));
            this.tagPage2.ClickAfter += new SAPbouiCOM._IFolderEvents_ClickAfterEventHandler(this.tagPage2_ClickAfter);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.edFromDateList = ((SAPbouiCOM.EditText)(this.GetItem("edFormDate").Specific));
            this.edFromDateList.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.edFromDateList_ValidateBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.edToDateList = ((SAPbouiCOM.EditText)(this.GetItem("edToDate").Specific));
            this.edToDateList.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.edToDateList_ValidateBefore);
            this.cbNullDate = ((SAPbouiCOM.CheckBox)(this.GetItem("cbNullDate").Specific));
            this.btnSearch = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.btnSearch.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.btnSearch_PressedBefore);
            this.btnSearch.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnSearch_PressedAfter);
            this.grList = ((SAPbouiCOM.Grid)(this.GetItem("grList").Specific));
            this.grList.DoubleClickAfter += new SAPbouiCOM._IGridEvents_DoubleClickAfterEventHandler(this.grList_DoubleClickAfter);
            this.btnDelete = ((SAPbouiCOM.Button)(this.GetItem("btnDelete").Specific));
            this.btnDelete.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnDelete_PressedAfter);
            this.btnCancle = ((SAPbouiCOM.Button)(this.GetItem("btnCancle").Specific));
            this.btnCancle.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnCancle_PressedAfter);
            this.btnConfirm = ((SAPbouiCOM.Button)(this.GetItem("btnConfirm").Specific));
            this.btnConfirm.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnConfirm_ClickBefore);
            this.btnAppSapList = ((SAPbouiCOM.Button)(this.GetItem("btnAppLst").Specific));
            this.btnAppSapList.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAppSapList_ClickBefore);
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_14").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_20").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_24").Specific));
            this.edNo = ((SAPbouiCOM.EditText)(this.GetItem("edNo").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_28").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_30").Specific));
            this.edDateAdd = ((SAPbouiCOM.EditText)(this.GetItem("edDateAdd").Specific));
            this.edDateAdd.ValidateBefore += new SAPbouiCOM._IEditTextEvents_ValidateBeforeEventHandler(this.edDateAdd_ValidateBefore);
            this.edDateAdd.ValidateAfter += new SAPbouiCOM._IEditTextEvents_ValidateAfterEventHandler(this.edDateAdd_ValidateAfter);
            this.StaticText12 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_34").Specific));
            this.StaticText13 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_35").Specific));
            this.edNote = ((SAPbouiCOM.EditText)(this.GetItem("edNote").Specific));
            this.btnLoadItem = ((SAPbouiCOM.Button)(this.GetItem("btnLoad").Specific));
            this.btnLoadItem.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnLoadItem_ClickBefore);
            this.grAdd = ((SAPbouiCOM.Grid)(this.GetItem("grAdd").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.btnCancelAdd = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.btnCancelAdd.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnCancelAdd_PressedAfter);
            this.btnAddNew = ((SAPbouiCOM.Button)(this.GetItem("btnAddNew").Specific));
            this.btnAddNew.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAddNew_ClickBefore);
            this.btnAppSapAdd = ((SAPbouiCOM.Button)(this.GetItem("btnAppAdd").Specific));
            this.btnAppSapAdd.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAppSapAdd_ClickBefore);
            //              this.UD_Warehouse = ((SAPbouiCOM.UserDataSource)(this.UIAPIRawForm.DataSources.UserDataSources.Item("UD_WH")));
            //                 UserDataSource udStatus { get { return this.Form.UIAPIRawForm.DataSources.UserDataSources.Item("UD_Status"); } }
            this.cbbFmWh = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbFmWh").Specific));
            this.cbbFmWh.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cbbFmWh_ComboSelectAfter);
            this.cbbToWh = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbToWh").Specific));
            this.cbbToWh.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cbbToWh_ComboSelectAfter);
            this.cbbFromBin = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbFBin").Specific));
            this.cbbFromBin.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cbbFromBin_ComboSelectAfter);
            this.cbbToBin = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbTBin").Specific));
            this.cbbToBin.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cbbToBin_ComboSelectAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.LoadAfter += new SAPbouiCOM.Framework.FormBase.LoadAfterHandler(this.Form_LoadAfter);
            this.CloseBefore += new SAPbouiCOM.Framework.FormBase.CloseBeforeHandler(this.Form_CloseBefore);
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);

        }

        private void OnCustomInitialize()
        {

        }

        private void LoadMainGrid()
        {
            try
            {
                ListSelect.Clear();

                this.grList.DataTable.Clear();
                // throw new System.NotImplementedException();
                var query = string.Format(Querystring.sp_LoadOutStockRequest, "1",
                    this.FromDate.ToString(Globals.DateQueryFormat),
                    this.ToDate.ToString(Globals.DateQueryFormat),
                    1);
                this.grList.DataTable.ExecuteQuery(query);

                #region Load main
                this.grList.Columns.Item("Choo").TitleObject.Caption = "";
                this.grList.Columns.Item("Choo").Editable = true;
                this.grList.Columns.Item("Choo").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox;
                this.grList.Columns.Item("Choo").ClickAfter += OutStockRequest_ClickAfter;

                this.grList.Columns.Item("StockNo").TitleObject.Caption = "Doc No.";
                this.grList.Columns.Item("StockNo").Editable = false;

                this.grList.Columns.Item("StockDate").TitleObject.Caption = "Doc Date";
                this.grList.Columns.Item("StockDate").Editable = false;

                this.grList.Columns.Item("FromWhsCode").TitleObject.Caption = "From Whs. Code";
                this.grList.Columns.Item("FromWhsCode").Editable = false;

                this.grList.Columns.Item("FromWhsName").TitleObject.Caption = "From Whs. Name";
                this.grList.Columns.Item("FromWhsName").Editable = false;

                this.grList.Columns.Item("BinCode").TitleObject.Caption = "Team Code";
                this.grList.Columns.Item("BinCode").Editable = false;

                this.grList.Columns.Item("ToWhsCode").TitleObject.Caption = "To Whs. Code";
                this.grList.Columns.Item("ToWhsCode").Editable = false;

                this.grList.Columns.Item("ToWhsName").TitleObject.Caption = "To Whs. Name";
                this.grList.Columns.Item("ToWhsName").Editable = false;

                this.grList.Columns.Item("BinCode1").TitleObject.Caption = "Team Code";
                this.grList.Columns.Item("BinCode1").Editable = false;

                this.grList.Columns.Item("TransferReqNo").TitleObject.Caption = "Transfer Req. No";
                this.grList.Columns.Item("TransferReqNo").Editable = false;

                this.grList.Columns.Item("TransferNo").TitleObject.Caption = "Transfer No";
                this.grList.Columns.Item("TransferNo").Editable = false;

                this.grList.Columns.Item("DeliveryStatus").TitleObject.Caption = "Delivery Status";
                this.grList.Columns.Item("DeliveryStatus").Editable = false;

                this.grList.Columns.Item("ApplySAP").TitleObject.Caption = "Apply SAP";
                this.grList.Columns.Item("ApplySAP").Editable = false;

                this.grList.Columns.Item("Confirm").TitleObject.Caption = "Cofirm Status";
                this.grList.Columns.Item("Confirm").Editable = false;

                this.grList.Columns.Item("Note").TitleObject.Caption = "Note";
                this.grList.Columns.Item("Note").Editable = false;

                this.grList.Columns.Item("UserName").TitleObject.Caption = "User Create";
                this.grList.Columns.Item("UserName").Editable = false;

                this.grList.Columns.Item("DateCreate").TitleObject.Caption = "Date Create";
                this.grList.Columns.Item("DateCreate").Editable = false;

                this.grList.Columns.Item("DateUpdate").TitleObject.Caption = "Date Update";
                this.grList.Columns.Item("DateUpdate").Editable = false;

                this.grList.Columns.Item("TotalWeight").TitleObject.Caption = "TotalWeight";
                this.grList.Columns.Item("TotalWeight").Editable = false;

                this.grList.Columns.Item("ApplyStatus").TitleObject.Caption = "Apply Status";
                this.grList.Columns.Item("ApplyStatus").Editable = false;

                this.grList.Columns.Item("ApplySAPRemark").TitleObject.Caption = "Apply SAP Remark";
                this.grList.Columns.Item("ApplySAPRemark").Editable = false;

                this.grList.Columns.Item("StockType").Visible = false;
                this.grList.Columns.Item("CustCode").Visible = false;
                this.grList.Columns.Item("CustName").Visible = false;
                this.grList.Columns.Item("TruckNo").Visible = false;
                this.grList.Columns.Item("OrderType").Visible = false;
                this.grList.Columns.Item("Name").Visible = false;
                this.grList.Columns.Item("AbsID").Visible = false;
                this.grList.Columns.Item("BpCode").Visible = false;
                this.grList.Columns.Item("BPName").Visible = false;
                this.grList.Columns.Item("AbsEntry").Visible = false;
                this.grList.Columns.Item("AbsEntry1").Visible = false;
                this.grList.Columns.Item("UserID").Visible = false;
                this.grList.Columns.Item("Canceled").Visible = false;
                this.grList.Columns.Item("StatusSAP").Visible = false;
                this.grList.AutoResizeColumns();
                #endregion

                UIHelper.LogMessage("Load data is successfully", UIHelper.MsgType.StatusBar, false);
                this.btnDelete.Item.Enabled = false;
                this.btnAppSapList.Item.Enabled = false;
                this.btnConfirm.Item.Enabled = false;
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Load data error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
            }
        }

        private void EnabledControl()
        {
            if (this.Mode == FormMode.Add)
            {
                this.cbbFmWh.Item.Enabled = true;
                this.cbbToWh.Item.Enabled = true;
                this.edDateAdd.Item.Enabled = true;
                this.edNo.Item.Enabled = true;
                //this.txtTruckNo.ReadOnly = false;
                this.cbbFromBin.Item.Enabled = true;
                this.cbbToBin.Item.Enabled = true;

                //foreach(SAPbouiCOM.Column col in this.grAdd.Columns)
                //{
                //    col.Editable = true;
                //}
                //this.grdStockDetail.CurrentTable.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.True;
            }
            else if (!this.btnAppSapList.Item.Enabled)
            {
                this.cbbFmWh.Item.Enabled = false;
                this.cbbToWh.Item.Enabled = false;
                this.edDateAdd.Item.Enabled = false;
                this.edNo.Item.Enabled = false;
                //this.txtTruckNo.ReadOnly = true;
                this.cbbFromBin.Item.Enabled = false;
                this.cbbToBin.Item.Enabled = false;
                //foreach (SAPbouiCOM.Column col in this.grAdd.Columns)
                //{
                //    col.Editable = false;
                //}
                //this.grdStockDetail.CurrentTable.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            }
            else
            {
                this.cbbFmWh.Item.Enabled = false;
                this.cbbToWh.Item.Enabled = false;
                this.edDateAdd.Item.Enabled = false;
                this.edNo.Item.Enabled = false;

                //foreach (SAPbouiCOM.Column col in this.grAdd.Columns)
                //{
                //    col.Editable = true;
                //}
                //this.txtNote.ReadOnly = Conversions.ToBoolean(Interaction.IIf(this.grdStockDetail.RootTable.Columns.Count == 0, (object)true, (object)false));

                //this.cboBinCode.ReadOnly = Conversions.ToBoolean(Interaction.IIf(this.grdStockDetail.RootTable.Columns.Count > 0, (object)true, (object)false));
                //this.cboBinCode1.ReadOnly = Conversions.ToBoolean(Interaction.IIf(this.grdStockDetail.RootTable.Columns.Count == 0, (object)true, (object)false));
                //this.txtTruckNo.ReadOnly = Conversions.ToBoolean(Interaction.IIf(this.grdStockDetail.RootTable.Columns.Count == 0, (object)true, (object)false));
                //this.txtNote.ReadOnly = Conversions.ToBoolean(Interaction.IIf(this.grdStockDetail.RootTable.Columns.Count == 0, (object)true, (object)false));
                //this.grdStockDetail.CurrentTable.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.True;
            }
        }
        private void LoadListAddForm()
        {
            try
            {
                this.grAdd.DataTable.Clear();
                var query = string.Format(Querystring.sp_LoadLotItem, this.FromWarehouseCode, this.StockNo, this.AbsEntry);

                Hashtable[] datas;
                string message;
                using (var connection = Globals.DataConnection)
                {
                    datas = connection.ExecQueryToArrayHashtable(query, out message);
                    connection.Dispose();
                }

                this.grAdd.DataTable.Columns.Add("ID", SAPbouiCOM.BoFieldsType.ft_Text);
                this.grAdd.Columns.Item("ID").Editable = false;

                this.grAdd.DataTable.Columns.Add("DES", SAPbouiCOM.BoFieldsType.ft_Text);
                this.grAdd.Columns.Item("DES").Editable = false;

                if (datas.Length > 0)
                {
                    var dynamicColumn = new Dictionary<string, string>();
                    var dynamicTotal = new Dictionary<string, int>();
                    foreach (var item in datas.Select(x => x["LotNo"].ToString()).Distinct())
                    {
                        var dateFormat = string.Format("{0}/{1}/{2}",
                            item.Substring(0, 4),
                            item.Substring(4, 2),
                            item.Substring(6, 2));
                        var lot = string.Format("Lot {0}", dateFormat);
                        var instock = string.Format("InStock {0}", dateFormat);
                        if (!dynamicColumn.ContainsKey("Lot" + item))
                        {
                            dynamicColumn.Add("Lot" + item, lot);
                        }

                        if (!dynamicColumn.ContainsKey("InStock" + item))
                        {
                            dynamicColumn.Add("InStock" + item, instock);
                        }
                    }

                    foreach (var data in dynamicColumn)
                    {
                        this.grAdd.DataTable.Columns.Add(data.Key, SAPbouiCOM.BoFieldsType.ft_Text);
                        this.grAdd.Columns.Item(data.Key).TitleObject.Caption = data.Value;
                        this.grAdd.Columns.Item(data.Key).Editable = false;
                    }

                    foreach (var data in datas)
                    {
                        this.grAdd.DataTable.Rows.Add();
                        this.grAdd.DataTable.SetValue("ID", this.grAdd.Rows.Count - 1, data["ItemCode"].ToString());
                        this.grAdd.DataTable.SetValue("DES", this.grAdd.Rows.Count - 1, data["ItemName"].ToString());
                        int inQty;
                        var lotID = data["LotNo"].ToString();

                        var inQtystr = data["QuantityIn"].ToString();
                        if(inQtystr.Contains("."))
                            inQtystr = inQtystr.Substring(0, inQtystr.IndexOf('.'));
                   
                        if (int.TryParse(inQtystr, out inQty))
                        {
                            if (inQty > 0)
                            {
                                this.grAdd.DataTable.SetValue("InStock" + lotID, this.grAdd.Rows.Count - 1, inQty.ToString());
                                if (dynamicTotal.ContainsKey("InStock" + lotID))
                                {
                                    dynamicTotal["InStock" + lotID] += inQty;
                                }
                                else
                                {
                                    dynamicTotal.Add("InStock" + lotID, inQty);
                                }
                            }
                        }

                        int outQty;
                        var outQtystr = data["QuantityOut"].ToString();
                        if(outQtystr.Contains("."))
                            outQtystr = outQtystr.Substring(0, outQtystr.IndexOf('.'));
                        if (int.TryParse(outQtystr, out outQty))
                        {
                            if (outQty > 0)
                            {
                                this.grAdd.DataTable.SetValue("Lot" + lotID, this.grAdd.Rows.Count - 1, outQty.ToString());
                                if (dynamicTotal.ContainsKey("Lot" + lotID))
                                {
                                    dynamicTotal["Lot" + lotID] += outQty;
                                }
                                else
                                {
                                    dynamicTotal.Add("Lot" + lotID, outQty);
                                }
                            }
                        }
                    }

                    this.grAdd.DataTable.Rows.Add();
                    this.grAdd.DataTable.SetValue("DES", this.grAdd.Rows.Count - 1, "Total");
                    foreach (var data in dynamicTotal)
                    {
                        //this.grAdd.DataTable.Columns.Add(data.Key, SAPbouiCOM.BoFieldsType.ft_Text);
                        this.grAdd.DataTable.SetValue(data.Key, this.grAdd.Rows.Count - 1, data.Value.ToString());
                    }
                }
                this.grAdd.AutoResizeColumns();

                UIHelper.LogMessage("Load data is successfully", UIHelper.MsgType.StatusBar, false);
                this.btnSearch.Item.Enabled = false;
                this.btnAppSapAdd.Item.Enabled = false;
                this.btnAddNew.Item.Enabled = false;
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Load data error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
            }
        }

        private void EnableButtonInList()
        {
            this.btnDelete.Item.Enabled = ListSelect.Count > 0;
            this.btnConfirm.Item.Enabled = ListSelect.Count > 0;

            if (ListSelect.Count > 0)
            {
                if (this.grList.DataTable.GetValue("ApplySAP", ListSelect.Min()).ToString() == "No")
                {
                    this.btnAppSapList.Item.Enabled = true;
                }
                else
                {
                    this.btnAppSapList.Item.Enabled = false;
                }
            }
        }

        // checl enable
        private bool EnableApplySAP()
        {
            var query = string.Format(Querystring.sp_OutStockRequestGetApplySAP, this.StockNo, 1);
            string message;
            Hashtable hash;
            using (var connection = Globals.DataConnection)
            {
              hash = connection.ExecQueryToHashtable(query, out message);
                connection.Dispose();
            }

            if (hash != null)
            {
                var data = hash["StatusSAP"].ToString();
                int status;
                if(int.TryParse(data, out status))
                {
                    return status > 0;
                }
            }
            return false;

        }

        private bool DeleteApplySAP()
        {
            if (this.grList.Rows.Count == 0)
            {
                UIHelper.LogMessage("The list is empty", UIHelper.MsgType.StatusBar, true);
                return false;
            }
            try
            {
                var query = string.Empty;
                foreach (var idx in ListSelect)
                {
                    if (this.grList.DataTable.GetValue("Choo", idx).ToString() == "Y")
                    {
                        var applySAP = this.grList.DataTable.GetValue("ApplySAP", idx).ToString();
                        var code = this.grList.DataTable.GetValue("StockNo", idx).ToString();

                        if (applySAP == "Yes")
                        {
                            int yes;
                            yes = UIHelper.LogMessage(string.Format("{0} have Apply SAP, you can't delete.Do you want continue delete ?", code),
                                                       UIHelper.MsgType.Msgbox, false, 1, "Yes", "No");
                            if (yes != 1)
                            {
                                continue;
                            }
                        }
                        using (var connection = Globals.DataConnection)
                        {
                            query = string.Format(Querystring.sp_DeleteOutStockRequest, code) + ";";
                            connection.ExecuteWithOpenClose(query);
                            connection.Dispose();
                        }
                    }
                }
                query = Querystring.sp_NotificationUpdateStock;
                using (var connection = Globals.DataConnection)
                {
                    connection.ExecuteWithOpenClose(query);
                    connection.Dispose();
                }
                UIHelper.LogMessage("Delete Successfully", UIHelper.MsgType.Msgbox);

                return true;
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Delete data error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
                return false;
            }
            finally
            {
                ListSelect.Clear();
            }
        }

        private int AddInventoryTransferRequest(string stockNo)
        {
            var ret = -1;
            try
            {
                var message = string.Empty;
                ret = DALAccess.CreateInventoryTrannsferRequest(stockNo, ref message);
                if (ret > 0)
                    UIHelper.LogMessage("Add Inventory Transfer Request is successfully", UIHelper.MsgType.StatusBar, false);
                else
                    UIHelper.LogMessage(string.Format("Add Inventory Transfer Request error {0}", message), UIHelper.MsgType.StatusBar, true);
            }
            catch (Exception ex)
            {
                ret = -1;
                UIHelper.LogMessage(string.Format("Add Inventory Transfer Request error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
            }
            return ret;
        }

        /// <summary>
        /// Foreach tag1
        /// </summary>
        /// <param name="stockNo"></param>
        /// <returns></returns>
        private bool ApplySAP(string stockNo)
        {
            try
            {
                var query = string.Format(Querystring.sp_OutStockRequestApplySAP, stockNo, 1);
                using (var connection = Globals.DataConnection)
                {
                    connection.ExecuteWithOpenClose(query);
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private void LoadBinCodes(string warehouseCode, SAPbouiCOM.ComboBox comboBox)
        {
            for (int i = comboBox.ValidValues.Count - 1; i >= 0; i--)
            {
                comboBox.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            comboBox.ValidValues.Add(Globals.BinNull, "");
            comboBox.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);

            if (!string.IsNullOrEmpty(warehouseCode) && warehouseCode != Globals.WarehouseNull)
            {

                var query = string.Format(Querystring.sp_GetBins, warehouseCode);
                Hashtable[] binDatasource;
                var errorstr = string.Empty;
                using (var connection = Globals.DataConnection)
                {
                    binDatasource = connection.ExecQueryToArrayHashtable(query, out errorstr);
                    connection.Dispose();
                }

                if (binDatasource != null && binDatasource.Length > 0)
                {
                    foreach (var item in binDatasource)
                    {
                        //cbbFmWh.ValidValues.Item.
                        comboBox.ValidValues.Add(item["AbsEntry"].ToString(), item["BinCode"].ToString());
                        comboBox.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
                    }
                }
            }

        }

        private void CreateStockNo()
        {
            this.Freeze(true);

            //this.grAdd.DataTable.Clear();
            this.btnSave.Item.Enabled = false;
            this.btnAppSapAdd.Item.Enabled = false;
            this.btnAddNew.Item.Enabled = false;

            try
            {
                var refix = string.Empty;

                // here: the old solution load refix from store: USP_BS_STOCKNO
                if (this.FromWarehouseCode == this.ToWarehouseCode)
                    refix = "BO";
                else
                    refix = "IT";
                //var next1 = "00000";
                var next2 = this.StockDate.Month >= 10 ? this.StockDate.Month.ToString() : string.Format("0{0}", this.StockDate.Month);
                //if(this.StockDate.Month)
                var maxNoInMonth = 0;
                var param = this.StockDate.Year.ToString().Substring(2, 2) + next2;
                using (var connection = Globals.DataConnection)
                {
                    var errorstr = string.Empty;
                    var query = string.Format(Querystring.sp_GetMaxStockNo, param);
                    var data = connection.ExecQueryToHashtable(query, out errorstr);
                    if (data != null)
                        maxNoInMonth = int.Parse(data["MaxID"].ToString()) + 1;
                    connection.Dispose();
                }

                var next1 = string.Format("{0:00000}", maxNoInMonth);

                edNo.Value = refix + this.StockDate.Year.ToString().Substring(2, 2) + next2 + next1;
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Gen code error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
            }

            this.Freeze(false);
        }


        private void LoadDataMain(int index)
        {
            DateTime date;
            if (DateTime.TryParse(this.grList.DataTable.GetValue("StockDate", index).ToString(), out date))
            {
                this.StockDate = date;
            }
            //int selected = 0;
            // this.cbbFmWh.Value.
            this.cbbFmWh.Select(this.grList.DataTable.GetValue("FromWhsCode", index).ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbToWh.Select(this.grList.DataTable.GetValue("ToWhsCode", index).ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.edNote.Value = this.grList.DataTable.GetValue("Note", index).ToString();
            this.cbbFromBin.Select(this.grList.DataTable.GetValue("AbsEntry", index).ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbToBin.Select(this.grList.DataTable.GetValue("AbsEntry1", index).ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);

            //CreateStockNo();
            this.edNo.Value = this.grList.DataTable.GetValue("StockNo", index).ToString();
            LoadListAddForm();
        }
        private void SetNullValues(bool isBool)
        {
            this.btnAddNew.Item.Enabled = isBool;
            this.btnAppSapAdd.Item.Enabled = isBool;
            this.cbbFmWh.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            this.cbbToWh.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            //this.cbbFromBin.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            //this.cbbToBin.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            this.StockDate = DateTime.Today.AddDays(1);
            this.edNote.Value = string.Empty;
        }
        private void EnableButtonInAdd()
        {
            //this.btnSave.Item.Enabled = ListSelect.Count > 0;
            //this.btnConfirm.Item.Enabled = ListSelect.Count > 0;

            //if (ListSelect.Count > 0)
            //{
            //    if (this.grList.DataTable.GetValue("ApplySAP", ListSelect.Min()).ToString() == "No")
            //    {
            //        this.btnAppSapList.Item.Enabled = true;
            //    }
            //    else
            //    {
            //        this.btnAppSapList.Item.Enabled = false;
            //    }
            //}
        }
        private void Freeze(bool freeze)
        {
            this.UIAPIRawForm.Freeze(freeze);
        }

        private SAPbouiCOM.Folder tagPage1;
        private SAPbouiCOM.Folder tagPage2;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText edFromDateList;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText edToDateList;
        private SAPbouiCOM.CheckBox cbNullDate;
        private SAPbouiCOM.Button btnSearch;
        private SAPbouiCOM.Grid grList;
        private SAPbouiCOM.Button btnDelete;
        private SAPbouiCOM.Button btnCancle;
        private SAPbouiCOM.Button btnConfirm;
        private SAPbouiCOM.Button btnAppSapList;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.EditText edNo;
        private SAPbouiCOM.StaticText StaticText9;
        private SAPbouiCOM.StaticText StaticText10;
        private SAPbouiCOM.EditText edDateAdd;
        private SAPbouiCOM.StaticText StaticText12;
        private SAPbouiCOM.StaticText StaticText13;
        private SAPbouiCOM.EditText edNote;
        private SAPbouiCOM.Button btnLoadItem;
        private SAPbouiCOM.Grid grAdd;
        private SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.Button btnCancelAdd;
        private SAPbouiCOM.Button btnAddNew;
        private SAPbouiCOM.Button btnAppSapAdd;
        private SAPbouiCOM.UserDataSource UD_Warehouse;

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            // throw new System.NotImplementedException();


        }
        private void btnSearch_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.Freeze(true);//Freeze
            this.LoadMainGrid();
            //try
            //{
            //    this.LoadMainGrid();
            //}
            //catch (Exception ex)
            //{
            //    Application.SBO_Application.SetStatusBarMessage(string.Format("Search has error : {0} ", ex.Message), SAPbouiCOM.BoMessageTime.bmt_Short, true);
            //}
            this.Freeze(false);//UnFreeze
        }

        private void btnSearch_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //throw new System.NotImplementedException();

        }

        private void Form_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            // Application.SBO_Application.Forms.Item(this).
        }

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            instance = null;
            IsFormOpen = false;

        }

        private void edFromDateList_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!string.IsNullOrEmpty(edFromDateList.Value))
            {
                DateTime outdate;
                if (DateTime.TryParseExact(edFromDateList.Value, Globals.DateParseFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out outdate))
                {
                    FromDate = outdate;
                }
                else
                {
                    FromDate = DateTime.Now;
                    Application.SBO_Application.SetStatusBarMessage("From Date Field must has format as (dd.MM.yyyy)", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                }
            }
        }

        private void edToDateList_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!string.IsNullOrEmpty(edToDateList.Value))
            {
                DateTime outdate;
                if (DateTime.TryParseExact(edToDateList.Value, Globals.DateParseFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out outdate))
                {
                    toDate = outdate;
                }
                else
                {
                    toDate = DateTime.Now;
                    Application.SBO_Application.SetStatusBarMessage("To Date Field must has format as (dd.MM.yyyy)", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                }
            }
        }

        private void btnDelete_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.Freeze(true);//Freeze
            try
            {
                if (DeleteApplySAP())
                {
                    this.grList.DataTable.Clear();
                    this.LoadMainGrid();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage(string.Format("Delete has error : {0} ", ex.Message), SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            this.Freeze(false);//UnFreeze
        }

        private void btnCancle_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Close();

        }

        private void OutStockRequest_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "Choo" && pVal.Row >= 0)
            {
                if (this.grList.DataTable.GetValue("Choo", pVal.Row).ToString() == "Y")
                {
                    if (!ListSelect.Contains(pVal.Row))
                        ListSelect.Add(pVal.Row);
                }
                else
                {
                    if (ListSelect.Contains(pVal.Row))
                        ListSelect.Remove(pVal.Row);
                }

            }
            this.EnableButtonInList();

        }

        private void btnAppSapList_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            // irnoge case call webservice of http://bvnsap.selfip.com/BTG_SalesTool/AddStockOutReq

            // work case create Invoice via DI API
            this.Freeze(true);
            // case 1  if ! LoggedIn_BySalesTool
            // handle only this case
            if (ListSelect.Count > 0)
            {
                var index = ListSelect.Min();
                var stockNo = this.grList.DataTable.GetValue("StockNo", index).ToString();
                if (AddInventoryTransferRequest(stockNo) > 0 && ApplySAP(stockNo))
                {
                    UIHelper.LogMessage("Add Inventory Transfer Request is successfully", UIHelper.MsgType.StatusBar, false);
                    this.LoadMainGrid();
                }
            }
            // case 2 LoggedIn_BySalesTool and G_UseWebSrv = 0
            //---- not handle

            // case 3: call service 
            //---= not handle
            this.Freeze(false);
        }

        private void btnConfirm_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            if (ListSelect.Count > 0)
            {
                var index = ListSelect.Min();
                var stockNo = this.grList.DataTable.GetValue("StockNo", index).ToString();
                var reqNo = this.grList.DataTable.GetValue("TransferReqNo", index).ToString();
                if (string.IsNullOrEmpty(stockNo) || string.IsNullOrEmpty(reqNo))
                {
                    UIHelper.LogMessage("Please apply SAP or select Document to confirm", UIHelper.MsgType.Msgbox);
                }
                try
                {
                    var query = string.Format(Querystring.sp_OutStockRequestConfirm, stockNo, reqNo);
                    using (var connection = Globals.DataConnection)
                    {
                        connection.ExecuteWithOpenClose(query);
                        connection.Dispose();
                    }
                    UIHelper.LogMessage("Comfirmation is successfully", UIHelper.MsgType.StatusBar, false);
                }
                catch (Exception ex)
                {
                    UIHelper.LogMessage(string.Format("Comfirmation is Error", ex.Message), UIHelper.MsgType.StatusBar, true);
                }
            }

            this.Freeze(false);
        }

        private void tagPage2_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
        }

        private void LoadComboboxWarehouse()
        {
            for (int i = cbbFmWh.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbFmWh.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            cbbFmWh.ValidValues.Add(Globals.WarehouseNull, "");
            cbbFmWh.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            for (int i = cbbToWh.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbToWh.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            cbbToWh.ValidValues.Add(Globals.WarehouseNull, "");
            cbbFmWh.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);

            var query = Querystring.sp_GetWarehouses;
            Hashtable[] whDatasource;
            var errorstr = string.Empty;
            using (var connection = Globals.DataConnection)
            {
                whDatasource = connection.ExecQueryToArrayHashtable(query, out errorstr);
                connection.Dispose();
            }
            if (whDatasource.Length > 0)
            {
                foreach (var item in whDatasource)
                {
                    //cbbFmWh.ValidValues.Item.
                    cbbFmWh.ValidValues.Add(item["WhsCode"].ToString(), item["WhsName"].ToString());
                    cbbFmWh.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;

                    cbbToWh.ValidValues.Add(item["WhsCode"].ToString(), item["WhsName"].ToString());
                    cbbToWh.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
                }
                //this.UD_Warehouse.
            }
        }

        private SAPbouiCOM.ComboBox cbbFmWh;
        private SAPbouiCOM.ComboBox cbbToWh;
        private SAPbouiCOM.ComboBox cbbFromBin;
        private SAPbouiCOM.ComboBox cbbToBin;

        private void cbbFmWh_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var value = this.cbbFmWh.Value;

            if (value != this.FromWarehouseCode)
            {
                LoadBinCodes(value, this.cbbFromBin);
                //this.FromWarehouseCode = value;
                CreateStockNo();
            }
        }

        private void cbbToWh_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var value = this.cbbToWh.Value;

            if (value != this.ToWarehouseCode)
            {
                LoadBinCodes(value, this.cbbToBin);
                //this.ToWarehouseCode = value;
                CreateStockNo();
            }
        }


        private void cbbFromBin_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {


        }

        private void cbbToBin_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            //throw new System.NotImplementedException();

        }

        private void edDateAdd_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            //throw new System.NotImplementedException();

        }

        private void edDateAdd_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (!string.IsNullOrEmpty(edDateAdd.Value) && edDateAdd.Value != StockDate.ToString(Globals.DateShowFormat))
            {
                DateTime outdate;
                if (DateTime.TryParseExact(edDateAdd.Value, Globals.DateParseFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out outdate))
                {
                    StockDate = outdate;
                }
                else
                {
                    StockDate = DateTime.Today.AddDays(1);
                    Application.SBO_Application.SetStatusBarMessage("Stock Date Field must has format as (dd.MM.yyyy)", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                }
                CreateStockNo();
            }
        }

        private void btnLoadItem_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            if (StockDate == Globals.NullDate)
            {
                UIHelper.LogMessage("Please choose Stock date", UIHelper.MsgType.StatusBar, true);
                return;
            }
            if (string.IsNullOrEmpty(FromWarehouseCode) || string.IsNullOrEmpty(ToWarehouseCode)
                || string.IsNullOrEmpty(this.cbbFromBin.Value) || this.cbbFromBin.Value == Globals.BinNull
                || string.IsNullOrEmpty(this.cbbToBin.Value) || this.cbbToBin.Value == Globals.BinNull)
            {
                UIHelper.LogMessage("Please select all informations (From warehouse, To warehouse, From team, To team)", UIHelper.MsgType.StatusBar, true);
                return;
            }

            if (this.cbbFromBin.Value == this.cbbToBin.Value)
            {
                UIHelper.LogMessage("From team not duplicate  To team", UIHelper.MsgType.StatusBar, false);
                this.cbbToBin.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);// = Globals.BinNull;
            }
            else
            {
                if (this.Mode == FormMode.Add)
                    CreateStockNo();
                this.LoadListAddForm();
            }
            this.Freeze(false);
        }

        private void grList_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.Freeze(true);
            Mode = FormMode.View;
            var index = pVal.Row;
            if (index >= 0)
            {
                this.LoadDataMain(index);
                var applySap = EnableApplySAP();
                this.EnabledControl();
                this.btnAppSapAdd.Item.Enabled = applySap;
                this.btnLoadItem.Item.Enabled = applySap;
                this.btnAddNew.Item.Enabled = true;
                this.tagPage2.Select();
            }
            this.Freeze(false);
        }

        private void btnAddNew_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);

            this.Mode = FormMode.Add;
            this.StockDate = DateTime.Today.AddDays(1);
            this.CreateStockNo();
            EnabledControl();
            SetNullValues(false);
            this.btnLoadItem.Item.Enabled = true;
            this.Freeze(false);
        }

        private void btnAppSapAdd_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            this.Freeze(true);
            // case 1  if ! LoggedIn_BySalesTool
            // handle only this case
            
            if (AddInventoryTransferRequest(this.StockNo) > 0 && ApplySAP(this.StockNo))
            {
                UIHelper.LogMessage("Add Inventory Transfer Request is successfully", UIHelper.MsgType.StatusBar, false);
                this.LoadMainGrid();
                this.btnAppSapAdd.Item.Enabled = false;
                this.btnLoadItem.Item.Enabled = false;
                this.LoadMainGrid();
                //this.loadDatagridMain(true); will code sau
                this.EnabledControl();
            }
            // case 2 LoggedIn_BySalesTool and G_UseWebSrv = 0
            //---- not handle

            // case 3: call service 
            //---= not handle
            this.Freeze(false);
        }
        
        private void btnCancelAdd_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Close();
        }
    }
}
