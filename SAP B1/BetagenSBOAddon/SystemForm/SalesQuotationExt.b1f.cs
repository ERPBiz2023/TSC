using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTCore;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon.SystemForm
{
    [FormAttribute("149", "SystemForm/SalesQuotationExt.b1f")]
    class SalesQuotationExt : SystemFormBase
    {
        private string UserName;
        public SalesQuotationExt()
        {
            UserName = Application.SBO_Application.Company.UserName;
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {
            SAPbouiCOM.Button _2Button = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.UIAPIRawForm.Items.Add("btnADis", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            btnApplyDiscount = ((SAPbouiCOM.Button)(this.GetItem("btnADis").Specific));
            //btnApplyDiscount..DisplayDesc = true;
            btnApplyDiscount.Item.DisplayDesc = false;
            btnApplyDiscount.Item.Top = _2Button.Item.Top;
            btnApplyDiscount.Item.Left = _2Button.Item.Left + 70;
            btnApplyDiscount.Item.Width = 80;
            btnApplyDiscount.Caption = "Apply Discount";
            btnApplyDiscount.ClickBefore += BtnApplyDiscount_ClickBefore;
        }

        private void BtnApplyDiscount_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            try
            {
                SAPbouiCOM.EditText docEntryField = ((SAPbouiCOM.EditText)(this.GetItem("8").Specific));
                var docEntry = -1;
                if (string.IsNullOrEmpty(docEntryField.Value) || !int.TryParse(docEntryField.Value, out docEntry))
                {
                    UIHelper.LogMessage(string.Format("No Discount apply. Please select and open a document"), UIHelper.MsgType.Msgbox);
                    this.Freeze(false);
                    return;
                }

                var query = string.Format(Querystring.sp_Discount_SaleQuotation, docEntry, UserName);
                Hashtable[] datas;
                using (var connection = Globals.DataConnection)
                {
                    datas = connection.ExecQueryToArrayHashtable(query);
                    connection.Dispose();
                }

                if (datas.Count() <= 0)
                {
                    UIHelper.LogMessage(string.Format("No Discount data"), UIHelper.MsgType.Msgbox);
                    this.Freeze(false);
                    return;
                }

                SAPbouiCOM.EditText cardCodeField = ((SAPbouiCOM.EditText)this.GetItem("4").Specific);
                var mtItems = ((SAPbouiCOM.Matrix)(this.GetItem("38").Specific));
                var warehouse = string.Empty;
                if (!string.IsNullOrEmpty(cardCodeField.Value) && mtItems.RowCount > 0)
                {
                    for (int i = 1; i < mtItems.RowCount; i++)
                    {
                        var itemCode = ((SAPbouiCOM.EditText)mtItems.GetCellSpecific("1", i)).Value;
                        if(string.IsNullOrEmpty(warehouse))
                        {
                            warehouse = ((SAPbouiCOM.EditText)mtItems.GetCellSpecific("24", i)).Value;
                        }
                        Hashtable dataFilter = datas.Where(x => x["ItemCode"].ToString() == itemCode && x["LineNum"].ToString() == (i - 1).ToString()).FirstOrDefault();
                        if(dataFilter != null)
                        {
                            ((SAPbouiCOM.EditText)mtItems.GetCellSpecific("15", i)).Value = dataFilter["DiscountPercent"].ToString();
                        }
                    }

                    var dataLenght = datas.Count();
                    for(var i = 0; i < dataLenght; i ++)
                    {
                        if(datas[i]["QType"].ToString() != "0")
                        {
                            int rowCount = mtItems.RowCount;
                            mtItems.GetLineData(rowCount);

                            ((SAPbouiCOM.EditText)mtItems.GetCellSpecific("1", rowCount)).Value = datas[i]["ItemCode"].ToString();
                            ((SAPbouiCOM.EditText)mtItems.GetCellSpecific("U_BaseItem", rowCount)).Value = datas[i]["ItemName"].ToString();
                            ((SAPbouiCOM.EditText)mtItems.GetCellSpecific("11", rowCount)).Value ="-1";
                            ((SAPbouiCOM.EditText)mtItems.GetCellSpecific("24", rowCount)).Value = warehouse;
                            ((SAPbouiCOM.EditText)mtItems.GetCellSpecific("14", rowCount)).Value = datas[i]["PriceAfterDiscount"].ToString();
                            
                        }
                    }
                    UIHelper.LogMessage(string.Format("Applied discount successfully."), UIHelper.MsgType.StatusBar, false);
                }
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Calculate discount is error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);

                this.Freeze(false);
                return;
            }
            this.Freeze(false);
            //SAPbouiCOM.Button _2Button = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
        }
        private void Freeze(bool freeze)
        {
            this.UIAPIRawForm.Freeze(freeze);
        }
        private SAPbouiCOM.Button btnApplyDiscount;
    }
}
