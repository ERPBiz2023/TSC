using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eDSC
{
    class fARInvoice
    {
        private SAPbouiCOM.Application SBO_Application;
        public fARInvoice(SAPbouiCOM.Application app)
        {
            SBO_Application = app;
            SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.BeforeAction == true)
                {
                    
                }
                else
                {
                    if (SBO_Application == null) return;
                    SAPbouiCOM.Form oForm = null;
                    if (pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD & pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_DEACTIVATE)
                        oForm = SBO_Application.Forms.Item(FormUID);

                    switch (pVal.FormTypeEx)
                    {
                            //QUOTATION, ORDER, DELIVERY-----------
                        case "149":
                        case "139":
                        case "140":
                            {
                                switch (pVal.EventType)
                                {
                                    case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
                                        {
                                            SAPbouiCOM.Button obtn;
                                            SAPbouiCOM.Item oitm, itm;
                                            itm = oForm.Items.Item("2");

                                            oitm = oForm.Items.Add("btn", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                                            oitm.Top = itm.Top;
                                            oitm.Left = itm.Left + 90;
                                            oitm.Width = itm.Width + 15;
                                            oitm.Height = itm.Height;
                                            oitm.AffectsFormMode = false;
                                            obtn = (SAPbouiCOM.Button)oitm.Specific;
                                            obtn.Caption = "Draft e-Invoice";
                                            break;
                                        }
                                    case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED:
                                        {
                                            switch (pVal.ItemUID)
                                            {
                                                case "btn":
                                                    { 
                                                        SAPbouiCOM.Button obtn = (SAPbouiCOM.Button)oForm.Items.Item("btn").Specific;

                                                        SAPbobsCOM.Documents trx = null;
                                                        if (pVal.FormTypeEx == "149")
                                                            trx = (SAPbobsCOM.Documents)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                                                        else if (pVal.FormTypeEx == "139")
                                                            trx = (SAPbobsCOM.Documents)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                                        else if (pVal.FormTypeEx == "140")
                                                            trx = (SAPbobsCOM.Documents)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
                                                        
                                                        SAPbouiCOM.DBDataSource odbds = (SAPbouiCOM.DBDataSource)oForm.DataSources.DBDataSources.Item(0);
                                                        string DocEntry = odbds.GetValue("DocEntry", odbds.Offset);


                                                        if (DocEntry == "")
                                                        {
                                                            SBO_Application.MessageBox("Please save document first!");
                                                            return;
                                                        }
                                                        VietteleInvoiceResponse response = new VietteleInvoiceResponse();
                                                        if (trx.GetByKey(int.Parse(DocEntry)))
                                                        {
                                                            //string validate = SAPUtils.TransactionValidation(trx);
                                                            //if (validate != "")
                                                            //{
                                                            //    SBO_Application.SetStatusBarMessage(validate, SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                            //    return;
                                                            //}


                                                            if (trx.CancelStatus == SAPbobsCOM.CancelStatusEnum.csCancellation)
                                                            {
                                                                SBO_Application.SetStatusBarMessage("Can't create draft on cancelled invoice!", SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                            }
                                                            else
                                                            {
                                                                if (SBO_Application.MessageBox("Do you want to create draft invoice for this document?", 1, "Yes", "No") == 1)
                                                                {
                                                                    string transactionUuid = System.Guid.NewGuid().ToString();
                                                                    response = APIUtils.PostTransactionToViettel(trx, true,transactionUuid);

                                                                    if (!string.IsNullOrEmpty(response.errorCode))
                                                                    {
                                                                        SBO_Application.SetStatusBarMessage("Create draft error: " + SAPUtils.GetErrorByCode(response.errorCode), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                    }
                                                                    else
                                                                    {
                                                                        SBO_Application.SetStatusBarMessage("Create draft invoice successful!", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                }
                               
                                break;
                            }
                            




                            //AR INVOICE AND AR CREDIT MEMO-------
                            case "133":
                            case "179":
                            {
                                switch (pVal.EventType)
                                {
                                        //====================Add button on AR Invoice and Credit Memo====================
                                    case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
                                        {
                                            SAPbouiCOM.ButtonCombo obtn;
                                            SAPbouiCOM.Item oitm, itm;
                                            itm = oForm.Items.Item("2");

                                            oitm = oForm.Items.Add("btn", SAPbouiCOM.BoFormItemTypes.it_BUTTON_COMBO);
                                            oitm.Top = itm.Top;
                                            oitm.Left = itm.Left + 90;
                                            oitm.Width = itm.Width+15;
                                            oitm.Height = itm.Height;
                                            oitm.AffectsFormMode = false;
                                            obtn = (SAPbouiCOM.ButtonCombo)oitm.Specific;
                                            obtn.Caption = "e-Invoice";
                                            obtn.ValidValues.Add("post", "Post");
                                            obtn.ValidValues.Add("pdf", "Download PDF");
                                            obtn.ValidValues.Add("exchange", "Exchange File");
                                            //obtn.ValidValues.Add("preview", "Preview");
                                            obtn.ValidValues.Add("draft", "Draft");
                                            obtn.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
                                            break;
                                        }
                                        
                                    case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED:
                                        {
                                            switch (pVal.ItemUID)
                                            {
                                                case "btn":
                                                    {
                                                        
                                                        SAPbouiCOM.ButtonCombo obtn = (SAPbouiCOM.ButtonCombo)oForm.Items.Item("btn").Specific;

                                                        if (obtn.Selected==null) return;

                                                        SAPbobsCOM.Documents trx = null;
                                                        bool isDraft = false;
                                                        if (oForm.Title.Contains("Draft"))
                                                        {
                                                            isDraft = true;
                                                            trx = (SAPbobsCOM.Documents)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                                                        }
                                                           
                                                        else if (pVal.FormTypeEx == "133")
                                                            trx = (SAPbobsCOM.Documents)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                                        else if (pVal.FormTypeEx == "179")
                                                            trx = (SAPbobsCOM.Documents)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);

                                                        
                                                        SAPbouiCOM.DBDataSource odbds = (SAPbouiCOM.DBDataSource)oForm.DataSources.DBDataSources.Item(0);
                                                        string DocEntry = odbds.GetValue("DocEntry", odbds.Offset);


                                                        if (DocEntry == "")
                                                        {
                                                            SBO_Application.MessageBox("Please save document first!");
                                                            return;
                                                        }
                                                        string ret="";
                                                        VietteleInvoiceResponse response = new VietteleInvoiceResponse();
                                                        VietteleInvoiceDraftResponse draftresponse = new VietteleInvoiceDraftResponse() ;
                                                        if (trx.GetByKey(int.Parse(DocEntry)))
                                                        {
                                                            //string validate = SAPUtils.TransactionValidation(trx);
                                                            //if (validate != "")
                                                            //{
                                                            //    SBO_Application.SetStatusBarMessage(validate, SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                            //    return;
                                                            //}
                                                            switch (obtn.Selected.Value)
                                                            {
                                                                case "draft": //create draft invoice on portal
                                                                    
                                                                    if (trx.CancelStatus == SAPbobsCOM.CancelStatusEnum.csCancellation)
                                                                    {
                                                                        SBO_Application.SetStatusBarMessage("Can't create draft on cancelled invoice!", SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                    }
                                                                    else
                                                                    {
                                                                        SAPbobsCOM.BusinessPartners obp = (SAPbobsCOM.BusinessPartners)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                                                                        obp.GetByKey(trx.CardCode);

                                                                        //if (obp.FederalTaxID =="")
                                                                        //{
                                                                        //    SBO_Application.SetStatusBarMessage("BP Tax Code is missing!", SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                        //    break;
                                                                        //}

                                                                        //if (obp.UserFields.Fields.Item("U_Address").Value.ToString() == "")
                                                                        //{
                                                                        //    SBO_Application.SetStatusBarMessage("BP Address is missing!", SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                        //    break;
                                                                        //}

                                                                        if (SBO_Application.MessageBox("Do you want to create draft invoice for this document?", 1, "Yes", "No") == 1)
                                                                        {
                                                                            string transactionUuid = ConnectSAP.GetInvoiceKey(trx); //System.Guid.NewGuid().ToString();
                                                                            response = APIUtils.PostTransactionToViettel(trx, true,transactionUuid);
                                                                            try
                                                                            {
                                                                                if (!string.IsNullOrEmpty(response.errorCode))
                                                                                {
                                                                                    SBO_Application.SetStatusBarMessage("Create draft error: " + SAPUtils.GetErrorByCode(response.errorCode), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                                }
                                                                                else
                                                                                {
                                                                                    SBO_Application.SetStatusBarMessage("Create draft invoice successful!", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                                                                                    try
                                                                                    {
                                                                                        trx.UserFields.Fields.Item("U_TransUid").Value = transactionUuid;
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        Message.MsgBoxWrapper("Trx Update U_TransUid Error: " + ex.Message, "", true);
                                                                                    }

                                                                                    try
                                                                                    {
                                                                                        if (!string.IsNullOrEmpty(response.result.invoiceNo))
                                                                                        {
                                                                                            trx.UserFields.Fields.Item("U_InvCode").Value = response.result.invoiceNo;
                                                                                        }

                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        Message.MsgBoxWrapper("Trx Update U_InvCode Error: " + ex.Message, "", true);
                                                                                    }
                                                                                    trx.Update();
                                                                                }
                                                                            }
                                                                            catch(Exception ex)
                                                                            {

                                                                            }
                                                                            
                                                                        }
                                                                    }

                                                                    

                                                                    break;
                                                                case "post": //create original invoice/cancel invoice/adjustment invoie on portal
                                                                    /*
                                                                     * Tao hoa don
                                                                     * Huy hoa don
                                                                     * Hoa don dieu chinh
                                                                     */
                                                                    if (isDraft)
                                                                    {
                                                                        SBO_Application.SetStatusBarMessage("Can't post e-Invoice from Draft", SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                        return;
                                                                    }
                                                                    if (SBO_Application.MessageBox("Are you sure you want to post this invoice?", 1, "Yes", "No") == 1)
                                                                    {
                                                                        if (trx.CancelStatus == SAPbobsCOM.CancelStatusEnum.csCancellation)
                                                                        {
                                                                            ret = cl_cancelTransactionInvoice.cancelTransactionInvoice(trx);
                                                                        }
                                                                        else
                                                                        {
                                                                            string transactionUuid = ConnectSAP.GetInvoiceKey(trx); //System.Guid.NewGuid().ToString();
                                                                            response =APIUtils.PostTransactionToViettel(trx, false, transactionUuid);
                                                                            try
                                                                            {
                                                                                ret = response.errorCode;
                                                                                if (string.IsNullOrEmpty(ret))
                                                                                {
                                                                                    ////========Add history==============
                                                                                    //SAPUtils.AddEInvoiceHistory(trx.DocEntry.ToString(), trx.DocNum.ToString(), trx.DocObjectCodeEx.ToString(),
                                                                                    //                     response.result.invoiceNo, "", response.uuId,
                                                                                    //                     Setting.templateCode, Setting.invoiceSeries);
                                                                                    //==========update to UDF==============
                                                                                    try
                                                                                    {
                                                                                        trx.UserFields.Fields.Item("U_TransUid").Value = transactionUuid;
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        Message.MsgBoxWrapper("Trx Update U_TransUid Error: " + ex.Message, "", true);
                                                                                    }

                                                                                    try
                                                                                    {
                                                                                        if (!string.IsNullOrEmpty(response.result.invoiceNo))
                                                                                        {
                                                                                            trx.UserFields.Fields.Item("U_InvCode").Value = response.result.invoiceNo;
                                                                                        }
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        Message.MsgBoxWrapper("Trx Update U_InvCode Error: " + ex.Message, "", true);
                                                                                    }

                                                                                    trx.Update();
                                                                                }
                                                                                if (!string.IsNullOrEmpty(ret))
                                                                                {
                                                                                    SBO_Application.SetStatusBarMessage("Posting e-Invoice error: " + SAPUtils.GetErrorByCode(ret), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                                }
                                                                                else
                                                                                {
                                                                                    SBO_Application.SetStatusBarMessage("e-Invoice posted completed!", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                                                                                }
                                                                            }
                                                                            catch(Exception ex)
                                                                            {

                                                                            }
                                                                            
                                                                        }

                                                                        
                                                                    }
                                                                    
                                                                    break;
                                                                case "pdf": //download original invoice in pdf  
                                                                    if (isDraft)
                                                                    {
                                                                        SBO_Application.SetStatusBarMessage("Can't download PDF file from Draft", SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                        return;
                                                                    }
                                                                    if (SBO_Application.MessageBox("Do you want to download original invoice in PDF?", 1, "Yes", "No") == 1)
                                                                    {
                                                                        //=====tai file hoa don pdf==============
                                                                        draftresponse = cl_getInvoiceRepresentationFile.getInvoiceRepresentationFile(trx.UserFields.Fields.Item("U_InvCode").Value.ToString());

                                                                        if (!string.IsNullOrEmpty(draftresponse.errorCode))
                                                                        {
                                                                            if(draftresponse.errorCode!="200")
                                                                            {
                                                                                SBO_Application.SetStatusBarMessage("Download PDF error: " + SAPUtils.GetErrorByCode(draftresponse.errorCode), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                            }
                                                                            else
                                                                            {
                                                                                SBO_Application.SetStatusBarMessage("PDF is downloaded!", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                                                                            }
                                                                           
                                                                        }
                                                                        else
                                                                        {
                                                                            SBO_Application.SetStatusBarMessage("PDF is downloaded!", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                                                                        }
                                                                    }
                                                                    
                                                                    break;
                                                                case "exchange": //create exchange invoice and download pdf
                                                                    if (isDraft)
                                                                    {
                                                                        SBO_Application.SetStatusBarMessage("Can't download exchange file from Draft", SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                        return;
                                                                    }
                                                                    if (SBO_Application.MessageBox("Do you want to download Exchange invoice in PDF?", 1, "Yes", "No") == 1)
                                                                    {
                                                                        //========tai file hoa don chuyen doi==========
                                                                        draftresponse = cl_createExchangeInvoiceFile.createExchangeInvoiceFile(trx.UserFields.Fields.Item("U_InvCode").Value.ToString(),trx.DocDate + trx.DocTime.TimeOfDay);
                                                                        if (!string.IsNullOrEmpty(draftresponse.errorCode))
                                                                        {
                                                                            SBO_Application.SetStatusBarMessage("Create Exchange error: " + SAPUtils.GetErrorByCode(draftresponse.errorCode), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                        }
                                                                        else
                                                                        {
                                                                            SBO_Application.SetStatusBarMessage("PDF is downloaded!", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                                                                        }
                                                                    }
                                                                   
                                                                    break;
                                                                
                                                                case "preview": //preview in pdf without saving
                                                                    if (SBO_Application.MessageBox("Do you want to preview this invoice in PDF? (No draft invoice created)", 1, "Yes", "No") == 1)
                                                                    {
                                                                        //===============xem hoa don nhap===============
                                                                        VietteleInvoiceDraftResponse res = cl_createInvoiceDraftPreview.createInvoiceDraftPreview(trx);
                                                                        if (!string.IsNullOrEmpty(res.errorCode))
                                                                        {
                                                                            SBO_Application.SetStatusBarMessage("preview error: " + SAPUtils.GetErrorByCode(res.errorCode), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
                                                                        }
                                                                        else
                                                                        {
                                                                            SBO_Application.SetStatusBarMessage("PDF is downloaded!", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                                                                        }
                                                                    }
                                                                    
                                                                    break;
                                                            }
                                                        }

                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                SBO_Application.SetStatusBarMessage("ItemEvent Event: " + ex.ToString(), SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }
      
        

    }
}
