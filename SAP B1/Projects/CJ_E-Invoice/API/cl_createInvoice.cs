using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace eDSC
{
    class cl_createInvoice
    {
        public static VietteleInvoiceResponse createInvoice(SAPbobsCOM.Documents trx)
        {
            try
            {
                //=========check history if invoice is posted==========
                ViettelEHistory his = SAPUtils.LoadeInvoiceHistory(trx);
                if (his != null)
                {
                    return new VietteleInvoiceResponse() { errorCode = "Invoice is already posted" };
                }

                //check tax percent if more than 1 tax code in same transaction
                double taxpercent = SAPUtils.GetTaxPercent(trx);
                if (taxpercent == -1)
                {
                    return new VietteleInvoiceResponse() { errorCode = "Invalid Tax!" };
                }


                //==if transaction is AR Credit Memo ==> Create Adjustment Decrease ==== DIEU CHINH GIAM
                if (trx.DocObjectCodeEx == "14")
                {
                    if (trx.UserFields.Fields.Item("U_IEV_OrgInvNo").Value.ToString() == "")
                    {
                        return new VietteleInvoiceResponse() { errorCode = "Original Invoice No. is missing!" };
                    }
                    else
                    {
                        return createAdjInvoice_Dec(trx);
                    }
                }
                else
                {
                    //==if transaction is AR Invoice and OrgInvo has value ==> Create Adjustment Increase ==== DIEU CHINH TANG
                    if (trx.UserFields.Fields.Item("U_IEV_OrgInvNo").Value.ToString() != "")
                        return createAdjInvoice_Inc(trx);
                    else
                        return createOrgInvoice(trx); // else create ORIGINAL INVOICE
                }
            }
            catch (Exception ex)
            {
                SAPUtils.SAPWriteLog("createInvoice", "", "", ex.ToString(), "");
                return new VietteleInvoiceResponse() { errorCode = ex.ToString() };
            }
        }

        public static VietteleInvoiceResponse createOrgInvoice(SAPbobsCOM.Documents trx)
        {
            try
            {
                string createInvoiceUrl = Setting.APIURL + "/InvoiceWS/createInvoice/" + Setting.supplierTaxCode;
                string responsebody = "";
                string json = string.Empty;
                string transactionUuid = System.Guid.NewGuid().ToString();

                VietteleInvoiceData data = new VietteleInvoiceData();

                SAPbobsCOM.BusinessPartners obp = (SAPbobsCOM.BusinessPartners)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                if (!obp.GetByKey(trx.CardCode))
                    return new VietteleInvoiceResponse() { errorCode = "Customer not found!" };

                DateTime issuedate = trx.DocDate + trx.DocTime.TimeOfDay;

                data.generalInvoiceInfo = new generalInvoiceInfo()
                {
                    invoiceType = Setting.invoiceType,
                    templateCode = Setting.templateCode,
                    invoiceSeries = Setting.invoiceSeries,
                    invoiceIssuedDate = ((Int64)(issuedate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString(),
                    currencyCode = trx.DocCurrency,
                    adjustmentType = eDSC.adjustmentType.Original,
                    paymentStatus = true,
                    cusGetInvoiceRight = true,
                    paymentType = "TM/CK",
                    paymentTypeName = "TM/CK",
                    transactionUuid = transactionUuid,
                    userName = Setting.APIUser,
                };


                data.buyerInfo = new buyerInfo()
                {
                    buyerName = "",
                    buyerBankAccount = "",
                    buyerBankName = "",
                    buyerCityName = "",
                    buyerDistrictName = "",
                    buyerEmail = obp.EmailAddress,
                    buyerPhoneNumber = obp.Cellular,
                    buyerTaxCode = obp.FederalTaxID,
                    buyerLegalName = trx.CardName,
                    buyerAddressLine = trx.Address,

                };
                List<itemInfo> lst = new List<itemInfo>();
                double totalTax = 0;
                for (int i = 0; i <= trx.Lines.Count - 1; i++)
                {
                    trx.Lines.SetCurrentLine(i);
                    lst.Add(new itemInfo()
                    {
                        lineNumber = i + 1,
                        itemName = trx.Lines.ItemDescription,
                        unitPrice = (decimal)trx.Lines.UnitPrice,
                        quantity = (decimal)trx.Lines.Quantity,
                        itemTotalAmountWithoutTax = (decimal)trx.Lines.LineTotal,
                        itemTotalAmountWithTax = (decimal)trx.Lines.GrossTotal,
                        itemTotalAmountAfterDiscount = (decimal)trx.Lines.TotalInclTax,
                        taxPercentage = (decimal)trx.Lines.TaxPercentagePerRow,
                        taxAmount = (decimal)trx.Lines.TaxTotal,
                        discount = (decimal)trx.Lines.DiscountPercent,
                        itemDiscount = (decimal)(trx.Lines.UnitPrice * trx.Lines.Quantity * trx.Lines.DiscountPercent / 100),
                        adjustmentTaxAmount = 1,
                    });
                    totalTax += trx.Lines.TaxTotal;
                }
                data.itemInfo = lst;


                data.summarizeInfo = new summarizeInfo()
                {
                    sumOfTotalLineAmountWithoutTax = (decimal)(trx.DocTotal - totalTax),
                    totalAmountWithoutTax = (decimal)(trx.DocTotal - totalTax),
                    totalTaxAmount = (decimal)totalTax,
                    totalAmountWithTax = (decimal)(trx.DocTotal),
                    totalAmountWithTaxInWords = "",
                    discountAmount = (decimal)trx.TotalDiscount,
                    settlementDiscountAmount = 0,
                };


                json = Newtonsoft.Json.JsonConvert.SerializeObject(data,new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});


                using (var webClient = new System.Net.WebClient())
                {
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Setting.APIUser + ":" + Setting.APIPassword));
                    webClient.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + credentials;
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    webClient.Headers[System.Net.HttpRequestHeader.Accept] = "application/json";

                    responsebody = webClient.UploadString(createInvoiceUrl, "POST", json);
                    SAPUtils.SAPWriteLog("createInvoice", json, responsebody, "", createInvoiceUrl);


                    VietteleInvoiceResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<VietteleInvoiceResponse>(responsebody);

                    if (string.IsNullOrEmpty(response.errorCode))
                    {
                        //if successfull => add to history
                        SAPUtils.AddEInvoiceHistory(trx.DocEntry.ToString(), trx.DocNum.ToString(), trx.DocObjectCodeEx.ToString(),
                            response.result.invoiceNo, response.result.transactionID, transactionUuid,
                            Setting.templateCode, Setting.invoiceSeries);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                SAPUtils.SAPWriteLog("createOrgInvoice", "", "", ex.ToString(), "");
                return new VietteleInvoiceResponse() { errorCode = ex.ToString() };
            }
        }

        public static VietteleInvoiceResponse createAdjInvoice_Inc(SAPbobsCOM.Documents trx)
        {
            ViettelEHistory his = SAPUtils.LoadeInvoiceHistory(trx);
            if (his != null)
            {
                return new VietteleInvoiceResponse() { errorCode = "Invoice is already posted" };
            }

            double taxpercent = SAPUtils.GetTaxPercent(trx);
            if (taxpercent == -1)
            {
                return new VietteleInvoiceResponse() { errorCode = "Invalid Tax!" };
            }

            try
            {
                string createInvoiceUrl = Setting.APIURL + "/InvoiceWS/createInvoice/" + Setting.supplierTaxCode;
                string responsebody = "";
                string json = string.Empty;
                string transactionUuid = System.Guid.NewGuid().ToString();

                VietteleInvoiceData data = new VietteleInvoiceData();

                SAPbobsCOM.BusinessPartners obp = (SAPbobsCOM.BusinessPartners)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                if (!obp.GetByKey(trx.CardCode))
                    return new VietteleInvoiceResponse() { errorCode = "Customer not found!" };


                DateTime issuedate = trx.DocDate + trx.DocTime.TimeOfDay;

                //===============AJUSTMENT===============
                string isIncreaseItem = null;
                string isTotalAmountPos = null;
                string isTotalTaxAmountPos = null;
                string isTotalAmtWithoutTaxPos = null;
                string isDiscountAmtPos = null;

                adjustmentType adjustmentType = eDSC.adjustmentType.Original;
                adjustmentInvoiceType adjustmentInvoiceType = eDSC.adjustmentInvoiceType.Money;
                string originalInvoiceId = trx.UserFields.Fields.Item("U_IEV_OrgInvNo").Value.ToString();
                string originalInvoiceIssueDate = "";

                if (trx.UserFields.Fields.Item("U_IEV_OrgInvDate").Value.ToString() != "")
                    originalInvoiceIssueDate = DateTime.Parse(trx.UserFields.Fields.Item("U_IEV_OrgInvDate").Value.ToString()).ToString("yyyyMMdd") + "000000";

                if (trx.DocObjectCodeEx == "14")
                {
                    //=====+AR Credit memo ===========> AJUSTMENT INVOICE=========
                    adjustmentType = eDSC.adjustmentType.Ajustment;
                    isIncreaseItem = "false";
                    isTotalAmountPos = "false";
                    isTotalTaxAmountPos = "false";
                    isTotalAmtWithoutTaxPos = "false";
                    isDiscountAmtPos = "false";
                    if (originalInvoiceId == "")
                    {
                        return new VietteleInvoiceResponse() { errorCode = "Please enter original Invoice No.!" };
                    }
                }

                if (originalInvoiceId != "")
                {
                    //=====NOT ORIGINAL INVOICE===========
                    adjustmentType = eDSC.adjustmentType.Ajustment;

                    //======DIEU CHINH TANG==========
                    if (trx.DocObjectCodeEx == "14")
                        isIncreaseItem = "true";
                }



                data.generalInvoiceInfo = new generalInvoiceInfo()
                {
                    invoiceType = Setting.invoiceType,
                    templateCode = Setting.templateCode,
                    invoiceSeries = Setting.invoiceSeries,
                    invoiceIssuedDate = ((Int64)(issuedate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString(),
                    currencyCode = trx.DocCurrency,
                    adjustmentType = adjustmentType,
                    adjustmentInvoiceType = adjustmentInvoiceType,
                    originalInvoiceId = originalInvoiceId,
                    originalInvoiceIssueDate = originalInvoiceIssueDate,
                    additionalReferenceDate = ((Int64)(issuedate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString(),
                    additionalReferenceDesc = trx.DocNum.ToString(),
                    paymentStatus = true,
                    cusGetInvoiceRight = true,
                    paymentType = "TM/CK",
                    paymentTypeName = "TM/CK",
                    transactionUuid = transactionUuid,
                    userName = Setting.APIUser,
                };


                data.buyerInfo = new buyerInfo()
                {
                    buyerName = "",
                    buyerBankAccount = "",
                    buyerBankName = "",
                    buyerCityName = "",
                    buyerDistrictName = "",
                    buyerEmail = obp.EmailAddress,
                    buyerPhoneNumber = obp.Cellular,
                    buyerTaxCode = obp.FederalTaxID,
                    buyerLegalName = trx.CardName,
                    buyerAddressLine = trx.Address,

                };
                List<itemInfo> lst = new List<itemInfo>();
                double totalTax = 0;
                for (int i = 0; i <= trx.Lines.Count - 1; i++)
                {
                    trx.Lines.SetCurrentLine(i);
                    lst.Add(new itemInfo()
                    {
                        lineNumber = i + 1,
                        itemName = trx.Lines.ItemDescription,
                        unitPrice = (decimal)trx.Lines.Price,
                        quantity = (decimal)trx.Lines.Quantity,
                        itemTotalAmountWithoutTax = (decimal)trx.Lines.LineTotal,
                        itemTotalAmountWithTax = (decimal)trx.Lines.GrossTotal,
                        itemTotalAmountAfterDiscount = (decimal)trx.Lines.TotalInclTax,
                        taxPercentage = (decimal)trx.Lines.TaxPercentagePerRow,
                        taxAmount = (decimal)trx.Lines.TaxTotal,
                        discount = (decimal)trx.Lines.DiscountPercent,
                        itemDiscount = (decimal)(trx.Lines.UnitPrice * trx.Lines.Quantity * trx.Lines.DiscountPercent / 100),
                        adjustmentTaxAmount = 1,
                        isIncreaseItem = isIncreaseItem
                    });
                    totalTax += trx.Lines.TaxTotal;
                }
                data.itemInfo = lst;


                //List<taxBreakdowns> taxlst = new List<taxBreakdowns>();

                //taxlst.Add(new taxBreakdowns()
                //{
                //    taxPercentage = 10,
                //    taxableAmount = (decimal)(trx.DocTotal - totalTax),
                //    taxAmount = (decimal)totalTax,
                //});
                //data.taxBreakdowns = taxlst;

                data.summarizeInfo = new summarizeInfo()
                {
                    sumOfTotalLineAmountWithoutTax = (decimal)(trx.DocTotal - totalTax),
                    totalAmountWithoutTax = (decimal)(trx.DocTotal - totalTax),
                    totalTaxAmount = (decimal)totalTax,
                    totalAmountWithTax = (decimal)(trx.DocTotal),
                    totalAmountWithTaxInWords = "",
                    discountAmount = (decimal)trx.TotalDiscount,
                    settlementDiscountAmount = 0,
                    taxPercentage = taxpercent,
                    isTotalAmountPos = isTotalAmountPos,
                    isTotalTaxAmountPos = isTotalTaxAmountPos,
                    isTotalAmtWithoutTaxPos = isTotalAmtWithoutTaxPos,
                    isDiscountAmtPos = isDiscountAmtPos

                };


                json = Newtonsoft.Json.JsonConvert.SerializeObject(data);


                using (var webClient = new System.Net.WebClient())
                {
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Setting.APIUser + ":" + Setting.APIPassword));
                    webClient.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + credentials;
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    webClient.Headers[System.Net.HttpRequestHeader.Accept] = "application/json";

                    responsebody = webClient.UploadString(createInvoiceUrl, "POST", json);
                    SAPUtils.SAPWriteLog("createInvoice", json, responsebody, "", createInvoiceUrl);


                    VietteleInvoiceResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<VietteleInvoiceResponse>(responsebody);

                    if (string.IsNullOrEmpty(response.errorCode))
                    {
                        //if successfull => add to history
                        SAPUtils.AddEInvoiceHistory(trx.DocEntry.ToString(), trx.DocNum.ToString(), trx.DocObjectCodeEx.ToString(),
                            response.result.invoiceNo, response.result.transactionID, transactionUuid,
                            Setting.templateCode, Setting.invoiceSeries);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                SAPUtils.SAPWriteLog("createInvoice", "", "", ex.ToString(), "");
                return new VietteleInvoiceResponse() { errorCode = ex.ToString() };
            }
        }

        public static VietteleInvoiceResponse createAdjInvoice_Dec(SAPbobsCOM.Documents trx)
        {
            try
            {
                string createInvoiceUrl = Setting.APIURL + "/InvoiceWS/createInvoice/" + Setting.supplierTaxCode;
                string responsebody = "";
                string json = string.Empty;
                string transactionUuid = System.Guid.NewGuid().ToString();
                double taxpercent = 0;

                VietteleInvoiceData data = new VietteleInvoiceData();

                SAPbobsCOM.BusinessPartners obp = (SAPbobsCOM.BusinessPartners)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                if (!obp.GetByKey(trx.CardCode))
                    return new VietteleInvoiceResponse() { errorCode = "Customer not found!" };


                //DateTime issuedate = trx.DocDate + trx.DocTime.TimeOfDay;


                string originalInvoiceId = trx.UserFields.Fields.Item("U_IEV_OrgInvNo").Value.ToString();
                DateTime originalInvoiceIssueDate = DateTime.Now;

                if (trx.UserFields.Fields.Item("U_IEV_OrgInvDate").Value.ToString() != "")
                    originalInvoiceIssueDate = DateTime.Parse(trx.UserFields.Fields.Item("U_IEV_OrgInvDate").Value.ToString());

                data.generalInvoiceInfo = new generalInvoiceInfo()
                {
                    invoiceType = Setting.invoiceType,
                    templateCode = Setting.templateCode,
                    invoiceSeries = Setting.invoiceSeries,
                    //invoiceIssuedDate = ((Int64)(issuedate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString(),
                    currencyCode = trx.DocCurrency,
                    adjustmentType = eDSC.adjustmentType.Ajustment,
                    adjustmentInvoiceType = eDSC.adjustmentInvoiceType.Money,
                    originalInvoiceId = originalInvoiceId,
                    originalInvoiceIssueDate =  ((Int64)(originalInvoiceIssueDate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString(),
                    additionalReferenceDate = ((Int64)(originalInvoiceIssueDate.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString(),
                    additionalReferenceDesc = trx.DocNum.ToString(),
                    paymentStatus = true,
                    cusGetInvoiceRight = true,
                    paymentType = "TM/CK",
                    paymentTypeName = "TM/CK",
                    transactionUuid = transactionUuid,
                    userName = Setting.APIUser,
                };


                data.buyerInfo = new buyerInfo()
                {
                    buyerName = "",
                    buyerBankAccount = "",
                    buyerBankName = "",
                    buyerCityName = "",
                    buyerDistrictName = "",
                    buyerEmail = obp.EmailAddress,
                    buyerPhoneNumber = obp.Cellular,
                    buyerTaxCode = obp.FederalTaxID,
                    buyerLegalName = trx.CardName,
                    buyerAddressLine = trx.Address,

                };
                List<itemInfo> lst = new List<itemInfo>();
                double totalTax = 0;
                for (int i = 0; i <= trx.Lines.Count - 1; i++)
                {
                    trx.Lines.SetCurrentLine(i);
                    lst.Add(new itemInfo()
                    {
                        lineNumber = i + 1,
                        itemName = trx.Lines.ItemDescription,
                        unitPrice = (decimal)trx.Lines.Price,
                        quantity = (decimal)trx.Lines.Quantity,
                        itemTotalAmountWithoutTax = (decimal)trx.Lines.LineTotal,
                        itemTotalAmountWithTax = (decimal)trx.Lines.GrossTotal,
                        itemTotalAmountAfterDiscount = (decimal)trx.Lines.TotalInclTax,
                        taxPercentage = (decimal)trx.Lines.TaxPercentagePerRow,
                        taxAmount = (decimal)trx.Lines.TaxTotal,
                        discount = (decimal)trx.Lines.DiscountPercent,
                        itemDiscount = (decimal)(trx.Lines.UnitPrice * trx.Lines.Quantity * trx.Lines.DiscountPercent / 100),
                        adjustmentTaxAmount = (decimal)trx.Lines.TaxTotal,
                        isIncreaseItem = "false"
                    });
                    totalTax += trx.Lines.TaxTotal;
                }
                data.itemInfo = lst;


                data.summarizeInfo = new summarizeInfo()
                {
                    sumOfTotalLineAmountWithoutTax = (decimal)(trx.DocTotal - totalTax),
                    totalAmountWithoutTax = (decimal)(trx.DocTotal - totalTax),
                    totalTaxAmount = (decimal)totalTax,
                    totalAmountWithTax = (decimal)(trx.DocTotal),
                    totalAmountWithTaxInWords = "",
                    discountAmount = (decimal)trx.TotalDiscount,
                    settlementDiscountAmount = 0,
                    taxPercentage = taxpercent,
                    isTotalAmountPos = "false",
                    isTotalTaxAmountPos = "false",
                    isTotalAmtWithoutTaxPos = "false",
                    isDiscountAmtPos = "false"

                };


                json = Newtonsoft.Json.JsonConvert.SerializeObject(data);


                using (var webClient = new System.Net.WebClient())
                {
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Setting.APIUser + ":" + Setting.APIPassword));
                    webClient.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + credentials;
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    webClient.Headers[System.Net.HttpRequestHeader.Accept] = "application/json";

                    responsebody = webClient.UploadString(createInvoiceUrl, "POST", json);
                    SAPUtils.SAPWriteLog("createAdjInvoice_Dec", json, responsebody, "", createInvoiceUrl);


                    VietteleInvoiceResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<VietteleInvoiceResponse>(responsebody);

                    if (string.IsNullOrEmpty(response.errorCode))
                    {
                        //if successfull => add to history
                        SAPUtils.AddEInvoiceHistory(trx.DocEntry.ToString(), trx.DocNum.ToString(), trx.DocObjectCodeEx.ToString(),
                            response.result.invoiceNo, response.result.transactionID, transactionUuid,
                            Setting.templateCode, Setting.invoiceSeries);
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                SAPUtils.SAPWriteLog("createAdjInvoice_Dec", "", "", ex.ToString(), "");
                return new VietteleInvoiceResponse() { errorCode = ex.ToString() };
            }
        }
    }
}
