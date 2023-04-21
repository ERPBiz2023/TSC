using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTCoreDI.DIAPI
{
    public class Connection
    {
        private static SAPbouiCOM.Application SBO_Application;
        private static SAPbobsCOM.Company oCompany;

        public static bool SetApplication(ref string message)
        {
            int lErrCode; 
            string sConnectionString;
            string sCookie = null;
            string sConnectionContext = null;
            SAPbouiCOM.SboGuiApi SboGuiApi;
            SboGuiApi = new SAPbouiCOM.SboGuiApi();
            if (Environment.GetCommandLineArgs().Length > 1)
                sConnectionString = Environment.GetCommandLineArgs()[1];
            else
                sConnectionString = Environment.GetCommandLineArgs()[0];
            try
            {
                SboGuiApi.Connect(sConnectionString);
                SBO_Application = SboGuiApi.GetApplication();
                oCompany = new SAPbobsCOM.Company();
                sCookie = oCompany.GetContextCookie();
                sConnectionContext = SBO_Application.Company.GetConnectionContext(sCookie);
                if (oCompany.Connected == true)
                    oCompany.Disconnect();
                if (oCompany.SetSboLoginContext(sConnectionContext) != 0)
                {
                    oCompany.GetLastError(out lErrCode, out message);
                    return false;
                }
                if (oCompany.Connect() != 0)
                {
                    oCompany.GetLastError(out lErrCode, out message);
                    return false;
                }

                return true;
            }
            catch { return false; }

        }
    }
}
