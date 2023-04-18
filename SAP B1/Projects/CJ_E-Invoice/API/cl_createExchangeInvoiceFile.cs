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
    class cl_createExchangeInvoiceFile
    {


        public static VietteleInvoiceDraftResponse createExchangeInvoiceFile(string invoiceNo, DateTime issuedate)
        {
            ///InvoiceAPI/InvoiceWS/createExchangeInvoiceFile
            try
            {
                if (invoiceNo == "")
                    return new VietteleInvoiceDraftResponse() { description = "Invoice No is blank!" };

                Logger.LogEvent("Start Exchange Method", Logger.EventType.Event);
                //string invoiceNo = "";
                //string transactionUuid = "";
                //ViettelEHistory his = SAPUtils.LoadeInvoiceHistory(trx);
                //if (his == null)
                //{
                //    return new VietteleInvoiceDraftResponse() { errorCode = "Invoice is not posted!" };
                //}
                //else
                //{
                //    invoiceNo = his.InvoiceNo;
                //    transactionUuid = his.TransactionId;
                //}

                
                string createInvoiceUrl = Setting.APIURL + "/InvoiceWS/createExchangeInvoiceFile";;

                //DateTime issuedate = 
                string autStr = CreateRequest.Base64Encode(Setting.APIUser + ":" + Setting.APIPassword);
                string contentType = "application/x-www-form-urlencoded";
                string request = "supplierTaxCode=" + HttpUtility.UrlEncode(Setting.supplierTaxCode) +
                                    "&invoiceNo=" + HttpUtility.UrlEncode(invoiceNo) +
                                    "&strIssueDate=" + HttpUtility.UrlEncode(issuedate.ToString("yyyyMMddhhmmss")) +
                                    //"&fileType=PDF" +
                                    "&exchangeUser=" + Setting.APIUser;

                Logger.LogEvent("createExchangeInvoiceFile json: "  + request, Logger.EventType.Event);

                string responsebody = CreateRequest.webRequest(createInvoiceUrl, request, autStr, "POST", contentType);

                Logger.LogEvent("createExchangeInvoiceFile ret: " + request, Logger.EventType.Event);

                VietteleInvoiceDraftResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<VietteleInvoiceDraftResponse>(responsebody);

                if (string.IsNullOrEmpty(response.errorCode))
                {
                    APIUtils.PDFFilename = response.fileName;
                    APIUtils.PDFFileToBytes = response.fileToBytes;

                    ThreadStart othr;
                    Thread myThread;
                    othr = new ThreadStart(APIUtils.showdialog);
                    myThread = new Thread(othr);
                    myThread.SetApartmentState(ApartmentState.STA);
                    myThread.Start();
                }
                Logger.LogEvent("End Exchange Method", Logger.EventType.Event);
                return response;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new VietteleInvoiceDraftResponse() { errorCode = ex.ToString() };
            }
        }

       
    }
}
