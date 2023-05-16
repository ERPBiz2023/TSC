using Microsoft.Office.Interop.Excel;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTCore
{
    public class GTDialog
    {
        public DialogResult result;
        public FileDialog dialog;

        public void ShowDialog()
        {
            result = dialog.ShowDialog();
        }
    }
    public class UIHelper
    {
        public enum MsgType
        {
            StatusBar,
            Msgbox,
            Windowbox
        }

        /// <summary>
        /// Show message in UI by type: Message box or Status bar
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="msgboxType"> Message box or Status bar</param>
        /// <param name="isError">this param is mean this message is error or notice</param>
        /// <param name="btnDefault">button default: only apply for message box type</param>
        /// <param name="btnCaption1"></param>
        /// <param name="btnCaption2"></param>
        /// <param name="btnCaption3"></param>
        /// <returns></returns>
        public static int LogMessage(string msg, MsgType msgboxType = MsgType.StatusBar, bool isError = false, int btnDefault = 1, string btnCaption1 = "Ok", string btnCaption2 = "", string btnCaption3 = "")
        {
            //IL_0061: Unknown result type (might be due to invalid IL or missing references)
            try
            {
                switch (msgboxType)
                {
                    case MsgType.StatusBar:
                        SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage(msg, SAPbouiCOM.BoMessageTime.bmt_Short, isError);
                        return 1;
                    default:// case MsgType.Msgbox:
                        return SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(msg, btnDefault, btnCaption1, btnCaption2, btnCaption3);
                    //default:
                    //    MessageBox.Show(msg);
                    //    return 1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Show Diaglog with new thread to make sure un frezee system
        /// </summary>
        /// <param name="dialog"></param>
        /// <returns></returns>
        public static DialogResult ShowGTDialog(FileDialog dialog)
        {
            GTDialog state = new GTDialog();
            state.dialog = dialog;

            System.Threading.Thread thread = new System.Threading.Thread(state.ShowDialog);
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
            thread.Join();

            return state.result;
        }

        /// <summary>
        ///  Show Save diaglog excel
        /// </summary>
        /// <param name="fileName">default name for file to save</param>
        /// <returns>full path for file to save</returns>
        public static string SaveExcelDiaglog(string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = fileName;
            saveFileDialog.InitialDirectory = @"C:\"; //System.Windows.Forms.Application.StartupPath;
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = "Export Excel File To";

            DialogResult ret = ShowGTDialog(saveFileDialog);
            if (ret == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }
            return string.Empty;
        }

        /// <summary>
        /// Browser diaglog to find file
        /// </summary>
        /// <returns>full path for file to open</returns>
        public static string BrowserExcelDiaglog()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"C:\"; //System.Windows.Forms.Application.StartupPath;
            openFileDialog1.Title = "Select a Excel file to open";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            DialogResult ret = ShowGTDialog(openFileDialog1);
            if (ret == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            return string.Empty;
        }
        
        public static void Freeze(IForm from)
        {
            from.Freeze(true);
        }

        public static void UnFreeze(IForm from)
        {
            from.Freeze(false);
        }
    }
}
