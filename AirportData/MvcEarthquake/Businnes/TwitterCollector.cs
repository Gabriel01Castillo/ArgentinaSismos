using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml;
using GlobalAppData;
using MvcEarthquake.Businnes.Factories;
using MvcEarthquake.Utils;
using Newtonsoft.Json.Linq;
using TestApp;
using XMLManagment;

namespace MvcEarthquake.Businnes
{
    public class TwitterCollector : ITwitterCollector
    {
        private ITwitterFactory twitterFactory;
        private XMLManager xmlManager;

        public TwitterCollector() {

            twitterFactory = new TweetFactory();
            xmlManager = new XMLManager();
        }

        public List<Tweet> GetTwitters()
        {
            List<Tweet> twits = new List<Tweet>();
            var twitter = new Twitter("xNBdzXy7tkyD1kstIDcg", "0nSriIwDZkFR7qcf6nsnQh3bqecn7IGoTqpIr8Xk6gc",
                        "1451422718-kwIAaVppRGys6dltQXKIUt39vNbIg5e4PFD1Rgu",
                        "1LPbBbwYcAELk3dl50WIwZMp38gt2wWGwkOkzVQGQM");

            var responseTwitter = twitter.GetTweets(MyConfigurationManager.GetTweeterQuery());

            JObject jobjectTwitter1 = JObject.Parse(responseTwitter);

            var resultado12 = jobjectTwitter1["statuses"];

            var twittsList2 = resultado12.ToList();

            foreach (JToken twitt in twittsList2)
            {
             
             var text = twitt["text"].ToString();
             var textUper = text.ToUpper();
             var uperText = text.ToString().ToUpper();
             

             bool containsForbiddenWords = ContainsForbiddenWords(textUper);
             bool containsKeyWords = ContainsKeyWords(uperText);

             if (!containsForbiddenWords && containsKeyWords)
             {
                 var user = twitt["user"];
                 var screenName = user["screen_name"].ToString();
                 var name = user["name"].ToString();
                 var date = twitt["created_at"].ToString().Replace("\\", "").Replace("\"", "");
                 const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
                 var realtime = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
                 var utcTime = GlobalWebData.ToUniversalTime(realtime);//.Subtract(System.TimeSpan.FromHours(System.Convert.ToDouble(2)));
                 twits.Add(twitterFactory.Create(name, screenName, text, utcTime));             
             }

            }


            return twits;
        }

        private bool ContainsKeyWords(string uperText)
        {
            var keyWords = xmlManager.GetKeyWords();


            foreach (var word in keyWords)
            {
                if (!uperText.Contains(word))
                {
                    return false;
                }

            }
           
            return true;
        }

        private bool ContainsForbiddenWords(string textUper)
        {            
            var forbiddenTwitts = xmlManager.GetTweeterForbiddenWords();

            foreach (var word in forbiddenTwitts)
            {
                if (textUper.Contains(word)) {
                    return true;
                }

            }
            return false;
        }
    }
}