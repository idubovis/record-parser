using RecordParser.WebApp.Models;
using System;
using System.Web.Http;

namespace RecordParser.WebApp.Controllers
{
    public class RecordController : ApiController
    {
        private IRecordRepository _recordRepository;

        public RecordController()
        {
            _recordRepository = new RecordRepository();
        }

        public RecordController(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        [Route("records")]
        [HttpGet]
        public IHttpActionResult Get() => Ok(_recordRepository.GetRecords());

        [Route("records/{sortType}")]
        [HttpGet]
        public IHttpActionResult Get(string sortType)
        {
            switch (sortType.ToLower())
            {
                case "gender":
                    return Ok(_recordRepository.GetRecords().SortByGenderAndThenByLastName());
                case "birthdate":
                    return Ok(_recordRepository.GetRecords().SortByDateOfBirth());
                case "name":
                    return Ok(_recordRepository.GetRecords().SortByLastNameThenByFirstName());
                default:
                    return BadRequest("Invalid request: records/" + sortType);
            }
        }

        [Route("records")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]string value)
        {
            try
            {
                var record = RecordSet.CreateRecord(value);
                var records = _recordRepository.GetRecords();
                if (records.Contains(record))
                    records.Remove(record);
                records.Add(record);
                return Ok(record);
            }
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
