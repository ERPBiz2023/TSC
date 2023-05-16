using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTCore.Helper
{
    public class ExcelHandler
    {
        /// <summary>
        /// Get data from excel file
        /// </summary>
        /// <param name="path">path for file excel</param>
        /// <param name="message">Message when has error</param>
        /// <returns></returns>
        public static DataSet GetDataFromExcel(string path, ref string message)
        {
            DataSet dataFromExcel = null;
            DataSet dataSet = new DataSet();
            try
            {
                OleDbConnection selectConnection;
                try
                {
                    selectConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties= Excel 8.0");
                    selectConnection.Open();
                }
                catch (Exception ex)
                {
                    //message = ex.Message;
                    selectConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties= Excel 12.0;");
                    selectConnection.Open();
                }

                if (selectConnection != null && selectConnection.State == ConnectionState.Open)
                {
                    DataTable dataTable = new DataTable();
                    DataTable oleDbSchemaTable = selectConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, (object[])null);
                    if (oleDbSchemaTable != null || oleDbSchemaTable.Rows.Count > 0)
                    {
                        int num = checked(oleDbSchemaTable.Rows.Count - 1);
                        int index = 0;
                        while (index <= num)
                        {
                            try
                            {
                                string srcTable = oleDbSchemaTable.Rows[index]["table_name"].ToString();
                                string selectCommandText = "SELECT * FROM [" + srcTable + "]";
                                //if (_importTimeSheet)
                                //    selectCommandText += " Where ThoiGianVao<>'' Or ThoiGianRa<>'' ";
                                new OleDbDataAdapter(selectCommandText, selectConnection).Fill(dataSet, srcTable);
                            }                           
                            catch (Exception ex)
                            {
                                message = ex.Message;
                            }
                            checked { ++index; }
                        }
                    }

                    selectConnection.Close();
                    dataFromExcel = dataSet;
                }
                else
                {
                    message = "Can not open connection to Excel ";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return dataFromExcel;
        }

        public bool ExportToExcel(
          string a_sFilename,
          DataTable dtData,
          string a_sSheetName,
          string a_sFileTitle,
          ref string a_sErrorMessage)
        {
            a_sErrorMessage = string.Empty;
            return false;
        }

        //public string GetExcelColumn(int index)
        //{
        //    int num = index / 26;
        //    return num > 0 ? this.GetExcelColumn(checked(num - 1)) + Strings.Chr(checked(unchecked(index % 26) + 64)).ToString() : Strings.Chr(checked(index + 64)).ToString();
        //}
    }
}
