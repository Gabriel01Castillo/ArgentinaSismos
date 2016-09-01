using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccessEF;

public interface  IRepository<T>
{
    ApplicationContext DbContext { get; set; }
    void Insert(T obj);
    void Save();
    T Get(Guid? Id);
    void Delete(Guid? id);
    void Delete(T obj);
    IEnumerable<T> FindAll();
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    void DetectChanges();
}
