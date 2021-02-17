using FurkanKARAKASChatApp.Model.MongoDB;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurkanKARAKASChatApp.Model
{
    public class ChatRoom : MongoBaseModel
    {
        [BsonElement("ChatRoomId")]
        public Guid ChatRoomId { get; set; }

        [BsonElement("ChatRoomName")]
        public string ChatRoomName { get; set; }
    }
}
