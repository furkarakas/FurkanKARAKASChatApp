using FurkanKARAKASChatApp.Model.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurkanKARAKASChatApp.Service
{
    public class BaseMongoRepository<TModel>
       where TModel : MongoBaseModel
    {
        private readonly IMongoCollection<TModel> mongoCollection;

        public BaseMongoRepository(string mongoDBConnectionString, string dbName, string collectionName)
        {
            var client = new MongoClient(mongoDBConnectionString);
            var database = client.GetDatabase(dbName);
            mongoCollection = database.GetCollection<TModel>(collectionName);
        }
           
        public virtual TModel Create(TModel model)
        {
            try
            {
                mongoCollection.InsertOne(model);
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public virtual List<TModel> GetAll()
        {
            return mongoCollection.Find(book => true).ToList();
        }
    }
}
