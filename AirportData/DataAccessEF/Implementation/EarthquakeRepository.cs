using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DataAccessEF.EntityModels;
using DataAccessEF.Factories;
using LinqKit;
using LogUtility;
using TestApp;

namespace DataAccessEF.Implementation
{
    public class EarthquakeRepository : GenericRepository<Earthquake>, IEarthquakeRepository
    {
        public EarthquakeRepository(IAplicationContextFactory aplicationContextFactory)
                                                                        : base(aplicationContextFactory)
        {   
            
        }

       
        public IEnumerable<Earthquake> Search(EarthquakeModel model)
        {
            try
            {
                var predicate = PredicateBuilder.True<Earthquake>();

                if(model.LocationModel.IsByCountry){
                    predicate = GetLocationPredicate(predicate, model);
                    predicate = GetMagnitudePredicate(predicate, model);
                    predicate = GetDateTimePredicate(predicate, model);
                    return this.DbContext.Set<Earthquake>()
                                         .Include(e => e.Place)
                                         .Include(e => e.MagnitudeType)
                                         .Include(e => e.Source)                                         
                                         .AsExpandable().Where(predicate);
                }
              
                predicate = GetMagnitudePredicate(predicate, model);
                predicate = GetDateTimePredicate(predicate, model);

                return model.LocationModel.NearEarthquakes
                                          .AsQueryable().Where(predicate).ToList();
               
                
                
            }
            catch(Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return new List<Earthquake>();
            }
        }

        private Expression<Func<Earthquake, bool>> GetDateTimePredicate(Expression<Func<Earthquake, bool>> predicate, EarthquakeModel model)
        {
            try
            {
                predicate = predicate.And(e => e.UTCDateTime >= model.DateTime);
                return predicate;
            }
            catch (Exception ex)
            {
                ExceptionUtility.Warn(ex, this.GetType());
                return predicate;
            }
        }

        private Expression<Func<Earthquake, bool>> GetMagnitudePredicate(Expression<Func<Earthquake, bool>> predicate, EarthquakeModel model)
        {
            try
            {
                if (model.Magnitude == "Sensible")
                {
                    predicate = predicate.And(e => e.IsSensible == true);
                }

                else
                {
                    decimal magnitude = Convert.ToDecimal(model.Magnitude, CultureInfo.InvariantCulture);
                    predicate = predicate.And(e => e.Magnitude >= magnitude);
                }

                return predicate;
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return predicate;
            }
        }

        private  Expression<Func<Earthquake, bool>> GetLocationPredicate(Expression<Func<Earthquake, bool>> predicate , EarthquakeModel model)
        {
            try
            {
                /*decimal latitude = Convert.ToDecimal(model.LocationModel.Latitude);
                decimal longitude = Convert.ToDecimal(model.LocationModel.Longitude);


                    bool isfirstCountryParameter = true;
                    for (int i = 0; i < model.LocationModel.Country.Length; i++)
                    {
                        string country = model.LocationModel.Country[i].ToString();

                        if (country != CountryEnum.EMPTY.ToString())
                        {
                            if (isfirstCountryParameter)
                            {
                                //que de chile solo se muestre los sensibles
                                if (country == CountryEnum.CHILE.ToString())
                                {
                                    predicate = predicate.And(e => e.Place.Country.Contains(country) && e.IsSensible == true);
                                }
                                else if (country == CountryEnum.WORLD.ToString())
                                {
                                    predicate = predicate.And(e => e.Place.Country != string.Empty);
                                }
                                else
                                {
                                    predicate = predicate.And(e => e.Place.Country.Contains(country));
                                }
                                isfirstCountryParameter = false;
                            }
                            else
                            {
                                //que de chile solo se muestre los sensibles
                                if (country == CountryEnum.CHILE.ToString())
                                {
                                    predicate = predicate.Or(e => e.Place.Country.Contains(country) && e.IsSensible == true);
                                }
                                else if (country == CountryEnum.WORLD.ToString())
                                {
                                    predicate = predicate.Or(e => e.Place.Country != string.Empty);
                                }
                                else
                                {
                                    predicate = predicate.Or(e => e.Place.Country.Contains(country));
                                }
                            }
                        }
                    }*/             
           
                return predicate;
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return predicate;
            }
        }


        public List<Guid> GetRadiusLocation(SqlParameter StartingLatitude, SqlParameter StartingLongitude, SqlParameter MaxDistance)
        {
            return DbContext.Database.SqlQuery<Guid>
            (" SELECT Id FROM Earthquake WHERE dbo.GetDistanceBetween(@latitude, @longitude, latitude, longitude) <= @radius",
                                                                                                          StartingLatitude, StartingLongitude, MaxDistance).ToList();
        }


        public Earthquake GetEarthquakeById(Guid id)
        {
           return DbContext.Earthquakes
                           .Include(e => e.Place)
                           .Include(e => e.MagnitudeType)
                           .Include(e => e.Source)  
                           .Where(e => e.Id == id).FirstOrDefault();
        }


        public IList<Source> GetAllSources()
        {
            return DbContext.Sources.ToList();
        }


        public IEnumerable<Earthquake> GetSensibleNotNotifiedEarthquakes()
        {
                         
            return DbContext.Earthquakes
                           .Include(e => e.Place)
                           .Include(e => e.MagnitudeType)
                           .Include(e => e.Source)
                           .Where(e => e.IsSensible && e.WasNotified == false).ToList();
        }


        public IEnumerable<Tweet> GetTweets(DateTime datetTime)
        {
            try
            {
                return this.DbContext.Tweets.Where(t => t.DateTime >= datetTime);
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return new List<Tweet>();
            }
        }


        public void DeleteTweet(Tweet tweet)
        {
            this.DbContext.Set<Tweet>().Remove(tweet);
        }

        public void DeleteDevice(RegistrationDevice device) {

            this.DbContext.Set<RegistrationDevice>().Remove(device);
        }
    }
}
