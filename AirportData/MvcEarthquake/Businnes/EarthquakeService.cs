using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DataAccessEF.EntityModels;
using DataAccessEF.Factories;
using DataAccessEF.Implementation;
using DataAccessEF.Utility;
using GlobalAppData;
using LogUtility;
using MvcEarthquake.GeoJsonData;
using MvcEarthquake.Utils;
using Newtonsoft.Json;
using TestApp;


namespace MvcEarthquake.Businnes
{
    public class EarthquakeService : IEarthquakeService
    {
        IEarthquakeRepository earthquakeRepository;
        IRegistrationManager registrationManager;

        public EarthquakeService(){
         
            earthquakeRepository = new EarthquakeRepository(new AplicationContextFactory());
            registrationManager = new RegistrationManager(earthquakeRepository);
        }

        public string FromObjectToGeoJSON(IEnumerable<Earthquake> earthquakes)
        {
            try
            {
                EarthquakeFeatureCollection earthquakeFeatureCollection = new EarthquakeFeatureCollection();
                earthquakeFeatureCollection.Features = new EarthquakeFeature[earthquakes.Count()];
                int i = 0;
                foreach (Earthquake earthquake in earthquakes)
                {
                    var latitude = Convert.ToDouble(earthquake.Latitude);
                    var longitude = Convert.ToDouble(earthquake.Longitude);
                    EarthquakeFeature earthquakeFeature = new EarthquakeFeature();
                 
                    earthquakeFeature.Geometry = new GeoJSON.Point(latitude, longitude);
                    EarthquakeProperties properties = new EarthquakeProperties();
                    properties.Country = earthquake.Place.Country;
                    properties.Depth = earthquake.Depth.ToString();
                    properties.IsSensible = earthquake.IsSensible.ToString();
                    properties.Magnitude = earthquake.Magnitude.ToString();
                    properties.MagnitudeType = earthquake.MagnitudeType.Type;
                    properties.PlaceName = earthquake.Place.PlaceName.Replace("'","");
                    properties.SourceDirection = earthquake.Source.SourceDirection;
                    properties.SourceName = earthquake.Source.SourceName;
                    properties.UTCDateTime = earthquake.UTCDateTime.ToString("dd/MM/yyyy HH:mm:ss");
                    earthquakeFeature.Properties = properties;

                    earthquakeFeatureCollection.Features[i] = earthquakeFeature;
                    i++;
                }

                StringBuilder Builder = new System.Text.StringBuilder();
                StringWriter Writer = new System.IO.StringWriter(Builder);

                new JsonSerializer().Serialize(Writer, earthquakeFeatureCollection);

                return Builder.ToString();

            }
            catch (Exception ex)
            {
                ExceptionUtility.Warn(ex, this.GetType());
                return ExceptionUtility.BadRequestMessage;
            }
        
        }

        public string FromObjectToGeoJSON(IEnumerable<Source> sources)
        {
            try
            {
                SourceFeatureCollection sourceFeatureCollection = new SourceFeatureCollection();
                sourceFeatureCollection.Features = new SourceFeature[sources.Count()];
              
                int i = 0;
                foreach(Source source in sources){

                    SourceFeature sourceFeature = new SourceFeature();
                    SourceProperties properties = new SourceProperties();
                    properties.SourceDirection = source.SourceDirection;
                    properties.SourceName = source.SourceName;
                    properties.IsOnLine = source.IsOnline.ToString();
                    sourceFeature.Properties = properties;
                    sourceFeatureCollection.Features[i] = sourceFeature;
                    i++;

                }
                StringBuilder Builder = new System.Text.StringBuilder();
                StringWriter Writer = new System.IO.StringWriter(Builder);

                new JsonSerializer().Serialize(Writer, sourceFeatureCollection);

                return Builder.ToString();

            }
            catch (Exception ex)
            {
                ExceptionUtility.Warn(ex, this.GetType());
                return ExceptionUtility.BadRequestMessage;
            }

        }

        public string FromObjectToGeoJSON(IEnumerable<Tweet> tweets)
        {
            try
            {
                TweetFeatureCollection tweetFeatureCollection = new TweetFeatureCollection();
                tweetFeatureCollection.Features = new TweetFeature[tweets.Count()];

                int i = 0;
                foreach (Tweet tweet in tweets)
                {

                    TweetFeature tweetFeature = new TweetFeature();
                    TweetProperties properties = new TweetProperties();
                    properties.DateTime = tweet.DateTime.ToString("dd/MM/yyyy HH:mm:ss"); 
                    properties.UserName = tweet.UserName;
                    properties.UserScreenName = tweet.UserScreenName;
                    properties.Tweeter = tweet.Tweeter;                   
                    tweetFeature.Properties = properties;
                    tweetFeatureCollection.Features[i] = tweetFeature;
                    i++;

                }
                StringBuilder Builder = new System.Text.StringBuilder();
                StringWriter Writer = new System.IO.StringWriter(Builder);

                new JsonSerializer().Serialize(Writer, tweetFeatureCollection);

                return Builder.ToString();

            }
            catch (Exception ex)
            {
                ExceptionUtility.Warn(ex, this.GetType());
                return ExceptionUtility.BadRequestMessage;
            }

        }


        public string GetAll()
        {
            try
            {
                IList<Earthquake> earthquakes = earthquakeRepository.FindAll().ToList();
                IList<Source> sources = earthquakeRepository.GetAllSources();

                
                if (earthquakes.Count > 0)
                {
                    return FromObjectToGeoJSON(earthquakes);
                }
                else {
                    return ExceptionUtility.NoElementsMessage;
                }

            }

            catch (Exception ex)
            {
                ExceptionUtility.Warn(ex, this.GetType());
                return ExceptionUtility.BadRequestMessage;
            }
        }


        public object GetData(string location, string latitude, string longitude, string magnitude, string date)
        {
            string data = string.Empty;
            try
            {               
                EarthquakeModel model = new EarthquakeModel();
                LocationModel locationModel = GetLocationModel(location,latitude,longitude);
                
                model.LocationModel = locationModel;
                model.Magnitude = GetMagnitude(magnitude);
                model.DateTime = GetDate(date);
                
                var earthquakes = earthquakeRepository.Search(model);
                if (earthquakes.Count()> 0)
                {
                    data =  FromObjectToGeoJSON(earthquakes);
                }
                else
                {
                   data = ExceptionUtility.NoElementsMessage;
                }

                object obj = new object();
                obj = data;
                return obj;
            }

            catch (Exception ex)
            {
                ExceptionUtility.Warn(ex, this.GetType());
                data = ExceptionUtility.BadRequestMessage;
                //data[1] = ExceptionUtility.BadRequestMessage;
                return data;
            }
        }

        private string GetMagnitude(string magnitude)
        {
            String realMagnitude="2";

            switch (magnitude)
            {

                 //2.5+
                case ("1"):
                    realMagnitude = "2.5";
                    break;
                //3.0+
                case ("2"):
                    realMagnitude = "3.0";
                    break;
                //4.0+
                case ("3"):
                    realMagnitude = "4.0";
                    break;
                //4.5+
                case ("4"):
                    realMagnitude = "4.5";
                    break;
                //5+
                case ("5"):
                    realMagnitude = "5.0";
                    break;
                //5+
                case ("6"):
                    realMagnitude = "5.5";
                    break;
                //5+
                case ("7"):
                    realMagnitude = "Sensible";
                    break;

            }

            return realMagnitude;
        }

        private DateTime GetDate(string date)
        {

            DateTime dateTime = DateTime.UtcNow;
            var todayData = date.Split('-');
            if (todayData.Length == 4) { 
               date = todayData[0];
            }

            switch (date)
            {       
                
                    //TODAY
                case ("1"):
                    int year = int.Parse(todayData[3]);
                    int month = int.Parse(todayData[2]);
                    int day = int.Parse(todayData[1]);
                    dateTime = new DateTime(year, month, day, 0, 0, 0).AddHours(3);
                    break;
                    //LAST 24 HS.
                case ("2"):                     
                    dateTime = new DateTime(GlobalWebData.ToUniversalTime().Ticks).Add(System.TimeSpan.FromHours(System.Convert.ToDouble((24 * -1))));                    
                    break;
                    //LAST 48 HS.
                case ("3"):                    
                    dateTime = new DateTime(GlobalWebData.ToUniversalTime().Ticks).Add(System.TimeSpan.FromHours(System.Convert.ToDouble((48 * -1))));                   
                    break;
                    //LAST WEEK
                case ("4"):              
                    dateTime = new DateTime(GlobalWebData.ToUniversalTime().Ticks).Add(System.TimeSpan.FromHours(System.Convert.ToDouble((168 * -1))));                   
                    break;
                //LAST TWO WEEKS
                case ("5"):                   
                    dateTime = new DateTime(GlobalWebData.ToUniversalTime().Ticks).Add(System.TimeSpan.FromHours(System.Convert.ToDouble((336 * -1))));                   
                    break;
                
            }

            return dateTime;
        }

        private LocationModel GetLocationModel(string location, string latitude, string longitude)
        {
            LocationModel locationModel = new LocationModel();
            
            switch(location){
                    //250km
                case("1"):
                    locationModel = GetRadiusLocation(latitude, longitude, "250");
                    locationModel.IsByCountry = false;
                    break;
                    //500km
                case("2"):
                    locationModel = GetRadiusLocation(latitude, longitude, "500");
                     locationModel.IsByCountry = false;
                    break;
                    //1000km
                case("3"):
                    locationModel = GetRadiusLocation(latitude, longitude, "1000");
                     locationModel.IsByCountry = false;
                    break;
                    //by country
                case("4"):
                    locationModel = GetLocationByCountry(latitude);
                    locationModel.IsByCountry = true;
                    break;
            
            }

            return locationModel;
        }

       

        private LocationModel GetLocationByCountry(string latitude)
        {
            LocationModel locationModel = new LocationModel();

            switch(latitude){
                    //ARGENTINA-CHILE
                case("0"):
                    locationModel.Country[0] = CountryEnum.ARGENTINA;
                    locationModel.Country[1] = CountryEnum.CHILE;                    
                    break;
                    //ARGENTINA
                case("1"):
                    locationModel.Country[0] = CountryEnum.ARGENTINA;                    
                    break; 
                    //CHILE
                case("2"):                   
                    locationModel.Country[1] = CountryEnum.CHILE;                    
                    break;
                    //WORLD
                case("3"):                    
                    locationModel.Country[2] = CountryEnum.WORLD;
                    break;                
            }
            return locationModel;
        }

        private LocationModel GetRadiusLocation(string latitude, string longitude, string radius)
        {
            try
            {
                LocationModel locationModel = new LocationModel();

                double radiusTemp = Convert.ToDouble(radius);
                var radiusMiles = ConvertDistance.ConvertKilometersToMiles(radiusTemp);


                SqlParameter StartingLatitude = new SqlParameter("@latitude", latitude);
                SqlParameter StartingLongitude = new SqlParameter("@longitude", longitude);
                SqlParameter MaxDistance = new SqlParameter("@radius", radiusMiles);

                var earthquakesGuids = earthquakeRepository.GetRadiusLocation(StartingLatitude, StartingLongitude, MaxDistance);
                IList<Earthquake> nearEarthquakes = new List<Earthquake>();

                foreach (Guid id in earthquakesGuids)
                {
                    var ea = earthquakeRepository.GetEarthquakeById(id); 
                    if (ea != null)
                    {
                        nearEarthquakes.Add(ea);
                    }
                }
                locationModel.NearEarthquakes = nearEarthquakes;

                return locationModel;
            }

            catch (Exception ex)
            {
                ExceptionUtility.Warn(ex, this.GetType());
                return new LocationModel();
            }
        }


        public object GetTweets(string days)
        {
             string data = string.Empty;
             try
             {
                 DateTime datetTime = GetDate(days);
                 var tweets = earthquakeRepository.GetTweets(datetTime);

                 if (tweets.Count() > 0)
                 {
                     data = FromObjectToGeoJSON(tweets);
                 }
                 else
                 {
                     data = ExceptionUtility.NoElementsMessage;
                 }

                 object obj = new object();
                 obj = data;
                 return obj;
             }
             catch (Exception ex)
             {
                 ExceptionUtility.Warn(ex, this.GetType());
                 return ExceptionUtility.BadRequestMessage;
             }
        }


        public bool RegisterIfNotExist(string registerId,String deviceId)
        {
             try{
                 return registrationManager.RegisterIfNotExist(registerId, deviceId);             
             }

            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return false;
            }
        }




        public bool Unregister(string deviceId)
        {
            try
            {
                return registrationManager.Unregister(deviceId);
            }

            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return false;
            }
        }


        public void ResetMailCount()
        {
            try
            {
                MailManagment.MailManagement.ResetCount();
            }

            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());                
            }
        }


        public void Log(string log, string deviceId)
        {
            try
            {
                DeviceLogManager deviceLogManager = new DeviceLogManager(earthquakeRepository);
                deviceLogManager.SaveLog(log,deviceId);
            }

            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
            }
        }
    }

}