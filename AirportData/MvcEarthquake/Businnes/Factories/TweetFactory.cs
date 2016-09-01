using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp;

namespace MvcEarthquake.Businnes.Factories
{
    public class TweetFactory : ITwitterFactory
    {
        public Tweet Create(string userName,string screenName,string tweet,DateTime dateTime) {
            return new Tweet(Guid.NewGuid(), dateTime, tweet, userName, screenName);
        }

    }
}