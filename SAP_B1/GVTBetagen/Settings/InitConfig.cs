using GVTBetagen.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Settings
{
    public class InitConfig
    {
        public static DateTime MyCurrServerDate = DateTime.MinValue;
        public static string Pub_ServerName = "";
        public static string Pub_DBName = "";
        public static string Pub_UserName = "";
        public static string Pub_Password = "";
        public static int UserId_Pub = -1;
        public static string UserCode_Pub = "";
        public static string User_HRCode = "";
        public static short UserLocation_Pub = -1;
        public static short UserPOCol_Order = 0;
        public static string UserName_Pub = "";
        public static string WhsbyUser = "";
        public static string User_Password_Pub = "";
        public static short GroupPolicy = -1;
        public static short SalEmpId = -1;
        public static string SalEmpCode = "";
        public static bool User_Admin_Pub = false;
        public static string G_Promotion = "";
        public static string G_SalesIncentive = "";
        public static string G_SalesTarget = "";
        public static string G_TargetActual = "";
        public static string G_SOReqForm = "";
        public static string G_LogInfo = "";
        public static string G_InvStatus = "";
        public static string G_IssueEInv = "Y";
        public static string G_DelInfo = "";
        public static string G_POReqFrom = "";
        public static string G_InvReqForm = "";
        public static string G_PostMonthEnd = "";
        public static string G_ImportBAP = "";
        public static string G_HR = "";
        public static short G_UseWebSrv = 0;
        public static string G_SaleOutWeekly = "";
        public static string G_ApprovedStockReqImport = "";
        public static bool LoggedIn_BySalesTool = false;
        public static string EInvoice_Acc = "betagenadmin";
        public static string EInvoice_AccPass = "123456aA@";
        public static string EInvoice_User = "betagenservice";
        public static string EInvoice_UserPass = "123456aA@";
        public static string EInvoice_MSHD = "01GTKT0/001";
        public static string EInvoice_KHHD = "AA/19E";
        public static double G_PersonDeduct = 11000000.0;
        public static double G_RelationDeduct = 4400000.0;
        public static long Pub_PONo = -1;
    }

    public class SystemInformation
    {
        public static bool CurrentAccountConnectionInfo(ref string message)
        {
            try
            {
                var query = string.Format(Querystring.sp_GetUserFromUserCode, SAPbouiCOM.Framework.Application.SBO_Application.Company.UserName);
                Hashtable data;
                using (var connection = Globals.DataConnection)
                {
                    data = connection.ExecQueryToHashtable(query);
                    connection.Dispose();
                }
                if (data == null)
                {
                    message = "Can not get infomation to system. Please log out and log in again";
                    return false;
                }

                int.TryParse(data["USERID"].ToString(), out InitConfig.UserId_Pub);
                short.TryParse(data["GroupPolicy"].ToString(), out InitConfig.GroupPolicy);
                short.TryParse(data["U_POReqOrder"].ToString(), out InitConfig.UserPOCol_Order);
                InitConfig.User_HRCode = data["U_HRCode"].ToString();
                InitConfig.User_Admin_Pub = data["SUPERUSER"].ToString() == "Y";
                InitConfig.G_Promotion = data["Promotion"].ToString();
                InitConfig.G_SalesIncentive = data["SalInc"].ToString();
                InitConfig.G_SalesTarget = data["SalTarget"].ToString();
                InitConfig.G_TargetActual = data["SalTargetAct"].ToString();
                InitConfig.G_SOReqForm = data["SOReq"].ToString();
                InitConfig.G_LogInfo = data["UpdateLog"].ToString(); 
                InitConfig.G_InvStatus = data["UpdateIncStat"].ToString();
                InitConfig.G_DelInfo = data["UpdateDel"].ToString();
                InitConfig.G_POReqFrom = data["PurReq"].ToString(); //Conversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["PurReq"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["PurReq"])));
                InitConfig.G_InvReqForm = data["InTReq"].ToString(); //Conversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["InTReq"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["InTReq"])));
                InitConfig.G_PostMonthEnd = data["FIMonthEnd"].ToString(); //Conversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["FIMonthEnd"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["FIMonthEnd"])));
                InitConfig.G_ImportBAP = data["ImportBAP"].ToString(); //Conversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["ImportBAP"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["ImportBAP"])));
                InitConfig.G_HR = data["HR"].ToString(); // Conversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["HR"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["HR"])));
                InitConfig.G_IssueEInv = data["IssueEInv"].ToString(); //Conversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["IssueEInv"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["IssueEInv"])));
                double.TryParse(data["PersonDeduct"].ToString(), out InitConfig.G_PersonDeduct);
                double.TryParse(data["RelationDeduct"].ToString(), out InitConfig.G_PersonDeduct);
                InitConfig.EInvoice_Acc = data["EInvoice_Acc"].ToString(); //CConversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["EInvoice_Acc"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["EInvoice_Acc"])));
                InitConfig.EInvoice_AccPass = data["EInvoice_AccPass"].ToString(); //CConversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["EInvoice_AccPass"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["EInvoice_AccPass"])));
                InitConfig.EInvoice_User = data["EInvoice_User"].ToString(); //CConversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["EInvoice_User"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["EInvoice_User"])));
                InitConfig.EInvoice_UserPass = data["EInvoice_UserPass"].ToString(); //CConversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["EInvoice_UserPass"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["EInvoice_UserPass"])));
                InitConfig.G_ApprovedStockReqImport = data["ApprovedStockReqImport"].ToString(); //C Conversions.ToString(Interaction.IIf(userIdFromUserCode.Tables[0].Rows[0]["ApprovedStockReqImport"] == DBNull.Value, (object)"", RuntimeHelpers.GetObjectValue(userIdFromUserCode.Tables[0].Rows[0]["ApprovedStockReqImport"])));

                return true;
            }
            catch(Exception ex)
            {
                message = ex.Message;
                return false;
            }
            return false;
        }
    }   
}
