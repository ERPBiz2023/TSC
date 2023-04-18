using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDSC
{
    class ViettelCommonDataInput
    {
        public string supplierTaxCode { get; set; }
        public string invoiceNo { get; set; }
        public string templateCode { get; set; }
        public string transactionUuid { get; set; }
        public string fileType { get; set; }

    }

    class ViettelEHistory
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string DocNum { get; set; }
        public string DocEntry { get; set; }
        public string DocType { get; set; }
        public string InvoiceNo { get; set; }
        public string uuId { get; set; }
        public string TransactionId { get; set; }
        public string templateCode { get; set; }
        public string Serial { get; set; }

    }
    class VietteleInvoiceData
    {
        public generalInvoiceInfo generalInvoiceInfo { get; set; }
        public buyerInfo buyerInfo { get; set; }
        public sellerInfo sellerInfo { get; set; }
        //public extAttribute extAttribute { get; set; }
        //public payments payments { get; set; }
        //public deliveryInfo deliveryInfo { get; set; }
        public List< itemInfo> itemInfo { get; set; }
        //public metadata metadata { get; set; }
       // public meterReading meterReading { get; set; }
        public summarizeInfo summarizeInfo { get; set; }
        public List<taxBreakdowns> taxBreakdowns { set; get; }
        public List<payments> payments { set; get; }
    }
    class generalInvoiceInfo
    {
        public string invoiceType { get; set; }
        public string templateCode { get; set; }
        public string invoiceSeries { get; set; }
        public string invoiceIssuedDate { get; set; }
        public string currencyCode { get; set; }
        public adjustmentType adjustmentType { get; set; } 
        public adjustmentInvoiceType adjustmentInvoiceType { get; set; }
        public string invoiceNo { get; set; }
        public string originalInvoiceId { get; set; }
        public string originalInvoiceIssueDate { get; set; }
        public string additionalReferenceDesc { get; set; }
        public string additionalReferenceDate { get; set; }
         

        public bool paymentStatus { get; set; }
        public bool cusGetInvoiceRight { get; set; }
        public string userName { get; set; }
        public string paymentTypeName { get; set; }
        public string paymentType { get; set; }
        public string transactionUuid { get; set; }
        

        
    }
    class sellerInfo
    { 
        public string sellerLegalName { get; set; }
        public string sellerAddressLine { get; set; }
        public string sellerBankName { get; set; }
        public string sellerPhoneNumber { get; set; }
    }
    class buyerInfo
    {
        public string buyerName { get; set; }
        public string buyerLegalName { get; set; }
        public string buyerTaxCode { get; set; }
        public string buyerAddressLine { get; set; }
        public string buyerDistrictName { get; set; }
        public string buyerCityName { get; set; }
        public string buyerPhoneNumber { get; set; }
        public string buyerEmail { get; set; }
        public string buyerBankName { get; set; }
        public string buyerBankAccount { get; set; }
        public int buyerNotGetInvoice { get; set; }

        public string buyerIdType { get; set; }
        public string buyerIdNo { get; set; }
        public string buyerCode { get; set; }

    }
    class extAttribute
    { }
    class payments
    {
        public string paymentMethodName { get; set; }
    
    }
    class deliveryInfo
    { }
    class itemInfo
    {
       
        public int lineNumber { get; set; }
        public int selection { get; set; }
        public string itemName { get; set; }
        public string unitName { get; set; }
        public decimal unitPrice { get; set; }
        public decimal quantity { get; set; }
        public decimal itemTotalAmountWithoutTax { get; set; }
        public decimal itemTotalAmountWithTax { get; set; }
        public decimal itemTotalAmountAfterDiscount { get; set; }
        public string taxPercentage { get; set; }
        public decimal taxAmount { get; set; }
        public decimal discount { get; set; }
        public decimal itemDiscount { get; set; }
        public decimal adjustmentTaxAmount { get; set; }
        public bool isIncreaseItem { get; set; }
        
    }
    class metadata
    { }
    class meterReading
    { }
    class summarizeInfo
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal  sumOfTotalLineAmountWithoutTax { get; set; }
        public decimal   totalAmountWithoutTax { get; set; }
        public decimal totalTaxAmount { get; set; }
        public decimal totalAmountWithTax { get; set; }
        public string totalAmountWithTaxInWords { get; set; }
        public decimal discountAmount { get; set; }
        public decimal settlementDiscountAmount { get; set; }
        public double taxPercentage  { get; set; }
        public bool isTotalAmountPos { get; set; }
        public bool isTotalTaxAmountPos { get; set; }

        public bool isTotalAmtWithoutTaxPos { get; set; }
        public bool isDiscountAmtPos { get; set; }
        
        
    }
    class taxBreakdowns
    {
       public decimal taxPercentage { get; set; }
       public decimal taxableAmount{ get; set; }
      public decimal taxAmount { get; set; }
    }

    
    enum adjustmentType
    { 
        Original=1,
        Replace=3,
        Ajustment=5
    }

    enum adjustmentInvoiceType
    {
        Money = 1,
        Infor = 2,
    }
    public class responseGetAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
        public string iat { get; set; }
        public string invoice_cluster { get; set; }
        public string type { get; set; }
        public string jti { get; set; }

    }
}
