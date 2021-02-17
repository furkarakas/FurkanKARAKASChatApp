using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text; 

namespace FurkanKARAKASChatApp.Model.MongoDB
{
    public abstract class MongoBaseModel
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }
    }
}
