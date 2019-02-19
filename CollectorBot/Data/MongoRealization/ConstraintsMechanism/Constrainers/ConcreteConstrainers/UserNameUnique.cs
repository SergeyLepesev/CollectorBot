using System.Threading.Tasks;
using CollectorBot.Exception;
using CollectorBot.Model.DataBase;
using MongoDB.Driver;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers.ConcreteConstrainers {
    public class UserNameUnique : IConcreteConstrainer<User> {
        private readonly IMongoCollection<User> _userRepository;

        public UserNameUnique(IMongoCollection<User> userRepository) {
            _userRepository = userRepository;
        }

        public async Task Constrain(User entity) {
            var userWithSameName = await _userRepository.Find(u => u.Name == entity.Name).FirstOrDefaultAsync();
            if (userWithSameName != null) {
                throw new CollectorException($"Username {entity.Name} already exists");
            }
        }
    }
}