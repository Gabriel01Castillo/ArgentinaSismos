using System.Collections.Generic;

namespace DataAccessEF
{
    public interface IViewModel<T>
    {
        int PageLength { get; set; }
        int CurrentPage { get; set; }
        bool HasPrevius { get; set; }
        bool HasNext { get; set; }
        int LastPage { get; set; }
        IEnumerable<T> Collection { get; set; }
    }
}