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
using Newtonsoft.Json;
namespace eDSC
{
    class cl_createInvoiceDraftPreview
    {
        
        
        public static VietteleInvoiceDraftResponse createInvoiceDraftPreview(SAPbobsCOM.Documents trx)
        {
            try
            {
                Logger.LogEvent("Start DraftPreview Method", Logger.EventType.Event);

                string createInvoiceUrl = Setting.APIURL + "/InvoiceUtilsWS/createInvoiceDraftPreview/" + Setting.supplierTaxCode;
                string responsebody = "";
                string json = string.Empty;
                string transactionUuid = System.Guid.NewGuid().ToString();

                json = APIUtils.TransactionToViettelJson(trx, transactionUuid);
                if (json == "") return new VietteleInvoiceDraftResponse { errorCode = "There's no data in json! Please check log file." };

                Logger.LogEvent("createInvoiceDraftPreview URL: " + createInvoiceUrl + "POST" + json, Logger.EventType.Event);


                using (var webClient = new System.Net.WebClient())
                {
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Setting.APIUser + ":" + Setting.APIPassword));
                    webClient.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + credentials;
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    webClient.Headers[System.Net.HttpRequestHeader.Accept] = "application/json";

                    responsebody = webClient.UploadString(createInvoiceUrl, "POST", json);
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
                    else
                        Logger.LogEvent("file error: "+ response.errorCode +"--"+response.description, Logger.EventType.Event);

                    Logger.LogEvent("End DraftPreview Method", Logger.EventType.Event);

                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new VietteleInvoiceDraftResponse() { errorCode = ex.ToString() };
            }
        }

        

    }
}
