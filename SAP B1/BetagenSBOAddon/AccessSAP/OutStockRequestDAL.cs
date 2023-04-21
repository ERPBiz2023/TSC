using GTCore.SAP.DIAPI;
using GTCoreDI.DIAPI;
using SAPbobsCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetagenSBOAddon.AccessSAP
{
    public class OutStockRequestDAL
    {
        public int CreateInventoryTrannsferRequest(string stockNo, ref string message)
        {
            int lRetCode = -1, lErrCode;
            var query = string.Format(Querystring.sp_LoadOutStockRequestByID, stockNo);
            Hashtable data = null;

            var queryDetail = string.Format(Querystring.sp_LoadOutStockRequestDetailByID, stockNo);
            Hashtable[] dataDetails = null;
            using (var connection = Globals.DataConnection)
            {
                var errorstr = string.Empty;
                data = connection.ExecQueryToHashtable(query, out errorstr);
                dataDetails = connection.ExecQueryToArrayHashtable(queryDetail, out errorstr);
                connection.Dispose();
            }

            if (data == null || (dataDetails == null || dataDetails.Length <= 0))
            {
                message = "Data is not loaded";
                return -1;
            }

            try
            {
                var ret1 = Connection.SetApplication(ref message);

                var ret = DIConnection.Instance.Connect(ref message);
                if (ret)
                {
                    var oStockTransfer = (StockTransfer)DIConnection.Instance.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryTransferRequest);
                    DateTime dtStockDate;
                    var requestNo = data["StockNo"].ToString();
                    if (DateTime.TryParse(data["StockDate"].ToString(), out dtStockDate))
                    {
                        oStockTransfer.DocDate =  dtStockDate;
                        oStockTransfer.TaxDate = dtStockDate;
                    }

                    oStockTransfer.Comments = string.Format("Yêu cầu chuyển kho nội bộ:{0}", requestNo);
                    oStockTransfer.UserFields.Fields.Item("U_DescEN").Value = string.Format("Internal transfer Req:{0}", requestNo);// "Internal transfer Req: " + ReqNo);
                                                                                                                                    //BS_STOCKOUTREQUEST_DETAIL_LoadbyID
                    oStockTransfer.FromWarehouse = data["FromWhsCode"].ToString();
                    oStockTransfer.ToWarehouse = data["ToWhsCode"].ToString();
                    oStockTransfer.UserFields.Fields.Item("U_SoPhieu").Value = requestNo;
                    oStockTransfer.UserFields.Fields.Item("U_FromBIN").Value = string.IsNullOrEmpty(data["AbsEntry"].ToString()) ? "-1" : data["AbsEntry"].ToString();
                    oStockTransfer.UserFields.Fields.Item("U_FromBinCode").Value = string.IsNullOrEmpty(data["BinCode"].ToString()) ? "" : data["BinCode"].ToString(); //RuntimeHelpers.GetObjectValue(Interaction.IIf(table1.Rows[index1]["BinCode"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(table1.Rows[index1]["BinCode"])));
                    oStockTransfer.UserFields.Fields.Item("U_TOBIN").Value = string.IsNullOrEmpty(data["AbsEntry1"].ToString()) ? "-1" : data["AbsEntry1"].ToString(); //RuntimeHelpers.GetObjectValue(Interaction.IIf(table1.Rows[index1]["AbsEntry1"] == DBNull.Value, (object)-1, RuntimeHelpers.GetObjectValue(table1.Rows[index1]["AbsEntry1"])));
                    oStockTransfer.UserFields.Fields.Item("U_ToBinCode").Value = string.IsNullOrEmpty(data["BinCode1"].ToString()) ? "" : data["BinCode1"].ToString(); //RuntimeHelpers.GetObjectValue(Interaction.IIf(table1.Rows[index1]["BinCode1"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(table1.Rows[index1]["BinCode1"])));

                    var index = 0;
                    foreach (var detail in dataDetails)
                    {
                        oStockTransfer.Lines.ItemCode = detail["ItemCode"].ToString();
                        oStockTransfer.Lines.Quantity = double.Parse(detail["Quantity"].ToString());
                        oStockTransfer.Lines.FromWarehouseCode = data["FromWhsCode"].ToString();
                        oStockTransfer.Lines.WarehouseCode = data["ToWhsCode"].ToString();
                        oStockTransfer.Lines.UserFields.Fields.Item("U_BatchNo").Value = detail["LotNo"].ToString();

                        if (index != dataDetails.Length - 1)
                            oStockTransfer.Lines.Add();
                        index++;
                    }
                    lRetCode = oStockTransfer.Add();
                    if (lRetCode != 0)
                    {
                        DIConnection.Instance.Company.GetLastError(out lErrCode, out message);

                        lErrCode = - 1;
                    }
                    else
                    {

                        lErrCode = 1;
                    }
                    DIConnection.Instance.DIDisconnect();
                    return lErrCode;

                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                message = string.Format("Error {0}", ex.Message);
                return -1;
            }
        }
    }
}
