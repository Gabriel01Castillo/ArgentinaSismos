using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DataAccessEF.Factories;
using DataAccessEF.Implementation;
using GlobalAppData;
using LogUtility;
using MvcEarthquake.Businnes.Factories;
using MvcEarthquake.Utils;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public class EarthquakeExpert : IEarthquakeExpert {
        public IDictionary<string, EarthquakeCollector> Collectors { get; set; }
        public IEarthquakeRepository Repository { get; set; }
        public IAplicationContextFactory  AplicationContextFactory {get;set;}
        public IMylogRepository MylogRepository { get; set; }
        public ITwitterCollector TwitterCollector { get; set; }
        public EarthquakeExpert(IDictionary<string, EarthquakeCollector> collectors, IEarthquakeRepository repository,
                                                                                        IAplicationContextFactory aplicationContextFactory,
                                                                                                 IMylogRepository logRepository, ITwitterCollector twitterCollector)
        {
            Repository = repository;
            Collectors = collectors;
            AplicationContextFactory = aplicationContextFactory;
            MylogRepository = logRepository;
            TwitterCollector = twitterCollector;
        }
       

        public void CollectData()
        {
            try
            {    
                ResetContext();
                
                CollectData("EMSCEarthquakeCollectorXML");
                
                ResetContext();

                CollectData("InpresEarthquakeCollectorHtml");
               
                ResetContext();

                CollectData("SSUCHEarthquakeCollectorHtml");
             
            }

            catch (Exception ex){

                ExceptionUtility.Error(ex, this.GetType());
            }
        }

        public  void CollectTwitterData()
        {
            try
            {
                List<Tweet> tweets = TwitterCollector.GetTwitters();

                foreach (Tweet tweet in tweets)
                {

                    var existing = Repository.DbContext.Tweets.Where(t => t.Tweeter.Contains(tweet.Tweeter)).ToList();

                    if (existing.Count <= 0) {

                        Repository.DbContext.Tweets.Add(tweet);
                    }
                    if (existing.Count >= 2) {

                        var duplicatesTweets =  existing.OrderByDescending(t => t.DateTime);
                        var deleteThis = duplicatesTweets.Skip(1);
                        foreach (Tweet tw in deleteThis)
                        {
                            Repository.DbContext.Tweets.Remove(tw);
                        }
                    }                   
                }
                Repository.Save();
                
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
            }
        }      

        private void ResetContext()
        {
            Repository.DbContext.Dispose();          
            Repository.DbContext = AplicationContextFactory.GetApplicationContext();
        }

       
        private void CollectData(string colectorName)
        {
            try
            {
                EarthquakeCollector collector = null;
                Collectors.TryGetValue(colectorName, out collector);
                var earthquakeFromColector = FilterDuplicate (collector.Collect());                
                
                foreach (Earthquake earthquake in earthquakeFromColector)
                {
                  
                    if (!EarthquakeExist(earthquake))
                    {
                       
                        if (earthquake.MagnitudeType != null && earthquake.MagnitudeType.Id != Guid.Empty)
                        {
                            earthquake.MagnitudeType = Repository.DbContext.MagtitudesTypes.Where(p => p.Id == earthquake.MagnitudeType.Id).FirstOrDefault();                         
                        }

                        if (earthquake.Source != null && earthquake.Source.Id != Guid.Empty)
                        {
                            earthquake.Source = Repository.DbContext.Sources.Where(p => p.Id == earthquake.Source.Id).FirstOrDefault();                           
                        }

                        if (earthquake.Place != null && earthquake.Place.Id != Guid.Empty)
                        {                          
                            earthquake.Place = Repository.DbContext.Places.Where(p => p.Id == earthquake.Place.Id).FirstOrDefault();                          
                        }                      

                        Repository.DbContext.Earthquakes.Add(earthquake);
                    }
                  
                }               
             
                Repository.Save();                
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbevex)
            {
                ExceptionUtility.Error(dbevex, this.GetType());                
            }

            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
            }
        }

        private IList<Earthquake> FilterDuplicate(IList<Earthquake> list)
        {
            IList<Earthquake> nonDuplicates = new List<Earthquake>();
            foreach(var earthquake in list){

                DateTime startDate = earthquake.UTCDateTime.AddSeconds(-60);
                DateTime endDate = earthquake.UTCDateTime.AddSeconds(60);
                decimal startLat = earthquake.Latitude + new Decimal(1);
                decimal endLat = earthquake.Latitude + new Decimal(-1);
                decimal startLong = earthquake.Longitude + new Decimal(1);
                decimal endLong = earthquake.Longitude + new Decimal(-1);
                var query = from all in nonDuplicates
                            where (all.UTCDateTime >= startDate && all.UTCDateTime <= endDate) &&
                            (all.Latitude <= startLat && all.Latitude >= endLat) &&
                            (all.Longitude <= startLong && all.Longitude >= endLong)
                            select all;

                if (query.Count() <= 0) {
                    nonDuplicates.Add(earthquake);
                }
            }
          
           return nonDuplicates;            
        }
        
        bool EarthquakeExist(Earthquake earthquake)
        {
         
                    DateTime startDate = earthquake.UTCDateTime.AddSeconds(-60);
                    DateTime endDate = earthquake.UTCDateTime.AddSeconds(60);
                    decimal startLat = earthquake.Latitude + new Decimal (1);
                    decimal endLat = earthquake.Latitude + new Decimal(-1);
                    decimal startLong = earthquake.Longitude + new Decimal (1);
                    decimal endLong = earthquake.Longitude + new Decimal(-1);

                    var query = from all in Repository.DbContext.Earthquakes
                                where (all.UTCDateTime >= startDate && all.UTCDateTime <= endDate) &&                               
                                (all.Latitude <= startLat && all.Latitude >= endLat) &&
                                (all.Longitude <= startLong && all.Longitude >= endLong) 
                                select all;

                    var duplicateCounts = query.Count();                         
                    return duplicateCounts > 0;
        }

        public void DeleteUnusedLogs()
        {
          
            try
            {
                var logs = Repository.DbContext.MyLogs.Where(l => l.Id > 0).ToList();
                var utcNow = GlobalWebData.ToUniversalTime();
                var utcPast24 = new DateTime(GlobalWebData.ToUniversalTime().Ticks).Add(System.TimeSpan.FromHours(System.Convert.ToDouble((24 * -1))));
                var log24 = logs.Where(l => l.Date >= utcPast24 && l.Date <= utcNow).ToList();
                var logBefore24 = logs.Except(log24);
                var logsABorrrar = logBefore24.ToList();

                if(logsABorrrar.Count > 0){
                    foreach(MyLogs log in logsABorrrar ){
                        MylogRepository.Delete(log);                       
                    }
                    MylogRepository.DetectChanges();
                    MylogRepository.Save();
                    ExceptionUtility.Info("Se borraron antiguos logs...");
                }
            }
            catch(Exception ex){
              ExceptionUtility.Error(ex, this.GetType());
            }
           
        }

        public void DeleteEarthquakesBeforeTwoWeeks()
        {
            try
            {
                    var all = Repository.FindAll().ToList();
                    var before = GetEarthquakesBeforeTwoWeeks(all);

                    if (before.Count > 0)
                    {
                        foreach (Earthquake earthq in before)
                        {
                            Repository.Delete(earthq);                            
                        }

                        Repository.Save();
                    }

                    ExceptionUtility.Info("Se borraron antiguos sismos...");
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
            }

        }

        public IList<Earthquake> GetEarthquakesBeforeTwoWeeks(IList<Earthquake> allEarthquakes)
        {
            try
            {
                var utcNow = GlobalWebData.ToUniversalTime();
                var utcPastSevenDays = new DateTime(GlobalWebData.ToUniversalTime().Ticks).Add(System.TimeSpan.FromHours(System.Convert.ToDouble((336 * -1))));
                var pastSevenDays = allEarthquakes.Where(e => e.UTCDateTime >= utcPastSevenDays).ToList();
                var earthBeforeSevenDays = allEarthquakes.Except(pastSevenDays);
                return earthBeforeSevenDays.ToList();
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return new List<Earthquake>();
            }
        }

        public void DeleteDuplicateEarthQuakes()
        {
            try
            {
                var all = Repository.FindAll().ToList();
                var toDelete = GetDuplicateEarthQuakes(all);

                foreach (Earthquake earthq in toDelete)
                {
                    Repository.Delete(earthq);
                    ExceptionUtility.Warn("Borrando duplicado..." + earthq);
                }
                Repository.Save();
            }

            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
            }
        }

        public IList<Earthquake> GetDuplicateEarthQuakes(List<Earthquake> allEarthquakes)
        {
            try
            {
                IList<Earthquake> toDeleteEarthquakes = new List<Earthquake>();

                foreach (Earthquake earthquake in allEarthquakes)
                {

                    DateTime starDate = earthquake.UTCDateTime.AddSeconds(-5);
                    DateTime endDate = earthquake.UTCDateTime.AddSeconds(5);

                    var query = from all in allEarthquakes
                                where (all.UTCDateTime >= starDate && all.UTCDateTime <= endDate) &&
                                all.Place.PlaceName.Equals(earthquake.Place.PlaceName) &&
                                all.Source.SourceName.Equals(earthquake.Source.SourceName)
                                select all;

                    var duplicateCounts = query.Count();

                    if (duplicateCounts > 1)
                    {

                        var duplicatesEarthqakes = query.ToList().OrderByDescending(e => e.PublicatedDatTime);
                        var deleteThis = duplicatesEarthqakes.Skip(1);
                        foreach (Earthquake ea in deleteThis)
                        {
                            toDeleteEarthquakes.Add(ea);
                        }
                    }
                }

                return toDeleteEarthquakes;
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return new List<Earthquake>();
            }
        }

        public IList<Earthquake> GetLast24HoursEarthquakes(IList<Earthquake> allEarthquakes)
        {
            try
            {
                var utcNow = GlobalWebData.ToUniversalTime();
                var utcPast24 = new DateTime(GlobalWebData.ToUniversalTime().Ticks).Add(TimeSpan.FromHours(System.Convert.ToDouble((24 * -1))));
                return allEarthquakes.Where(e => e.UTCDateTime >= utcPast24 && e.UTCDateTime <= utcNow).ToList();
            }

            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return new List<Earthquake>();
            }
        }

        public IList<Tweet> GetEarliesTweets(IList<Tweet> allTweets)
        {
            try
            {
                var utcNow = GlobalWebData.ToUniversalTime();
                var utcMonthPast = new DateTime(GlobalWebData.ToUniversalTime().Ticks).Add(TimeSpan.FromHours(System.Convert.ToDouble((720 * -1))));
                return allTweets.Where(t => t.DateTime < utcMonthPast).ToList();
            }

            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return new List<Tweet>();
            }
        }

        public void NotifySensibleEarthquakes()
        {
            try
            {
                var sensibleNotNotifiedEarthquakes = Repository.GetSensibleNotNotifiedEarthquakes();

                if (sensibleNotNotifiedEarthquakes.Count() > 0)
                {      

                    foreach (Earthquake earthquake in sensibleNotNotifiedEarthquakes)
                    {
                        SendSensibleEarthquakeNotification(earthquake);
                        earthquake.WasNotified = true;
                        Repository.DbContext.Entry(earthquake).State = EntityState.Modified;
                        Repository.DbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());                
            }
        }

        private void SendSensibleEarthquakeNotification(Earthquake earthquake)
        {
            try
            {

                PushNotification.PushNotification pn = new PushNotification.PushNotification();
                var devices = Repository.DbContext.RegistrationDevices.OrderBy(e => e.Date).ToList();
               
                //--------------------------------------------------------------------------------------

                int elementsByList = 20000;
                int numberOfLists = (devices.Count / elementsByList) + 1;
                List<List<RegistrationDevice>> listOfLists = new List<List<RegistrationDevice>>();


                for (int i = 0; i < numberOfLists; i++)
                {
                    List<RegistrationDevice> newList = new List<RegistrationDevice>();
                    newList = devices.Skip(i * elementsByList).Take(elementsByList).ToList();
                    listOfLists.Add(newList);

                    Task task4 = new Task(() =>
                    {
                        NotificateMultipleDevices(newList, earthquake);
                    }) ;

                    task4.Start();
                }
                //--------------------------------------------------------------------------------------

            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
            }
       }

        private void NotificateMultipleDevices(List<RegistrationDevice> devices, Earthquake earthquake)
        {

            PushNotification.PushNotification pn = new PushNotification.PushNotification();

            foreach (var device in devices)
            {
                //------------------------Send Notification----------------------------                    
                var ok = pn.Android(device.RegistrationId, earthquake.GetNotificationData());
                //--------------------------------------------------------------------

                if (ok.Contains("success\":0"))
                {
                    ExceptionUtility.Error(new Exception(device.RegistrationId), this.GetType());
                    Repository.DeleteDevice(device);
                    Repository.Save();
                }
            }

            ExceptionUtility.Info(string.Concat("send notification to GCM ", GlobalWebData.ToUniversalTime()));
        }


        public void DeleteEarliesTweeters()
        {
            try
            {
                var alltweets = GetAllTweets();
                var toDeleteTweets = GetEarliesTweets(alltweets);

                if (toDeleteTweets.Count > 0)
                {
                    foreach (Tweet tweet in toDeleteTweets)
                    {
                        Repository.DeleteTweet(tweet);
                    }

                   Repository.Save();
                   ExceptionUtility.Info("Se borraron antiguos tweets ...");
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
            }
        }

        private IList<Tweet> GetAllTweets()
        {
            return Repository.DbContext.Tweets.ToList();
        }
    }
       
}