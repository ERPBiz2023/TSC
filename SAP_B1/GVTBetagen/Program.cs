using System;
using System.Collections.Generic;
using GVTBetagen.Forms;
using GVTBetagen.Settings;
using SAPbouiCOM.Framework;

namespace GVTBetagen
{
    class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application MainApplication = null;
                if (args.Length < 1)
                {
                    MainApplication = new Application();
                }
                else
                {
                    MainApplication = new Application(args[0]);
                }
                
                Menu MyMenu = new Menu();              
                
                MainApplication.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);

                var message = string.Empty;
                if(!SystemInformation.CurrentAccountConnectionInfo(ref message))
                {
                    System.Windows.Forms.MessageBox.Show(message);
                }

                MainApplication.Run();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Register Event for application 
        /// </summary>
        /// <param name="EventType"></param>
        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    break;
            }
        }
    }
}
