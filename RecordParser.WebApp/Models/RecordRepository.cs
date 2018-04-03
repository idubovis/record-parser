using System.Web;

namespace RecordParser.WebApp.Models
{
    public class RecordRepository : IRecordRepository
    {
        private RecordSet _recordSet;
        public RecordRepository()
        {
            _recordSet = RecordSet.CreateRecordSet(new string[]
            {
                HttpContext.Current.Request.MapPath(@"~/App_Data/records1.txt"),
                HttpContext.Current.Request.MapPath(@"~/App_Data/records2.txt"),
                HttpContext.Current.Request.MapPath(@"~/App_Data/records3.txt")
            });
        }

        public RecordSet GetRecords() => _recordSet;

        public void SaveRecord(Record record)
        {
            // update record in the data storage
            // omitted according to the task spec
        }
    }
}