using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTCore.Forms
{
    public class AddonUserForm
    {
        public string FormID { get; set; }
        public string MenuID { get; set; }
        public string MenuName { get; set; }
        public string ParentID { get; set; } = string.Empty;
        public string MenuFolderID { get; set; } = string.Empty;
        public string MenuFolderName { get; set; } = string.Empty;
        
    }

    public enum FormMode
    {
        Add,
        View
    }
}
    