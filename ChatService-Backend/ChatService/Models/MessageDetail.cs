using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.Models
{
    public class MessageDetail
    {
        

        public string username { get; set; }
        public string message { get; set; }

    }
}
