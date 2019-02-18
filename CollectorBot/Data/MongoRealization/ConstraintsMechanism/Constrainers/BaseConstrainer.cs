using System.Collections.Generic;
using CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers.ConcreteConstrainers;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers {
    public class BaseConstrainer<T> {
        protected Dictionary<RepositoryMethod, List<IConcreteConstrainer<T>>> _constrainerByRepoMethod;

        public void InvokeConstrain(T entity, RepositoryMethod method) {
            if (_constrainerByRepoMethod.TryGetValue(method, out var constrainers)) {
                constrainers?.ForEach(c => c.Constrain(entity));
            }
        }
    }
}