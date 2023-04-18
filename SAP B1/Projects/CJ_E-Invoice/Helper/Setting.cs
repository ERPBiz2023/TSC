using System;
using System.Data;
using System.IO;
using System.Xml;
using SAPbobsCOM;

namespace eDSC
{
    class Setting
    {
        public static SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
        public static int daysToKeepLog = 90;
        public static int xml = 90;

        public static string APIURL = "";
        public static string APIUser = "";
        public static string APIPassword = "";
        public static string supplierTaxCode = "";
        public static string invoiceType = "";
        public static string templateCode = "";
        public static string invoiceSeries= "";
        public static string IsConsolidateInvoice = "";
        public static string IsEnableLog = "N";
        public static string proxy = "";
        public static int port = 0;
        public static int ssl = 1;
        //public static string strAccessToken = "";
        /* ================test data========
         public static string APIURL = "https://demo-sinvoice.viettel.vn:8443/InvoiceAPI";
         public static string APIUser = "0100109106-558";
         public static string APIPassword = "123456a@A";
         public static string supplierTaxCode = "0100109106-558";
         public static string invoiceType = "01GTKT";//01GTKT, 02GTTT, 07KPTQ, 03XKNB, 04HGDL, 01BLP
         public static string templateCode = "01GTKT0/003";
         public static string invoiceSeries= "AA/18E";
         public static string IsConsolidateInvoice = "N";
         */
        public static bool LoadSettingsFiles()
        {
            try
            {
                string settingFile = string.Format(@"{0}\{1}\settings.xml", AssemblyInfo.appPath, AssemblyInfo.appName);

                if (!File.Exists(settingFile))
                {
                    bool createFile = CreateSettingsFile(settingFile);
                    if (!createFile)
                        return false;
                }

                bool validateFile = ValidateSettingsFiles(settingFile);
                if (!validateFile)
                    return false;

                XmlDocument xmlSettings = new XmlDocument();

                xmlSettings.Load(settingFile);

                daysToKeepLog =  int.Parse(xmlSettings.SelectSingleNode("//settings/general/daysToKeepLog").InnerText);
                xml = int.Parse(xmlSettings.SelectSingleNode("//settings/general/xml").InnerText);

               

                Logger.LogEvent(Translation.settingLoaded, Logger.EventType.Event, true);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, true);
                return false;
            }

        }

        private static bool ValidateSettingsFiles(string settingFile)
        {

            if (File.Exists(settingFile))
            {

                XmlDocument xmlSettings = new XmlDocument();
                xmlSettings.Load(settingFile);

                #region days log

                try
                {
                    string validateDaysToKeepLog = xmlSettings.SelectSingleNode("//settings/general/daysToKeepLog").InnerText;

                    if (string.IsNullOrEmpty(validateDaysToKeepLog))
                    {
                        Logger.LogEvent(string.Format(Translation.emptyTag, "//settings/general/daysToKeepLog"), Logger.EventType.Error, true);
                        return false;
                    }

                    int num;
                    bool res = int.TryParse(validateDaysToKeepLog, out num);

                    if (!res)
                    {
                        Logger.LogEvent(string.Format(Translation.invalidValueTag, "//settings/general/daysToKeepLog"), Logger.EventType.Error, true);
                        return false;
                    }
                }
                catch
                {
                    Logger.LogEvent(string.Format(Translation.invalidTag, "//settings/general/daysToKeepLog"), Logger.EventType.Error, true);
                    return false;
                }

                #endregion

                #region xml receipt

                try
                {
                    string validateDaysToKeepLog = xmlSettings.SelectSingleNode("//settings/general/xml").InnerText;

                    if (string.IsNullOrEmpty(validateDaysToKeepLog))
                    {
                        Logger.LogEvent(string.Format(Translation.emptyTag, "//settings/general/xml"), Logger.EventType.Error, true);
                        return false;
                    }
                }
                catch
                {
                    Logger.LogEvent(string.Format(Translation.invalidTag, "//settings/general/xml"), Logger.EventType.Error, true);
                    return false;
                }

                #endregion

                #region Attention

                try
                {
                    string attention = xmlSettings.SelectSingleNode("//settings/apiurl/attention").InnerText;

                    if (string.IsNullOrEmpty(attention))
                    {
                        Logger.LogEvent(string.Format(Translation.emptyTag, "//settings/apiurl/attention"), Logger.EventType.Error, true);
                        return false;
                    }
                }
                catch
                {
                    Logger.LogEvent(string.Format(Translation.invalidTag, "//settings/apiurl/attention"), Logger.EventType.Error, true);
                    return false;
                }

                #endregion

                #region Attention

                try
                {
                    string verifyPin = xmlSettings.SelectSingleNode("//settings/apiurl/verifyPin").InnerText;

                    if (string.IsNullOrEmpty(verifyPin))
                    {
                        Logger.LogEvent(string.Format(Translation.emptyTag, "//settings/apiurl/verifyPin"), Logger.EventType.Error, true);
                        return false;
                    }
                }
                catch
                {
                    Logger.LogEvent(string.Format(Translation.invalidTag, "//settings/apiurl/verifyPin"), Logger.EventType.Error, true);
                    return false;
                }

                #endregion

                #region GetStatus

                try
                {
                    string getStatus = xmlSettings.SelectSingleNode("//settings/apiurl/getStatus").InnerText;

                    if (string.IsNullOrEmpty(getStatus))
                    {
                        Logger.LogEvent(string.Format(Translation.emptyTag, "//settings/apiurl/getStatus"), Logger.EventType.Error, true);
                        return false;
                    }
                }
                catch
                {
                    Logger.LogEvent(string.Format(Translation.invalidTag, "//settings/apiurl/getStatus"), Logger.EventType.Error, true);
                    return false;
                }

                #endregion

                #region SignInvoice

                try
                {
                    string getStatus = xmlSettings.SelectSingleNode("//settings/apiurl/signInvoice").InnerText;

                    if (string.IsNullOrEmpty(getStatus))
                    {
                        Logger.LogEvent(string.Format(Translation.emptyTag, "//settings/apiurl/signInvoice"), Logger.EventType.Error, true);
                        return false;
                    }
                }
                catch
                {
                    Logger.LogEvent(string.Format(Translation.invalidTag, "//settings/apiurl/signInvoice"), Logger.EventType.Error, true);
                    return false;
                }

                #endregion

                #region GetSignedInvoice

                try
                {
                    string getSignedInvoice = xmlSettings.SelectSingleNode("//settings/apiurl/getSignedInvoice").InnerText;

                    if (string.IsNullOrEmpty(getSignedInvoice))
                    {
                        Logger.LogEvent(string.Format(Translation.emptyTag, "//settings/apiurl/getSignedInvoice"), Logger.EventType.Error, true);
                        return false;
                    }
                }
                catch
                {
                    Logger.LogEvent(string.Format(Translation.invalidTag, "//settings/apiurl/getSignedInvoice"), Logger.EventType.Error, true);
                    return false;
                }

                #endregion
            }

            return true;
        }

        private static bool CreateSettingsFile(string settingFile)
        {
            try
            {

                using (StreamWriter sw = new StreamWriter(settingFile, false))
                {
                    sw.WriteLine("<settings>");
                    sw.WriteLine("  <general>");
                    sw.WriteLine("    <!-- General Settings -->");
                    sw.WriteLine("    <daysToKeepLog>90</daysToKeepLog>");
                    sw.WriteLine("    <xml>0</xml>");
                    sw.WriteLine("  </general>");
                    sw.WriteLine("  <apiurl>");
                    sw.WriteLine("    <attention>api/Status/Attention</attention>");
                    sw.WriteLine("    <verifyPin>api/Status/VerifyPin</verifyPin>");
                    sw.WriteLine("    <getStatus>api/Status/GetStatus</getStatus>");
                    sw.WriteLine("    <signInvoice>api/Sign/SignInvoice</signInvoice>");
                    sw.WriteLine("    <getSignedInvoice>api/Sign/GetSignedInvoice</getSignedInvoice>");
                    sw.WriteLine("  </apiurl>");
                    sw.WriteLine("</settings>");
                }

                Logger.LogEvent(Translation.settingCreated, Logger.EventType.Warning, true);

                return true;

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, true);
                return false;
            }

        }

        public static bool SaveSettingsFiles()
        {
            try
            {

                string settingFile = string.Format(@"{0}\{1}_settings.xml", AssemblyInfo.appPath, AssemblyInfo.appName);
                
                XmlDocument xmlSettings = new XmlDocument();

                xmlSettings.Load(settingFile);

                xmlSettings.SelectSingleNode("//settings/general/daysToKeepLog").InnerText = daysToKeepLog.ToString();

                xmlSettings.Save(settingFile);
                
                Logger.LogEvent(Translation.settingSaved, Logger.EventType.Event, true);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, true);
                return false;
            }

        }
    }
}
