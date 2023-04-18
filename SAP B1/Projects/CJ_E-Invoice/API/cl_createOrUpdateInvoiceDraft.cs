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
    class cl_createOrUpdateInvoiceDraft
    {


        public static VietteleInvoiceResponse createOrUpdateInvoiceDraft(SAPbobsCOM.Documents trx)
        {
            try
            {
                Logger.LogEvent("Start Draft Method", Logger.EventType.Event);

                string createInvoiceUrl = Setting.APIURL + "/InvoiceWS/createOrUpdateInvoiceDraft/" + Setting.supplierTaxCode;
                string responsebody = "";
                string json = string.Empty;
                string transactionUuid = System.Guid.NewGuid().ToString();

                json = APIUtils.TransactionToViettelJson(trx, transactionUuid);

                Logger.LogEvent("createOrgInvoice URL: " + createInvoiceUrl + "POST" + json, Logger.EventType.Event);

                using (var webClient = new System.Net.WebClient())
                {
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Setting.APIUser + ":" + Setting.APIPassword));
                    webClient.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + credentials;
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    webClient.Headers[System.Net.HttpRequestHeader.Accept] = "application/json";
                   

                    responsebody = webClient.UploadString(createInvoiceUrl, "POST", json);
                    Logger.LogEvent("createOrUpdateInvoiceDraft ret: "+responsebody,Logger.EventType.Event);

                    VietteleInvoiceResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<VietteleInvoiceResponse>(responsebody);

                    Logger.LogEvent("End Draft Method", Logger.EventType.Event);
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new VietteleInvoiceResponse() { errorCode = ex.ToString() };
            }
        }

    }
}
