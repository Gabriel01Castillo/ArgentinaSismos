using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DataAccessEF.Implementation;
using MvcEarthquake.Businnes.Factories;
using MvcEarthquake.Utils;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public abstract class EarthquakeCollector : IEarthquakeCollector
    {

        public Source Source { get; set; }
        public IPlaceRepository IPlaceRepository { get; set; }
        public IPlaceFactory IPlaceFactory { get; set; }
        public IMagnitudeTypeRepository IMagnitudeTypeRepository { get; set; }
        public IMagnitudeTypeFactory IMagnitudeTypeFactory { get; set; }
        public IEarthqueakeFactory IEarthqueakeFactory { get; set; }
        public string WebSiteUrl { get; set; }

        public abstract IList<Earthquake> Collect();

        public EarthquakeCollector(string webSiteUrl, Source source,
                                 IPlaceRepository iPlaceRepository, IPlaceFactory iPlaceFactory, IMagnitudeTypeRepository iMagnitudeTypeRepository,
            IMagnitudeTypeFactory iMagnitudeTypeFactory, IEarthqueakeFactory iEarthqueakeFactory)
        {
           
            WebSiteUrl = webSiteUrl;
            Source = source;
            this.IPlaceRepository = iPlaceRepository;
            this.IPlaceFactory = iPlaceFactory;
            this.IMagnitudeTypeRepository = iMagnitudeTypeRepository;
            this.IMagnitudeTypeFactory = iMagnitudeTypeFactory;
            this.IEarthqueakeFactory = iEarthqueakeFactory;
        }

        protected void UpdateSourceStatus(bool isOnline)
        {
            Source.IsOnline = isOnline;
            IMagnitudeTypeRepository.DbContext.Sources.Attach(Source);
            IMagnitudeTypeRepository.DbContext.Entry(Source).State = EntityState.Modified;
            IMagnitudeTypeRepository.DbContext.SaveChanges();
        }
    }
}