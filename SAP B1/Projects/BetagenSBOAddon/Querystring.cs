using GTCore.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetagenSBOAddon
{
    public class Querystring
    {
        public static string LoadOutStockRequest
        {
            get
            {
                if(CoreSetting.System == SystemType.SAP_HANA)
                    return "CALL \"USP_BS_STOCKOUTREQUEST\" ('{0}','{1}','{2}', {3})";
                else
                    return "EXEC USP_BS_STOCKOUTREQUEST '{0}','{1}','{2}', {3}";
            }
        }

        public static string DeleteOutStockRequest
        {
            get
            {
                if (CoreSetting.System == SystemType.SAP_HANA)
                    return "CALL \"USP_BS_STOCKOUTREQUEST2_DELETE\" ('{0}')";
                else
                    return "EXEC USP_BS_STOCKOUTREQUEST2_DELETE '{0}'";
            }
        }
        public static string NotificationUpdateStock
        {
            get
            {
                if (CoreSetting.System == SystemType.SAP_HANA)
                    return "CALL \"usp_Notification_UpdateStock\" ()";
                else
                    return "EXEC usp_Notification_UpdateStock ";
            }
        }

        public static string BS_STOCKOUTREQUEST_LoadbyID
        {
            get
            {
                if (CoreSetting.System == SystemType.SAP_HANA)
                    return "CALL \"usp_BS_STOCKOUTREQUEST_LoadbyID\" ('{0}')";
                else
                    return "EXEC usp_BS_STOCKOUTREQUEST_LoadbyID '{0}'";
            }
        }
        public static string BS_STOCKOUTREQUEST_DETAIL_LoadbyID
        {
            get
            {
                if (CoreSetting.System == SystemType.SAP_HANA)
                    return "CALL \"usp_BS_STOCKOUTREQUEST_DETAIL_LoadbyID\" ('{0}')";
                else
                    return "EXEC usp_BS_STOCKOUTREQUEST_DETAIL_LoadbyID '{0}'";
            }
        }
    }
}
