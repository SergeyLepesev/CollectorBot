using System.Threading.Tasks;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers.ConcreteConstrainers {
    public interface IConcreteConstrainer<T> {
        Task Constrain(T entity);
    }
}