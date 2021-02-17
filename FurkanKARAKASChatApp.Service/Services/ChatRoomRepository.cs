using FurkanKARAKASChatApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurkanKARAKASChatApp.Service.Services
{ 
    public class ChatRoomRepository : BaseMongoRepository<ChatRoom>
    {
        public ChatRoomRepository(string mongoDBConnectionString, string dbName, string collectionName) : base(mongoDBConnectionString, dbName, collectionName)
        {
        }
    }
}
