using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Earthquake : BaseClass
    {
           public DateTime UTCDateTime { get; set; }           
           public Coordenate Latitude { get; set; }
           public Coordenate Longitude { get; set; }
           public Depth Depth { get; set; }
           public Magnitude Magnitude { get; set; }
           public bool IsSensible {get; set;}
           public Place Place {get; set;}
           public Source Source { get; set; }
           
           
           public Earthquake() { }
        
           public Earthquake(DateTime utcDateTime,Coordenate latitude,Coordenate longitude,
                                                    Depth depth, Magnitude magnitude, bool isSensible, Place place,Source source)
            {

            UTCDateTime = utcDateTime;
            Latitude = latitude;
            Longitude = longitude;
            Depth= depth;
            Magnitude = magnitude;
            IsSensible= isSensible;
            Place = place;
            Source = source;
           }

           public DateTime GetUTCDateTime() {
               var utc = Place.UTC * -1;
               var temporal = new DateTime(UTCDateTime.Ticks);
               return temporal.AddHours(utc);
           }

           public override string ToString()
           {
               return string.Concat(UTCDateTime," ",Latitude," ",Longitude," ",Depth," ",Magnitude," ",Place," ",Source);
           }
    }
}
