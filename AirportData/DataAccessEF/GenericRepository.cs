using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataAccessEF.Factories;


namespace DataAccessEF
{
    public abstract class GenericRepository<T> : IRepository<T> where T : Entity, new() 
    {
        public ApplicationContext DbContext { get; set; }

        public GenericRepository(IAplicationContextFactory aplicationContextFactory)
        {
            DbContext = aplicationContextFactory.GetApplicationContext();
        }

        public void  Insert(T obj)
        {
            DbContext.Set<T>().Add(obj);
            
        }

        public void Save()
        {
            DbContext.SaveChanges();
        }

       
        public T Get(Guid? id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public void Delete(Guid? id)
        {
               var factura = Get(id);
                DbContext.Set<T>().Remove(factura);
                
        }

        public void Delete(T obj)  {
            
            DbContext.Set<T>().Remove(obj);

        }

        public void DetectChanges()
        {
            DbContext.ChangeTracker.DetectChanges();
        }

        public IEnumerable<T> FindAll()
        {
            return DbContext.Set<T>(); 
        }

        public  IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            
            return DbContext.Set<T>().Where(predicate);
        }

        public IEnumerable<T> Query(Func<T, bool> predicate)
        {
            return DbContext.Set<T>().AsEnumerable().Where(predicate);
        }

      protected IEnumerable<T> Paginate(IEnumerable<T> query, int currentPage, int pageLength)
      {
            
          return query.Skip(currentPage*pageLength).Take(pageLength);
      }

        protected  bool  HasNext(IEnumerable<T> query, int currentPage, int pageLength)
        {
            return ((currentPage * pageLength) + pageLength) < query.Count();
        }

        protected  bool HasPrevius(IEnumerable<T> query, int currentPage, int pageLength)
        {
            return (currentPage) > 0 && query.Count() > 0;
        }

        protected int LastPage(int elementCount, int pageLength) {
            return elementCount / pageLength;
        }
    }
}














