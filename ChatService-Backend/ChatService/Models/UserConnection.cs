
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ChatService.Models
{
    public class UserConnection
    {

        public string Username { get; set; } = string.Empty;

        public string ChatRoomId { get; set; } = string.Empty;

        [BsonIgnore]
        public string Message { get; set; }
    }
}
