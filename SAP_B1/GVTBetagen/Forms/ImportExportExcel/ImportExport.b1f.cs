using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace GVTBetagen.Forms.ImportExportExcel
{
    [FormAttribute("GVTBetagen.Forms.ImportExportExcel.ImportExport", "Forms/ImportExportExcel/ImportExport.b1f")]
    class ImportExport : UserFormBase
    {
        public ImportExport()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btnImp").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("btnExp").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("btnCan").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_10").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_12").Specific));
            this.Button5 = ((SAPbouiCOM.Button)(this.GetItem("Item_13").Specific));
            this.Grid0 = ((SAPbouiCOM.Grid)(this.GetItem("Item_14").Specific));
            this.Button6 = ((SAPbouiCOM.Button)(this.GetItem("Item_15").Specific));
            this.Button7 = ((SAPbouiCOM.Button)(this.GetItem("Item_16").Specific));
            this.Button8 = ((SAPbouiCOM.Button)(this.GetItem("Item_17").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {

        }
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.Button Button5;
        private SAPbouiCOM.Grid Grid0;
        private SAPbouiCOM.Button Button6;
        private SAPbouiCOM.Button Button7;
        private SAPbouiCOM.Button Button8;
    }
}
