using System.Threading.Tasks;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism {
    public interface IConstrainer<T> {
        Task Constrain(T entity);
    }
}