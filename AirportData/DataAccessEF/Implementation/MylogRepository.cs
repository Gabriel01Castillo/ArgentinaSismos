using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessEF.Factories;
using TestApp;

namespace DataAccessEF.Implementation
{
    public class MylogRepository : IMylogRepository
    {
        public ApplicationContext DbContext{get;set;}

        public MylogRepository(IAplicationContextFactory aplicationContextFactory)
                                                                       
        {
            DbContext = aplicationContextFactory.GetApplicationContext();
        }        

        public void Insert(MyLogs obj)
        {
              DbContext.Set<MyLogs>().Add(obj);
        }

        public new MyLogs Get(Guid? Id)
        {
            throw new NotImplementedException();
        }

        public void Delete(MyLogs obj)
        {
            var log = DbContext.Set<MyLogs>().Find(obj.Id);
            DbContext.Set<MyLogs>().Remove(log);
        }

        public  IEnumerable<MyLogs> FindAll()
        {
            return DbContext.Set<MyLogs>(); 
        }

        public IEnumerable<MyLogs> Find(System.Linq.Expressions.Expression<Func<MyLogs, bool>> predicate)
        {
             return DbContext.Set<MyLogs>().Where(predicate);
        }
             

        public void Save()
        {
            DbContext.SaveChanges();
        }

        public void Delete(Guid? id)
        {
            throw new NotImplementedException();
        }

        public void DetectChanges()
        {
            DbContext.ChangeTracker.DetectChanges();
        }
    }
}
