using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers.ConcreteConstrainers;

namespace CollectorBot.Data.MongoRealization.ConstraintsMechanism.Constrainers {
    public class BaseConstrainer<T> {
        protected Dictionary<RepositoryMethod, List<IConcreteConstrainer<T>>> _constrainerByRepoMethod;

        public void InvokeConstrain(T entity, RepositoryMethod method) {
            if (_constrainerByRepoMethod.TryGetValue(method, out var constrainers)) {
                var tasks = constrainers?.Select(z => z.Constrain(entity));
                if (tasks != null) {
                    Task.WhenAll(tasks).GetAwaiter().GetResult();
                }
            }
        }
    }
}