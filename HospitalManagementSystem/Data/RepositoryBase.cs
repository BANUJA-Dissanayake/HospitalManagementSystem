using System.Collections.Generic;
using System.Linq;

namespace HospitalManagementSystem.Data
{
    // Generic base: shows generics + shared CRUD plumbing that every
    // entity-specific repository below builds on (avoids repeating
    // GetAll/Delete/SaveChanges in each of the seven repositories).
    public abstract class RepositoryBase<T> where T : class
    {
        protected readonly HospitalContext Context;

        protected RepositoryBase(HospitalContext context)
        {
            Context = context;
        }

        public virtual List<T> GetAll() => Context.Set<T>().ToList();

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }
    }
}
