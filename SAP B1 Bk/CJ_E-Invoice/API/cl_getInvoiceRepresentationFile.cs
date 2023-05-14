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
    class cl_getInvoiceRepresentationFile
    {
        public static VietteleInvoiceDraftResponse getInvoiceRepresentationFile(string invoiceNo)
        {
            try
            {
                if (invoiceNo=="")
                    return new VietteleInvoiceDraftResponse() { description = "Invoice No is blank!" };

                Logger.LogEvent("Start PDF Method", Logger.EventType.Event);

                //string invoiceNo="";
                //string transactionUuid = "";
                
                //ViettelEHistory his = SAPUtils.LoadeInvoiceHistory(trx);
                //if (his == null)
                //{
                //    return new VietteleInvoiceDraftResponse() { description = "Invoice is not posted!" };
                //}
                //else
                //{
                //    invoiceNo = his.InvoiceNo;
                //    transactionUuid = his.uuId;
                //}


                string createInvoiceUrl = Setting.APIURL + "/InvoiceUtilsWS/getInvoiceRepresentationFile";// +Setting.supplierTaxCode;
                string responsebody = "";
                string json = string.Empty;
                ViettelCommonDataInput data = new ViettelCommonDataInput() 
                { 
                    supplierTaxCode=Setting.supplierTaxCode,
                    invoiceNo= invoiceNo,
                    templateCode=Setting.templateCode,
                    //transactionUuid = transactionUuid,
                    fileType="PDF"
                };
                json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                Logger.LogEvent("getInvoiceRepresentationFile json: " + json, Logger.EventType.Event);


                using (var webClient = new System.Net.WebClient())
                {
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Setting.APIUser + ":" + Setting.APIPassword));
                    webClient.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + credentials;
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    webClient.Headers[System.Net.HttpRequestHeader.Accept] = "application/json";

                    responsebody = webClient.UploadString(createInvoiceUrl, "POST", json);
                    Logger.LogEvent("getInvoiceRepresentationFile ret: " + responsebody, Logger.EventType.Event);

                    VietteleInvoiceDraftResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<VietteleInvoiceDraftResponse>(responsebody);

                    if (string.IsNullOrEmpty(response.description))
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
                    Logger.LogEvent("End PDF Method", Logger.EventType.Event);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new VietteleInvoiceDraftResponse() { errorCode=ex.ToString()};
            }
        }


       
    }
}
