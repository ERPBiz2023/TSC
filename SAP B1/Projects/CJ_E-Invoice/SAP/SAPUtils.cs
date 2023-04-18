using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using SAPbouiCOM;
using System.Windows.Forms;
using System.Diagnostics;

namespace eDSC
{
    class SAPUtils
    {
       
        public static string GetErrorByCode(string errorcode)
        {
            SAPbobsCOM.UserTable oUserTable = Setting.oCompany.UserTables.Item("ERRORCODE");
            if (oUserTable.GetByKey(errorcode))
                return oUserTable.UserFields.Fields.Item("U_DescriptionEN").Value.ToString();
            else
                return "Unknown error: " + errorcode;
        }
        public string Initialize()
        { 
            string msg="";

            try
            {
              
                //===========generate configuration table=============
                if (!CheckTableExists("@CONFIGRATION"))
                    msg = CreateUDT("CONFIGRATION", "CONFIGRATION", SAPbobsCOM.BoUTBTableType.bott_NoObject);

                //=========generatel U_Value in configuration table==========
                if (!CheckFieldExists("@CONFIGRATION", "U_Value"))
                    msg = CreateUDF("CONFIGRATION", "Value", "Value", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "", "");


                if (msg == "")
                {
                    int ret = 0;
                    SAPbobsCOM.UserTable oUserTable = Setting.oCompany.UserTables.Item("CONFIGRATION");

                    //======adding APIURL================
                    if (oUserTable.GetByKey("APIURL"))
                    {
                        Setting.APIURL = oUserTable.UserFields.Fields.Item("U_Value").Value.ToString();
                    }
                    else
                    {
                        oUserTable.Code = "APIURL";
                        oUserTable.Name = "APIURL";
                        Setting.APIURL = "https://api-vinvoice.viettel.vn/services/einvoiceapplication/api/InvoiceAPI";
                        oUserTable.UserFields.Fields.Item("U_Value").Value = Setting.APIURL;
                        ret = oUserTable.Add();
                    }

                    //======adding APIUser================
                    if (oUserTable.GetByKey("APIUser"))
                    {
                        Setting.APIUser = oUserTable.UserFields.Fields.Item("U_Value").Value.ToString();
                    }
                    else
                    {
                        oUserTable.Code = "APIUser";
                        oUserTable.Name = "APIUser";
                        Setting.APIUser = "0100109106-509";
                        oUserTable.UserFields.Fields.Item("U_Value").Value = Setting.APIUser;
                        ret = oUserTable.Add();
                    }

                    //======adding APIPassword================
                    if (oUserTable.GetByKey("APIPassword"))
                    {
                        Setting.APIPassword = oUserTable.UserFields.Fields.Item("U_Value").Value.ToString();
                    }
                    else
                    {
                        oUserTable.Code = "APIPassword";
                        oUserTable.Name = "APIPassword";
                        Setting.APIPassword = "123456a@A";
                        oUserTable.UserFields.Fields.Item("U_Value").Value = Setting.APIPassword;
                        ret = oUserTable.Add();
                    }

                    //======adding supplierTaxCode================
                    if (oUserTable.GetByKey("supplierTaxCode"))
                    {
                        Setting.supplierTaxCode = oUserTable.UserFields.Fields.Item("U_Value").Value.ToString();
                    }
                    else
                    {
                        oUserTable.Code = "supplierTaxCode";
                        oUserTable.Name = "supplierTaxCode";
                        Setting.APIPassword = "0100109106-509";
                        oUserTable.UserFields.Fields.Item("U_Value").Value = Setting.APIPassword;
                        ret = oUserTable.Add();
                    }

                    //======adding invoiceType================
                    if (oUserTable.GetByKey("invoiceType"))
                    {
                        Setting.invoiceType = oUserTable.UserFields.Fields.Item("U_Value").Value.ToString();
                    }
                    else
                    {
                        oUserTable.Code = "invoiceType";
                        oUserTable.Name = "invoiceType";
                        Setting.invoiceType = "1";
                        oUserTable.UserFields.Fields.Item("U_Value").Value = Setting.invoiceType;
                        ret = oUserTable.Add();
                    }

                    //======adding templateCode================
                    if (oUserTable.GetByKey("templateCode"))
                    {
                        Setting.templateCode = oUserTable.UserFields.Fields.Item("U_Value").Value.ToString();
                    }
                    else
                    {
                        oUserTable.Code = "templateCode";
                        oUserTable.Name = "templateCode";
                        Setting.templateCode = "1/3138";
                        oUserTable.UserFields.Fields.Item("U_Value").Value = Setting.templateCode;
                        ret = oUserTable.Add();
                    }

                    //======adding invoiceSeries================
                    if (oUserTable.GetByKey("invoiceSeries"))
                    {
                        Setting.invoiceSeries = oUserTable.UserFields.Fields.Item("U_Value").Value.ToString();
                    }
                    else
                    {
                        oUserTable.Code = "invoiceSeries";
                        oUserTable.Name = "invoiceSeries";
                        Setting.invoiceSeries = "K22TCJ";
                        oUserTable.UserFields.Fields.Item("U_Value").Value = Setting.invoiceSeries;
                        ret = oUserTable.Add();
                    }
              
                }

                if (!CheckFieldExists("ORDR", "U_InvSerial"))
                    msg = CreateUDF("ORDR", "InvSerial", "E-Invoice Serial", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "", "");

                if (!CheckFieldExists("ORDR", "U_InvForm"))
                    msg = CreateUDF("ORDR", "InvForm", "E-Invoice Form", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "", "");

                if (!CheckFieldExists("ORDR", "U_InvCode"))
                    msg = CreateUDF("ORDR", "InvCode", "E-Invoice No", SAPbobsCOM.BoFieldTypes.db_Alpha,  100, "", "");

                if (!CheckFieldExists("ORDR", "U_InvDate"))
                    msg = CreateUDF("ORDR", "InvDate", "E-Invoice Date", SAPbobsCOM.BoFieldTypes.db_Date, 10, "", "");

                if (!CheckFieldExists("ORDR", "U_OrgInvNo"))
                    msg = CreateUDF("ORDR", "OrgInvNo", "Original E-Invoice No", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "", "");

                if (!CheckFieldExists("ORDR", "U_OrgInvDate"))
                    msg = CreateUDF("ORDR", "OrgInvDate", "Original E-Invoice Date", SAPbobsCOM.BoFieldTypes.db_Date, 50, "", "");

                if (!CheckFieldExists("ORDR", "U_IsReplace"))
                    msg = CreateUDF("ORDR", "IsReplace", "Is Replacement", SAPbobsCOM.BoFieldTypes.db_Alpha, 1, "", "N");
                if (!CheckFieldExists("ORDR", "U_TransUid"))
                    msg = CreateUDF("ORDR", "TransUid", "TransactionUuid", SAPbobsCOM.BoFieldTypes.db_Alpha, 100, "", "");

                //===========generate LOG table=============
                if (!CheckTableExists("@LOG"))
                    msg = CreateUDT("LOG", "LOG", SAPbobsCOM.BoUTBTableType.bott_NoObject);

                if (!CheckFieldExists("@LOG", "U_Function"))
                    msg = CreateUDF("LOG", "Function", "Function", SAPbobsCOM.BoFieldTypes.db_Alpha, 50, "", "");

                if (!CheckFieldExists("@LOG", "U_Json"))
                    msg = CreateUDF("LOG", "Json", "Json", SAPbobsCOM.BoFieldTypes.db_Memo, 50, "", "");

                if (!CheckFieldExists("@LOG", "U_Response"))
                    msg = CreateUDF("LOG", "Response", "Response", SAPbobsCOM.BoFieldTypes.db_Memo, 50, "", "");

                if (!CheckFieldExists("@LOG", "U_Exception"))
                    msg = CreateUDF("LOG", "Exception", "Exception", SAPbobsCOM.BoFieldTypes.db_Memo, 50, "", "");

                if (!CheckFieldExists("@LOG", "U_APIUrl"))
                    msg = CreateUDF("LOG", "APIUrl", "APIUrl", SAPbobsCOM.BoFieldTypes.db_Alpha, 200, "", "");


               
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            

            return msg;

        }
        public int GetFieldidByName(string TableName, string FieldName)
        {
            int index = -1;
            SAPbobsCOM.Recordset ors = default(SAPbobsCOM.Recordset);
            try
            {
                ors =(SAPbobsCOM.Recordset)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                if (Setting.oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                {
                    ors.DoQuery("select \"FieldID\" from \"CUFD\" where \"TableID\" = '" + TableName + "' and \"AliasID\" = '" + FieldName + "';");
                }
                else
                {
                    ors.DoQuery("select FieldID from CUFD where TableID = '" + TableName + "' and AliasID = '" + FieldName + "'");
                }

                if (!ors.EoF)
                {
                    index = (int)ors.Fields.Item("FieldID").Value;
                }
            }
            catch (Exception ex)
            {

            }

            return index;
        }
        public bool CheckFieldExists(string TableName, string FieldName)
        {
            bool ret = false;
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = null;
            try
            {
                FieldName = FieldName.Replace("U_", "");
                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

                int FieldID = GetFieldidByName(TableName, FieldName);
                if (oUserFieldsMD.GetByKey(TableName, FieldID))
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }

            }
            catch (Exception ex)
            {
                ret = false;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();
            }
            return ret;
        }
        public bool CheckTableExists(string TableName)
        {
            bool ret = false;
            SAPbobsCOM.UserTablesMD oUdtMD = null;
            try
            {
                TableName = TableName.Replace("@", "");
                oUdtMD = (SAPbobsCOM.UserTablesMD)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
                if (oUdtMD.GetByKey(TableName))
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUdtMD);
                oUdtMD = null;
                GC.Collect();
            }
            return ret;
        }

        public string CreateUDT(string tableName, string tableDesc, SAPbobsCOM.BoUTBTableType tableType)
        {
            string ret = "";
            SAPbobsCOM.UserTablesMD oUdtMD = null;
            try
            {
                int lRetCode = 0;
                oUdtMD = (SAPbobsCOM.UserTablesMD)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
                if (oUdtMD.GetByKey(tableName) == false)
                {
                    oUdtMD.TableName = tableName;
                    oUdtMD.TableDescription = tableDesc;
                    oUdtMD.TableType = tableType;
                    lRetCode = oUdtMD.Add();
                    if ((lRetCode != 0))
                    {
                        if ((lRetCode == -2035))
                        {
                            ret = "-2035";
                        }
                        ret = Setting.oCompany.GetLastErrorDescription();

                    }

                    ret = "";
                }
                else
                {
                    ret = "";
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUdtMD);
                oUdtMD = null;
                GC.Collect();
            }
            return ret;

        }
        public string CreateUDF(string tableName, string fieldName, string desc, SAPbobsCOM.BoFieldTypes fieldType, int Size, string LinkTab,
            string DefaultValue = "", string companyDB = "", SAPbobsCOM.BoFldSubTypes fieldsubType = SAPbobsCOM.BoFldSubTypes.st_None)
        {
            string ret = "";
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = default(SAPbobsCOM.UserFieldsMD);
            try
            {
                int lRetCode = 0;
                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)Setting.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = tableName;
                oUserFieldsMD.Name = fieldName;
                oUserFieldsMD.Description = desc;
                oUserFieldsMD.Type = fieldType;
                if (Size > 0)
                    oUserFieldsMD.EditSize = Size;

                oUserFieldsMD.SubType = fieldsubType;
                oUserFieldsMD.DefaultValue = DefaultValue;

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if ((lRetCode == -2035 | lRetCode == -1120))
                    {
                        ret = Convert.ToString(lRetCode);
                    }
                    return Setting.oCompany.GetLastErrorDescription();
                }

                ret = "";
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            finally
            {
            }
            return ret;
        }


       
        public static void SAPWriteLog(string function, string json, string response, string exception, string apiurl)
        {
            string code = DateTime.Now.ToString("yyyyMMddhhmmssFFF");
            SAPbobsCOM.UserTable oUserTable = Setting.oCompany.UserTables.Item("LOG");
            oUserTable.Code = code;
            oUserTable.Name = code;
            oUserTable.UserFields.Fields.Item("U_Json").Value = json;
            oUserTable.UserFields.Fields.Item("U_Response").Value = response;
            oUserTable.UserFields.Fields.Item("U_Function").Value = function;
            oUserTable.UserFields.Fields.Item("U_Exception").Value = exception;
            oUserTable.UserFields.Fields.Item("U_APIUrl").Value = apiurl;
            int ret = oUserTable.Add();
        }


     
        public static int GetTaxPercent(SAPbobsCOM.Documents trx)
        {
            try
            {
                int TaxPercent = -1;
                string current = "";
                string previous = "";
                trx.Lines.SetCurrentLine(0);
                current = trx.Lines.TaxCode;
                previous = trx.Lines.TaxCode;
                TaxPercent =int.Parse(trx.Lines.TaxPercentagePerRow.ToString());

                for (int i = 1; i <= trx.Lines.Count - 1; i++)
                {
                    trx.Lines.SetCurrentLine(i);
                    current = trx.Lines.TaxCode;

                    if (current != previous)
                        return -1;
                    
                    previous = current;
                }

                return TaxPercent;
            }
            catch (Exception ex)
            {
                return -1;
            }
            
        }
    }
}
