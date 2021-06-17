using AutoBooksInterviewAssessment.Data.Models;
using AutoBooksInterviewAssessment.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoBooksInterviewAssessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        IReadWriteLib _readWrite;
        public CustomersController(IReadWriteLib readWrite) 
        {
            _readWrite = readWrite;
        }

        // GET: api/<CustomersController>
        [HttpGet]
        public string Get()
        {
            JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@".\Data\database.json"));

            return o1.ToString();
        }

        // GET api/<CustomersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var jsonItems = _readWrite.GetData();

            foreach (JToken j in jsonItems)
            {
                if ((int)j["id"] == id)
                    return j.ToString(Formatting.None);
            }

            return "Not Found";
        }

        // POST api/<CustomersController>
        [HttpPost]
        public string Post([FromBody] Customer customer)
        {
            var jsonItems = _readWrite.GetData();

            foreach (JToken j in jsonItems)
            {
                if ((int)j["id"] == customer.id)
                    return "Customer with that ID already exists";
            }

            var newJsonItem = new Customer()
            {
                id = customer.id,
                name = customer.name
            };

            var newJtoken = JToken.FromObject(newJsonItem);
            jsonItems.Add(newJtoken);

            _readWrite.WriteNewJson(jsonItems);

            return "New Item successfully added";
        }

        // PUT api/<CustomersController>/5
        [HttpPut]
        public string Put([FromBody] Customer customer)
        {
            var jsonItems = _readWrite.GetData();

            foreach (JToken j in jsonItems)
            {
                if ((int)j["id"] == customer.id)
                {
                    j["name"] = customer.name;
                    _readWrite.WriteNewJson(jsonItems);
                    return "Item successfully updated";
                }
            }

            return "Not Found";
        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            var jsonItems = _readWrite.GetData();

            foreach (JToken j in jsonItems)
            {
                if ((int)j["id"] == id)
                {
                    jsonItems.Remove(j);
                    _readWrite.WriteNewJson(jsonItems);
                    return "Item successfully deleted";
                }
            }            

            return "Not Found";
        }

    }
}
