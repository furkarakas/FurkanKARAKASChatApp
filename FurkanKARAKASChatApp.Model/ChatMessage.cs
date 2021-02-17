using FurkanKARAKASChatApp.Model.MongoDB;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FurkanKARAKASChatApp.Model
{
    public class ChatMessage : MongoBaseModel
    {
        [BsonElement("ChatRoomId")]
        public Guid ChatRoomId { get; set; }

        [BsonElement("ChatRoomUsersId")]
        public Guid ChatRoomUsersId { get; set; }

        [BsonElement("Message")]
        public string Message { get; set; }

        [BsonElement("SendDate")]
        public DateTime SendDate { get; set; }


        public virtual ChatRoomUsers ChatRoomUsers { get; set; }

    }
}
