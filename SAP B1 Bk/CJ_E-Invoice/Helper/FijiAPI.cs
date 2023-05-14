using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDSC
{
    class FijiAPI
    {
        public static bool Attention()
        {
            try
            {
                string responsebody = "";
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        ATT = "Attention"
                    });
                    Logger.LogEvent("API REQUEST -  Attention: ", Logger.EventType.Event);
                    Logger.LogEvent(json, Logger.EventType.Event);
                    responsebody = webClient.UploadString(Setting.APIURL + "/api/Status/Attention", "POST", json);
                    Logger.LogEvent("API RESPONSE - Attention: ", Logger.EventType.Event);
                    Logger.LogEvent(responsebody, Logger.EventType.Event);
                    var responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responsebody);

                    if (responseObj.ATT_GSC.ToString() == "0000") // All OK
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }

        public static bool VerifyPin()
        {
            try
            {
                string responsebody = "";
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        VPIN = Setting.Pin
                    });
                    Logger.LogEvent("API REQUEST -  VerifyPin: ", Logger.EventType.Event);
                    Logger.LogEvent(json, Logger.EventType.Event);
                    responsebody = webClient.UploadString(Setting.APIURL + "/api/Status/VerifyPin", "POST", json);
                    Logger.LogEvent("API RESPONSE - VerifyPin: ", Logger.EventType.Event);
                    Logger.LogEvent(responsebody, Logger.EventType.Event);
                    var responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responsebody);
                    if (responseObj.VPIN_GSC.ToString() == "0100") // All OK
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return false;
        }
        
        public static InvoiceFiscalizationResponse SaleSignedInvoice(SAPbobsCOM.Documents trx, string hash, InvoiceType invoiceType, string invoiceNumber = "", bool isTraining = false)
        {
            try
            {
                string responsebody = "";
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    List<object> items = new List<object>();
                    for(int i =0;i<=trx.Lines.Count-1;i++)
                    {
                        trx.Lines.SetCurrentLine(i);
                        string tbTax = SAPUtils.GetTaxLabel(trx.Lines.TaxCode);
                        List<string> lables = new List<string>();
                        if (tbTax != "")
                        {
                            lables.Add(tbTax);
                            items.Add(new
                            {
                                Name = trx.Lines.ItemDescription,
                                Quantity = trx.Lines.Quantity,
                                UnitPrice = trx.Lines.Price,
                                Discount = trx.Lines.DiscountPercent * trx.Lines.Price * trx.Lines.Quantity,
                                TotalAmount = trx.Lines.Price * trx.Lines.Quantity,
                                Labels = lables
                            });
                        }
                        else {
                            items.Add(new
                            {
                                Name = trx.Lines.ItemDescription,
                                Quantity = trx.Lines.Quantity,
                                UnitPrice = trx.Lines.Price,
                                Discount = trx.Lines.DiscountPercent * trx.Lines.Price * trx.Lines.Quantity,
                                TotalAmount = trx.Lines.Price * trx.Lines.Quantity
                            });
                        }
                    }

                    string dateAndTimeOfIssue = trx.DocDate.Date.Add(trx.DocDate.TimeOfDay).ToString("o");
                    string json = string.Empty;
                   
                    if (invoiceType == InvoiceType.Copy)
                    {
                        if (isTraining)
                        {
                            invoiceType = InvoiceType.Training;
                        }

                        json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            DateAndTimeOfIssue = dateAndTimeOfIssue,
                            IT = invoiceType.ToString(),
                            TT = TransactionType.Sale.ToString(),
                            PaymentType = trx.UserFields.Fields.Item("U_PaymentType").Value.ToString(),
                            InvoiceNumber = trx.DocEntry.ToString(),
                            ReferentDocumentNumber = invoiceNumber,
                            ReferentDocumentDateAndTime = dateAndTimeOfIssue,
                            BD = trx.NumAtCard ,
                            Hash = hash,
                            Items = items
                        });
                    }
                    else
                    {
                        if (isTraining)
                        {
                            invoiceType = InvoiceType.Training;
                        }

                        json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            DateAndTimeOfIssue = dateAndTimeOfIssue,
                            IT = invoiceType.ToString(),
                            TT = TransactionType.Sale.ToString(),
                            PaymentType = trx.UserFields.Fields.Item("U_PaymentType").Value.ToString(),
                            InvoiceNumber = trx.DocEntry.ToString(),
                            BD = trx.NumAtCard,
                            Hash = hash,
                            Items = items
                        });
                    }
                    if (Attention())
                    {
                        Logger.LogEvent("API REQUEST -  SaleSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(json, Logger.EventType.Event);
                        responsebody = webClient.UploadString(Setting.APIURL + "/api/Sign/SignInvoice", "POST", json);
                        Logger.LogEvent("API RESPONSE - SaleSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(responsebody, Logger.EventType.Event);
                        InvoiceFiscalizationResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<InvoiceFiscalizationResponse>(responsebody);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }
        public static InvoiceFiscalizationResponse OrderSignedInvoice(SAPbobsCOM.Documents trx, string hash, InvoiceType invoiceType, string invoiceNumber = "", bool isTraining = false)
        {
            try
            {
                string responsebody = "";
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    List<object> items = new List<object>();


                    for (int i = 0; i <= trx.Lines.Count - 1; i++)
                    {
                        trx.Lines.SetCurrentLine(i);
                        string tbTax = SAPUtils.GetTaxLabel(trx.Lines.TaxCode);
                        List<string> lables = new List<string>();
                        if (tbTax != "")
                        {
                            lables.Add(tbTax);
                            items.Add(new
                            {
                                Name = trx.Lines.ItemDescription,
                                Quantity = trx.Lines.Quantity,
                                UnitPrice = trx.Lines.Price,
                                Discount = trx.Lines.DiscountPercent * trx.Lines.Price * trx.Lines.Quantity,
                                TotalAmount = trx.Lines.Price * trx.Lines.Quantity,
                                Labels = lables
                            });
                        }
                        else {
                            items.Add(new
                            {
                                Name = trx.Lines.ItemDescription,
                                Quantity = trx.Lines.Quantity,
                                UnitPrice = trx.Lines.Price,
                                Discount = trx.Lines.DiscountPercent * trx.Lines.Price * trx.Lines.Quantity,
                                TotalAmount = trx.Lines.Price * trx.Lines.Quantity
                            });
                        }
                    }

                    string dateAndTimeOfIssue = trx.DocDate.Date.Add(trx.DocDate.TimeOfDay).ToString("o");
                    string json = string.Empty;
                    if (invoiceType == InvoiceType.Copy)
                    {
                        InvoiceFiscalizationResponse response = GetSignedInvoice(hash);
                        return response;
                      
                    }
                    else
                    {
                        if (isTraining)
                        {
                            invoiceType = InvoiceType.Training;
                        }
                        json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            DateAndTimeOfIssue = dateAndTimeOfIssue,
                            IT = invoiceType.ToString(),
                            TT = TransactionType.Sale.ToString(),
                            PaymentType = trx.UserFields.Fields.Item("U_PaymentType").Value.ToString(),
                            InvoiceNumber = trx.DocEntry.ToString(),
                            Hash = hash,
                            BD = trx.NumAtCard,
                            Items = items
                        });
                    }
                    if (Attention())
                    {
                        Logger.LogEvent("API REQUEST -  OrderSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(json, Logger.EventType.Event);
                        responsebody = webClient.UploadString(Setting.APIURL + "/api/Sign/SignInvoice", "POST", json);
                        Logger.LogEvent("API RESPONSE - OrderSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(responsebody, Logger.EventType.Event);
                        InvoiceFiscalizationResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<InvoiceFiscalizationResponse>(responsebody);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }
        public static InvoiceFiscalizationResponse RefundSignedInvoice(SAPbobsCOM.Documents trx, string hash, InvoiceType invoiceType, string invoiceNumber = "", bool isTraining = false)
        {
            try
            {
                string responsebody = "";
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    List<object> items = new List<object>();
                    long originalDocumentKey = 0;
                    
                    for (int i = 0; i <= trx.Lines.Count - 1; i++)
                    {
                        trx.Lines.SetCurrentLine(i);
                        originalDocumentKey = 0;
                        string tbTax = SAPUtils.GetTaxLabel(trx.Lines.TaxCode);
                        List<string> lables = new List<string>();
                        if (tbTax != "")
                        {
                            lables.Add(tbTax);
                            items.Add(new
                            {
                                Name = trx.Lines.ItemDescription,
                                Quantity = trx.Lines.Quantity,
                                UnitPrice = trx.Lines.Price,
                                Discount = trx.Lines.DiscountPercent * trx.Lines.Price * trx.Lines.Quantity,
                                TotalAmount = trx.Lines.Price * trx.Lines.Quantity,
                                Labels = lables
                            });
                        }
                        else {
                            items.Add(new
                            {
                                Name = trx.Lines.ItemDescription,
                                Quantity = trx.Lines.Quantity,
                                UnitPrice = trx.Lines.Price,
                                Discount = trx.Lines.DiscountPercent * trx.Lines.Price * trx.Lines.Quantity,
                                TotalAmount = trx.Lines.Price * trx.Lines.Quantity
                            });
                        }
                    }


                    string json = string.Empty;
                    string dateAndTimeOfIssue = trx.DocDate.Date.Add(trx.DocDate.TimeOfDay).ToString("o");

                    if (invoiceType == InvoiceType.Copy)
                    {
                        if (isTraining)
                        {
                            invoiceType = InvoiceType.Training;
                        }
                        json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            DateAndTimeOfIssue = dateAndTimeOfIssue,
                            IT = invoiceType.ToString(),
                            TT = TransactionType.Refund.ToString(),
                            PaymentType = trx.UserFields.Fields.Item("U_PaymentType").Value.ToString(),
                            InvoiceNumber = trx.DocDate.ToString(),
                            ReferentDocumentNumber = invoiceNumber,
                            ReferentDocumentDateAndTime = dateAndTimeOfIssue,
                            BD = trx.NumAtCard,
                            Hash = hash,
                            Items = items
                        });
                    }
                    else
                    {
                        if (isTraining)
                        {
                            invoiceType = InvoiceType.Training;
                        }
                        DataTable tb = SAPUtils.GetOriginalTransaction(originalDocumentKey);
                        if (tb != null && tb.Rows.Count > 0)
                        {
                            invoiceNumber = tb.Rows[0]["U_InvoiceNumber"].ToString();
                            DateTime businessDate = DateTime.Parse(tb.Rows[0]["BusinessDate"].ToString());
                            DateTime actualDate = DateTime.Parse(tb.Rows[0]["ActualDate"].ToString());
                            dateAndTimeOfIssue = businessDate.Date.Add(actualDate.TimeOfDay).ToString("o");
                        }

                        json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            DateAndTimeOfIssue = dateAndTimeOfIssue,
                            IT = invoiceType.ToString(),
                            TT = TransactionType.Refund.ToString(),
                            PaymentType = trx.UserFields.Fields.Item("U_PaymentType").Value.ToString(),
                            InvoiceNumber = trx.DocEntry.ToString(),
                            ReferentDocumentNumber = invoiceNumber,
                            ReferentDocumentDateAndTime = dateAndTimeOfIssue,
                            BD = trx.NumAtCard,
                            Hash = hash,
                            Items = items
                        });
                    }
                    if (Attention())
                    {
                        Logger.LogEvent("API REQUEST -  RefundSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(json, Logger.EventType.Event);
                        responsebody = webClient.UploadString(Setting.APIURL + "/api/Sign/SignInvoice", "POST", json);
                        Logger.LogEvent("API RESPONSE -  RefundSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(responsebody, Logger.EventType.Event);
                        InvoiceFiscalizationResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<InvoiceFiscalizationResponse>(responsebody);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }
        public static InvoiceFiscalizationResponse OrderRefundSignedInvoice(SAPbobsCOM.Documents trx, string hash, InvoiceType invoiceType, string invoiceNumber = "", bool isTraining = false)
        {
            try
            {
                string responsebody = "";
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    List<object> items = new List<object>();
                    long originalDocumentKey = 0;
                    
                    for (int i = 0; i <= trx.Lines.Count - 1; i++)
                    {
                        trx.Lines.SetCurrentLine(i);

                        string tbTax = SAPUtils.GetTaxLabel(trx.Lines.TaxCode);
                        List<string> lables = new List<string>();
                        if (tbTax != "")
                        {
                            lables.Add(tbTax);
                            items.Add(new
                            {
                                Name = trx.Lines.ItemDescription,
                                Quantity = trx.Lines.Quantity,
                                UnitPrice = trx.Lines.Price,
                                Discount = trx.Lines.DiscountPercent * trx.Lines.Price * trx.Lines.Quantity,
                                TotalAmount = trx.Lines.Price * trx.Lines.Quantity,
                                Labels = lables
                            });
                        }
                        else {
                            items.Add(new
                            {
                                Name = trx.Lines.ItemDescription,
                                Quantity = trx.Lines.Quantity,
                                UnitPrice = trx.Lines.Price,
                                Discount = trx.Lines.DiscountPercent * trx.Lines.Price * trx.Lines.Quantity,
                                TotalAmount = trx.Lines.Price * trx.Lines.Quantity
                            });
                        }
                    }


                    string json = string.Empty;
                    string dateAndTimeOfIssue = trx.DocDate.Date.Add(trx.DocDate.TimeOfDay).ToString("o");

                    if (invoiceType == InvoiceType.Copy)
                    {
                        json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            DateAndTimeOfIssue = dateAndTimeOfIssue,
                            IT = invoiceType.ToString(),
                            TT = TransactionType.Refund.ToString(),
                            PaymentType = trx.UserFields.Fields.Item("U_PaymentType").Value.ToString(),
                            InvoiceNumber = trx.DocEntry.ToString(),
                            ReferentDocumentNumber = invoiceNumber,
                            ReferentDocumentDateAndTime = dateAndTimeOfIssue,
                            BD = trx.NumAtCard,
                            Hash = hash,
                            Items = items
                        });
                    }
                    else
                    {
                        DataTable tb = SAPUtils.GetOriginalTransaction(originalDocumentKey);
                        if (tb != null && tb.Rows.Count > 0)
                        {
                            invoiceNumber = tb.Rows[0]["U_InvoiceNumber"].ToString();
                            DateTime businessDate = DateTime.Parse(tb.Rows[0]["BusinessDate"].ToString());
                            DateTime actualDate = DateTime.Parse(tb.Rows[0]["ActualDate"].ToString());
                            dateAndTimeOfIssue = businessDate.Date.Add(actualDate.TimeOfDay).ToString("o");
                        }

                        json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            DateAndTimeOfIssue = dateAndTimeOfIssue,
                            IT = invoiceType.ToString(),
                            TT = TransactionType.Refund.ToString(),
                            PaymentType = trx.UserFields.Fields.Item("U_PaymentType").Value.ToString(),
                            InvoiceNumber = trx.DocEntry.ToString(),
                            ReferentDocumentNumber = invoiceNumber,
                            ReferentDocumentDateAndTime = dateAndTimeOfIssue,
                            BD = trx.NumAtCard,
                            Hash = hash,
                            Items = items
                        });
                    }
                    if (Attention())
                    {
                        Logger.LogEvent("API REQUEST -  OrderRefundSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(json, Logger.EventType.Event);
                        responsebody = webClient.UploadString(Setting.APIURL + "/api/Sign/SignInvoice", "POST", json);
                        Logger.LogEvent("API RESPONSE -  OrderRefundSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(responsebody, Logger.EventType.Event);
                        InvoiceFiscalizationResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<InvoiceFiscalizationResponse>(responsebody);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }
        public static List<InvoiceFiscalizationResponse> PostTransactionToFiji(SAPbobsCOM.Documents trx)
        {
            List<InvoiceFiscalizationResponse> responses = new List<InvoiceFiscalizationResponse>();

            try
            {
                string hash = string.Empty;
                InvoiceFiscalizationResponse response = null;
                bool isTraining = SAPUtils.IsTranning();

                //Sales
                if (trx.DocObjectCode==SAPbobsCOM.BoObjectTypes.oInvoices)
                {
                    hash = Utils.GenerateHashString();
                    response = FijiAPI.SaleSignedInvoice(trx, hash, InvoiceType.Normal, "", isTraining);
                    if (response != null)
                    {
                        responses.Add(response);
                    }
                    else {
                        response = FijiAPI.GetSignedInvoice(hash);
                        if (response != null)
                        {
                            responses.Add(response);
                        }
                    }
                }

                //Refund
                if (trx.DocObjectCode == SAPbobsCOM.BoObjectTypes.oCreditNotes)
                {
                    hash = Utils.GenerateHashString();
                    response = FijiAPI.RefundSignedInvoice(trx, hash, InvoiceType.Normal,"", isTraining);
                    if (response != null)
                    {
                        responses.Add(response);
                    }
                    else {
                        response = FijiAPI.GetSignedInvoice(hash);
                        if (response != null)
                        {
                            responses.Add(response);
                        }
                    }
                }
                //Quotation
                if (trx.DocObjectCode == SAPbobsCOM.BoObjectTypes.oQuotations)
                {
                    bool isRefund = false;

                    if (!isRefund)
                    {
                        hash = Utils.GenerateHashString();
                        response = FijiAPI.OrderSignedInvoice(trx, hash, InvoiceType.ProForma, "", isTraining);
                        if (response != null)
                        {
                            responses.Add(response);
                        }
                        else {
                            response = FijiAPI.GetSignedInvoice(hash);
                            if (response != null)
                            {
                                responses.Add(response);
                            }
                        }
                    }
                    else
                    {
                        hash = Utils.GenerateHashString();
                        response = FijiAPI.OrderRefundSignedInvoice(trx, hash, InvoiceType.ProForma, "", isTraining);
                        if (response != null)
                        {
                            responses.Add(response);
                        }
                        else {
                            response = FijiAPI.GetSignedInvoice(hash);
                            if (response != null)
                            {
                                responses.Add(response);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return responses;
        }

        public static List<InvoiceFiscalizationResponse> CopyTransactionFromFiji(SAPbobsCOM.Documents trx, string invoiceNumber, string hash = "")
        {
            List<InvoiceFiscalizationResponse> responses = new List<InvoiceFiscalizationResponse>();
            try
            {
                InvoiceFiscalizationResponse response = null;

                //Sales
                if (trx.DocObjectCode == SAPbobsCOM.BoObjectTypes.oInvoices)
                {
                    hash = Utils.GenerateHashString();
                    response = FijiAPI.SaleSignedInvoice(trx, hash, InvoiceType.Copy, invoiceNumber);
                    if (response != null)
                    {
                        responses.Add(response);
                    }
                }

                //Refund
                if (trx.DocObjectCode==SAPbobsCOM.BoObjectTypes.oCreditNotes)
                {
                    hash = Utils.GenerateHashString();
                    response = FijiAPI.RefundSignedInvoice(trx, hash, InvoiceType.Copy, invoiceNumber);
                    if (response != null)
                    {
                        responses.Add(response);
                    }
                }
                //Quotation
                if (trx.DocObjectCode == SAPbobsCOM.BoObjectTypes.oQuotations)
                {
                    response = FijiAPI.OrderSignedInvoice(trx, hash, InvoiceType.Copy, invoiceNumber);
                    if (response != null)
                    {
                        responses.Add(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return responses;
        }

        public static InvoiceFiscalizationResponse GetSignedInvoice(string hash)
        {
            try
            {
                string responsebody = "";
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        GI = hash
                    });
                    if (Attention())
                    {
                        Logger.LogEvent("API REQUEST -  GetSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(json, Logger.EventType.Event);
                        responsebody = webClient.UploadString(Setting.APIURL + "/api/Sign/GetSignedInvoice", "POST", json);
                        Logger.LogEvent("API RESPONSE -  GetSignedInvoice: ", Logger.EventType.Event);
                        Logger.LogEvent(responsebody, Logger.EventType.Event);
                        InvoiceFiscalizationResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<InvoiceFiscalizationResponse>(responsebody);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        
    }
}
