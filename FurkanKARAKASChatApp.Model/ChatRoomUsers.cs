using FurkanKARAKASChatApp.Model.MongoDB;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurkanKARAKASChatApp.Model
{
    public class ChatRoomUsers : MongoBaseModel
    {
        [BsonElement("ChatRoomUsersId")]
        public Guid ChatRoomUsersId { get; set; }

        [BsonElement("ChatRoomId")]
        public Guid ChatRoomId { get; set; }

        [BsonElement("UserNickName")]
        public string UserNickName { get; set; }
    }
}
