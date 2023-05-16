using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
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

        public static bool ExportToExcel(string template,  string filename, DataTable dtData, string sheetName, ref string message)
        {
            try
            {
                var excelFilePath = UIHelper.SaveExcelDiaglog(filename);
                // check file path
                if (!string.IsNullOrEmpty(excelFilePath))
                {
                    byte[] bytes = null;
                    //string spath = Globals.Dashboard + "/Content/Files/Templates/Selling Category.xlsx";
                    using (WebClient client = new WebClient())
                    {
                        bytes = client.DownloadData(template);
                    }

                    using (Stream stream = new MemoryStream(bytes))
                    {
                        XLWorkbook wb = new XLWorkbook(stream);
                        IXLWorksheet ws = wb.Worksheet(sheetName);                      
                        ws.Cell(3, 1).InsertData(dtData.AsEnumerable());
                        wb.SaveAs(excelFilePath);
                    }
                    message = excelFilePath;
                }
                else
                { 
                    message = "no file path is given";
                    return false;
                }
            }
            catch(Exception ex)
            {
                message = ex.Message;
                return false;
            }
           // message = string.Empty;
            return true;
        }

        //public string GetExcelColumn(int index)
        //{
        //    int num = index / 26;
        //    return num > 0 ? this.GetExcelColumn(checked(num - 1)) + Strings.Chr(checked(unchecked(index % 26) + 64)).ToString() : Strings.Chr(checked(index + 64)).ToString();
        //}
    }
}
