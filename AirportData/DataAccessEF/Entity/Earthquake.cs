using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessEF;
using MvcEarthquake.Utils;

namespace TestApp
{
    public class Earthquake : Entity
    {
           public DateTime UTCDateTime { get; set; }           
           public decimal Latitude { get; set; }
           public decimal Longitude { get; set; }
           public decimal Depth { get; set; }
           public decimal Magnitude { get; set; }
           public bool IsSensible {get; set;}
           public virtual Place Place {get; set;}
           public virtual Source Source { get; set; }
           public virtual MagnitudeType MagnitudeType { get; set; }
           public virtual bool WasNotified { get; set; }

           public DateTime PublicatedDatTime { get; set; }
            
           
           public Earthquake() { }
        
           public Earthquake(Guid id,DateTime utcDateTime,decimal latitude,decimal longitude,
                                                    decimal depth, decimal magnitude, bool isSensible, Place place, Source source,
                                                                                                     MagnitudeType magnitudeType, DateTime publicatedDatTime)
            {
                base.Id = id;
                UTCDateTime = utcDateTime;
                Latitude = Math.Round(latitude, 10);
                Longitude = Math.Round(longitude, 10);
                Depth = Math.Round(depth, 10);
                Magnitude = Math.Round(magnitude, 10); 
                IsSensible= isSensible;
                Place = place;
                Source = source;
                MagnitudeType = magnitudeType;
                PublicatedDatTime = publicatedDatTime;
           }

          
           public override string ToString()
           {
               return string.Concat(UTCDateTime," ",Latitude," ",Longitude," ",Depth," ",Magnitude," ",Place," ",Source);
           }


           //-------------------------------------coordinate managment--------------------------------------------
           public static decimal GetDecimal(decimal degrees, decimal minutes, decimal seconds)
           {
               UraniaDMS dddd = new UraniaDMS();
               dddd.setDegree(Convert.ToInt64(degrees));
               dddd.setMinutes(Convert.ToInt64(minutes));
               dddd.setSeconds(Convert.ToInt64(seconds));
               dddd.setSign(Convert.ToInt64(degrees));
               return Convert.ToDecimal(dddd.getDecFormat());

           }

           public static decimal[] ToDMS(decimal decimalValue)
           {
               decimal[] DegreesMinutesSeconds = new decimal[3];
               DegreesMinutesSeconds[0] = Convert.ToInt32(Math.Truncate(decimalValue));
               DegreesMinutesSeconds[1] = Convert.ToInt32(Math.Truncate((decimalValue - DegreesMinutesSeconds[0]) * 60));
               DegreesMinutesSeconds[2] = (((decimalValue - DegreesMinutesSeconds[0]) * 60) - DegreesMinutesSeconds[1]) * 60;
               DegreesMinutesSeconds[0] = DegreesMinutesSeconds[0];
               DegreesMinutesSeconds[1] = Math.Abs(DegreesMinutesSeconds[1]);
               DegreesMinutesSeconds[2] = Math.Abs(DegreesMinutesSeconds[2]);
               return DegreesMinutesSeconds;
           }

           public string GetDegreesMinutesSecondsString(decimal decimalValue)
           {
               decimal[] DegreesMinutesSeconds = Earthquake.ToDMS(decimalValue);
               StringBuilder sb = new StringBuilder();
               sb.Append(DegreesMinutesSeconds[0]).Append("° ").Append(DegreesMinutesSeconds[1]).Append("' ").Append(DegreesMinutesSeconds[2]).Append("\"");
               return sb.ToString().Replace(",", ".");
           }

        //-------------------------------------coordinate managment--------------------------------------------

           public string GetNotificationData()
           {
               StringBuilder sb = new StringBuilder();

               sb.Append(Latitude).Append(";");//0
               sb.Append(Longitude).Append(";");//1
               sb.Append(UTCDateTime.ToString("dd/MM/yyyy HH:mm:ss")).Append(";");//2
               sb.Append(Depth).Append(";");//3
               sb.Append(Magnitude).Append(";");//4
               sb.Append(Place.PlaceName).Append(";");//5
               sb.Append(Place.Country).Append(";");//6               
               sb.Append(MagnitudeType.Type).Append(";");//7
               sb.Append(Source.SourceName);//8

               return sb.ToString();
           }

          /* public override bool Equals(Object obj)
           {
               // Check for null values and compare run-time types.
               if (obj == null || GetType() != obj.GetType())
                   return false;

               Earthquake anotherEarthquake = (Earthquake)obj;
               return (this.UTCDateTime.Equals(anotherEarthquake.UTCDateTime)) && (this.Longitude.Equals(anotherEarthquake.Longitude)) &&
                   (this.Latitude.Equals(anotherEarthquake.Latitude) && this.Place.PlaceName.Equals(anotherEarthquake.Place.PlaceName));
           }*/
    }

}
