using System;
using System.Collections.Generic;
using System.Text;
using BetagenSBOAddon.Forms;
using BetagenSBOAddon.Settings;
using SAPbouiCOM.Framework;

namespace BetagenSBOAddon
{
    class Menu
    {
        /// <summary>
        /// Init Menu for system
        /// </summary>
        public Menu()
        {
            var infor = GLPosting.Information;
            
            AddMenuItem(infor.MenuID, infor.MenuName, infor.ParentID);
        }

        /// <summary>
        /// Check Menu id Exist
        /// </summary>
        /// <param name="menuID">MenuId to check</param>
        /// <returns></returns>
        public bool MenuExists(string menuID)
        {
            try
            {
                Application.SBO_Application.Menus.Item(menuID);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Add Menu Item to Menus list
        /// </summary>
        /// <param name="menuID">MenuId</param>
        /// <param name="menuName">Menu Description</param>
        /// <param name="parentId">Parent menu ID to set</param>
        public void AddMenuItem(string menuID, string menuName, string parentId)
        {
            if (MenuExists(menuID))
                return;
            try
            {
                // Get the menu collection of the newly added pop-up item
                var oMenuItem = Application.SBO_Application.Menus.Item(parentId);
                var oMenus = oMenuItem.SubMenus;

                var item = ((SAPbouiCOM.MenuCreationParams)
                           (Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));

                item.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                item.UniqueID = menuID;
                item.String = menuName;

                oMenus.AddEx(item);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        /// <summary>
        /// Add Folder to Menu
        /// </summary>
        /// <param name="folderID">Folder ID</param>
        /// <param name="foldelName">Folder Description</param>
        /// <param name="parentId">Parent menu ID to set</param>
        public void AddMenuFolder(string folderID, string foldelName, string parentId = "")
        {
            if (MenuExists(folderID))
                return;
            try
            {
                SAPbouiCOM.Menus oMenus = null;
                SAPbouiCOM.MenuItem oMenuItem = null;
                oMenus = Application.SBO_Application.Menus;
                SAPbouiCOM.MenuCreationParams oCreationPackage = null;
                oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));

                if (string.IsNullOrEmpty(parentId))
                {
                    oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'
                }
                else
                {
                    oMenuItem = Application.SBO_Application.Menus.Item(parentId); // moudles'
                }

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
                oCreationPackage.UniqueID = folderID;
                oCreationPackage.String = foldelName;
                oCreationPackage.Enabled = true;
                oCreationPackage.Position = -1;

                oMenus = oMenuItem.SubMenus;
                oMenus.AddEx(oCreationPackage);

            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage(string.Format("Create Folder {0} has error: {1}", ex.Message, folderID), SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        /// <summary>
        /// Register event for Menus
        /// </summary>
        /// <param name="pVal"></param>
        /// <param name="BubbleEvent"></param>
        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                // check form OutStockRequestFrm
                if (pVal.MenuUID == GLPosting.Information.MenuID)
                {
                    if (pVal.BeforeAction)
                    {
                        GLPosting.ShowForm();
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

        /// <summary>
        /// Check form is openning
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        private bool CheckFormOpen(string formId)
        {
            foreach (var item in Application.SBO_Application.Forms)
            {
                var test = ((SAPbouiCOM.IForm)item).UniqueID;
                var test2 = ((SAPbouiCOM.IForm)item).Title;
                if (((SAPbouiCOM.IForm)item).UniqueID == formId)
                {
                    ((SAPbouiCOM.IForm)item).Visible = true;
                    ((SAPbouiCOM.IForm)item).Select();
                    return true;
                }
            }

            return false;
        }

    }
}
