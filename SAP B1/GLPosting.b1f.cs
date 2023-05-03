using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTCore.Forms;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon.Forms
{
    [FormAttribute("BetagenSBOAddon.Forms.GLPosting", "Forms/GLPosting.b1f")]
    class GLPosting : UserFormBase
    {
        private GLPosting()
        {
        }

        private static AddonUserForm information;
        public static AddonUserForm Information
        {
            get
            {
                if (information == null)
                {
                    information = new AddonUserForm();
                    information.FormID = "GLPosting_F";
                    information.MenuID = "GLPosting_M";
                    information.MenuName = "Postting/Re-Allocate GL";
                    information.ParentID = "1536"; // Inventory Transactions
                }
                return information;
            }
        }
        private static GLPosting instance;

        public static bool IsFormOpen = false;
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new GLPosting();
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
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.cbbMon = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbMon").Specific));
            this.cbbYear = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbYea").Specific));
            this.cbbGrp = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbGrp").Specific));
            this.btnPost = ((SAPbouiCOM.Button)(this.GetItem("btnPo").Specific));
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCan").Specific));
            this.btnCancel.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnCancel_ClickAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.ComboBox cbbMon;
        private SAPbouiCOM.ComboBox cbbYear;
        private SAPbouiCOM.ComboBox cbbGrp;
        private SAPbouiCOM.Button btnPost;
        private SAPbouiCOM.Button btnCancel;

        private void btnCancel_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            instance = null;
            IsFormOpen = false;
        }
    }
}
