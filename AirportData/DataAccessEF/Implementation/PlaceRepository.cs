using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessEF.Factories;
using TestApp;

namespace DataAccessEF.Implementation
{
    public class PlaceRepository : GenericRepository<Place>, IPlaceRepository
    {

        public PlaceRepository(IAplicationContextFactory aplicationContextFactory)
                                                                        : base(aplicationContextFactory)
        {
            
        }

        public Place FindByPlaceCountry(string namePlace, string countryName)
        {
            return DbContext.Set<Place>().AsNoTracking().Where(p => p.PlaceName.Equals(namePlace) && p.Country.Equals(countryName)).FirstOrDefault();
        }


    }
}
