using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp;

namespace DataAccessEF.Implementation
{
    public interface IPlaceRepository : IRepository<Place>
    {
        Place FindByPlaceCountry(string namePlace, string countryName);
    }
}
