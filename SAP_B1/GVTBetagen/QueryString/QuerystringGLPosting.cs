using GTCore.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen
{
    public partial class Querystring
    {
        /// <summary>
        /// usp_BS_GL_Allocate_CancelSAP "{0}, {1}, {2}"
        /// </summary>
        public static string sp_GetAllocateCancelSAP
        {
            get
            {
                return CallStoreBySystem("usp_BS_GL_Allocate_CancelSAP", "{0}, {1}, {2}");
            }
        }

        /// <summary>
        /// usp_BS_GL_Allocate_ApplySAP "{0}, {1}, {2}"
        /// </summary>
        public static string sp_GetAllocateApplySAP
        {
            get
            {
                return CallStoreBySystem("usp_BS_GL_Allocate_ApplySAP", "{0}, {1}, {2}");
            }
        }

        /// <summary>
        /// usp_BS_GL_Allocate_GroupBY_AccSKUChanBra "{0}, {1}"
        /// </summary>
        public static string sp_GetAllocateApplySAPAccSKUChanBra
        {
            get
            {
                return CallStoreBySystem("usp_BS_GL_Allocate_GroupBY_AccSKUChanBra", "{0}, {1}");
            }
        }
        /// <summary>
        ///  usp_BS_SalaryActual_Account  "{0}, {1}, '{2}'"
        /// </summary>
        public static string sp_GetAllocateSalaryActual_Account
        {
            get
            {
                return CallStoreBySystem("usp_BS_SalaryActual_Account", "{0}, {1}, '{2}'");
            }
        }
    }
}
