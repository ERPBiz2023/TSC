using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace GVTBetagen.Forms
{
    [FormAttribute("GVTBetagen.Forms.ImportExport", "Forms/ImportExportExcel/ImportExport.b1f")]
    public partial class ImportExport : UserFormBase
    {
        private ImportExport()
        {
            UserName = Application.SBO_Application.Company.UserName;
        }
        private static ImportExport instance;

        public static bool IsFormOpen = false;
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new ImportExport();
                instance.InitControl();
                instance.Show();
                IsFormOpen = true;
            }
        }

        private void InitControl()
        { }

      
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.btnImport = ((SAPbouiCOM.Button)(this.GetItem("btnImp").Specific));
            this.btnExpprt = ((SAPbouiCOM.Button)(this.GetItem("btnExp").Specific));
            this.btnReset = ((SAPbouiCOM.Button)(this.GetItem("btnRes").Specific));
            this.stModule = ((SAPbouiCOM.StaticText)(this.GetItem("stMol").Specific));
            this.cbbModule = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbMol").Specific));
            this.cbbModule.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cbbModule_ComboSelectAfter);
            this.stPath = ((SAPbouiCOM.StaticText)(this.GetItem("stPath").Specific));
            this.edPath = ((SAPbouiCOM.EditText)(this.GetItem("edPath").Specific));
            this.btnFind = ((SAPbouiCOM.Button)(this.GetItem("btnFin").Specific));
            this.grdData = ((SAPbouiCOM.Grid)(this.GetItem("grdData").Specific));
            this.btnEdit = ((SAPbouiCOM.Button)(this.GetItem("btnEdit").Specific));
            this.btnApplySap = ((SAPbouiCOM.Button)(this.GetItem("btnASap").Specific));
            this.btnCancel = ((SAPbouiCOM.Button)(this.GetItem("btnCan").Specific));
            this.grdDetail = ((SAPbouiCOM.Grid)(this.GetItem("grdDet").Specific));
            this.stHeader = ((SAPbouiCOM.StaticText)(this.GetItem("stHea").Specific));
            this.stDetail = ((SAPbouiCOM.StaticText)(this.GetItem("stDet").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.CloseAfter += new SAPbouiCOM.Framework.FormBase.CloseAfterHandler(this.Form_CloseAfter);
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private void Freeze(bool freeze)
        {
            this.UIAPIRawForm.Freeze(freeze);
        }


        private void OnCustomInitialize()
        {
            SetControlLocation();
            if (this.cbbModule.ValidValues.Count > 0)
                this.cbbModule.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            this.cbbModule.Item.DisplayDesc = true;
        }

        private SAPbouiCOM.Button btnImport;
        private SAPbouiCOM.Button btnExpprt;
        private SAPbouiCOM.Button btnReset;
        private SAPbouiCOM.StaticText stModule;
        private SAPbouiCOM.ComboBox cbbModule;
        private SAPbouiCOM.StaticText stPath;
        private SAPbouiCOM.EditText edPath;
        private SAPbouiCOM.Button btnFind;
        private SAPbouiCOM.Grid grdData;
        private SAPbouiCOM.Button btnEdit;
        private SAPbouiCOM.Button btnApplySap;
        private SAPbouiCOM.Button btnCancel;
        private SAPbouiCOM.Grid grdDetail;
        private SAPbouiCOM.StaticText stHeader;
        private SAPbouiCOM.StaticText stDetail;

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            instance = null;
            IsFormOpen = false;
        }

        private void cbbModule_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SetControlLocation(); 
            //if (this.cbbModule.Selected.Value == "17")
            //{
            //    var distance = this.btnEdit.Item.Top - 10 - this.grdData.Item.Top;

            //    this.grdData.Item.Height = distance / 2 - 20;

            //    this.stDetail.Item.Top = this.grdData.Item.Top + this.grdData.Item.Height + 20;
            //    this.stDetail.Item.Visible = true;

            //    this.grdDetail.Item.Top = this.stDetail.Item.Top + 30;
            //    this.grdDetail.Item.Visible = true;
            //    this.grdDetail.Item.Height = this.btnEdit.Item.Top - this.grdDetail.Item.Top - 10;
            //}
          
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            SetControlLocation();
        }
    }
}
