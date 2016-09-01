using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEarthquake.Businnes
{
    public abstract class XMLEarthquakeCollector: EarthquakeCollector, IXMLEarthquakeCollector
    {
        public IWebSourceManagerXML SourceManagerXML { get; set; }

        public XMLEarthquakeCollector(string webSiteUrl, TestApp.Source source, DataAccessEF.Implementation.IPlaceRepository 
           iPlaceRepository, Factories.IPlaceFactory iPlaceFactory, DataAccessEF.Implementation.IMagnitudeTypeRepository 
           iMagnitudeTypeRepository, Factories.IMagnitudeTypeFactory iMagnitudeTypeFactory, Factories.IEarthqueakeFactory
                                                                                                         iEarthqueakeFactory):base( webSiteUrl,  source,  
           iPlaceRepository, iPlaceFactory, iMagnitudeTypeRepository, iMagnitudeTypeFactory,      iEarthqueakeFactory)
       {
           // TODO: Complete member initialization
           this.WebSiteUrl = webSiteUrl;
           this.Source = source;
           this.IPlaceRepository = iPlaceRepository;
           this.IPlaceFactory = iPlaceFactory;
           this.IMagnitudeTypeRepository = iMagnitudeTypeRepository;
           this.IMagnitudeTypeFactory = iMagnitudeTypeFactory;
           this.IEarthqueakeFactory = iEarthqueakeFactory;
       }
    }
}