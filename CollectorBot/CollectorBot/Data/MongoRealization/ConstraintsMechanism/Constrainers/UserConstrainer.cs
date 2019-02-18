using System.Collections.Generic;
using CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers.ConcreteConstrainers;
using CollectorBot.Model.DataBase;
using MongoDB.Driver;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers {
    public class UserConstrainer : BaseConstrainer<User> {
        public UserConstrainer(IMongoCollection<User> userRepository) {
            _constrainerByRepoMethod = new Dictionary<RepositoryMethod, List<IConcreteConstrainer<User>>> {
                {
                    RepositoryMethod.Create,
                    new List<IConcreteConstrainer<User>> {
                        new UserNameUnique(userRepository)
                    }
                }
            };
        }
    }
}