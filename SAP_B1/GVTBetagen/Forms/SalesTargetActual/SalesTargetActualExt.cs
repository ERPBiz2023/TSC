using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Forms
{
    public partial class SalesTargetActual
    {
        private string UserName;

        /// <summary>
        /// Current selected Sales Manager
        /// </summary>
        private string SalesManagerSelected
        {
            get
            {
                if (cbbSalesManager.Selected != null && cbbSalesManager.Selected?.Value != "All")
                    return cbbSalesManager.Selected?.Value;
                return "";
            }
        }

        /// <summary>
        /// Current selected KA/ASM
        /// </summary>
        private string KASelected
        {
            get
            {
                if (cbbKA_ASM.Selected != null && cbbKA_ASM.Selected?.Value != "All")
                    return cbbKA_ASM.Selected?.Value;
                return "";
            }
        }


        /// <summary>
        /// Current selected Sales Supervizer
        /// </summary>
        private string SalesSupSelected
        {
            get
            {
                if (cbbSalesSup.Selected != null && cbbSalesSup.Selected?.Value != "All")
                    return cbbSalesSup.Selected?.Value;
                return "";
            }
        }

        /// <summary>
        /// Current selected Team Leader
        /// </summary>
        private string TeamleaderSelected
        {
            get
            {
                if (cbbTeamLeader.Selected != null && cbbTeamLeader.Selected?.Value != "All")
                    return cbbTeamLeader.Selected?.Value;
                return "";
            }
        }

        
        /// <summary>
        ///  Get month selected in form
        /// </summary>
        private int MonthSelected
        {
            get
            {
                int month;
                if (!int.TryParse(cbbMon.Selected.Value, out month))
                {
                    month = DateTime.Now.Month;
                }
                return month;
            }
        }

        /// <summary>
        ///  get year selected in form
        /// </summary>
        private int YearSelected
        {
            get
            {
                int year;
                if (!int.TryParse(cbbYear.Selected.Value, out year))
                {
                    year = DateTime.Now.Year;
                }
                return year;
            }
        }
    }
}
