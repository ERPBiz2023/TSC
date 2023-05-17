using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Forms    
{
    public partial class ImportExport
    {
        private string UserName;
        private void SetControlLocation()
        {
            var max = this.UIAPIRawForm.ClientHeight;
            var maxw = this.UIAPIRawForm.ClientWidth;

            this.btnEdit.Item.Top = max - 30;
            this.btnApplySap.Item.Top = this.btnEdit.Item.Top;
            this.btnCancel.Item.Top = this.btnEdit.Item.Top;
            this.btnCancel.Item.Left = maxw - 20 - this.btnCancel.Item.Width;

            //this.grdData.Item.Left = 40;
            this.grdData.Item.Left = 20;
            this.grdData.Item.Width = maxw - 40;
            this.grdDetail.Item.Width= this.grdData.Item.Width;
            this.grdDetail.Item.Left = this.grdData.Item.Left;
           

            if (this.cbbModule.Selected?.Value == "17")
            {
                var distance = this.btnEdit.Item.Top - 10 - this.grdData.Item.Top;

                this.grdData.Item.Height = distance / 2 - 20;

                this.stDetail.Item.Top = this.grdData.Item.Top + this.grdData.Item.Height + 20;
                this.stDetail.Item.Visible = true;

                this.grdDetail.Item.Top = this.stDetail.Item.Top + 30;
                this.grdDetail.Item.Visible = true;
                this.grdDetail.Item.Height = this.btnEdit.Item.Top - this.grdDetail.Item.Top - 10;
            }
            else
            {
                this.grdData.Item.Height = this.btnEdit.Item.Top - this.grdData.Item.Top - 20;
                this.stDetail.Item.Visible = false;
                this.grdDetail.Item.Visible = false;
            }
        }
    }

}
