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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <param name="raw"></param>
        /// <returns></returns>
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
        /// <param name="dataView"></param>
        /// <returns></returns>
        public static Hashtable[] ToHashtableArray(this DataView dataView)
        {
            return (from DataRowView row in dataView
                    select row.ToHashtable()).ToArray();
        }

        public static string ToString(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            return value.ToString();
        }

        public static bool ExportToExcel(this SAPbouiCOM.DataTable tbl, string filename, ref string message)
        {
            try
            {
                if (tbl == null || tbl.Columns.Count == 0)
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
                var index = 0;
                foreach (SAPbouiCOM.Column col in tbl.Columns)
                {
                    workSheet.Cells[1, index + 1] = col.Title;
                    index++;
                }

                for (var i = 0; i < tbl.Rows.Count; i++)
                {
                    var j = 0;
                    foreach (SAPbouiCOM.Column col in tbl.Columns)
                    {
                        workSheet.Cells[i + 2, j + 1] = tbl.GetValue(col.UniqueID, j).ToString();
                        j++;
                    }
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
