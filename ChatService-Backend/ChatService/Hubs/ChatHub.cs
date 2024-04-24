using ChatService.DataService;
using ChatService.DbConnection;
using ChatService.Models;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;

namespace ChatService.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SharedDb _shared;
        private readonly MongoDBService _mongoDBService;

        public ChatHub(SharedDb shared, MongoDBService mongoDBService)
        {
            _shared = shared;
            _mongoDBService = mongoDBService;
        }

        public async Task JoinChat(UserConnection conn)
        {
            await Clients.All.SendAsync("ReceiveMessage", conn.Username, $"{conn.Username} has joined");
            _shared.connections[Context.ConnectionId] = conn;
        }

        public async Task JoinSpecificChatRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoomId);

            var chatRoom = await _mongoDBService.GetChatRoomById(conn.ChatRoomId);
            //cargar chat o agregar un chat
            if (chatRoom == null)
            {
                await _mongoDBService.CreateNewChatRoom(conn.ChatRoomId);
            }
            else
            {

                await Clients.Client(conn.ChatRoomId)
                    .SendAsync("ChatHistory", chatRoom);
            }

            await Clients.Group(conn.ChatRoomId)
                .SendAsync("JoinSpecificChatRoom", conn.Username, $"{conn.Username} has joined {conn.ChatRoomId}");
        }


        public async Task SendMessage(string message)
        {
            if (_shared.connections.TryGetValue(Context.ConnectionId, out UserConnection connection))
            {
                //Agregar mensaje al chat
                 await Clients.Group(connection.ChatRoomId)
                        .SendAsync("ReceiveSpecificMessage", new { username = connection.Username, message });
            }
        }

    }
}
