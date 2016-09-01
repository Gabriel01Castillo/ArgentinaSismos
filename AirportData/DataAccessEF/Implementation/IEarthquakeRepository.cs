using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DataAccessEF.EntityModels;
using TestApp;

namespace DataAccessEF.Implementation
{
     public interface IEarthquakeRepository : IRepository<Earthquake>
    {
         IEnumerable<Earthquake> Search(EarthquakeModel model);
         List<Guid>  GetRadiusLocation(SqlParameter StartingLatitude, SqlParameter StartingLongitude, SqlParameter MaxDistance);
         Earthquake GetEarthquakeById(Guid id);
         IList<Source> GetAllSources();
         IEnumerable<Earthquake> GetSensibleNotNotifiedEarthquakes();
         IEnumerable<Tweet> GetTweets(DateTime datetTime);
         void DeleteTweet(Tweet tweet);
         void DeleteDevice(RegistrationDevice device);
    }
}
