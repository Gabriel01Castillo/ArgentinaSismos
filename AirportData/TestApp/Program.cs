using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using GeoJSON;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using TestApp;


namespace Entity
{
    class Program
    {
        static string strAudioFilePath = "sounds\\beep_05.wav";
        
        static void Main(string[] args)
        {
            //SimulateSiteAttack();
            //PlayAlertSound();
            var resultado = GetJSON();
          
           
             Console.ReadLine();
        }

        private static void SimulateSiteAttack()
        {
            string url = "http://localhost:5129/Home/GetData?location=2&latitude=-34.1338888889&longitude=-68.7611111111&magnitude=2.0&date=1";
           
            while(true){
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
	            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }
              
            }
        }

       
        public static string GetJSON()
        {

           

            IList<Earthquake> earthquakes = new List<Earthquake>();
            Country Chile = new Country("CHILE");
            Country Argentina = new Country("ARGENTINA");

            var usgs = "http://earthquake.usgs.gov/earthquakes/feed/v0.1/summary/1.0_hour.geojson";
            var urlInpres = "http://www.inpres.gov.ar/seismology/xultimos.php";
            var ssuch = "http://www.sismologia.cl/links/ultimos_sismos.html";          
            string filename = "http://www.emsc-csem.org/service/rss/rss.php";

            var urlInpres1 = "J:\\Instituto Nacional de Prevención Sísmica.htm";
            var ssuch1 = "J:\\Ultimos Sismos.mht";
            var usgs1 = "J:\\earthquake\\2.5_day.csv";
            
            //-----------------------------------------------------------------------------------------
            while (true)
            {
                if (earthquakes.Count > 0) {

                     Console.WriteLine("                                                                                                                                                   ");
                     Console.WriteLine("                                                                                                                                                   ");
                     Console.WriteLine("                                                                                                                                                   ");

                     var earthquakesArgentina = earthquakes.Where(e => e.Place.Country.CountryName.Equals("ARGENTINA")).OrderByDescending(e => e.UTCDateTime);

                     var earthquakesChile = earthquakes.Where(e => e.Place.Country.CountryName.Equals("CHILE")).OrderByDescending(e => e.UTCDateTime);

                     var earthquakesWorld = earthquakes.Except(earthquakesArgentina).Except(earthquakesChile).OrderByDescending(e => e.UTCDateTime);


                    foreach (var earthquake in earthquakesArgentina)
                    {

                        
                        if (earthquake.IsSensible)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;

                            if(earthquake.Place.PlaceName.Contains("MENDOZA")){
                                PlayAlertSound();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.WriteLine(earthquake);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------");
                        Thread.Sleep(1000);

                    }

                    foreach (var earthquake in earthquakesChile)
                    {


                        if (earthquake.IsSensible)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.WriteLine(earthquake);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------");
                        Thread.Sleep(1000);

                    }

                    foreach (var earthquake in earthquakesWorld)
                    {


                        if (earthquake.IsSensible)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.WriteLine(earthquake);
                        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------");
                        Thread.Sleep(1000);

                    }
                
                }


                earthquakes = new List<Earthquake>();
                try
                {
                 #region ARGENTINA

                    //Console.WriteLine("#################################ARGENTINA###################################ARGENTINA#################################ARGENTINA#######################################");


                    // Load the html document
                    HtmlWeb webArgentina = new HtmlWeb();
                    HtmlDocument docArgentina = webArgentina.Load(urlInpres);


                    // Get all tables in the document
                    HtmlNodeCollection tables = docArgentina.DocumentNode.SelectNodes("//table");

                    var earthquakeTable = tables.Where(tb => tb.Id.Equals("sismos")).Single();

                    // Iterate all rows in the first table
                    HtmlNodeCollection rows1 = earthquakeTable.SelectNodes(".//tr");
                    for (int i = 1; 6 > i; ++i)
                    {

                        // Iterate all columns in this row
                        HtmlNodeCollection cols = rows1[i].SelectNodes(".//td");

                        var sensible1 = cols[8];
                        bool isSensible = sensible1.OuterHtml.Contains("#D40000");


                        string LocalDateTimeHour = string.Empty;
                        string LocalDateTimeYear = string.Empty;
                        DateTime LocalDateTime = new DateTime();
                        Coordenate Latitude = null;
                        Coordenate Longitude = null;
                        Depth Depth = null;
                        Magnitude Magnitude = null;
                        decimal decimalValue = 0;
                        Source Source = null;
                        Place Place = null;

                        for (int j = 1; j < cols.Count; ++j)
                        {
                            string value = cols[j].InnerText;
                            switch (j)
                            {

                                //Local time
                                case (1):
                                    LocalDateTimeYear = value;
                                    break;
                                case (2):
                                    LocalDateTimeHour = value;

                                    DateTime.TryParse(LocalDateTimeYear + " " + LocalDateTimeHour, out LocalDateTime);
                                    break;

                                //Latitude
                                case (3):
                                    string[] coord1 = value.Split('�');
                                    string[] coord2 = coord1[1].Split('\'');
                                    var degrees = Convert.ToDecimal(coord1[0], CultureInfo.InvariantCulture);
                                    var minutes = Convert.ToDecimal(coord2[0], CultureInfo.InvariantCulture);
                                    var seconds = Convert.ToDecimal(coord2[1], CultureInfo.InvariantCulture);
                                    Latitude = new Coordenate(degrees, minutes, seconds);
                                    break;

                                //Longitude
                                case (4):
                                    string[] coord3 = value.Split('�');
                                    string[] coord4 = coord3[1].Split('\'');
                                    var degrees1 = Convert.ToDecimal(coord3[0], CultureInfo.InvariantCulture);
                                    var minutes1 = Convert.ToDecimal(coord4[0], CultureInfo.InvariantCulture);
                                    var seconds1 = Convert.ToDecimal(coord4[1], CultureInfo.InvariantCulture);
                                    Longitude = new Coordenate(degrees1, minutes1, seconds1);
                                    break;

                                //Depth
                                case (5):
                                    string[] depthArray = value.Split(' ');
                                    decimalValue = Convert.ToDecimal(depthArray[0], CultureInfo.InvariantCulture);
                                    Depth = new Depth(decimalValue, depthArray[1]);
                                    break;

                                //Magnitude
                                case (6):
                                    string[] magnitudArray = value.Split(' ');
                                    decimalValue = Convert.ToDecimal(magnitudArray[0], CultureInfo.InvariantCulture);
                                    Magnitude = new Magnitude(decimalValue, "??");
                                    break;

                                //Place
                                case (7):
                                    Place = new Place(value, Argentina, -3);
                                    Source = new Source("INPRES", "");
                                    break;

                            }


                        }

                        earthquakes.Add(new Earthquake(LocalDateTime, Latitude, Longitude, Depth, Magnitude, isSensible, Place, Source));
                    }
                    #endregion
                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null)
                    {

                        Console.WriteLine("ARGENTINA REGION ERROR:" + ex.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine("ARGENTINA REGION ERROR:" + ex.Message);
                    }
                }
                try{
                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------
                    #region Twitter

                    /*
                     The Twitter REST API v1 will soon stop functioning. Please migrate to API v1.1.
                     https://dev.twitter.com/docs/api/1.1/overview
                     */

                    
                    var twitter = new Twitter("xNBdzXy7tkyD1kstIDcg", "0nSriIwDZkFR7qcf6nsnQh3bqecn7IGoTqpIr8Xk6gc",
                          "1451422718-kwIAaVppRGys6dltQXKIUt39vNbIg5e4PFD1Rgu",
                          "1LPbBbwYcAELk3dl50WIwZMp38gt2wWGwkOkzVQGQM");


                    var responseTwitter = twitter.GetTweets("sismo argentina");

                    JObject jobjectTwitter1 = JObject.Parse(responseTwitter);

                    var resultado12 = jobjectTwitter1["statuses"];

                    var twittsList2 = resultado12.ToList();

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("--------------------Twitter---------------Twitter----------------Twitter------------------Twitter--------------------Twitter-----------------Twitter");
                    foreach (var twitt in twittsList2)
                    {
                          Console.ForegroundColor = ConsoleColor.Blue;
                        //Wed, 22 May 2013 14:01:58 +0000                       
                        DateTime timestamp;
                        //var rrrr = twitt.First.First.ToString().Replace(",", "").Replace("\\", "").Replace("\"","");

                       // DateTime.TryParseExact(rrrr, "ddd MMM dd HH:mm:ss K yyyy", null, DateTimeStyles.None, out timestamp); 
                       
                       

                        var text = twitt["text"];
                        var date = twitt["created_at"].ToString().Replace("\\", "").Replace("\"", "");

                        const string format = "ddd MMM dd HH:mm:ss zzzz yyyy";
                        var realtime = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture); 


                        if (text.ToString().Contains("gol") || text.ToString().Contains("futbol") || text.ToString().Contains("jugador"))
                        { 
                          Console.ForegroundColor = ConsoleColor.Yellow;
                        
                        }
                        

                        Console.WriteLine(string.Concat(realtime.ToString(), " ", text));
                        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------");
                        Thread.Sleep(500);
                    }
                    #endregion

                }
                catch (Exception ex)
                {

                    if (ex.InnerException != null)
                    {

                        Console.WriteLine("TWITTER ERROR:" + ex.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine("TWITTER ERROR:" + ex.Message);
                    }
                    Thread.Sleep(3000);
                }

                try{
                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------
                   
                    #region Chile
                    //Console.WriteLine("########################CHILE######################CHILE########################CHILE####################################################");
                    
                    // Load the html document
                    HtmlWeb webChile = new HtmlWeb();
                    HtmlDocument docChile = webChile.Load(ssuch);


                    // Get all tables in the document
                    HtmlNodeCollection tables1 = docChile.DocumentNode.SelectNodes("//table");

                    var earthquakeTable1 = tables1[0];

                    // Iterate all rows in the first table
                    HtmlNodeCollection rows = earthquakeTable1.SelectNodes(".//tr");
                    for (int i = 1; 6 > i; ++i)
                    {

                         bool IsSensible= false;
                        //determina si fue percibido
                        string sensible = rows[i].Attributes[0].Value; 

                         if (sensible.Contains("sensible")) {
                              IsSensible = true;                                      
                         }

                        
                        
                        // Iterate all columns in this row
                        HtmlNodeCollection cols = rows[i].SelectNodes(".//th");
                        if (cols == null)
                        {
                             cols = rows[i].SelectNodes(".//td");
                        }

                       

                        DateTime LocalDateTime = new DateTime();
                        Coordenate Latitude = null;
                        Coordenate Longitude = null;
                        Depth Depth = null;
                        Magnitude Magnitude = null;                        
                        decimal decimalValue = 0;
                        Source Source = null;
                        Place Place = null;

                        for (int j = 0; j < cols.Count; ++j)
                        {
                            string value = cols[j].InnerText;

                            switch (j) {

                                //Local time : ignore
                                case(0):                                    
                                break;

                                //UTC time
                                case (1):
                                DateTime.TryParse(value, out LocalDateTime);
                                break;

                                //Latitude
                                case (2):
                                   decimalValue =  Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                                   Latitude = new Coordenate(decimalValue);
                                break;

                                //Longitude
                                case (3):
                                   decimalValue = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                                   Longitude = new Coordenate(decimalValue);
                                break;
                                
                                //Depth
                                case (4):
                                  decimalValue = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                                  Depth = new Depth(decimalValue, "KM.");
                                break;
                                
                                //Magnitude
                                case (5):
                                  string[] magnitudArray = value.Trim().Split(' ');
                                  decimalValue = Convert.ToDecimal(magnitudArray[0], CultureInfo.InvariantCulture);
                                  Magnitude = new Magnitude(decimalValue, magnitudArray[1]);
                                break;
                                
                                //Source
                                case (6):
                                  Source = new Source(value,"");
                                break;

                                //Place
                                case (7):
                                  Place = new Place(value, Chile,-4);
                                break;
                            
                            }                           

                        }

                        earthquakes.Add(new Earthquake(LocalDateTime, Latitude, Longitude, Depth, Magnitude, IsSensible, Place, Source));
                    }
                  #endregion    
                }
                catch (Exception ex)
                {
                    Thread.Sleep(3000);
                    if (ex.InnerException != null)
                    {

                        Console.WriteLine("CHILE REGION ERROR:" + ex.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine("CHILE REGION ERROR:" + ex.Message);
                    }
                }
                    //---------------------------------------------------------------------------------------------------------------------------------------------------------------
                try{
                    #region EMS
                   

                    XDocument doc = XDocument.Load(filename);
                    IEnumerable<XElement> query = from c in doc.Descendants("item") select c;
                    IEnumerable<XElement> query5 = from d in query.Descendants("title") /*.Where(d => d.Value.Contains("ARGENTINA"))*/ select d.Parent;
                    /*     
                    ML 2.3 WESTERN TURKEY
                    ML 2.3 CRETE, GREECE
                    Mw 5.8 EASTERN UZBEKISTAN
                    ML 4.3 JUJUY, ARGENTINA
                    ML 3.5 SOUTHERN IRAN
                    M 4.1 POTOSI, BOLIVIA
                    ML 3.2 OFF COAST OF ATACAMA, CHILE
                    ML 2.8 FYR OF MACEDONIA
                    ML 3.8 OFFSHORE COQUIMBO, CHILE
                    mb 4.8 PAPUA, INDONESIA
                    mb 4.1 SEA OF OKHOTSK
                    ML 3.0  ALBANIA
                    M  4.6  OFF COAST OF SOUTHEASTERN ALASKA"
                     */
                    foreach (var aaa in query5)
                    {
                        var cantidad = query5.Count();
                        string[] asdf = aaa.ToString().Split(new Char[] { '\n' });
                        string LocalDateTimeHour = string.Empty;
                        string LocalDateTimeYear = string.Empty;
                        DateTime LocalDateTime = new DateTime();
                        Coordenate Latitude = null;
                        Coordenate Longitude = null;
                        Depth Depth = null;
                        Magnitude Magnitude = null;
                        decimal decimalValue = 0;
                        Source Source = null;
                        Place Place = null;
                        bool IsSensible = false;

                        for (int j = 1; j < asdf.Length; ++j)
                        {
                            var value = asdf[j];
                            switch (j)
                            {
                                //Local time
                                case (1):


                                    var haber = value.Replace("<title>", "").Replace("</title>", "").Trim();
                                    string[] ololo = haber.Split(',');
                                    if (ololo.Length == 2)
                                    {
                                        var carac = ololo[0].ToCharArray();

                                        int cantidadEspacios = 0;

                                        StringBuilder sbs = new StringBuilder();
                                        bool anteriorSpace = false;

                                        for (int i = 0; i < carac.Length; ++i)
                                        {
                                            if (cantidadEspacios == 2)
                                            {
                                                sbs.Append(carac[i]);
                                            }

                                            if (cantidadEspacios < 2 && carac[i].Equals(' ') && !anteriorSpace)
                                            {
                                                cantidadEspacios++;
                                                anteriorSpace = true;
                                            }
                                            else
                                            {
                                                anteriorSpace = false;
                                            }
                                        }


                                        var locationTemp = sbs.ToString().Trim();

                                        var location = locationTemp.ToString();
                                        var pais = ololo[1].Trim();
                                        var country = new Country(pais);
                                        Place = new Place(location, country, 0);
                                    }
                                    else
                                    {
                                        char[] carac = ololo[0].ToCharArray();
                                        int cantidadEspacios = 0;

                                        StringBuilder sbs = new StringBuilder();
                                        bool anteriorSpace = false;
                                        for (int i = 0; i < carac.Length; ++i)
                                        {
                                            if (cantidadEspacios == 2)
                                            {
                                                sbs.Append(carac[i]);
                                            }

                                            if (cantidadEspacios < 2 && carac[i].Equals(' ') && !anteriorSpace)
                                            {
                                                cantidadEspacios++;
                                                anteriorSpace = true;
                                            }
                                            else
                                            {
                                                anteriorSpace = false;
                                            }
                                        }

                                        if (!sbs.ToString().Equals(string.Empty))
                                        {   
                                            var locationTemp = sbs.ToString().Trim();
                                            string[] side = new string[6] { "EASTERN", "SOUTHERN", "WESTERN", "NORTHEM", "CENTRAL", "GULF OF" };
                                           
                                            string[] aaasas = locationTemp.Split(' ');
                                            var pais = aaasas[aaasas.Length - 1].Trim(); ;
                                            var location = locationTemp.ToString();
                                            

                                             if(side.Contains(pais)){
                                                 var country = new Country(pais);
                                                 Place = new Place(location, country, 0);
                                             }else{
                                                 var country = new Country(location);
                                                 Place = new Place(location, country, 0);
                                             
                                             }
                                            
                                        }
                                    }

                                    break;

                                //ignore, id earthquake
                                case (2):
                                    break;

                                //Latitude    
                                case (3):
                                    var decimalString = value.Replace("<geo:lat xmlns:geo=\"http://www.w3.org/2003/01/geo/\">", "").Replace("</geo:lat>", "").Trim();
                                    var val = Convert.ToDecimal(decimalString, CultureInfo.InvariantCulture);
                                    Latitude = new Coordenate(val);
                                    break;

                                //Longitude   
                                case (4):
                                    var decimalString1 = value.Replace("<geo:long xmlns:geo=\"http://www.w3.org/2003/01/geo/\">", "").Replace("</geo:long>", "").Trim();
                                    var val1 = Convert.ToDecimal(decimalString1, CultureInfo.InvariantCulture);
                                    Longitude = new Coordenate(val1);
                                    break;

                                //Depth  
                                case (5):
                                    var decimalString2 = value.Replace("<emsc:depth xmlns:emsc=\"http://www.emsc-csem.org\">", "").Replace("</emsc:depth>", "").Trim();
                                    string[] tempString = decimalString2.Split(' ');
                                    var val2 = Convert.ToDecimal(tempString[0], CultureInfo.InvariantCulture);
                                    Depth = new Depth(val2, "Km.");
                                    break;

                                //Magnitude 
                                case (6):
                                    var decimalString3 = value.Replace("<emsc:magnitude xmlns:emsc=\"http://www.emsc-csem.org\">", "").Replace("</emsc:magnitude>", "").Trim();
                                    string[] magnitudArray = decimalString3.Split(' ');
                                    decimalValue = Convert.ToDecimal(magnitudArray[magnitudArray.Length - 1], CultureInfo.InvariantCulture);
                                    Magnitude = new Magnitude(decimalValue, magnitudArray[0]);
                                    break;

                                //DateTime 
                                case (7):
                                    var decimalString4 = value.Replace("<emsc:time xmlns:emsc=\"http://www.emsc-csem.org\">", "").Replace("</emsc:time>", "").Trim();
                                    string[] a1 = decimalString4.Split('-');
                                    string[] a2 = a1[2].Split(' ');
                                    string[] a3 = a2[1].Replace("UTC", "").Split(':');
                                    LocalDateTime = new DateTime(int.Parse(a1[0]), int.Parse(a1[1]), int.Parse(a2[0]), int.Parse(a3[0]), int.Parse(a3[1]), int.Parse(a3[2]));

                                    break;
                            }


                        }

                        Source = new Source("emsc-csem.", "");
                        if (Magnitude.MagnitudeValue > Convert.ToDecimal(4.5))
                        {
                            IsSensible = true;
                        }

                        earthquakes.Add(new Earthquake(LocalDateTime, Latitude, Longitude, Depth, Magnitude, IsSensible, Place, Source));


                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    Thread.Sleep(3000);
                    if (ex.InnerException != null)
                    {

                        Console.WriteLine("EMS ERROR:" + ex.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine("EMS ERROR:" + ex.Message);
                    }
                }

                try
                {
                    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    #region USA REGION
                    //Console.WriteLine("########################USA######################WORLD########################USA######################WORDL##############################");

                    /* Stream str = new FileStream(usgs1, FileMode.Open,FileAccess.Read);
                      StreamReader sr = new StreamReader(str);
                      string line = string.Empty;
                    
                      int lineNumber= 0;
                      string[] dataLine = null;
                    
                    
                      DateTime LocalDateTimeUsa = new DateTime();
                      Coordenate LatitudeUsa = null;
                      Coordenate LongitudeUsa = null;
                      Depth DepthUsa = null;
                      Magnitude MagnitudeUsa = null;
                      decimal decimalValueUsa = 0;
                      Source SourceUsa = null;
                      Place PlaceUsa = null;

                      while (line != null )
                      {
                          line = sr.ReadLine();
                          if(lineNumber != 0 && lineNumber != 6){

                              dataLine = line.ToString().Split(',');

                            

                              decimalValueUsa = Convert.ToDecimal(dataLine[0], CultureInfo.InvariantCulture);
                          }
                          else if(lineNumber ==  6){
                           break;
                          }
                          lineNumber++;
                      }*/

                    //create the constructor with post type and few data
                    MyWebRequest myRequest = new MyWebRequest(usgs, "POST", "");
                    //show the response string on the console screen.
                    var response = myRequest.GetResponse();


                    JObject jobject = JObject.Parse(response);
                    JToken metadata = null;
                    JToken type = null;
                    JToken features = null;

                    jobject.TryGetValue("metadata", out metadata);
                    jobject.TryGetValue("features", out features);
                    jobject.TryGetValue("type", out type);

                    var cantidadIncidentes = features.Count();

                    var eee = features.ToArray<JToken>();

                    for (int i = 0; i < cantidadIncidentes; i++)
                    {
                        var incidente = eee[i];




                        var jTokenIntensity = (JToken)incidente.First.Next.First.First.First;
                        var jTokenTime = (JToken)((incidente.First.Next.First.First).Next).Next.Last;

                        string miliseconds = (string)jTokenTime.Value<Newtonsoft.Json.Linq.JValue>();

                        Newtonsoft.Json.Linq.JValue valor = jTokenIntensity.Value<Newtonsoft.Json.Linq.JValue>();
                        JTokenType jtype = jTokenIntensity.Type;



                        Int64 intensidadInteger = 0;
                        double intensidadDouble = 0.0;

                        if (jtype.Equals(JTokenType.Float))
                        {
                            intensidadDouble = (double)valor.Value;
                        }
                        else if (jtype.Equals(JTokenType.Integer))
                        {
                            intensidadInteger = (Int64)valor.Value;
                        }

                        bool isSensible = false;

                        if (intensidadInteger != 0 && intensidadDouble == 0.0)
                        {
                            isSensible = intensidadInteger >= 4.5;
                        }
                        else if (intensidadInteger == 0 && intensidadDouble != 0.0)
                        {
                            isSensible = intensidadDouble >= 4.5;

                        }

                        if (isSensible)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(incidente.ToString());
                            Console.WriteLine("..........................................................................................................................");
                            Thread.Sleep(2000);
                        }
                        else
                        {

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(incidente.ToString());
                            Console.WriteLine("..........................................................................................................................");
                            Thread.Sleep(2000);
                        }
                    }

                    #endregion
                }
                 
                catch (Exception ex) {
                    Thread.Sleep(3000);
                    if(ex.InnerException != null){

                        Console.WriteLine("USA REGION ERROR:" + ex.InnerException.Message);
                    }
                     else{
                         Console.WriteLine("USA REGION ERROR:" + ex.Message);
                    }
                }
                   //---------------------------------------------------------------------------------------------------------------------------------------------------------------------


                    Console.WriteLine("ªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªª");
                    Console.WriteLine("ªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªª");
                    Console.WriteLine("ªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªªª");

                  


               

            }
                    
            //-------------------------------------------------------------------------------------------



            while (true)
            {

            //create the constructor with post type and few data
            MyWebRequest myRequest = new MyWebRequest(usgs, "POST", "");
            //show the response string on the console screen.
            var response = myRequest.GetResponse();

               
            JObject jobject = JObject.Parse(response);
            JToken metadata = null;
            JToken type = null;
            JToken features = null;

            jobject.TryGetValue("metadata", out metadata);
            jobject.TryGetValue("features", out features);
            jobject.TryGetValue("type", out type);

            var cantidadIncidentes = features.Count();
            
            var eee = features.ToArray<JToken>();

            for (int i = 0; i < cantidadIncidentes;i++ )
            {
                var incidente = eee[i];
                Console.WriteLine(incidente.ToString());
                Console.WriteLine("..........................................................................................................................");
                Thread.Sleep(2000);
            }
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

                Thread.Sleep(10000);
            }


            /*
                 {
	                "type": "FeatureCollection",
	                "features": [
		                {
			                "geometry": {
				                "type": "Point",
				                "coordinates": [6.18218, 45.5949]
			                },
			                "type": "Feature",
			                "properties": {
				                "elevation": 1770,
				                "name": "Col d'Arclusaz"
			                },
			                "id": 472
		                }, ... more features...
	                }
                }
             */

            object MyCollection = new MyFeatureCollection
            {
                Features = new MyFeature[] {
			new MyFeature {
				ID = "472",
				Geometry = new GeoJSON.Point(6.18218, 45.5949),
				Properties = new MyProperties {
					Elevation = "1770",
					Name = "CollectionBase d'Arclusaz"
				}
			},
			new MyFeature {
				ID = "458",
				Geometry = new GeoJSON.Point(6.27827, 45.6769),
				Properties = new MyProperties {
					Elevation = "1831",
					Name = "Pointe de C\\u00f4te Favre"
				}
			}
		}
            };

             StringBuilder Builder = new System.Text.StringBuilder();
            StringWriter Writer = new System.IO.StringWriter(Builder);

            new Newtonsoft.Json.JsonSerializer().Serialize(Writer, MyCollection);

            return Builder.ToString();
        }

        private static void PlayAlertSound()
        {
            try
            {
               var hh= Assembly.GetExecutingAssembly().Location;
               var sas = hh.ToString().Replace("bin\\Debug\\TestApp.exe", "").Replace("file:", "\\") + "sounds\\beep_05.wav";
               SoundPlayer simpleSound = new SoundPlayer(sas);
                simpleSound.Play();
            }
            catch(Exception ex){
                Console.WriteLine("sound:" + ex.Message);
            }
        }




       
    }

    public interface IOAuthData
    {
        string ID { get; }
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        string Token { get; set; }
        string TokenSecret { get; set; }
        int? UserID { get; set; }
        string ScreenName { get; set; }

        bool Load(string id);

        void Save();
    }
}
