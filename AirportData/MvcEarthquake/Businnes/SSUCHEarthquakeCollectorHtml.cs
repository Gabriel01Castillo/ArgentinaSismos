using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using DataAccessEF.Implementation;
using HtmlAgilityPack;
using LogUtility;
using MvcEarthquake.Businnes.Factories;
using MvcEarthquake.Utils;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public class SSUCHEarthquakeCollectorHtml : HtmlEarthquakeCollector
    {
        static string countryChile ="CHILE";
        static string countryArgentina = "ARGENTINA";
        static string countryBolivia = "BOLIVIA";
        
        static string magnitudeTypeChile = "ML";

        public SSUCHEarthquakeCollectorHtml(IWebSourceManagerHtml webSourceManagerHtml, string webSiteUrl, Source source,
            IPlaceRepository iPlaceRepository, IPlaceFactory iPlaceFactory, IMagnitudeTypeRepository iMagnitudeTypeRepository,
            IMagnitudeTypeFactory iMagnitudeTypeFactory, IEarthqueakeFactory iEarthqueakeFactory) :
            base(webSiteUrl, source, iPlaceRepository, iPlaceFactory, iMagnitudeTypeRepository,iMagnitudeTypeFactory, iEarthqueakeFactory)
        {
            base.SourceManagerHTML = webSourceManagerHtml;
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

                var docChile = SourceManagerHTML.GetDocument(WebSiteUrl);


                //check if data is available
                if (string.IsNullOrEmpty(docChile.DocumentNode.InnerText))
                {
                    UpdateSourceStatus(false);
                    return new List<Earthquake>();
                }

                UpdateSourceStatus(true);


                // Get all tables in the document
                HtmlNodeCollection tables = docChile.DocumentNode.SelectNodes("//table");

                var earthquakeTable = tables[0];

                // Iterate all rows in the first table
                HtmlNodeCollection rows = earthquakeTable.SelectNodes(".//tr");
                if (rows.Count > 1)
                {

                    for (int i = 1; rows.Count > i; ++i)
                    {
                          bool IsSensible= false;
                        //determina si fue percibido
                        string sensible = rows[i].Attributes[0].Value;

                        if (sensible.Contains("sensible"))
                        {
                            IsSensible = true;
                        }




                        //Si es percibido entonces obtengo los datos
                        if (IsSensible) {

                            // Iterate all columns in this row
                            HtmlNodeCollection cols = rows[i].SelectNodes(".//th");
                            if (cols == null)
                            {
                                cols = rows[i].SelectNodes(".//td");
                            }

                            DateTime UTCDateTime = new DateTime();
                            decimal Latitude = 0;
                            decimal Longitude = 0;
                            decimal Depth = 0;
                            decimal Magnitude = 0;
                            decimal decimalValue = 0;
                            Place Place = null;
                            MagnitudeType MagnitudeType = null;

                            for (int j = 1; j < cols.Count; ++j)
                            {
                                string value = cols[j].InnerText;
                                switch (j)
                                {
                                    //Local time : ignore
                                    case (0):
                                        break;

                                    //UTC time
                                    case (1):
                                        const string format = "yyyy/MM/dd HH:mm:ss";
                                        UTCDateTime = DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
                                        break;

                                    //Latitude
                                    case (2):
                                        Latitude = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                                        break;

                                    //Longitude
                                    case (3):
                                        Longitude = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                                        break;

                                    //Depth
                                    case (4):
                                        Depth = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                                        break;

                                    //Magnitude
                                    case (5):
                                        string[] magnitudArray = value.Trim().Split(' ');
                                        decimalValue = Convert.ToDecimal(magnitudArray[0], CultureInfo.InvariantCulture);
                                        Magnitude = decimalValue;

                                        var mtypes = MagnitudesTypes.Where(mt => mt.Type.Equals(magnitudeTypeChile)).FirstOrDefault();

                                        if (mtypes == null)
                                        {
                                            mtypes = this.IMagnitudeTypeRepository.FindByType(magnitudeTypeChile);
                                            if (mtypes != null)
                                            {
                                                MagnitudesTypes.Add(mtypes);
                                            }
                                        }

                                        if (mtypes == null)
                                        {
                                            mtypes = this.IMagnitudeTypeFactory.Create(magnitudeTypeChile);
                                            IMagnitudeTypeRepository.DbContext.MagtitudesTypes.Add(mtypes);
                                            IMagnitudeTypeRepository.Save();
                                            MagnitudesTypes.Add(mtypes);
                                        }

                                        MagnitudeType = mtypes;

                                        break;

                                    //Place
                                    case (7):
                                        //"AR\r\n"
                                        string realCountry /*= countryChile*/ ;
                                        StringBuilder countrySB = new StringBuilder();
                                        countrySB.Append("http://api.geonames.org/countryCode?");

                                        var lat1 = Latitude.ToString().Replace(",", ".");
                                        lat1 = lat1.Remove(6, 1);

                                        var long2 = Longitude.ToString().Replace(",", ".");
                                        long2 = long2.Remove(6, 1);

                                        countrySB.Append("lat=").Append(lat1);
                                        countrySB.Append("&");
                                        countrySB.Append("lng=").Append(long2);
                                        countrySB.Append("&username=argentinaearthquake");

                                        var findedCountry = SourceManagerHTML.GetDocument(countrySB.ToString());
                                        //AR\r\n
                                        if (!string.IsNullOrEmpty(findedCountry.DocumentNode.InnerText) && findedCountry.DocumentNode.InnerText.Contains("AR"))
                                        {
                                            //si informan desde chile un sismo en Argentina, da por hecho que es sensible 
                                            IsSensible = true;
                                            realCountry = countryArgentina;
                                        }
                                        //CL\r\n
                                        else if (!string.IsNullOrEmpty(findedCountry.DocumentNode.InnerText) && findedCountry.DocumentNode.InnerText.Contains("CL"))
                                        {
                                            realCountry = countryChile;

                                        }
                                        //BL\r\n   //Error: no country code found
                                        else if (!string.IsNullOrEmpty(findedCountry.DocumentNode.InnerText) && findedCountry.DocumentNode.InnerText.Contains("BL") && !findedCountry.DocumentNode.InnerText.Contains("no country code found"))
                                        {
                                            realCountry = countryBolivia;

                                        }

                                        else
                                        {

                                            realCountry = countryChile;
                                            if (!findedCountry.DocumentNode.InnerText.Contains("no country code found"))
                                            {
                                                ExceptionUtility.Warn(string.Concat("GEONAMES-API, CHILE COLLECTOR ", findedCountry.DocumentNode.InnerText));
                                            }

                                            //"no country code found" : pacific ocean geopoint
                                        }
                                        value = value.ToUpper();

                                        var pla = Places.Where(pl => pl.PlaceName.Equals(value) && pl.Country.Equals(realCountry)).FirstOrDefault();

                                        if (pla == null)
                                        {
                                            pla = this.IPlaceRepository.FindByPlaceCountry(value, realCountry);
                                            if (pla != null)
                                            {
                                                Places.Add(pla);
                                            }
                                        }

                                        if (pla == null)
                                        {
                                            pla = this.IPlaceFactory.Create(value, realCountry);
                                            IPlaceRepository.DbContext.Places.Add(pla);
                                            IPlaceRepository.Save();
                                            Places.Add(pla);
                                        }

                                        Place = pla;
                                        break;

                                }
                            }
                            Earthquake earthquake = this.IEarthqueakeFactory.Create(UTCDateTime, Latitude, Longitude, Depth,
                                                                                          Magnitude, IsSensible, Place, Source, MagnitudeType);
                            earthquakes.Add(earthquake);
                        
                        }
                        
                        
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