using GTCore.SAP.DIAPI;
using SAPbobsCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetagenSBOAddon.AccessSAP
{
    public class PODAL
    {
        public int CreateGRPOBaseonPO(string PONo, DateTime ReceiptDate, ref string message)
        {
            var query = string.Format(Querystring.sp_PurchaseOrder_LoadbyEntry, PONo);
            Hashtable data = null;

            var queryDetail = string.Format(Querystring.sp_PurchaseOrderDetail_LoadbyEntry, PONo);
            Hashtable[] dataDetails = null;
            try
            {
                using (var connection = Globals.DataConnection)
                {
                    data = connection.ExecQueryToHashtable(query);
                    dataDetails = connection.ExecQueryToArrayHashtable(queryDetail);
                    connection.Dispose();
                }

                if (data == null || (dataDetails == null || dataDetails.Length <= 0))
                {
                    message = "Data is not loaded";
                    return -1;
                }
                var ret = DIConnection.Instance.Connect(ref message);
                if (ret)
                {
                    var oGRPO = (Documents)DIConnection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);
                    oGRPO.DocDate = ReceiptDate;
                    oGRPO.TaxDate = ReceiptDate;
                    oGRPO.DocDueDate = ReceiptDate;
                    var index = 0;
                    foreach (var item in dataDetails)
                    {
                        oGRPO.Lines.BaseEntry = int.Parse(item["DocEntry"].ToString());
                        oGRPO.Lines.BaseLine = int.Parse(item["LineNum"].ToString());
                        if (item["DocNum"].ToString() != PONo.ToString())
                        {
                            oGRPO.Lines.BaseType = 18;
                        }
                        else
                        {
                            oGRPO.Lines.BaseType = 22;
                        }
                        oGRPO.Lines.LineTotal = double.Parse(item["LineTotal"].ToString());
                        var ItemCode = item["ItemCode"].ToString();
                        var lotNoQuery = string.Format(Querystring.sp_PurchaseOrderDetail_LoadLotNo, PONo, ItemCode);
                        Hashtable[] dataLots;
                        using (var connection = Globals.DataConnection)
                        {
                            dataLots = connection.ExecQueryToArrayHashtable(lotNoQuery);
                            connection.Dispose();
                        }
                        if(dataLots.Count() > 0)
                        {
                            foreach(var datalot in dataLots)
                            {
                                oGRPO.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
                                oGRPO.Lines.BinAllocations.BinAbsEntry = int.Parse(datalot["BinEntry"].ToString()); //Conversions.ToInteger(dataTable.Rows[index]["BinEntry"]);
                                oGRPO.Lines.BinAllocations.Quantity = double.Parse(datalot["Quantity"].ToString());//; Conversions.ToDouble(dataTable.Rows[index]["Quantity"]);
                                oGRPO.Lines.BinAllocations.Add();
                            }
                        }
                        if (index != dataDetails.Length - 1)
                            oGRPO.Lines.Add();
                        index++;
                    }

                    var retAdd = oGRPO.Add();
                    if (retAdd != 0)
                    {
                        int error;
                        DIConnection.Instance.Company.GetLastError(out error, out message);
                        //this.oCommon.ShowMessage("Không thêm được Goods Receipt PO. Thông tin lỗi: " + Conversions.ToString(this.ErrCode) + ": " + this.ErrMsg, (short)3);
                        return -1;
                    }
                    else
                    {
                        return int.Parse(DIConnection.Instance.Company.GetNewObjectKey());
                    }
                    
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return -1;
            }
            return -1;
        }
    }
}
