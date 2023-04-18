using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eDSC
{
    class Addon
    {

        private SAPbouiCOM.Application SBO_Application;
        public Addon() : base()
        {
            try
            {

                //Initialize and connect SAP
                //Class_Init();
                ConnectSAP.SetApplication();
                // events handled by SBO_Application_AppEvent
                //SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                SBO_Application = ConnectSAP.SBO_Application;
                //ConnectSAP.SBO_Application = SBO_Application;
                //ConnectSAP.SetApplicationDI();
               
                new SAPUtils().Initialize();

                if (Setting.APIURL == "" || Setting.APIUser == "" || Setting.APIPassword == "" ||
                    Setting.supplierTaxCode == "" || Setting.invoiceType == "" || Setting.templateCode == "" ||
                    Setting.invoiceSeries == "" )
                {
                    MessageBox.Show("Please update configuration then restart Addon!");
                }
                else
                {
                    Logger.StartLog(10);

                    string err =APIUtils.getAccessToken();

                    if (err != "")
                    {
                        MessageBox.Show(err);
                        return;
                    }
                    SBO_Application.SetStatusBarMessage("Get API token successful!", SAPbouiCOM.BoMessageTime.bmt_Short,false);   
                    fARInvoice f = new fARInvoice(SBO_Application);
                }
                

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                SBO_Application.SetStatusBarMessage(ex.ToString(), SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

        }
        private int ConnectToCompany()
        {
            int connectToCompanyReturn = 0;

            // Establish the connection to the company database.
            connectToCompanyReturn =Setting.oCompany.Connect();

            return connectToCompanyReturn;
        }
        private void Class_Init()
        {
            SetApplication();
            if (!(SetConnectionContext() == 0))
            {
               SBO_Application.MessageBox("Failed setting a connection to DI API");
                System.Environment.Exit(0); // Terminating the Add-On Application
            }
            if (!(ConnectToCompany() == 0))
            {
                SBO_Application.MessageBox("Failed connecting to the company's Database");
                System.Environment.Exit(0); // Terminating the Add-On Application
            }
            else
                SBO_Application.SetStatusBarMessage("Please wait, addon is loading.......",SAPbouiCOM.BoMessageTime.bmt_Short, false);

            SBO_Application.SetStatusBarMessage("Add-on (2022_06_01_1) is loaded", SAPbouiCOM.BoMessageTime.bmt_Short, false);
        }
        private int SetConnectionContext()
        {
            string sCookie;
            string sConnectionContext;
            int setConnectionContextReturn = 0;
            try
            {
                Setting.oCompany = new SAPbobsCOM.Company();
                sCookie = Setting.oCompany.GetContextCookie();
                sConnectionContext =SBO_Application.Company.GetConnectionContext(sCookie);
                if (Setting.oCompany.Connected == true)
                    Setting.oCompany.Disconnect();
                setConnectionContextReturn= Setting.oCompany.SetSboLoginContext(sConnectionContext);
                return setConnectionContextReturn;
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
                
                return 0;
            }
        }
      
        public void SetApplication()
        {
            SAPbouiCOM.SboGuiApi sbogui;
            string oconnection;
            sbogui = new SAPbouiCOM.SboGuiApi();
            if (Environment.GetCommandLineArgs().Length == 1)
                oconnection = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056";
            else
                oconnection = Environment.GetCommandLineArgs().GetValue(1).ToString() ;
            try
            {
                sbogui.Connect(oconnection);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("No SAP Application Running "+ex.ToString());
                
                System.Environment.Exit(0);
            }
            SBO_Application = sbogui.GetApplication(-1);
        }
        private void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                {
                    Logger.EndLog();
                   Setting.oCompany.Disconnect();
                    System.Windows.Forms.Application.Exit();
                    break;
                }

                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                {
                    Logger.EndLog();
                    Setting.oCompany.Disconnect();
                    System.Windows.Forms.Application.Exit();
                    break;
                }

                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                {
                    Logger.EndLog();
                    Setting.oCompany.Disconnect();
                    System.Windows.Forms.Application.Exit();
                    break;
                }
            }
        }
        

    }
}
