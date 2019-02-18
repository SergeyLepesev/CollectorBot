using CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers;
using CollectorBot.Model.DataBase;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism {
    public class MongoConstrain {
        private readonly UserConstrainer _userConstrainer;

        public MongoConstrain(MongoContext context) {
            var userRepository = context.GetItems<User>();
            _userConstrainer = new UserConstrainer(userRepository);
        }

        public void InvokeConstrainEntity<T>(T entity, RepositoryMethod method) {
            if (typeof(T) == typeof(User)) {
                _userConstrainer.InvokeConstrain(entity as User, method);
            }
        }
    }
}