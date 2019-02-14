using System.Collections.Generic;
using CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers;
using CollectorBot.Model.DataBase;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism {
    public class EntityConstraint {
        private readonly IRepositoryAsync<User> _userRepository;
        private List<IConstrainer<User>> _userConstrainer;

        public EntityConstraint(IRepositoryAsync<User> userRepository) {
            _userRepository = userRepository;
            InitConstrainers();
        }

        public void InvokeConstrainEntity<T>(T entity) {
            if (typeof(T) == typeof(User)) {
                Invoke(entity as User, _userConstrainer);
            }
        }

        private void Invoke<T>(T entity, List<IConstrainer<T>> handlers) {
            handlers.ForEach(h => h.Constrain(entity));
        }

        private void InitConstrainers() {
            _userConstrainer = new List<IConstrainer<User>>{
                new UserNameUnique(_userRepository)
            };
        }
    }
}