using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Models
{    
    public class SOImport
    {
        public static SOImport CreateSORow(DataRow data)
        {
            if (data == null)
                return null;

            try
            {
                var row = new SOImport();
                row.PONo = data[0].ToString();
               //DateTime orderDate;
                //DateTime.TryParseExact(data[1].ToString(), "dd-MM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out orderDate);
                row.OrderDate = data[1].ToString();
                row.ItemCode = data[2].ToString();
                row.ItemName = data[3].ToString();
                //long qty;
                //long.TryParse(data[4].ToString(), out qty);
                row.Quantity = data[4].ToString();
                //decimal price;
                //decimal.TryParse(data[5].ToString(), out price);
                row.Price = data[5].ToString();
                row.CustomerCode = data[6].ToString();
                row.ShiptoCode = data[7].ToString();
                row.Warehouse = data[8].ToString();
                row.Bincode = data[9].ToString();
                row.BatchNumber = data[10].ToString();
                row.ProductType = data[11].ToString();
                row.Remark = data[12].ToString();
                row.Status = "N"; // New import
                return row;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private SOImport()
        {

        }
        public string ImportID { get; set; }

        public string PONo { get; set; }
        public string OrderDate { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string Warehouse { get; set; }
        public string Bincode { get; set; }
        public string BatchNumber { get; set; }
        public string ProductType { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }

        public string Gt_Message { get; set; }
    }
}
