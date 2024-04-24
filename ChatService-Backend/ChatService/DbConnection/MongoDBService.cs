using ChatService.Models;
using MongoDB.Driver;

namespace ChatService.DbConnection
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<ChatRoom>? chatRoomCollection;

        public MongoDBService(IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("ChatService");
            chatRoomCollection = _database.GetCollection<ChatRoom>("ChatRoom");
        }

        public async Task<ChatRoom> GetChatRoomById(string chatRoomId)
        {
            var filter = Builders<ChatRoom>.Filter.Eq(cr => cr.Id, chatRoomId);
            var chatRoom = await chatRoomCollection.Find(filter).FirstOrDefaultAsync();

            return chatRoom;
        }

        public async Task<ChatRoom> CreateNewChatRoom(string chatRoomid)
        {
            var chatRoom = new ChatRoom();
            chatRoom.Id = chatRoomid;

            await chatRoomCollection.InsertOneAsync(chatRoom);

            return chatRoom;
        }

    }

}
