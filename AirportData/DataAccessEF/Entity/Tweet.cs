using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class Tweet
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Tweeter { get; set; }
        public string UserName { get; set; }
        public string UserScreenName { get; set; }

        public Tweet() { } 

        public Tweet(Guid id ,DateTime dateTime ,string tweeter , string userName , string userScreenName ) {
          Id = id;
          DateTime = dateTime;
          Tweeter = ParseText(tweeter);
          UserName = userName;
          UserScreenName = userScreenName;
        }

        private String ParseText(string tweeter)
        {
            return tweeter.Replace("\"", "").Replace("\\","");
        }
    }
}
