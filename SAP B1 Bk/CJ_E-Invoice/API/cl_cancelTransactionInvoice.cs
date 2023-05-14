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
using SAPbobsCOM;

namespace eDSC
{
    class cl_cancelTransactionInvoice
    {
        public static string cancelTransactionInvoice(SAPbobsCOM.Documents trx)
        {
            string request = "";
            string createInvoiceUrl = Setting.APIURL + "/InvoiceWS/cancelTransactionInvoice";// +Setting.supplierTaxCode;
            try
            {
                Logger.LogEvent("Start Cancel Method", Logger.EventType.Event);
                string invoiceNo = "";
                string transactionUuid = "";
                SAPbobsCOM.Documents orgtrx=null;
                if (trx.DocObjectCodeEx == "13")
                    orgtrx = (SAPbobsCOM.Documents)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                else if (trx.DocObjectCodeEx == "14")
                    orgtrx = (SAPbobsCOM.Documents)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);

                if (!orgtrx.GetByKey(trx.Lines.BaseEntry))
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        errorCode = "-1",
                        description = "Base document is not found!"
                    });
                }

                //ViettelEHistory his = SAPUtils.LoadeInvoiceHistory(orgtrx);
                //if (his == null)
                //{
                //    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                //    {
                //        errorCode = "-1",
                //        description = "Invoice is not posted!"
                //    });
                //}
                //else
                //{
                //    invoiceNo = his.InvoiceNo;
                //    transactionUuid = his.uuId;
                //}
                

                

                DateTime issuedate;
                try
                {
                    issuedate = orgtrx.DocDate + DateTime.Parse(orgtrx.UserFields.Fields.Item("U_IEV_DocTime").Value.ToString()).TimeOfDay;
                }
                catch (Exception ex)
                {
                    issuedate = orgtrx.DocDate + orgtrx.DocTime.TimeOfDay;
                    Logger.LogException(ex);
                }

                DateTime refdate = issuedate;// trx.DocDate + trx.DocTime.TimeOfDay;
                invoiceNo = orgtrx.UserFields.Fields.Item("U_IEV_InvCode").Value.ToString();

                string autStr = CreateRequest.Base64Encode(Setting.APIUser+":"+Setting.APIPassword);
                string contentType = "application/x-www-form-urlencoded";
                request = "supplierTaxCode=" + HttpUtility.UrlEncode(Setting.supplierTaxCode) +
                                    "&invoiceNo=" + HttpUtility.UrlEncode(invoiceNo) +
                                    "&strIssueDate=" + HttpUtility.UrlEncode(issuedate.ToString("yyyyMMddhhmmss")) +
                                    "&additionalReferenceDesc=" + HttpUtility.UrlEncode(trx.DocNum.ToString()) +
                                    "&additionalReferenceDate=" + HttpUtility.UrlEncode(refdate.ToString("yyyyMMddhhmmss"));

                Logger.LogEvent("cancelTransactionInvoice json: " + request, Logger.EventType.Event);

                string result = CreateRequest.webRequest(createInvoiceUrl, request, autStr, "POST", contentType);
                
                Logger.LogEvent("cancelTransactionInvoice ret: " + request, Logger.EventType.Event);

                var responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);

                Logger.LogEvent("End Cancel Method", Logger.EventType.Event);

                if (string.IsNullOrEmpty(responseObj.errorCode.ToString()))
                {
                    //if successfull => add to history
                    //SAPUtils.AddEInvoiceHistory(trx.DocEntry.ToString(), trx.DocNum.ToString(), trx.DocObjectCodeEx.ToString(),
                    //    his.InvoiceNo, his.TransactionId, transactionUuid,
                    //    Setting.templateCode, Setting.invoiceSeries);
                    return "";
                }
                else
                    return responseObj.errorCode;
              
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    errorCode = ex.ToString(),
                    description = ex.ToString()
                });
            }
        }

       

        
    }
}
