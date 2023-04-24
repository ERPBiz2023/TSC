using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTCore.Forms;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon.Forms
{
    [FormAttribute("BetagenSBOAddon.Forms.POAllocationBatch", "Forms/POAllocationBatch.b1f")]
    class POAllocationBatch : UserFormBase
    {
        public POAllocationBatch()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private static POAllocationBatch instance;

        public static bool IsFormOpen = false;
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new POAllocationBatch();
                instance.Show();
                IsFormOpen = true;
            }
        }

        private static AddonUserForm information;
        public static AddonUserForm Information
        {
            get
            {
                if (information == null)
                {
                    information = new AddonUserForm();
                    information.FormID = "POAllocationBatch_F";
                    information.MenuID = "";
                    information.MenuName = "";
                    information.ParentID = ""; 
                }
                return information;
            }
        }
    }
}
