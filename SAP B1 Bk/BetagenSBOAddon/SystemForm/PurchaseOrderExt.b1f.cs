using BetagenSBOAddon.AccessSAP;
using BetagenSBOAddon.Forms;
using GTCore;
using GTCore.Config;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace BetagenSBOAddon.SystemForm
{
    [FormAttribute("142", "SystemForm/PurchaseOrderExt.b1f")]
    class PurchaseOrderExt : SystemFormBase
    {
        private string PoNo;
        private string PoStatus;
        private PODAL POAccess;
        public PurchaseOrderExt()
        {
            POAccess = new PODAL();
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
            //    var no = ((SAPbouiCOM.EditText)(this.GetItem("8").Specific));
            //    PoNo = no.Value;
            //  this.btnCopyGRPO = ((SAPbouiCOM.Button)(this.GetItem("btnCTo").Specific));
            
            //this.btnCopyGRPO = ((SAPbouiCOM.Button)(this.GetItem("btnCTo").Specific));
            
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
            this.UIAPIRawForm.Items.Add("btnCop", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            this.btnCopyGRPO = ((SAPbouiCOM.Button)(this.GetItem("btnCop").Specific));
            this.btnCopyGRPO.Caption = "Copy to GRPO";
            this.btnCopyGRPO.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCopyGRPO_ClickBefore);

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
            this.btnAllBa.Item.Top = this.btnAllFr.Item.Top;
            this.btnAllBa.Item.Left = _2349990001Button.Item.Left;
            this.btnAllBa.Item.Width = _2349990001Button.Item.Width;
            this.btnAllBa.Item.Height = _2349990001Button.Item.Height;
           
            SAPbouiCOM.Button _2Button = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            _2Button.Item.Top = this.btnAllBa.Item.Top + 30;
            _2Button.Item.Width = 110;
            _2Button.Item.Left = _2349990001Button.Item.Left + 110;
            this.btnCopyGRPO.Item.Top = this.btnAllFr.Item.Top;
            this.btnCopyGRPO.Item.Left = _2Button.Item.Left;
            this.btnCopyGRPO.Item.Width = _2Button.Item.Width;
            this.btnCopyGRPO.Item.Height = _2Button.Item.Height;
            var GRPOStandard = ConfigurationManager.AppSettings["GRPOStandard"].ToString();
            this.btnCopyGRPO.Item.Enabled = true;
            //if (GRPOStandard == "1")
            //    this.btnCopyGRPO.Item.Visible = false;
            //else
            this.btnCopyGRPO.Item.Visible = true;
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
                POAllocationBatch.ShowForm(PoNo, PoStatus);
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

            var docHeaderDS = this.UIAPIRawForm.DataSources.DBDataSources.Item("OPOR");

            var status = docHeaderDS.GetValue("DocStatus", 0);// ((SAPbouiCOM.EditText)(this.GetItem("81").Specific));
            PoStatus = status;
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
                    var totalVol = 0.0;
                    var linesDS = this.UIAPIRawForm.DataSources.DBDataSources.Item("POR1");
                    Dictionary<int, double> volumns = new Dictionary<int, double>();
                    var isSetVol = true;

                    for (int i = 1; i <mtItems.RowCount; i++)
                    {
                        var query = string.Empty;

                        var itemcode = ((EditText)mtItems.GetCellSpecific("1", i)).Value;
                        if (CoreSetting.System == SystemType.SAP_HANA)
                        {
                            var schema = ConfigurationManager.AppSettings["Schema"];
                            query = string.Format("SELECT \"U_Volume\", \"NumInBuy\" FROM \"{1}\".OITM WHERE \"ItemCode\" = '{0}'", itemcode, schema);
                        }
                        else
                        {
                            query = string.Format(@"SELECT U_Volume, 
                                                           NumInBuy
                                                      FROM OITM
                                                     WHERE ItemCode = '{0}'", itemcode);
                        }
                        Hashtable data;
                        using (var connection = Globals.DataConnection)
                        {
                            data = connection.ExecQueryToHashtable(query);
                            connection.Dispose();
                        }
                        
                        if (data != null)
                        {
                            var volText = data["U_Volume"].ToString();
                            var numinByText = data["NumInBuy"].ToString();

                            decimal vol;
                            if(!decimal.TryParse(volText, out vol) || vol == 0)
                            {
                                isSetVol = false;
                                break;
                            }
                           
                            decimal num;
                            decimal.TryParse(numinByText, out num);


                            var qtyField = ((EditText)mtItems.GetCellSpecific("11", i)).Value;
                            decimal qty;
                            decimal.TryParse(qtyField, out qty);

                            double volNumber = (double)( num * vol * qty);
                            volumns.Add(i, volNumber);
                            totalVol += volNumber;
                        }

                    }

                    if(!isSetVol)
                    {
                        UIHelper.LogMessage(string.Format("There is Item not have Volume (ml), please re-check!"), UIHelper.MsgType.StatusBar, true);
                        this.Freeze(false);
                        return;
                    }
                    var totalOrg = 0.0;

                    var rateField = docHeaderDS.GetValue("DocRate", 0);
                    var docRate = double.Parse(rateField);
                    var newTotal = 0.0;
                    for (int i = 1; i < mtItems.RowCount; i++)
                    {
                        var freight = 0.0;
                        freight = volumns[i] / totalVol * (double)(freightcostNumber * docRate);
                        var lineTotal = linesDS.GetValue("LineTotal", i-1);
                        var lineorgTotal = linesDS.GetValue("U_OrgTotal", i - 1);
                        var rate = double.Parse(linesDS.GetValue("Rate", i - 1));

                        //var lineTotalField = ((EditText)mtItems.GetCellSpecific("23", i)).Value.Replace("VND", "").Replace("USD", "");
                        double lineorgTotalNumber;
                        if (string.IsNullOrEmpty(lineorgTotal) || !double.TryParse(lineorgTotal, out lineorgTotalNumber))
                        {
                            //double lineTotalNumber;
                            if (!double.TryParse(lineTotal, out lineorgTotalNumber))
                            {
                                continue;
                                /// throw message or no acion in here?
                            }
                            ((EditText)mtItems.GetCellSpecific("U_OrgTotal", i)).Value = lineorgTotalNumber.ToString();
                        }
                        
                        
                        totalOrg += lineorgTotalNumber;
                        if (rate>0)
                        {
                            freight /= rate;
                            lineorgTotalNumber /= rate;
                        }
                        var lineValue = lineorgTotalNumber + freight;
                        newTotal += lineValue;
                        ((EditText)mtItems.GetCellSpecific("23", i)).Value = lineValue.ToString();
                        ((EditText)mtItems.GetCellSpecific("U_LineTotalAfterF", i)).Value = lineValue.ToString();
                    }

                    
                    var totalField = ((double)(totalOrg + freightcostNumber) / docRate).ToString();

                    ((EditText)this.GetItem("29").Specific).Value = newTotal.ToString() ;
                    ((EditText)this.GetItem("24").Specific).Value = "0";
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

        private Button btnCopyGRPO;

        private void btnCopyGRPO_ClickBefore(object sboObject, SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_OK_MODE )
            {
                UIHelper.LogMessage(string.Format("Please select 1 document or update/add document completed"), UIHelper.MsgType.StatusBar, true);
                
                return;
            }

            this.Freeze(true);
            try
            {
                var dateText = ((SAPbouiCOM.EditText)(this.GetItem("12").Specific)).Value;
                DateTime date;
                //DateTime.TryParse(dateText, out date);
                DateTime.TryParseExact(dateText, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                var yes = UIHelper.LogMessage(string.Format("Do you want to Create Goods Receipt PO Documemnt with Receipt Date: {0}", dateText),
                                                      UIHelper.MsgType.Msgbox, false, 1, "Yes", "No");
                if (yes == 1)
                {
                    var message = string.Empty;
                    var docentry = POAccess.CreateGRPOBaseonPO(PoNo, date, ref message);
                    if(docentry > 0)
                    {
                        UIHelper.LogMessage(string.Format("Create Good Receipt PO {0} successfully", docentry), UIHelper.MsgType.StatusBar, false);
                    }
                    else
                    {
                        UIHelper.LogMessage(string.Format("Create Good Receipt PO error {0}", message), UIHelper.MsgType.StatusBar, true);
                    }
                }

            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Copy to Good Receipt PO failed {0}", ex.Message), UIHelper.MsgType.StatusBar, true);

                this.Freeze(false);
                return;
            }
            this.Freeze(false);
        }
        
    }
}
