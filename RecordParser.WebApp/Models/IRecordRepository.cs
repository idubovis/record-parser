namespace RecordParser.WebApp.Models
{
    public interface IRecordRepository
    {
        // Get all records
        RecordSet GetRecords();

        // Add/update a record in a data storage
        void SaveRecord(Record record);
    }
}
