using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using GTCore;
using GTCore.Helper;
using GVTBetagen.Models;
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
        {
            // chua code toi
            this.btnExpprt.Item.Enabled = false;

            if (string.IsNullOrEmpty(edPath.Value))
            {
                this.btnImport.Item.Enabled = false;
            }
            else
            {
                this.btnImport.Item.Enabled = true;
            }
        }


        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.btnImport = ((SAPbouiCOM.Button)(this.GetItem("btnImp").Specific));
            this.btnImport.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnImport_ClickBefore);
            this.btnExpprt = ((SAPbouiCOM.Button)(this.GetItem("btnExp").Specific));
            this.btnReset = ((SAPbouiCOM.Button)(this.GetItem("btnRes").Specific));
            this.btnReset.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnReset_ClickBefore);
            this.stModule = ((SAPbouiCOM.StaticText)(this.GetItem("stMol").Specific));
            this.cbbModule = ((SAPbouiCOM.ComboBox)(this.GetItem("cbbMol").Specific));
            this.cbbModule.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cbbModule_ComboSelectAfter);
            this.stPath = ((SAPbouiCOM.StaticText)(this.GetItem("stPath").Specific));
            this.edPath = ((SAPbouiCOM.EditText)(this.GetItem("edPath").Specific));
            this.btnFind = ((SAPbouiCOM.Button)(this.GetItem("btnFin").Specific));
            this.btnFind.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnFind_ClickBefore);
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
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            SetControlLocation();
        }

        private void btnFind_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            try
            {
                var path = UIHelper.BrowserExcelDiaglog(this.UIAPIRawForm);
                if (!string.IsNullOrEmpty(path))
                {
                    this.edPath.Value = path;
                }
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(ex.Message, UIHelper.MsgType.StatusBar, true);
            }
            this.Freeze(false);
        }

        private void btnReset_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            this.cbbModule.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            this.edPath.Value = string.Empty;
            this.InitControl();
            this.SetControlLocation();
        }

        private void btnImport_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.Freeze(true);
            try
            {

                var message = string.Empty;

                DataSet dataFromExcel = ExcelHandler.GetDataFromExcel(this.edPath.Value.Trim(), ref message);
                if (dataFromExcel == null)
                {
                    UIHelper.LogMessage(message);
                    this.Freeze(false);
                    return;
                }
                DataRow[] dataRowArray = dataFromExcel.Tables[0].Select();
                var count = 0;
                ImportSOProcessing.Importing(dataRowArray, this.edPath.Value.Trim(), UserName);
               
               //var dataHeader = dataRowArray.Select(x=>x.)
            }
            catch (Exception ex)
            {
                UIHelper.LogMessage(string.Format("Import error {0}", ex.Message), UIHelper.MsgType.StatusBar, true);
            }
            this.Freeze(false);
        }
    }
}
}
