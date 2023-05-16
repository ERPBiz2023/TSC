using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace GTCore.Helper
{
    public static class Extensions
    {
        /// <summary>
        /// Get Description for enum
        /// </summary>
        /// <param name="enumdata">value of enum</param>
        /// <returns>desciption of enum value</returns>
        public static string GetDescription(this Enum enumdata)
        {
            return (enumdata.GetType().GetTypeInfo().GetMember(enumdata.ToString())
                .FirstOrDefault(x => x.MemberType == MemberTypes.Field).GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)
                .SingleOrDefault() as DescriptionAttribute)?.Description ?? enumdata.ToString();
        }

        /// <summary>
        /// Get enum from discripiton
        /// </summary>
        /// <typeparam name="T">Type of enum to convert</typeparam>
        /// <param name="description">The Description of enum</param>
        /// <returns>The enum value</returns>
        public static T GetEnumValueByDescription<T>(this string description) where T : Enum
        {
            foreach (Enum enumItem in Enum.GetValues(typeof(T)))
            {
                if (enumItem.GetDescription() == description)
                {
                    return (T)enumItem;
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
        }

        /// <summary>
        /// Read data from datarow to hash table
        /// </summary>
        /// <param name="raw">data row form reader</param>
        /// <returns>hash table with data</returns>
        public static Hashtable ToHashtable(this DataRowView raw)
        {
            var hashtable = new Hashtable();
            foreach (DataColumn col in raw.Row.Table.Columns)
            {
                hashtable[col.ColumnName] = ToString(raw[col.ColumnName]);
            }

            return hashtable;
        }

        /// <summary>
        /// /Read datas from data view to hash tables
        /// </summary>
        /// <param name="dataView">data view form reader</param>
        /// <returns>list hash table with data</returns>
        public static Hashtable[] ToHashtableArray(this DataView dataView)
        {
            return (from DataRowView row in dataView
                    select row.ToHashtable()).ToArray();
        }

        /// <summary>
        /// To string for object
        /// return null or empty object
        /// </summary>
        /// <param name="value">string of object</param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            return value.ToString();
        }

        /// <summary>
        /// Export to excel for SAP Grid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="filename"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool ExportToExcel(this SAPbouiCOM.Grid grid, string filename, ref string message)
        {
            try
            {
                if (grid.DataTable == null || grid.DataTable.Columns.Count == 0)
                {
                    message = string.Format("ExportToExcel: Null or empty input table!\n");
                    return false;
                }

                // load excel, and create a new workbook
                var excelApp = new Excel.Application();
                excelApp.Workbooks.Add();

                // single worksheet
                Excel._Worksheet workSheet = excelApp.ActiveSheet;

                // column headings
                var dataIndex = 0;
                for(var index = 0; index < grid.DataTable.Columns.Count; index ++)
                {
                    var id = grid.DataTable.Columns.Item(index).Name;
                    var visible = grid.Columns.Item(id).Visible;
                    if (!visible)
                    {
                        continue;
                    }
                    workSheet.Cells[1, dataIndex + 1] = grid.Columns.Item(id).TitleObject.Caption;
                    workSheet.Cells[1, dataIndex + 1].Font.Bold = true;

                    for (var i = 0; i < grid.DataTable.Rows.Count; i++)
                    {
                        workSheet.Cells[i + 2, dataIndex + 1] = grid.DataTable.GetValue(id, i).ToString();
                    }
                    dataIndex++;
                }

                var excelFilePath = UIHelper.SaveExcelDiaglog(filename);
                // check file path
                if (!string.IsNullOrEmpty(excelFilePath))
                {
                    try
                    {
                        workSheet.SaveAs(excelFilePath);
                        excelApp.Quit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        return false;
                    }
                }
                else
                { // no file path is given
                    excelApp.Visible = true;
                    message = "no file path is given";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = string.Format("ExportToExcel: \n" + ex.Message);
                return false;
            }
        }
    }
}
