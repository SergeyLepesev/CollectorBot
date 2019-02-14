using System;
using System.Linq;
using System.Threading.Tasks;
using CollectorBot.Model.DataBase;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers {
    public class UserNameUnique : IConstrainer<User> {
        private readonly IRepositoryAsync<User> _userRepository;

        public UserNameUnique(IRepositoryAsync<User> userRepository) {
            _userRepository = userRepository;
        }

        public async Task Constrain(User entity) {
            var userWithThisName = await _userRepository.GetItems(u => u.Name == entity.Name);
            if (userWithThisName.Any()) {
                throw new ArgumentException($"User with name = {entity.Name} allready add");
            }
        }
    }
}