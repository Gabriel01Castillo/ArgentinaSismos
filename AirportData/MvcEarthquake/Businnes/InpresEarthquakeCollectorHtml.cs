using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using DataAccessEF.Implementation;
using GlobalAppData;
using HtmlAgilityPack;
using LogUtility;
using MvcEarthquake.Businnes.Factories;
using MvcEarthquake.Utils;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public class InpresEarthquakeCollectorHtml : HtmlEarthquakeCollector
    {
       
        static string countryArgentina ="ARGENTINA";
        static string countryChile = "CHILE";
        static string countryBolivia = "BOLIVIA";

        static string magnitudeTypeArgentina = "ML";

        public InpresEarthquakeCollectorHtml(IWebSourceManagerHtml webSourceManagerHtml, string webSiteUrl, Source source,
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

                var docArgentina = SourceManagerHTML.GetDocument(WebSiteUrl);


                //check if data is available
                if (string.IsNullOrEmpty(docArgentina.DocumentNode.InnerText))
                {
                    UpdateSourceStatus(false);
                    return new List<Earthquake>();
                }
                UpdateSourceStatus(true);

                // Get all tables in the document
                HtmlNodeCollection tables = docArgentina.DocumentNode.SelectNodes("//table");

                var earthquakeTable = tables.Where(tb => tb.Id.Equals("sismos")).Single();




                // Iterate all rows in the first table
                HtmlNodeCollection rows1 = earthquakeTable.SelectNodes(".//tr");
                if (rows1.Count > 1)
                {

                    for (int i = 2; rows1.Count > i; ++i)
                    {

                        // Iterate all columns in this row
                        HtmlNodeCollection cols = rows1[i].SelectNodes(".//td");
                        var cantidadColumnas = cols.Count;
                        if (cantidadColumnas >= 8)
                        {
                            var sensible1 = cols[0];
                            bool isSensible = sensible1.OuterHtml.Contains("#ff0000");



                            string LocalDateTimeYear = string.Empty;
                            DateTime UTCDateTime = new DateTime();
                            decimal Latitude = 0;
                            decimal Longitude = 0;
                            decimal Depth = 0;
                            decimal Magnitude = 0;
                            decimal decimalValue = 0;
                            Place Place = null;
                            MagnitudeType MagnitudeType = null;

                            for (int j = 0; j < cols.Count; ++j)
                            {
                                string value = cols[j].InnerText;
                                switch (j)
                                {

                                    //Local time
                                    case (1):
                                        LocalDateTimeYear = value;
                                        break;
                                    case (2):
                                        StringBuilder sb = new StringBuilder();                                       
                                        string stringValue = sb.Append(LocalDateTimeYear).Append(" ").Append(value).ToString();
                                        const string format = "dd/MM/yyyy HH:mm:ss";
                                         var realtime = DateTime.ParseExact(stringValue, format, CultureInfo.InvariantCulture);
                                         UTCDateTime = GlobalWebData.ToUniversalTime(realtime);
                                        break;
                                    
                                    //Depth
                                    case (3):
                                        string[] depthArray = value.Split(' ');
                                        decimalValue = Convert.ToDecimal(depthArray[0], CultureInfo.InvariantCulture);
                                        Depth = decimalValue;
                                        break;

                                    //Magnitude
                                    case (4):
                                        string[] magnitudArray = value.Split(' ');
                                        decimalValue = Convert.ToDecimal(magnitudArray[0], CultureInfo.InvariantCulture);
                                        Magnitude = decimalValue;

                                        //tambien inferir que los sismos de 3.5 o mas son sensibles...

                                        double? d = 3.5;
                                        decimal? topMagnitude = (decimal?)d.Value;
                                        // decimal topMagnitude = decimal.Parse( "3.5");
                                        if (Magnitude >= topMagnitude)
                                        {
                                            isSensible = true;
                                        }
                                        var mtypes = MagnitudesTypes.Where(mt => mt.Type.Equals(magnitudeTypeArgentina)).FirstOrDefault();

                                        if (mtypes == null)
                                        {
                                            mtypes = this.IMagnitudeTypeRepository.FindByType(magnitudeTypeArgentina);
                                            if (mtypes != null)
                                            {
                                                MagnitudesTypes.Add(mtypes);
                                            }
                                        }

                                        if (mtypes == null)
                                        {
                                            mtypes = this.IMagnitudeTypeFactory.Create(magnitudeTypeArgentina);
                                            IMagnitudeTypeRepository.DbContext.MagtitudesTypes.Add(mtypes);
                                            IMagnitudeTypeRepository.Save();
                                            MagnitudesTypes.Add(mtypes);
                                        }

                                        MagnitudeType = mtypes;
                                       
                                        break;

                                    //Latitude
                                    case (5):
                                        Latitude = decimal.Parse(value, CultureInfo.InvariantCulture);                                        
                                        break;
                                    
                                    //Longitude
                                    case (6):
                                        Longitude = decimal.Parse(value, CultureInfo.InvariantCulture);
                                        break;

                                    //Place
                                    case (7):
                                        value = value.ToUpper();
                                        string country;
                                        
                                        if (value.Contains("CHILE"))
                                        {
                                            country = countryChile;
                                        }
                                        else if (value.Contains("BOLIVIA"))
                                        {
                                            country = countryBolivia;
                                        }
                                        else {
                                            country = countryArgentina;
                                        }

                                        var pla = Places.Where(pl => pl.PlaceName.Equals(value) && pl.Country.Equals(country)).FirstOrDefault();

                                        if (pla == null)
                                        {
                                            pla = this.IPlaceRepository.FindByPlaceCountry(value, country);
                                            if (pla != null)
                                            {
                                                Places.Add(pla);
                                            }
                                        }

                                        if (pla == null)
                                        {
                                            pla = this.IPlaceFactory.Create(value, country);
                                            IPlaceRepository.DbContext.Places.Add(pla);
                                            IPlaceRepository.Save();
                                            Places.Add(pla);
                                        }

                                        Place = pla;
                                        break;

                                }


                            }
                            Earthquake earthquake = this.IEarthqueakeFactory.Create(UTCDateTime, Latitude, Longitude, Depth,
                                                                                          Magnitude, isSensible, Place, Source, MagnitudeType);
                            earthquakes.Add(earthquake);
                        }
                        else
                        {
                            ExceptionUtility.Warn(string.Concat("cantidad de columnas", " ", cantidadColumnas," ",this.GetType()));
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