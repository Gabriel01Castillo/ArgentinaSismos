using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using DataAccessEF.Implementation;
using LogUtility;
using MvcEarthquake.Businnes.Factories;
using MvcEarthquake.Utils;
using TestApp;



namespace MvcEarthquake.Businnes
{
    public class EMSCEarthquakeCollectorXML : XMLEarthquakeCollector
    {
        string[] side = new string[6] { "EASTERN", "SOUTHERN", "WESTERN", "NORTHEM", "CENTRAL", "GULF OF" };

        public EMSCEarthquakeCollectorXML(IWebSourceManagerXML webSourceManagerXML, string webSiteUrl, Source source,
            IPlaceRepository iPlaceRepository, IPlaceFactory iPlaceFactory, IMagnitudeTypeRepository iMagnitudeTypeRepository,
            IMagnitudeTypeFactory iMagnitudeTypeFactory, IEarthqueakeFactory iEarthqueakeFactory) :
            base(webSiteUrl, source, iPlaceRepository, iPlaceFactory, iMagnitudeTypeRepository,
                                                                                                                                  iMagnitudeTypeFactory, iEarthqueakeFactory)
        {
            base.SourceManagerXML = webSourceManagerXML;
            base.WebSiteUrl = webSiteUrl;
            Source = source;
            this.IPlaceRepository = iPlaceRepository;
            this.IPlaceFactory = iPlaceFactory;
            this.IMagnitudeTypeRepository = iMagnitudeTypeRepository;
            this.IMagnitudeTypeFactory = iMagnitudeTypeFactory;
            this.IEarthqueakeFactory = iEarthqueakeFactory;
        }

        public override IList<Earthquake> Collect()
        {
            try
            {

                IList<Earthquake> earthquakes = new List<Earthquake>();
                List<MagnitudeType> MagnitudesTypes = new List<MagnitudeType>();
                List<Place> Places = new List<Place>();

                var docEMSC = SourceManagerXML.GetDocument(WebSiteUrl);


                //check if data is available
                if (string.IsNullOrEmpty(docEMSC.Document.ToString()))
                {
                    UpdateSourceStatus(false);
                    return new List<Earthquake>();
                }

                UpdateSourceStatus(true);

                IEnumerable<XElement> query = from c in docEMSC.Descendants("item") select c;
                IEnumerable<XElement> query5 = from d in query.Descendants("title") select d.Parent;
               
                foreach (var aaa in query5)
                {
                    var cantidad = query5.Count();
                    string[] asdf = aaa.ToString().Split(new Char[] { '\n' });
                    
                    var viewCountry = asdf[1].Replace("<title>", "").Replace("</title>", "").Trim();
                   
                    //IGNORES WORLD EARTHQUAKES ----Only parse ARGENTINA CHILE AND BOLIVIA ....
                    if (viewCountry.Contains("ARGENTINA") || viewCountry.Contains("CHILE") || viewCountry.Contains("BOLIVIA")
                                                                                              || viewCountry.Contains("PARAGUAY") || viewCountry.Contains("URUGUAY"))
                    {
                        string LocalDateTimeHour = string.Empty;
                        string LocalDateTimeYear = string.Empty;
                        DateTime UtcDateTime = new DateTime();
                        decimal Latitude = 0;
                        decimal Longitude = 0;
                        decimal Depth = 0;
                        decimal Magnitude = 0;

                        Place Place = null;
                        MagnitudeType MagnitudeType = null;
                        bool IsSensible = false;

                        #region PARSE-REGION
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

                                        var location = locationTemp.ToString().ToUpper();
                                        var pais = ololo[1].Trim().ToUpper();

                                        var pla = Places.Where(pl => pl.PlaceName.Equals(location) && pl.Country.Equals(pais)).FirstOrDefault();

                                        if (pla == null)
                                        {
                                            pla = this.IPlaceRepository.FindByPlaceCountry(location, pais);
                                            if (pla != null)
                                            {
                                                Places.Add(pla);
                                            }
                                        }

                                        if (pla == null)
                                        {
                                            pla = this.IPlaceFactory.Create(location, pais);
                                            IPlaceRepository.DbContext.Places.Add(pla);
                                            IPlaceRepository.Save();
                                            Places.Add(pla);
                                        }

                                        Place = pla;
                                    }
                                    else
                                    {
                                        ololo[0] = ololo[0].ToUpper();
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

                                        string pais = string.Empty;
                                        string location = string.Empty;

                                        if (!sbs.ToString().Equals(string.Empty))
                                        {
                                            var locationTemp = sbs.ToString().Trim();

                                            string[] aaasas = locationTemp.Split(' ');

                                            var paisTemp = aaasas[0].Trim().ToUpper();

                                            location = locationTemp.ToString().ToUpper();


                                            if (side.Contains(paisTemp))
                                            {
                                                pais = aaasas[aaasas.Length - 1];
                                            }
                                            else
                                            {
                                                pais = location;
                                            }


                                            Place = this.IPlaceRepository.FindByPlaceCountry(location, pais);
                                            if (Place == null)
                                            {

                                                Place = this.IPlaceFactory.Create(location, pais);
                                                IPlaceRepository.DbContext.Places.Add(Place);
                                                IPlaceRepository.Save();
                                            }
                                        }

                                        var pla = Places.Where(pl => pl.PlaceName.Equals(location) && pl.Country.Equals(pais)).FirstOrDefault();

                                        if (pla == null)
                                        {
                                            pla = this.IPlaceRepository.FindByPlaceCountry(location, pais);
                                            if (pla != null)
                                            {
                                                Places.Add(pla);
                                            }
                                        }

                                        if (pla == null)
                                        {
                                            pla = this.IPlaceFactory.Create(location, pais);
                                            IPlaceRepository.DbContext.Places.Add(pla);
                                            IPlaceRepository.Save();
                                            Places.Add(pla);
                                        }

                                        Place = pla;
                                    }

                                    break;

                                //ignore, id earthquake
                                case (2):
                                    break;

                                //Latitude    
                                case (3):
                                    var decimalString = value.Replace("<geo:lat xmlns:geo=\"http://www.w3.org/2003/01/geo/\">", "").Replace("</geo:lat>", "").Trim();
                                    Latitude = Convert.ToDecimal(decimalString, CultureInfo.InvariantCulture);
                                    break;

                                //Longitude   
                                case (4):
                                    var decimalString1 = value.Replace("<geo:long xmlns:geo=\"http://www.w3.org/2003/01/geo/\">", "").Replace("</geo:long>", "").Trim();
                                    Longitude = Convert.ToDecimal(decimalString1, CultureInfo.InvariantCulture);
                                    break;

                                //Depth  
                                case (5):
                                    var decimalString2 = value.Replace("<emsc:depth xmlns:emsc=\"http://www.emsc-csem.org\">", "").Replace("</emsc:depth>", "").Trim();
                                    string[] tempString = decimalString2.Split(' ');
                                    Depth = Convert.ToDecimal(tempString[0], CultureInfo.InvariantCulture);
                                    break;

                                //Magnitude 
                                case (6):
                                    var decimalString3 = value.Replace("<emsc:magnitude xmlns:emsc=\"http://www.emsc-csem.org\">", "").Replace("</emsc:magnitude>", "").Trim();
                                    string[] magnitudArray = decimalString3.Split(' ');
                                    Magnitude = Convert.ToDecimal(magnitudArray[magnitudArray.Length - 1], CultureInfo.InvariantCulture);
                                    
                                   
                                    magnitudArray[0] = magnitudArray[0].ToUpper();

                                    var mtypes = MagnitudesTypes.Where(mt => mt.Type.Equals(magnitudArray[0])).FirstOrDefault();

                                    if (mtypes == null)
                                    {
                                        mtypes = this.IMagnitudeTypeRepository.FindByType(magnitudArray[0]);
                                        if (mtypes != null)
                                        {
                                            MagnitudesTypes.Add(mtypes);
                                        }
                                    }

                                    if (mtypes == null)
                                    {
                                        mtypes = this.IMagnitudeTypeFactory.Create(magnitudArray[0]);
                                        IMagnitudeTypeRepository.DbContext.MagtitudesTypes.Add(mtypes);
                                        IMagnitudeTypeRepository.Save();
                                        MagnitudesTypes.Add(mtypes);
                                    }

                                    MagnitudeType = mtypes;

                                    break;

                                //DateTime 
                                case (7):
                                    var decimalString4 = value.Replace("<emsc:time xmlns:emsc=\"http://www.emsc-csem.org\">", "").Replace("</emsc:time>", "").Trim();
                                    string[] a1 = decimalString4.Split('-');
                                    string[] a2 = a1[2].Split(' ');
                                    string[] a3 = a2[1].Replace("UTC", "").Split(':');
                                    StringBuilder sb = new StringBuilder(); 
                                    const string format = "yyyy/MM/dd HH:mm:ss";
                                    string stringValue = sb.Append(a1[0]).Append("/").Append(a1[1]).Append("/").Append(a2[0]).Append(" ")
                                                                     .Append(a3[0]).Append(":").Append(a3[1]).Append(":").Append(a3[2]).ToString();

                                    UtcDateTime = DateTime.ParseExact(stringValue, format, CultureInfo.InvariantCulture);                                        

                                    break;
                            }


                        }
                        //si informan desde el exterior un sismo en Argentina, da por hecho que es sensible 
                        if (Magnitude > Convert.ToDecimal(4.5) || viewCountry.Contains("ARGENTINA"))
                        {
                            IsSensible = true;
                        }
                        if (viewCountry.Contains("ARGENTINA") || (!viewCountry.Contains("ARGENTINA") && IsSensible))
                        {

                            Earthquake earthquake = this.IEarthqueakeFactory.Create(UtcDateTime, Latitude, Longitude, Depth,
                                                                                   Magnitude, IsSensible, Place, Source, MagnitudeType);
                            earthquakes.Add(earthquake);
                        }
                        
                     
                        
                        #endregion
                    }
                }



                return earthquakes;
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return new List<Earthquake>();
            }

        }
    }
}