using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDSC
{
    class VietteleInvoiceDraftResponse
    {
        public string errorCode { get; set; }
        public string description { get; set; }
        public string fileName { get; set; }
        public string fileToBytes { get; set; }
    }

    class VietteleInvoiceResponse
    {
        public string errorCode { get; set; }
        public string description { get; set; }
        public string uuId { get; set; }
        public VietteleInvoiceResponseResult result { get; set; }
    }
    class VietteleInvoiceResponseResult
    {
        public string supplierTaxCode { get; set; }
        public string invoiceNo { get; set; }
        public string transactionID { get; set; }
        public string reservationCode { get; set; }
    }
}
