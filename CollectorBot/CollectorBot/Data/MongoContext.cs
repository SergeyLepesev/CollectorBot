using CollectorBot.Model;
using CollectorBot.Model.DataBase;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CollectorBot.Data {
    public class MongoContext {
        private readonly IMongoDatabase _database;

        public MongoContext(MongoParameters mongo) {
            var client = new MongoClient(mongo.ConnectionString);

            BsonClassMap.RegisterClassMap<User>(cm => {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            _database = client.GetDatabase(mongo.DatabaseName);
        }

        public IMongoCollection<T> GetItems<T>() => _database.GetCollection<T>(typeof(T).Name);
    }
}