using CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers;
using CollectorBot.Model.DataBase;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism {
    public class EntityConstraint {
        private readonly IRepositoryAsync<User> _userRepository;
        private readonly UserConstrainer _userConstrainer;

        public EntityConstraint(IRepositoryAsync<User> userRepository) {
            _userRepository = userRepository;
            _userConstrainer = new UserConstrainer(userRepository);
        }

        public void InvokeConstrainEntity<T>(T entity, RepositoryMethod method) {
            if (typeof(T) == typeof(User)) {
                _userConstrainer.InvokeConstrain(entity as User, method);
            }
        }
    }
}