using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVTBetagen.Models
{
    public class ImportSOProcessing
    {
        private ImportSOProcessing ()
        {
            DataFromExcel = new List<SOImport>();
        }
        public string ImportID { get; set; }
        public string FileName { get; set; }
        public string UserName { get; set; }
        public DateTime ImportDate { get; set; }
        public string Status { get; set; }//Status
        public int CounterSOToImport { get; set; }//CounterSOToImport
        public int CounterSOSuccessed { get; set; }//CounterSOSuccessed
        public int CounterSOFailed { get; set; }//CounterSOFailed

        public List<SOImport> DataFromExcel { get; set; }
        public List<SOHeaderImport> Headers { get; set; }
        public static ImportSOProcessing ProcessData;
        public int CountLineFromExcel { get; set; }
        public int CountSOGroup { get; set; }

        public static void Importing(DataRow[] dataRowArray, string fileName, string userName)
        {
            if(ProcessData !=null)
            {
                ProcessData = new ImportSOProcessing();
                ProcessData.ImportID = string.Format("SOImp{0}", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                ProcessData.FileName = fileName;
                ProcessData.UserName = userName;
                ProcessData.ImportDate = DateTime.Now;
                ProcessData.Status = "N";
                foreach (var data in dataRowArray)
                {
                    var row = SOImport.CreateSORow(data);
                    if(row != null)
                    {
                        row.ImportID = ProcessData.ImportID;
                        ProcessData.DataFromExcel.Add(row);
                    }
                }
                ProcessData.CountLineFromExcel = ProcessData.DataFromExcel.Count;
                ProcessData.CounterSOToImport = ProcessData.DataFromExcel.Select(x => x.PONo).Distinct().Count();

                foreach (var poNo in ProcessData.DataFromExcel.Select(x => x.PONo).Distinct())
                {
                    var datas = ProcessData.DataFromExcel.Where(x => x.PONo == poNo).ToList();
                    ProcessData.Headers = new List<SOHeaderImport>();
                   // var first = datas[0];
                    var header = new SOHeaderImport(datas);
                    ProcessData.Headers.Add(header);
                }
            }
            //return ProcessData;
        }
    }
}
