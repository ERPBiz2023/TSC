using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace eDSC
{
    class Message
    {

        public static void MsgBoxWrapper(string msg, string isString = "", bool isErr = false)
        {
            try
            {

                if (!(ConnectSAP.SBO_Application == null))
                {
                    ConnectSAP.SBO_Application.SetStatusBarMessage(msg, SAPbouiCOM.BoMessageTime.bmt_Medium, isErr);
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("MsgBoxWrapper : " + msg);
                //ConnectSAP.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, isErr);
            }
        }
        public static void MsgBoxWrapperWin(string msg, string isString = "", bool isErr = false)
        {
            try
            {
                if (isString == "")
                {
                    isString = "Notification";
                }
                if (isErr == false)
                {
                    MessageBox.Show(msg, isString, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(msg, isString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, isString, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
