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
    public class GLPostingDAL
    {
        /// <summary>
        /// This function create JE form
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int AddJounalEntry_PostEndMonth(Hashtable[] datas, ref string message)
        {
            try
            {
                var listHeaderID = datas.Select(x => x["HeaderID"].ToString()).Distinct().ToList(); var ret = DIConnection.Instance.Connect(ref message);
                if (ret)
                {
                    int lRetCode = -1, lErrCode;
                    foreach (var headerID in listHeaderID)
                    {
                        var datafilters = datas.Where(x => x["HeaderID"].ToString() == headerID).ToList();
                        var number = long.Parse(datafilters[0]["HeaderID"].ToString());
                        var date = DateTime.Parse(datafilters[0]["RefDate"].ToString());
                        var remark = datafilters[0]["Remark"].ToString();
                        var indicator = datafilters[0]["Indicator"].ToString();

                        var oJE = (SAPbobsCOM.JournalEntries)DIConnection.Instance.Company.GetBusinessObject(BoObjectTypes.oJournalEntries);
                        oJE.TaxDate = date;
                        oJE.DueDate = date;
                        oJE.ReferenceDate = date;
                        oJE.Memo = remark;
                        oJE.Indicator = indicator;

                        var index2 = 0;
                        foreach (var data in datafilters)
                        {
                            var account = data["Account"].ToString();
                            if (!string.IsNullOrEmpty(account))
                            {
                                oJE.Lines.AccountCode = account;
                            }

                            var debitText = data["Debit"].ToString();
                            var debit = 0.0;
                            if (!string.IsNullOrEmpty(debitText))
                                debit = double.Parse(debitText);
                            oJE.Lines.Debit = debit;

                            var creditText = data["Credit"].ToString();
                            var credit = 0.0;
                            if (!string.IsNullOrEmpty(creditText))
                                credit = double.Parse(creditText);
                            oJE.Lines.Credit = credit;

                            var sku = data["SKU"].ToString();
                            if (!string.IsNullOrEmpty(sku))
                            {
                                oJE.Lines.CostingCode = sku;
                            }

                            var channel = data["Channel"].ToString();
                            if (!string.IsNullOrEmpty(channel))
                            {
                                oJE.Lines.CostingCode2 = channel;
                            }

                            var branch = data["Branch"].ToString();
                            if (!string.IsNullOrEmpty(branch))
                            {
                                oJE.Lines.CostingCode3 = branch;
                            }
                            oJE.Lines.LineMemo = remark;
                            if (index2 < datafilters.Count - 1)
                                oJE.Lines.Add();
                            index2++;
                        }
                        var ret1 = oJE.Add();
                        if (ret1 != 0)
                        {
                            DIConnection.Instance.Company.GetLastError(out lErrCode, out message);
                            break;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    DIConnection.Instance.DIDisconnect();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return -1;
            }
            finally
            {
                DIConnection.Instance.DIDisconnect();
            }
            return -1;
        }
    }
}
