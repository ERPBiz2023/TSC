using BetagenSBOAddon.Forms;
using GTCore;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;

namespace BetagenSBOAddon.SystemForm
{
    [FormAttribute("142", "SystemForm/PurchaseOrderExt.b1f")]
    class PurchaseOrderExt : SystemFormBase
    {
        private string PoNo;
        public PurchaseOrderExt()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.btnAllBa = ((SAPbouiCOM.Button)(this.GetItem("btnAllBa").Specific));
            this.btnAllBa.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAllBa_ClickBefore);
            this.btnAllFr = ((SAPbouiCOM.Button)(this.GetItem("btnAllFr").Specific));
            this.btnAllFr.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAllFr_ClickBefore);
            // var no = ((SAPbouiCOM.EditText)(this.GetItem("8").Specific));
            // PoNo = no.Value;
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new DataLoadAfterHandler(this.Form_DataLoadAfter);
        }

        private SAPbouiCOM.Button btnAllBa;

        private void OnCustomInitialize()
        {
            SAPbouiCOM.StaticText ownerText = ((SAPbouiCOM.StaticText)(this.GetItem("230").Specific));
            var finalHeight = ownerText.Item.Top;
            SAPbouiCOM.StaticText remartText = ((SAPbouiCOM.StaticText)(this.GetItem("17").Specific));
            remartText.Item.Top = finalHeight + 30;

            SAPbouiCOM.EditText remartEdit = ((SAPbouiCOM.EditText)(this.GetItem("16").Specific));
            remartEdit.Item.Top = finalHeight + 30;

            this.btnAllFr.Item.Top = remartEdit.Item.Top + 70;
            this.btnAllBa.Item.Top = remartEdit.Item.Top + 70;

            SAPbouiCOM.ButtonCombo _1Button = ((SAPbouiCOM.ButtonCombo)(this.GetItem("1").Specific));
            _1Button.Item.Top = this.btnAllBa.Item.Top + 30;
            _1Button.Item.Width = 90;
            this.btnAllFr.Item.Left = _1Button.Item.Left;
            this.btnAllFr.Item.Width = _1Button.Item.Width;
            this.btnAllFr.Item.Height = _1Button.Item.Height;

            SAPbouiCOM.ButtonCombo _2349990001Button = ((SAPbouiCOM.ButtonCombo)(this.GetItem("2349990001").Specific));
            _2349990001Button.Item.Top = this.btnAllBa.Item.Top + 30;
            _2349990001Button.Item.Width = 100;
            _2349990001Button.Item.Left = _1Button.Item.Left + 100;
            this.btnAllBa.Item.Left = _2349990001Button.Item.Left;
            this.btnAllBa.Item.Width = _2349990001Button.Item.Width;
            this.btnAllBa.Item.Height = _2349990001Button.Item.Height;
           
            SAPbouiCOM.Button _2Button = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            _2Button.Item.Top = this.btnAllBa.Item.Top + 30;
            _2Button.Item.Left = _2349990001Button.Item.Left + 110;

            SAPbouiCOM.ComboBox _CopyFromButton = ((SAPbouiCOM.ComboBox)(this.GetItem("10000330").Specific));
            _CopyFromButton.Item.Top = _1Button.Item.Top;

            SAPbouiCOM.ComboBox _CopyFromTo = ((SAPbouiCOM.ComboBox)(this.GetItem("10000329").Specific));
            _CopyFromTo.Item.Top = _1Button.Item.Top;
        }

        private SAPbouiCOM.Button btnAllFr;
        private void Freeze(bool freeze)
        {
            this.UIAPIRawForm.Freeze(freeze);
        }
        private void btnAllBa_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE ||
                this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
            {
                POAllocationBatch.ShowForm(PoNo);
            }
            else
            {
                UIHelper.LogMessage("Please select PO Entry and Confirm this PO", UIHelper.MsgType.Msgbox);
                return;
            }
        }

        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            var no = ((SAPbouiCOM.EditText)(this.GetItem("8").Specific));
            PoNo = no.Value;
        }

        private void btnAllFr_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE ||
                this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
            {
                try
                {
                    var docHeaderDS = this.UIAPIRawForm.DataSources.DBDataSources.Item("OPOR");
                    var datafreightcost = docHeaderDS.GetValue("U_FreightCost", 0);
                    //var freightcostField = ((SAPbouiCOM.EditText)(this.UIAPIRawForm.Items.Item("U_FreightCost")));
                    double freightcostNumber;
                    if (!double.TryParse(datafreightcost, out freightcostNumber))
                    {
                        UIHelper.LogMessage(string.Format("Please check Freight Cost field data (Only input numeric in here)"), UIHelper.MsgType.StatusBar, true);
                        this.Freeze(false);
                        return;
                    }
                    var mtItems = ((SAPbouiCOM.Matrix)(this.GetItem("38").Specific));
                   //mtItems.FlushToDataSource();
                    var totalWeigt = 0.0;
                    var linesDS = this.UIAPIRawForm.DataSources.DBDataSources.Item("POR1");
                    Dictionary<int, double> weights = new Dictionary<int, double>();
                    for (int i = 1; i <mtItems.RowCount; i++)
                    {
                        var weightField = ((EditText)mtItems.GetCellSpecific("58", i)).Value;
                        double weightNumber;
                        if (!double.TryParse(weightField.Replace("kg", ""), out weightNumber))
                        {
                            continue;
                            /// throw message or no acion in here?
                        }
                        weights.Add(i, weightNumber);
                        totalWeigt += weightNumber;
                    }
                    var totalOrg = 0.0;
                    for (int i = 1; i < mtItems.RowCount; i++)
                    {
                        var freight = 0.0;
                        freight = weights[i] / totalWeigt * freightcostNumber;
                        var lineTotal = linesDS.GetValue("LineTotal", i-1);
                        var rate = double.Parse(linesDS.GetValue("Rate", i - 1));

                        //var lineTotalField = ((EditText)mtItems.GetCellSpecific("23", i)).Value.Replace("VND", "").Replace("USD", "");
                        double lineTotalNumber;
                        if (!double.TryParse(lineTotal, out lineTotalNumber))
                        {
                            continue;
                            /// throw message or no acion in here?
                        }
                        if(rate>0)
                        {
                            freight /= rate;
                        }
                        totalOrg += lineTotalNumber;
                        ((EditText)mtItems.GetCellSpecific("U_LineTotalAfterF", i)).Value = (lineTotalNumber + freight).ToString();
                    }

                   // var totalField = docHeaderDS.GetValue("DocTotal", 0);/// (EditText)this.GetItem("29").Specific;
                    var rateField = docHeaderDS.GetValue("DocRate", 0);
                   // var docTotal = double.Parse(totalField);
                    var docRate = double.Parse(rateField);

                    //docHeaderDS.SetValue("DocTotal", 0, (docTotal + freightcostNumber).ToString());
                   var  totalField = ((totalOrg + freightcostNumber)/ docRate).ToString();
                    
                    ((EditText)this.GetItem("29").Specific).Value = totalField;
                    this.UIAPIRawForm.Refresh();
                }
                catch (Exception ex)
                    {
                    UIHelper.LogMessage(string.Format("Allocate Freight error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
                    this.Freeze(false);
                    return;
                }
            }
            else
            {
                UIHelper.LogMessage("Please select PO Entry and Confirm this PO", UIHelper.MsgType.Msgbox);

                this.Freeze(false);
                return;
            }
            this.Freeze(false);
        }
    }
}
