using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RecordParser.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // !!! assuming the web service is runnning at http://localhost/49999
            var client = new TestHttpClient("http://localhost/49999");

            try
            {
                // GET all records
                var records = client.GetRecordsAsync("name").GetAwaiter().GetResult();
                foreach (var rec in records)
                    Console.WriteLine(rec);

                // POST a record
                var record = new Record("Johnson", "Mike", Record.Genders.Male, "Green", new DateTime(1987, 2, 18));
                var record1 = client.PostRecordAsync(record).GetAwaiter().GetResult();
                Console.WriteLine(record1);

                Console.ReadKey();
               
            }
            catch (Exception e)
            {
                //Post failed
                Console.WriteLine(e.StackTrace);
            }
        }


    }

    class TestHttpClient
    {
        private readonly HttpClient _client;

        public TestHttpClient(string url)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(url);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<Record>> GetRecordsAsync(string sortType)
        {
            IEnumerable<Record> records = null;
            HttpResponseMessage response = await _client.GetAsync($"records/{sortType}");
            if (response.IsSuccessStatusCode)
                records = await response.Content.ReadAsAsync<IEnumerable<Record>>();
            return records;
        }

        public async Task<Record> PostRecordAsync(Record record)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("records", record);
            response.EnsureSuccessStatusCode();
            return record;
        }
    }
}
