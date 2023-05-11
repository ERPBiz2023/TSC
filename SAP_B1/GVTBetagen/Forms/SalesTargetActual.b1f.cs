using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTCore.Forms;
using SAPbouiCOM.Framework;

namespace GVTBetagen.Forms
{
    [FormAttribute("GVTBetagen.Forms.SalesTargetActual", "Forms/SalesTargetActual.b1f")]
    class SalesTargetActual : UserFormBase
    {
        private SalesTargetActual()
        {
        }
        //private static AddonUserForm information;
        //public static AddonUserForm Information
        //{
        //    get
        //    {
        //        if (information == null)
        //        {
        //            information = new AddonUserForm();
        //            information.FormID = "SalesTargetActual_F";
        //            information.MenuID = "SalesTargetActual_M";
        //            information.MenuName = "Sales Target Actual";
        //            information.ParentID = "2048"; 
        //        }
        //        return information;
        //    }
        //}
        private static SalesTargetActual instance;

        public static bool IsFormOpen = false;
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new SalesTargetActual();
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
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.ComboBox1 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbYea").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.ComboBox3 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbSMa").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.ComboBox4 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbKA").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));
            this.ComboBox5 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbSSu").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_12").Specific));
            this.ComboBox6 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbTLe").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btnSearch").Specific));
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("grData").Specific));
            this.Grid0.ClickAfter += new SAPbouiCOM._IGridEvents_ClickAfterEventHandler(this.Grid0_ClickAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("btnEEX").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("btnCancel").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.LoadAfter += new SAPbouiCOM.Framework.FormBase.LoadAfterHandler(this.Form_LoadAfter);
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.ComboBox ComboBox1;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.ComboBox ComboBox3;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.ComboBox ComboBox4;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.ComboBox ComboBox5;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.ComboBox ComboBox6;
        private SAPbouiCOM.Button Button0;

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
           

        }

        private SAPbouiCOM.Grid Grid0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;

        private void Grid0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
        }

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
