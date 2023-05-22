using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Models
{
    public class SOHeaderImport
    {
        public SOHeaderImport(List<SOImport> sOImports)
        {
            var sOImport = sOImports[0];

            this.ImportID = sOImport.ImportID;
            this.PONo = sOImport.PONo;
            this.OrderDate = sOImport.OrderDate;
            this.CustomerCode = sOImport.CustomerCode;
            this.ShiptoCode = sOImport.ShiptoCode;
            this.Remark = sOImport.Remark;
            this.Status = "N";
            this.Message = string.Empty;

            Details = new List<SODetailImport>();
            foreach(var line in sOImports)
            {
                Details.Add(new SODetailImport(line));
            }
        }

        //IMPORT_LOG_HEADER 
        //    PONo
        //    Status
        //    ImportID
        //    OrderDate
        //    CustomerCode
        //    CountLines
        //    ShiptoCode
        //    Remark
        //    Message

        public string PONo { get; set; }
        public string Status { get; set; }
        public string ImportID { get; set; }
        public string OrderDate { get; set; }
        public string CustomerCode { get; set; }
        public string ShiptoCode { get; set; }
        public string Remark { get; set; }
        public string Message { get; set; }

        public List<SODetailImport> Details { get; set; }
    }
}
