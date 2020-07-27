using System;
using ArtigoMongoDb.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ArtigoMongoDb.API.Controllers
{
    [Route("api/{controller}")]
    public class EventsController : ControllerBase
    {
        private const string DB_NAME = "EventDb";
        private const string COLLECTION_NAME = "Events";
        private readonly IMongoCollection<Event> _events;
        
        public EventsController(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDb:ConnectionString").Value;
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase(DB_NAME);
            
            _events = database.GetCollection<Event>(COLLECTION_NAME);
        }

        [HttpGet]
        public IActionResult GetEvents() {
            var eventsList = _events.Find(e => true).ToList();

            return Ok(eventsList);
        }

        [HttpGet("{id}")]
        public IActionResult GetEventById(string id) {
            var objectId = ObjectId.Parse(id);

            var foundEvent = _events.Find(e => e.Id == objectId).SingleOrDefault();

            if (foundEvent == null) {
                return NotFound();
            }
            
            return Ok(foundEvent);
        }

        [HttpPost]
        public IActionResult CreateEvent([FromBody]EventInputModel eventInputModel) {
            var newEvent = new Event(eventInputModel.Title, eventInputModel.Description, eventInputModel.TakesPlaceOn);

            _events.InsertOne(newEvent);

            return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id.ToString()}, newEvent);
        }   
    }
}