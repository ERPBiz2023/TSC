using System;
using System.Collections.Generic;
using BetagenSBOAddon.Forms;
using GTCoreDI.DIAPI;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon
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
                //SAPbouiCOM.SboGuiApi SboGuiApi = null;
                //SboGuiApi = new SAPbouiCOM.SboGuiApi();
                //var sConnectionString = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056";
                //SboGuiApi.Connect(sConnectionString);

                //Globals.Application = SboGuiApi.GetApplication(-1);
                
               // var ret1 = Connection.SetApplication(ref message);
                Application MainApplication = null;
                if (args.Length < 1)
                {
                    MainApplication = new Application();
                }
                else
                {
                    MainApplication = new Application(args[0]);
                }

                Menu MyMenu = new Menu(); // init menu in construction method                

                //MyMenu.AddMenuItems();
                //Globals.Application.MenuEvent += MyMenu.SBO_Application_MenuEvent;
                MainApplication.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                MainApplication.Run();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

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
