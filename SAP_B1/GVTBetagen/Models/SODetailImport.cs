using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Models
{
    public class SODetailImport
    {
        public SODetailImport(SOImport line)
        {
            this.PONo = line.PONo;
            this.Status = "N";
            this.ImportID = line.ImportID;
            this.ItemCode = line.ItemCode;
            this.Quantity = line.Quantity;
            this.Price = line.Price;
            this.Warehouse = line.Warehouse;
            this.BinCode = line.Bincode;
            this.BatchNumber = line.BatchNumber;
            this.ItemType = line.ProductType;
        }

        //IMPORT_LOG_DETAILS 
        //PONo
        //ItemCode
        //Status
        //ImportID
        //Quantity
        //Price
        //Warehouse
        //BinCode
        //BatchNumber
        //ItemType


        public string PONo { get; set; }
        public string Status { get; set; }
        public string ImportID { get; set; }
        public string ItemCode { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Warehouse { get; set; }
        public string BinCode { get; set; }
        public string BatchNumber { get; set; }
        public string ItemType { get; set; }
    }
}
