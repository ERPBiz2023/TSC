using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetagenSBOAddon.AccessSAP;
using GTCore;
using GTCore.Forms;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon.Forms
{
    [FormAttribute("BetagenSBOAddon.Forms.GLPosting", "Forms/GLPosting.b1f")]
    class GLPosting : UserFormBase
    {
        private GLPostingDAL GLPostingAccess;
        public GLPosting()
        {
            GLPostingAccess = new GLPostingDAL();
        }
        //private static AddonUserForm information;
        //public static AddonUserForm Information
        //{
        //    get
        //    {
        //        if (information == null)
        //        {
        //            information = new AddonUserForm();
        //            information.FormID = "GLAllPosting_F";
        //            information.MenuID = "GLAlPosting_M";
        //            information.MenuName = "Postting/Re-Allocate GL";
        //            information.ParentID = "1536"; // Inventory Transactions
        //        }
        //        return information;
        //    }
        //}
        private static GLPosting instance;

        public static bool IsFormOpen = false;
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new GLPosting();
                instance.InitControl();
                instance.Show();
                IsFormOpen = true;
            }
        }
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.cbbMon = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbMon").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.cbbYea = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbYea").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.cbbGrp = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbGrp").Specific));
            this.btnPos = ((SAPbouiCOM.Button)(this.GetItem("btnPos").Specific));
            this.btnPos.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnPos_ClickBefore);
            this.btnCan = ((SAPbouiCOM.Button)(this.GetItem("btnCan").Specific));
            this.btnCan.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnCan_ClickBefore);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ClickAfter += new SAPbouiCOM.Framework.FormBase.ClickAfterHandler(this.Form_ClickAfter);
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);
        }

        private void InitControl()
        {
            this.cbbYea.Select(DateTime.Now.Year.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbYea.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

            this.cbbMon.Select(DateTime.Now.Month.ToString(), SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbMon.ExpandType = SAPbouiCOM.BoExpandType.et_ValueOnly;

            this.cbbGrp.Select("1", SAPbouiCOM.BoSearchKey.psk_ByValue);
            this.cbbGrp.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            this.cbbGrp.Item.DisplayDesc = true;
        }

        private void Freeze(bool freeze)
        {
            this.UIAPIRawForm.Freeze(freeze);
        }


        private void Form_ClickAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
           
        }

        private void OnCustomInitialize()
        {
           
        }

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            instance = null;
            IsFormOpen = false;
        }

        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.ComboBox cbbMon;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.ComboBox cbbYea;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.ComboBox cbbGrp;
        private SAPbouiCOM.Button btnPos;
        private SAPbouiCOM.Button btnCan;

        private void btnPos_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            try
            {
                var month = int.Parse(this.cbbMon.Selected.Value);
                var year = int.Parse(this.cbbYea.Selected.Value);
                var grp = int.Parse(this.cbbGrp.Selected.Value);
                var query = string.Format(Querystring.sp_GetAllocateCancelSAP, month, year, grp);
                Hashtable[] datas;
                using (var connection = Globals.DataConnection)
                {
                    datas = connection.ExecQueryToArrayHashtable(query);
                    connection.Dispose();
                }

                if(datas.Count() > 0)
                {
                    UIHelper.LogMessage(string.Format(" Data already exist / Poted for this selection, Please check again"), UIHelper.MsgType.StatusBar, true);
                    this.Freeze(false);
                    return;                   
                }
                else
                {
                    query = string.Format(Querystring.sp_GetAllocateApplySAP, month, year, grp);
                    Hashtable[] data1s;
                    using (var connection = Globals.DataConnection)
                    {
                        data1s = connection.ExecQueryToArrayHashtable(query);
                        connection.Dispose();
                    }
                    if(data1s.Count() > 0)
                    {
                        var message = string.Empty;
                        if (GLPostingAccess.AddJounalEntry_PostEndMonth(data1s, ref message) == 1)
                        {
                            UIHelper.LogMessage(string.Format("Posted all transaction from this selection successfully"), UIHelper.MsgType.StatusBar, false);
                        }
                        else
                        {

                            UIHelper.LogMessage(string.Format("Posted all transaction from this selection error {0}", message), UIHelper.MsgType.StatusBar, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Posting GL error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
                this.Freeze(false);
                return;
            }
            this.Freeze(false);
        }

        private void btnCan_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            try
            {
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Cancel GL error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
                this.Freeze(false);
                return;
            }
            this.Freeze(false);
        }
    }
}
