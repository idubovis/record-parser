using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RecordParser.WebApp.Controllers;
using RecordParser.WebApp.Models;

namespace RecordParser.WebApp.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestGetRecordsRequest()
        {
            var controller = new RecordController(new TestRecordRepository());
            IHttpActionResult actionResult = controller.Get();
            var contentResult = actionResult as OkNegotiatedContentResult<RecordSet>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var expectedRecordSet = new RecordSet {
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10)),
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31)),
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10))
            };

            Assert.IsTrue(Enumerable.SequenceEqual(expectedRecordSet, contentResult.Content as IEnumerable<Record>));
        }

        [TestMethod]
        public void TestGetRecordsSortedByGenderThenByLastNameRequest()
        {
            var controller = new RecordController(new TestRecordRepository());
            IHttpActionResult actionResult = controller.Get("gender");
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Record>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var expectedRecordSet = new RecordSet {
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10)),
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10))
            };

            Assert.IsTrue(Enumerable.SequenceEqual(expectedRecordSet, contentResult.Content as IEnumerable<Record>));
        }

        [TestMethod]
        public void TestGetRecordsSortedByDateOfBirthRequest()
        {
            var controller = new RecordController(new TestRecordRepository());
            IHttpActionResult actionResult = controller.Get("birthdate");
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Record>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var expectedRecordSet = new RecordSet {
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10)),
            };

            Assert.IsTrue(Enumerable.SequenceEqual(expectedRecordSet, contentResult.Content as IEnumerable<Record>));
        }

        [TestMethod]
        public void TestGetRecordsSortedByLastNameThenByFirstNameRequest()
        {
            var controller = new RecordController(new TestRecordRepository());
            IHttpActionResult actionResult = controller.Get("name");
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Record>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var expectedRecordSet = new RecordSet {
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10)),
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10)),
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10))
            };

            Assert.IsTrue(Enumerable.SequenceEqual(expectedRecordSet, contentResult.Content as IEnumerable<Record>));
        }

        [TestMethod]
        public void TestInvalidGetRecordsRequest()
        {
            var controller = new RecordController(new TestRecordRepository());
            IHttpActionResult actionResult = controller.Get("color");

            Assert.IsNotInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<IEnumerable<Record>>));
        }

        [TestMethod]
        public void TestPostCommaDelimitedRecordRequest()
        {
            var controller = new RecordController(new TestRecordRepository());

            var record = "Johnson, Mike, Male, Green, 2/18/87";

            IHttpActionResult actionResult = controller.Post(record);
            var contentResult = actionResult as OkNegotiatedContentResult<Record>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var expectedRecord = new Record("Johnson", "Mike", Record.Genders.Male, "Green", new DateTime(1987, 2, 18));

            Assert.AreEqual(expectedRecord, contentResult.Content as Record);
        }

        [TestMethod]
        public void TestPostPipeDelimitedRecordRequest()
        {
            var controller = new RecordController(new TestRecordRepository());

            var record = "Johnson| Mike | Male | Green | 2/18/87";

            IHttpActionResult actionResult = controller.Post(record);
            var contentResult = actionResult as OkNegotiatedContentResult<Record>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var expectedRecord = new Record("Johnson", "Mike", Record.Genders.Male, "Green", new DateTime(1987, 2, 18));

            Assert.AreEqual(expectedRecord, contentResult.Content as Record);
        }

        [TestMethod]
        public void TestPostSpaceDelimitedRecordRequestAndVerifyPersistence()
        {
            var controller = new RecordController(new TestRecordRepository());

            var record = "Johnson Mike Male Green 2/18/87";

            // Post a record
            IHttpActionResult actionResult = controller.Post(record);
            var contentResult = actionResult as OkNegotiatedContentResult<Record>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var expectedRecord = new Record("Johnson", "Mike", Record.Genders.Male, "Green", new DateTime(1987, 2, 18));

            Assert.AreEqual(expectedRecord, contentResult.Content as Record);

            // Get all records => should include the new one
            actionResult = controller.Get();
            var getContentResult = actionResult as OkNegotiatedContentResult<RecordSet>;

            Assert.IsNotNull(getContentResult);
            Assert.IsNotNull(getContentResult.Content);

            var expectedRecordSet = new RecordSet {
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10)),
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31)),
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10)),
                // new record
                new Record("Johnson", "Mike", Record.Genders.Male, "Green", new DateTime(1987, 2, 18))
            };

            Assert.IsTrue(Enumerable.SequenceEqual(expectedRecordSet, getContentResult.Content as IEnumerable<Record>));
        }

        [TestMethod]
        public void TestInvalidPostRecordRequest()
        {
            var controller = new RecordController(new TestRecordRepository());

            var record = "Johnson; Mike; Male; Green; 2/18/87"; // Invalid delimiter

            IHttpActionResult actionResult = controller.Post(record);

            Assert.IsNotInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<Record>));
        }

        //-----------------------------  Test HTTP requests  ------------------------------//
        // requires RecordParser.WebApi deployed at http://localhost

        [TestMethod]
        public async void TestHTTPGetRecordRequest()
        {
            try
            {
                var client = new TestHttpClient("http://localhost/");
                var records = await client.GetRecordsAsync("name"); 

                Assert.IsNotNull(records);
            }
            catch (Exception e)
            {
                // bad URL
                Assert.IsTrue(false);
            }  
        }

        public async void TestHTTPPostRecordRequest()
        {
            var record = new Record("Johnson", "Mike", Record.Genders.Male, "Green", new DateTime(1987, 2, 18));
            try
            {
                var client = new TestHttpClient("http://localhost/");
                var record1 = await client.PostRecordAsync(record);
                Assert.IsNotNull(record1);
                Assert.AreEqual(record, record1);
            }
            catch (Exception)
            {
                //Post failed
                Assert.IsTrue(false);
            }
        }
    }

    /// <summary>
    /// Test Repository
    /// </summary>
    class TestRecordRepository : IRecordRepository
    {
        private RecordSet _recordSet;
        public TestRecordRepository()
        {
            _recordSet = new RecordSet {
                new Record("Doe", "John", Record.Genders.Male, "Blue", new DateTime(1990, 2, 10)),
                new Record("Brown", "Debbie", Record.Genders.Female, "White", new DateTime(1999, 8, 31)),
                new Record("Young", "Emily", Record.Genders.Female, "Green", new DateTime(1978, 9, 2)),
                new Record("Brown", "Amy", Record.Genders.Female, "Red", new DateTime(1995, 3, 13)),
                new Record("Craig", "Alex", Record.Genders.Male, "Purple", new DateTime(2001, 2, 10))
            };
        }

        public RecordSet GetRecords() => _recordSet;

        public void SaveRecord(Record record)
        {
            // do nothing
        }
    }

    /// <summary>
    /// HttpClient helper class
    /// </summary>
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
