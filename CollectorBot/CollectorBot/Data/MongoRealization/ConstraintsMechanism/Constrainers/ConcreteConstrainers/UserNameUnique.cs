using System;
using System.Linq;
using System.Threading.Tasks;
using CollectorBot.Exception;
using CollectorBot.Model.DataBase;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers.ConcreteConstrainers {
    public class UserNameUnique : IConcreteConstrainer<User> {
        private readonly IRepositoryAsync<User> _userRepository;

        public UserNameUnique(IRepositoryAsync<User> userRepository) {
            _userRepository = userRepository;
        }

        public async Task Constrain(User entity) {
            var userWithSameName = await _userRepository.GetItems(u => u.Name == entity.Name);
            if (userWithSameName.Any()) {
                throw new CollectorException($"Username {entity.Name} already exists");
            }
        }
    }
}