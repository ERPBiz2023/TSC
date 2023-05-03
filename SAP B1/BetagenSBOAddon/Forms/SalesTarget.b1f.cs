using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTCore.Forms;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon.Forms
{
    [FormAttribute("BetagenSBOAddon.Forms.SalesTarget", "Forms/SalesTarget.b1f")]
    class SalesTarget : UserFormBase
    {
        private SalesTarget()
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
                    information.FormID = "SalesTarget_F";
                    information.MenuID = "SalesTarget_M";
                    information.MenuName = "Sales Target";
                    information.ParentID = "2048"; // Inventory Transactions
                }
                return information;
            }
        }
        private static SalesTarget instance;

        public static bool IsFormOpen = false;
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new SalesTarget();
                //instance.InitControl();
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
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbMon").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.ComboBox2 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbYea").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.ComboBox3 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbWee").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.ComboBox4 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbSMa").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));
            this.ComboBox5 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbKA").Specific));
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_16").Specific));
            this.ComboBox8 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbSSu").Specific));
            this.StaticText9 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_18").Specific));
            this.ComboBox9 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbTLe").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btnSear").Specific));
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("grData").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("btnSave").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("btnCan").Specific));
            this.StaticText10 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_24").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("edFil").Specific));
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("btnFind").Specific));
            this.CheckBox0 = ((SAPbouiCOM.CheckBox)(this.GetItem("cbkAll").Specific));
            this.CheckBox1 = ((SAPbouiCOM.CheckBox)(this.GetItem("cbkFoc").Specific));
            this.Button4 = ((SAPbouiCOM.Button)(this.GetItem("btnIEx").Specific));
            this.Button5 = ((SAPbouiCOM.Button)(this.GetItem("btnEEx").Specific));
            this.Button6 = ((SAPbouiCOM.Button)(this.GetItem("btnApr").Specific));
            this.Button7 = ((SAPbouiCOM.Button)(this.GetItem("btnCop").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.ComboBox ComboBox2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.ComboBox ComboBox3;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.ComboBox ComboBox4;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.ComboBox ComboBox5;
        private SAPbouiCOM.StaticText StaticText8;
        private SAPbouiCOM.ComboBox ComboBox8;
        private SAPbouiCOM.StaticText StaticText9;
        private SAPbouiCOM.ComboBox ComboBox9;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Grid Grid0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.StaticText StaticText10;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.Button Button3;
        private SAPbouiCOM.CheckBox CheckBox0;
        private SAPbouiCOM.CheckBox CheckBox1;
        private SAPbouiCOM.Button Button4;
        private SAPbouiCOM.Button Button5;
        private SAPbouiCOM.Button Button6;
        private SAPbouiCOM.Button Button7;

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            instance = null;
            IsFormOpen = false;

        }

        private void Freeze(bool freeze)
        {
            this.UIAPIRawForm.Freeze(freeze);
        }
    }
}
