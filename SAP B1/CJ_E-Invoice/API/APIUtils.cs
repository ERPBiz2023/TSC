using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eDSC
{
    class APIUtils
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();



        public static string PDFFilename = "";
        public static string PDFFileToBytes = "";
        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
        }


        public static void showdialog()
        {

            using (SaveFileDialog exportSaveFileDialog = new SaveFileDialog())
            {
                IntPtr ptr = GetForegroundWindow();
                WindowWrapper oWindow = new WindowWrapper(ptr);
                exportSaveFileDialog.Title = "Select PDF File";
                exportSaveFileDialog.Filter = "PDF File(*.pdf)|*.pdf";
                exportSaveFileDialog.FileName = PDFFilename;

                if (DialogResult.OK == exportSaveFileDialog.ShowDialog(oWindow))
                {
                    PDFFilename = exportSaveFileDialog.FileName;
                    ByteArrayToFile(PDFFilename, Convert.FromBase64String(PDFFileToBytes));
                }
                else
                {
                    PDFFilename = "";
                }
            }
        }
        public static void showdialog2()
        {
            //Logger.LogEvent("open file dialog", Logger.EventType.Event);

            using (SaveFileDialog exportSaveFileDialog = new SaveFileDialog())
            {

                System.Windows.Forms.Form form = new System.Windows.Forms.Form();
                form.TopMost = true;
                exportSaveFileDialog.Title = "Select PDF File";
                exportSaveFileDialog.Filter = "PDF File(*.pdf)|*.pdf";
                //exportSaveFileDialog.FileName = System.Windows.Forms.Application.StartupPath + "\\" + PDFFilename + ".pdf";
                exportSaveFileDialog.FileName = PDFFilename;
                if (DialogResult.OK == exportSaveFileDialog.ShowDialog(form))
                {
                    PDFFilename = exportSaveFileDialog.FileName;
                    ByteArrayToFile(PDFFilename, Convert.FromBase64String(PDFFileToBytes));
                }
                else
                {
                    PDFFilename = "";
                    PDFFileToBytes = "";
                }
            }
        }

        public static string TestAPIConnection()
        {
            try
            {
                string createInvoiceUrl = Setting.APIURL + "/InvoiceUtilsWS/convertFont";
                var data = new {font="VNI",data="TEST" };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                using (var webClient = new System.Net.WebClient())
                {
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Setting.APIUser + ":" + Setting.APIPassword));
                    webClient.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + credentials;
                    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                    webClient.Headers[System.Net.HttpRequestHeader.Accept] = "application/json";
                    webClient.Encoding = Encoding.UTF8;

                    string response = webClient.UploadString(createInvoiceUrl, "POST", json);
                    dynamic ret = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                    if (!string.IsNullOrEmpty(ret["errorCode"].ToString()))
                    {
                        return ret["result"].ToString();
                    }
                    else
                        return ret["errorCode"].ToString();
                }


            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static string TransactionToViettelJson(SAPbobsCOM.Documents trx, string transactionUuid)
        {
            try
            {
                string json = "";
                VietteleInvoiceData data = new VietteleInvoiceData();

                SAPbobsCOM.BusinessPartners obp = (SAPbobsCOM.BusinessPartners)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                obp.GetByKey(trx.CardCode);

                DateTime doctime = DateTime.Now;
                DateTime docdate = trx.DocDate;
                //string str = trx.UserFields.Fields.Item("U_IEV_DocTime").Value.ToString();
                //DateTime.TryParse(str, out doctime);

                DateTime dateTime = new DateTime(trx.TaxDate.Year, trx.TaxDate.Month, trx.TaxDate.Day, doctime.Hour, doctime.Minute, 0);
                long unixDateTime = (Int64)((dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds);
                
                data.generalInvoiceInfo = new generalInvoiceInfo()
                {
                    invoiceType = Setting.invoiceType,
                    templateCode = Setting.templateCode,
                    invoiceSeries = Setting.invoiceSeries,
                    invoiceIssuedDate = unixDateTime.ToString(),
                    currencyCode = "VND", // trx.DocCurrency,
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
                    buyerCode=obp.CardCode,
                    buyerBankAccount = "",
                    buyerBankName = "",
                    buyerCityName = "",
                    buyerDistrictName = "",
                    buyerEmail = obp.EmailAddress,
                    buyerPhoneNumber = obp.Cellular,
                    buyerTaxCode = obp.FederalTaxID,
                    buyerLegalName = obp.CardName,
                    buyerAddressLine = obp.UserFields.Fields.Item("U_Address").Value.ToString()//trx.Address,

                };
                if (obp.CardForeignName == "")
                {
                    data.buyerInfo.buyerLegalName = trx.CardName;
                }
                if (obp.FederalTaxID == "")
                { 
                    //khach le - nguoi mua ko lay hoa don
                    data.buyerInfo.buyerNotGetInvoice = 1;
                    data.buyerInfo.buyerLegalName = "";
                    data.buyerInfo.buyerName = "Người mua không lấy hoá đơn";
                }

                //if (trx.FederalTaxID != "")
                //{
                //    data.buyerInfo.buyerIdType = "1";
                //    data.buyerInfo.buyerIdNo = trx.FederalTaxID;
                //}

                List<itemInfo> lst = new List<itemInfo>();
                double totalTax = 0;
                for (int i = 0; i <= trx.Lines.Count - 1; i++)
                {
                    string taxper;
                    string taxGroup;
                    trx.Lines.SetCurrentLine(i);
                    taxGroup = trx.Lines.VatGroup;
                    if (trx.Lines.VatGroup =="SVN")
                    {
                        taxper = "-2";
                    }
                    else
                    {
                        taxper = (string)trx.Lines.TaxPercentagePerRow.ToString();
                    }

                    itemInfo iteminfo = new itemInfo()
                    {
                        lineNumber = i + 1,
                        selection=1,
                        itemName = trx.Lines.UserFields.Fields.Item("U_Description_VN").Value.ToString(),
                        unitPrice = (decimal)trx.Lines.UnitPrice,
                        quantity = (decimal)trx.Lines.Quantity,
                        unitName = trx.Lines.MeasureUnit,
                        itemTotalAmountWithoutTax = (decimal)trx.Lines.LineTotal,
                        itemTotalAmountWithTax = (decimal)trx.Lines.GrossTotal,
                        itemTotalAmountAfterDiscount = (decimal)trx.Lines.TotalInclTax,
                        taxPercentage = taxper,
                        taxAmount = (decimal)trx.Lines.TaxTotal,
                        discount = (decimal)trx.Lines.DiscountPercent,
                        itemDiscount = (decimal)(trx.Lines.UnitPrice * trx.Lines.Quantity * trx.Lines.DiscountPercent / 100),
                        adjustmentTaxAmount = 1
                        
                    };
                    if (iteminfo.quantity == 0)
                        iteminfo.quantity = 1;

                    lst.Add(iteminfo);
                    totalTax += trx.Lines.TaxTotal;
                }

                data.generalInvoiceInfo.adjustmentType = eDSC.adjustmentType.Original;





                int taxpercent = SAPUtils.GetTaxPercent(trx);
                data.summarizeInfo = new summarizeInfo()
                {
                    sumOfTotalLineAmountWithoutTax = (decimal)(trx.DocTotal - totalTax),
                    totalAmountWithoutTax = (decimal)(trx.DocTotal - totalTax),
                    totalTaxAmount = (decimal)totalTax,
                    totalAmountWithTax = (decimal)(trx.DocTotal),
                    totalAmountWithTaxInWords = "",
                    discountAmount = (decimal)trx.TotalDiscount,
                    settlementDiscountAmount = 0,
                    taxPercentage=taxpercent
                };


                List<payments> lstpayment =new List<payments>();
                lstpayment.Add(new payments()
                    {
                     paymentMethodName="TM/CK"
                    });
                data.payments=lstpayment;

                List<taxBreakdowns> lsttax = new List<taxBreakdowns>();
                lsttax.Add(new taxBreakdowns() {
                    taxPercentage = taxpercent,
                    taxableAmount = (decimal)(trx.DocTotal - totalTax),
                    taxAmount = (decimal)totalTax

                });
                data.taxBreakdowns = lsttax;

                string originalInvoiceId = trx.UserFields.Fields.Item("U_OrgInvNo").Value.ToString();
                string originalInvoiceIssueDate = trx.UserFields.Fields.Item("U_OrgInvDate").Value.ToString();
                string IsReplace = trx.UserFields.Fields.Item("U_IsReplace").Value.ToString();

                if (originalInvoiceId != "")
                {
                    data.generalInvoiceInfo.originalInvoiceId = originalInvoiceId;
                    data.generalInvoiceInfo.originalInvoiceIssueDate = ((Int64)(DateTime.Parse(originalInvoiceIssueDate).ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString(); // ((Int64)(DateTime.Parse(originalInvoiceIssueDate).ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString();
                    data.generalInvoiceInfo.additionalReferenceDate = ((Int64)(DateTime.Parse(originalInvoiceIssueDate).ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString();
                    data.generalInvoiceInfo.additionalReferenceDesc = trx.DocNum.ToString();
                    string originalInvoiceIssueDateOnly = DateTime.Parse(originalInvoiceIssueDate).ToString("dd/MM/yyyy");

                    if (IsReplace == "Y") //HOA DON THAY THE
                    {
                        data.generalInvoiceInfo.adjustmentType = eDSC.adjustmentType.Replace;
                    }
                    else
                    {
                        if (trx.DocObjectCodeEx == "14") //HOA DON DIEU CHINH GIAM
                        {
                            data.generalInvoiceInfo.adjustmentType = eDSC.adjustmentType.Ajustment;
                            data.generalInvoiceInfo.adjustmentInvoiceType = eDSC.adjustmentInvoiceType.Money;
                            data.summarizeInfo.isTotalAmountPos = true;
                            data.summarizeInfo.isTotalTaxAmountPos = true;
                            data.summarizeInfo.isTotalAmtWithoutTaxPos = true;
                            data.summarizeInfo.isDiscountAmtPos = true;
                            data.summarizeInfo.discountAmount = -data.summarizeInfo.discountAmount;
                            data.summarizeInfo.sumOfTotalLineAmountWithoutTax = -data.summarizeInfo.discountAmount;
                            data.summarizeInfo.totalAmountWithoutTax = -data.summarizeInfo.totalAmountWithoutTax;
                            data.summarizeInfo.totalAmountWithTax = -data.summarizeInfo.totalAmountWithTax;
                            data.summarizeInfo.totalTaxAmount = -data.summarizeInfo.totalTaxAmount;

                            foreach (itemInfo item in lst)
                            {
                                item.adjustmentTaxAmount = 1;
                                item.isIncreaseItem = false;
                                item.itemName = "Điều chỉnh giảm tiền hàng, tiền thuế của hàng hóa/dịch vụ: " + item.itemName;
                                item.unitPrice = -1 * item.unitPrice;
                                item.itemTotalAmountAfterDiscount = -item.itemTotalAmountAfterDiscount;
                                item.itemTotalAmountWithoutTax = -item.itemTotalAmountWithTax;
                                item.itemTotalAmountWithTax = -item.itemTotalAmountWithTax;
                                


                            }
                            itemInfo des = new itemInfo()
                            {
                                selection = 2,
                                itemName = string.Format("Điều chỉnh giảm tiền hàng, tiền thuế cho hóa đơn điện tử số {0} lập ngày {1} số tiền: {2} ", originalInvoiceId, originalInvoiceIssueDateOnly, trx.DocTotal.ToString("###,###,###,###"))
                            };
                            lst.Add(des);

                        }
                        else if (trx.DocObjectCodeEx == "13") //HOA DON DIEU CHINH TANG
                        {
                            if (trx.DocTotal == 0) //HOA DON DIEU CHINH THONG TIN
                            {
                                data.generalInvoiceInfo.adjustmentType = eDSC.adjustmentType.Ajustment;
                                data.generalInvoiceInfo.adjustmentInvoiceType = eDSC.adjustmentInvoiceType.Infor;
                                
                                foreach (itemInfo item in data.itemInfo)
                                {
                                    item.selection = 2;
                                }
                            }
                            else //HOA DON DIEU CHINH TIEN TANG
                            {
                                data.generalInvoiceInfo.adjustmentType = eDSC.adjustmentType.Ajustment;
                                data.generalInvoiceInfo.adjustmentInvoiceType = eDSC.adjustmentInvoiceType.Money;
                                data.summarizeInfo.isTotalAmountPos = true;
                                data.summarizeInfo.isTotalTaxAmountPos = true;
                                data.summarizeInfo.isTotalAmtWithoutTaxPos = true;
                                data.summarizeInfo.isDiscountAmtPos = true;
                                
                                
                                foreach (itemInfo item in lst)
                                {
                                    item.adjustmentTaxAmount = 1;
                                    item.isIncreaseItem = true;
                                    item.itemName = @"Điều chỉnh tăng tiền hàng, tiền thuế của hàng hóa/dịch vụ: " + item.itemName;
                                }
                                itemInfo des = new itemInfo()
                                {
                                    selection = 2,
                                    itemName = string.Format("Điều chỉnh tăng tiền hàng, tiền thuế cho hóa đơn điện tử số {0} lập ngày {1} số tiền: {2} ", originalInvoiceId, originalInvoiceIssueDateOnly, trx.DocTotal.ToString("###,###,###,###")),
                                };
                                lst.Add(des);

                            }
                        }
                    }

                    if (IsReplace == "Y") //HOA DON THAY THE
                    {
                        itemInfo des = new itemInfo()
                        {
                            selection = 2,
                            itemName = string.Format("Hóa đơn thay thế cho hóa đơn điện tử mẫu {0} ký hiệu {1} số {2} lập ngày {3}",
                            Setting.invoiceType, Setting.invoiceSeries, originalInvoiceId, originalInvoiceIssueDateOnly)
                        };
                        lst.Add(des);
                    }
                }


                string remark = "";
                remark = trx.UserFields.Fields.Item("U_InvRemark").Value.ToString();
                if (remark != "")
                {
                    itemInfo rmk = new itemInfo()
                    {
                        selection = 2,
                        itemName = remark
                    };
                    lst.Add(rmk);
                }
                data.itemInfo = lst;

                if (originalInvoiceId != "")
                {
                    json = JsonConvert.SerializeObject(data);
                }
                else
                {
                    json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
                }
                    // 

                   

                return json;
            }

            catch (Exception ex)
            {
                SAPUtils.SAPWriteLog("TransactionToViettelJson", "", trx.DocNum.ToString(), ex.ToString(), "");
                return "";
            }
        }
        public static VietteleInvoiceResponse PostTransactionToViettel(SAPbobsCOM.Documents trx, bool isDraft,string transactionUuid)
        {
            string createInvoiceUrl = Setting.APIURL + @"/InvoiceWS/createInvoice/" + Setting.supplierTaxCode;
            if (isDraft)
                createInvoiceUrl = Setting.APIURL + @"/InvoiceWS/createOrUpdateInvoiceDraft/" + Setting.supplierTaxCode;

            string responsebody = "";
            string json = "";

            //string transactionUuid = System.Guid.NewGuid().ToString();
            json = TransactionToViettelJson(trx, transactionUuid);

            try
            {
                string token = getAccessTokenEachCall();

                SAPUtils.SAPWriteLog("PostTransactionToViettel", json, "senddata", token, createInvoiceUrl);

                responsebody=CreateRequest.webRequestV2(createInvoiceUrl, json, token, "POST", "application/json", "", 0, 0);

                VietteleInvoiceResponse ret = Newtonsoft.Json.JsonConvert.DeserializeObject<VietteleInvoiceResponse>(responsebody);
                //ret.uuId = transactionUuid;

                SAPUtils.SAPWriteLog("PostTransactionToViettel", json, responsebody, "responsedata", createInvoiceUrl);

                return ret;
                //using (var webClient = new System.Net.WebClient())
                //{
                //    webClient.Encoding = Encoding.UTF8;
                //    webClient.Headers[System.Net.HttpRequestHeader.ContentType] = "application/json";
                //    webClient.Headers.Add("Cookie", token );
                //    responsebody = webClient.UploadString(createInvoiceUrl, "POST", json);

                   

                //    VietteleInvoiceResponse ret=Newtonsoft.Json.JsonConvert.DeserializeObject<VietteleInvoiceResponse>(responsebody);
                //    ret.uuId = transactionUuid;

                //    SAPUtils.SAPWriteLog("PostTransactionToViettel", json, responsebody, "responsedata", createInvoiceUrl);

                //    return ret;
                //}
            }
            
            catch (Exception ex)
            {
                SAPUtils.SAPWriteLog("PostTransactionToViettel", json, ex.ToString(), "exception", createInvoiceUrl);
                return new VietteleInvoiceResponse() { errorCode = ex.ToString() };
            }
        }

        public static string getAccessTokenEachCall()
        {
            string request = @"{""username"":""" + Setting.APIUser + @""",""password"":""" + Setting.APIPassword + @"""}";

            string apiLink = Setting.APIURL.Replace("/services/einvoiceapplication/api/InvoiceAPI", "") + @"/auth/login";
            string contentType = "application/json";
            string method = "POST";
            try
            {
                SAPUtils.SAPWriteLog("getAccessTokenEachCall", request, "senddata", "", apiLink);

                string result = CreateRequest.webRequestgetToken(apiLink, request, method, contentType, Setting.proxy, Setting.port, Setting.ssl);
                responseGetAccessToken auth = JsonConvert.DeserializeObject<responseGetAccessToken>(result);
                //return @"""access_token"": " + @"""" + auth.access_token + @"""";// auth.access_token;

                return @"access_token=" + auth.access_token;
            }
            catch (Exception ex)
            {
                SAPUtils.SAPWriteLog("getAccessTokenEachCall", request, ex.ToString(), "exception", apiLink);
                return "";
            }
        }
         public static string getAccessToken()
        {
            string apiLink = Setting.APIURL.Replace("/services/einvoiceapplication/api/InvoiceAPI", "") + @"/auth/login";
            string contentType = "application/json";
            string method = "POST";


            string request = @"{""username"":""" + Setting.APIUser + @""",""password"":""" + Setting.APIPassword + @"""}";

            try
            {
                SAPUtils.SAPWriteLog("getAccessToken", request, "senddata", "", apiLink);

               
                string result = CreateRequest.webRequestgetToken(apiLink, request, method, contentType, Setting.proxy, Setting.port, Setting.ssl);

                responseGetAccessToken auth = JsonConvert.DeserializeObject<responseGetAccessToken>(result);

                //Setting.strAccessToken = @"access_token=" + auth.access_token;

                return "";
            }
            catch (Exception ex)
            {
                SAPUtils.SAPWriteLog("getAccessToken", request, ex.ToString(), "exception", apiLink);
                return ex.ToString();
            }
        }
       


    }
}
