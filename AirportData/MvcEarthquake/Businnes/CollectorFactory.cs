using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessEF.Factories;
using DataAccessEF.Implementation;
using MvcEarthquake.Businnes.Factories;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public class CollectorFactory : ICollectorFactory 
    {
        public ISourceRepository SourceRepository { get; set; }
        public SourceFactory SourceFactory { get; set; }

        public CollectorFactory(ISourceRepository iSourceRepository)
        {
            SourceRepository = iSourceRepository;
            SourceFactory = new SourceFactory();
             
        }

        public InpresEarthquakeCollectorHtml GetInpresEarthquakeCollectorHtml()
        {
            Source inpres = this.SourceRepository.Find(s => s.SourceName.Equals("INPRES")).FirstOrDefault();
            if (inpres == null) {
                inpres = SourceFactory.Create("INPRES", "http://www.inpres.gov.ar/");
                SourceRepository.Insert(inpres);
                SourceRepository.Save();
            }

            var urlInpres = "http://www.inpres.gov.ar/seismology/xultimos.php";
            IWebSourceManagerHtml webSource = new MyWebSourceManagerHtml();
            return new InpresEarthquakeCollectorHtml(webSource, urlInpres, inpres, new PlaceRepository(new AplicationContextFactory()), new PlaceFactory(),
                                                                                        new MagnitudeTypeRepository(new AplicationContextFactory()), new MagnitudeTypeFactory(), new EarthquakeFactory());
        }




        public EMSCEarthquakeCollectorXML GetEMSCEarthquakeCollectorXML()
        {
            Source emsc = this.SourceRepository.Find(s => s.SourceName.Equals("EMSC-CSEM")).FirstOrDefault();
            if (emsc == null)
            {
                emsc = SourceFactory.Create("EMSC-CSEM", "http://www.emsc-csem.org/");
                SourceRepository.Insert(emsc);
                SourceRepository.Save();
            }

            var urlEmsc = "http://www.emsc-csem.org/service/rss/rss.php";
            IWebSourceManagerXML WebSource = new MyWebSourceManagerXML();
            return new EMSCEarthquakeCollectorXML(WebSource, urlEmsc, emsc, new PlaceRepository(new AplicationContextFactory()), new PlaceFactory(),
                                                                                        new MagnitudeTypeRepository(new AplicationContextFactory()), new MagnitudeTypeFactory(), new EarthquakeFactory());
        }

        public SSUCHEarthquakeCollectorHtml GetSSUCHEarthquakeCollectorHtml()
        {
            Source ssuch = this.SourceRepository.Find(s => s.SourceName.Equals("SSUCH")).FirstOrDefault();
            if (ssuch == null)
            {
                ssuch = SourceFactory.Create("SSUCH", "http://www.sismologia.cl");
                SourceRepository.Insert(ssuch);
                SourceRepository.Save();
            }

            var urlSsuch = "http://www.sismologia.cl/links/ultimos_sismos.html";
            IWebSourceManagerHtml webSource = new MyWebSourceManagerHtml();
            return new SSUCHEarthquakeCollectorHtml(webSource, urlSsuch, ssuch, new PlaceRepository(new AplicationContextFactory()), new PlaceFactory(),
                                                                                        new MagnitudeTypeRepository(new AplicationContextFactory()), new MagnitudeTypeFactory(), new EarthquakeFactory());
        }
    }
}