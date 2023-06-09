﻿using SAPbouiCOM;
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
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
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

    }
}
