using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.Models
{
    public class ChatRoom
    {
        public string Id { get; set; }

        public List<MessageDetail>? messages { get; set; } = new List<MessageDetail>();
    }
}
