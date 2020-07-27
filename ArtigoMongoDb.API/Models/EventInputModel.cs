using System;

namespace ArtigoMongoDb.API.Models
{
    public class EventInputModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TakesPlaceOn { get; set; }
    }
}