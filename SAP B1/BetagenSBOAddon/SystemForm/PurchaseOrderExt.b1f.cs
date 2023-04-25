using BetagenSBOAddon.Forms;
using GTCore;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon.SystemForm
{
    [FormAttribute("142", "SystemForm/PurchaseOrderExt.b1f")]
    class PurchaseOrderExt : SystemFormBase
    {
        private string PoNo;
        public PurchaseOrderExt()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.btnAllBa = ((SAPbouiCOM.Button)(this.GetItem("btnAllBa").Specific));
            this.btnAllBa.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAllBa_ClickBefore);
            this.btnAllFr = ((SAPbouiCOM.Button)(this.GetItem("btnAllFr").Specific));
            //var no = ((SAPbouiCOM.EditText)(this.GetItem("8").Specific));
            //PoNo = no.Value;
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new DataLoadAfterHandler(this.Form_DataLoadAfter);

        }

        private SAPbouiCOM.Button btnAllBa;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.Button btnAllFr;

        private void btnAllBa_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE ||
                this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
            {
                POAllocationBatch.ShowForm(PoNo);
            }
            else
            {
                UIHelper.LogMessage("Please select PO Entry and Confirm this PO", UIHelper.MsgType.Msgbox, false);
                return;
            }
        }

        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            var no = ((SAPbouiCOM.EditText)(this.GetItem("8").Specific));
            PoNo = no.Value;
        }
    }
}
