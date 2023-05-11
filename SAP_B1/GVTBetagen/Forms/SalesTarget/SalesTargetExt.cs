using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Forms
{
    public partial class SalesTarget
    {
        private string UserName;

        /// <summary>
        /// Current selected Sales Manager
        /// </summary>
        private string SalesManagerSelected
        {
            get
            {
                return cbbSalesManager.Selected?.Value;
            }
        }

        /// <summary>
        /// Current selected KA/ASM
        /// </summary>
        private string KASelected
        {
            get
            {
                return cbbKA_ASM.Selected?.Value;
            }
        }


        /// <summary>
        /// Current selected Sales Supervizer
        /// </summary>
        private string SalesSupSelected
        {
            get
            {
                return cbbSalesSup.Selected?.Value;
            }
        }

        /// <summary>
        /// Current selected Team Leader
        /// </summary>
        private string TeamleaderSelected
        {
            get
            {
                return cbbTeamLeader.Selected?.Value;
            }
        }

        /// <summary>
        /// Load data source to combox Sales Managers
        /// Call store from Database 
        /// </summary>
        public void LoadComboboxSalesManagers()
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
        public void LoadComboboxKAASM()
        {
            var query = string.Format(Querystring.sp_GetKA_ASMByUser, UserName, this.SalesManagerSelected);
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
        public void LoadComboboxSalesSups()
        {
            var query = string.Format(Querystring.sp_GetSalesSupByUser, UserName, this.SalesManagerSelected, this.KASelected);
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
        public void LoadComboboxTeamLeaders()
        {
            var query = string.Format(Querystring.sp_GetTeamLeaderByUser, UserName, this.SalesManagerSelected, this.KASelected, this.SalesSupSelected);
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
