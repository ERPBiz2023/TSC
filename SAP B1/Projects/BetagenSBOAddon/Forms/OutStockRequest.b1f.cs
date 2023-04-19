using BetagenSBOAddon.AccessSAP;
using GTCore;
using GTCore.Forms;
using GTCore.SAP.DIAPI;
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
        private bool checkNullDate;
        private List<int> ListSelect;
        private OutStockRequestDAL DALAccess;

        public DateTime FromDate
        {
            get => fromDate;
            set
            {
                fromDate = value;
                if (this.fromDate == Globals.NullDate)
                    this.edFromDateList.Value = string.Empty;
                else if (this.edFromDateList.Value != fromDate.ToString("dd.MM.yyyy"))
                    this.edFromDateList.Value = fromDate.ToString("dd.MM.yyyy");
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
                else if (this.edToDateList.Value != toDate.ToString("dd.MM.yyyy"))
                    this.edToDateList.Value = toDate.ToString("dd.MM.yyyy");
            }
        }
        public bool CheckNullDate
        {
            get => checkNullDate;
            set
            {
                checkNullDate = value;
                this.cbNullDate.Item.Enabled = true;
                // this.cbNullDate.Checked = checkNullDate;
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
            this.FromDate = DateTime.Now;
            this.ToDate = DateTime.Now;
            this.CheckNullDate = true;

            ListSelect = new List<int>();
            DALAccess = new OutStockRequestDAL();
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.tagPage1 = ((SAPbouiCOM.Folder)(this.GetItem("tagPage1").Specific));
            this.tagPage2 = ((SAPbouiCOM.Folder)(this.GetItem("tagPage2").Specific));
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
            this.btnDelete = ((SAPbouiCOM.Button)(this.GetItem("btnDelete").Specific));
            this.btnDelete.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnDelete_PressedAfter);
            this.btnCancle = ((SAPbouiCOM.Button)(this.GetItem("btnCancle").Specific));
            this.btnCancle.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnCancle_PressedAfter);
            this.btnConfirm = ((SAPbouiCOM.Button)(this.GetItem("btnConfirm").Specific));
            this.btnConfirm.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnConfirm_ClickBefore);
            this.btnAppSapList = ((SAPbouiCOM.Button)(this.GetItem("btnAppLst").Specific));
            this.btnAppSapList.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAppSapList_ClickBefore);
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_14").Specific));
            this.edFromWH = ((SAPbouiCOM.EditText)(this.GetItem("edFromWH").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_20").Specific));
            this.edFromTeam = ((SAPbouiCOM.EditText)(this.GetItem("edFromTeam").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_24").Specific));
            this.edNo = ((SAPbouiCOM.EditText)(this.GetItem("edNo").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_28").Specific));
            this.edToWH = ((SAPbouiCOM.EditText)(this.GetItem("edToWH").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_30").Specific));
            this.edToTeam = ((SAPbouiCOM.EditText)(this.GetItem("edToTeam").Specific));
            this.edDateAdd = ((SAPbouiCOM.EditText)(this.GetItem("edDateAdd").Specific));
            this.StaticText12 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_34").Specific));
            this.StaticText13 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_35").Specific));
            this.edNote = ((SAPbouiCOM.EditText)(this.GetItem("edNote").Specific));
            this.btnLoadItem = ((SAPbouiCOM.Button)(this.GetItem("btnLoad").Specific));
            this.grAdd = ((SAPbouiCOM.Grid)(this.GetItem("grAdd").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.btnCancelAdd = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.btnAddNew = ((SAPbouiCOM.Button)(this.GetItem("btnAddNew").Specific));
            this.btnAppSapAdd = ((SAPbouiCOM.Button)(this.GetItem("btnAppAdd").Specific));
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
                    this.FromDate.ToString("yyyy-MM-dd"),
                    this.ToDate.ToString("yyyy-MM-dd"),
                    1);
                this.grList.DataTable.ExecuteQuery(query);

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

                this.grList.Columns.Item("Cofirm").TitleObject.Caption = "Cofirm Status";
                this.grList.Columns.Item("Cofirm").Editable = false;

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

        private bool DeleteApplySAP()
        {
            if(this.grList.Rows.Count == 0)
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
                        query += "\n" + string.Format(Querystring.sp_DeleteOutStockRequest, code);
                    }
                }
                if (!string.IsNullOrEmpty(query))
                {
                    query += "\n" + Querystring.sp_NotificationUpdateStock;
                    using (var connection = Globals.DataConnection)
                    {
                        connection.ExecuteWithOpenClose(query);
                        connection.Dispose();
                    }
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
                if(ret > 0)
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
        private SAPbouiCOM.EditText edFromWH;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.EditText edFromTeam;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.EditText edNo;
        private SAPbouiCOM.StaticText StaticText9;
        private SAPbouiCOM.EditText edToWH;
        private SAPbouiCOM.StaticText StaticText10;
        private SAPbouiCOM.EditText edToTeam;
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
                if (DateTime.TryParseExact(edFromDateList.Value, "dd'.'MM'.'yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out outdate))
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
                if (DateTime.TryParseExact(edToDateList.Value, "dd'.'MM'.'yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out outdate))
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
                if(DeleteApplySAP())
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
                    if(!ListSelect.Contains(pVal.Row))
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
            if(ListSelect.Count > 0)
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
                if(string.IsNullOrEmpty(stockNo) || string.IsNullOrEmpty(reqNo))
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
    }
}
