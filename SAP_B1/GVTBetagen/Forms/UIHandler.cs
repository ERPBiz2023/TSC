using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Forms
{
    public class UIHandler
    {/// <summary>
     /// Load data source to combox Sales Managers
     /// Call store from Database 
     /// </summary>
        public static void LoadComboboxSalesManagers(SAPbouiCOM.ComboBox cbbSalesManager, string UserName)
        {
            var query = string.Format(Querystring.sp_GetSaleManagerByUser, UserName);
            Hashtable[] datas;
            using (var connection = Globals.DataConnection)
            {
                datas = connection.ExecQueryToArrayHashtable(query);
                connection.Dispose();
            }
            if (datas == null || datas.Count() <= 0)
            {
                return;
            }

            for (int i = cbbSalesManager.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbSalesManager.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            foreach (var data in datas)
            {
                cbbSalesManager.ValidValues.Add(data["ExtEmpNo"].ToString(), data["FullName"].ToString());
                cbbSalesManager.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
            }
            cbbSalesManager.Select(datas[0]["ExtEmpNo"], SAPbouiCOM.BoSearchKey.psk_ByValue);
        }

        /// <summary>
        /// Load data source for KA/ASM combobox
        /// </summary>
        public static void LoadComboboxKAASM(SAPbouiCOM.ComboBox cbbKA_ASM, string UserName, string SMSelect)
        {
            var query = string.Format(Querystring.sp_GetKA_ASMByUser, UserName, SMSelect);
            Hashtable[] datas;
            using (var connection = Globals.DataConnection)
            {
                datas = connection.ExecQueryToArrayHashtable(query);
                connection.Dispose();
            }
            if (datas == null || datas.Count() <= 0)
            {
                return;
            }

            for (int i = cbbKA_ASM.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbKA_ASM.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            foreach (var data in datas)
            {
                cbbKA_ASM.ValidValues.Add(data["ExtEmpNo"].ToString(), data["FullName"].ToString());
                cbbKA_ASM.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
            }
            cbbKA_ASM.Select(datas[0]["ExtEmpNo"], SAPbouiCOM.BoSearchKey.psk_ByValue);
        }

        /// <summary>
        /// Load data source for Sales sup combobox
        /// </summary>
        public static void LoadComboboxSalesSups(SAPbouiCOM.ComboBox cbbSalesSup, string UserName, string SMSelect, string KASelected)
        {
            var query = string.Format(Querystring.sp_GetSalesSupByUser, UserName, SMSelect, KASelected);
            Hashtable[] datas;
            using (var connection = Globals.DataConnection)
            {
                datas = connection.ExecQueryToArrayHashtable(query);
                connection.Dispose();
            }
            if (datas == null || datas.Count() <= 0)
            {
                return;
            }

            for (int i = cbbSalesSup.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbSalesSup.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            foreach (var data in datas)
            {
                cbbSalesSup.ValidValues.Add(data["ExtEmpNo"].ToString(), data["FullName"].ToString());
                cbbSalesSup.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
            }
            cbbSalesSup.Select(datas[0]["ExtEmpNo"], SAPbouiCOM.BoSearchKey.psk_ByValue);
        }

        /// <summary>
        /// Load data source for Team leader combobox
        /// </summary>
        public static void LoadComboboxTeamLeaders(SAPbouiCOM.ComboBox cbbTeamLeader, string UserName, string SMSelect, string KASelected, string SalesSupSelected)
        {
            var query = string.Format(Querystring.sp_GetTeamLeaderByUser, UserName, SMSelect, KASelected, SalesSupSelected);
            Hashtable[] datas;
            using (var connection = Globals.DataConnection)
            {
                datas = connection.ExecQueryToArrayHashtable(query);
                connection.Dispose();
            }
            if (datas == null || datas.Count() <= 0)
            {
                return;
            }

            for (int i = cbbTeamLeader.ValidValues.Count - 1; i >= 0; i--)
            {
                cbbTeamLeader.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
            foreach (var data in datas)
            {
                cbbTeamLeader.ValidValues.Add(data["ExtEmpNo"].ToString(), data["FullName"].ToString());
                cbbTeamLeader.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
            }
            cbbTeamLeader.Select(datas[0]["ExtEmpNo"], SAPbouiCOM.BoSearchKey.psk_ByValue);
        }

    }
}
