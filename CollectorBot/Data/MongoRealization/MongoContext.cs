using CollectorBot.Model;
using CollectorBot.Model.DataBase;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CollectorBot.Data.MongoRealization {
    public class MongoContext {
        private readonly IMongoDatabase _database;

        public MongoContext(MongoParameters mongo) {
            var client = new MongoClient(mongo.ConnectionString);

            var pack = new ConventionPack {
                new EnumRepresentationConvention(BsonType.String)
            };
            ConventionRegistry.Register("EnumStringConvention", pack, t => true);

            BsonClassMap.RegisterClassMap<User>(cm => {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });

            BsonClassMap.RegisterClassMap<Transaction>(cm => {
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