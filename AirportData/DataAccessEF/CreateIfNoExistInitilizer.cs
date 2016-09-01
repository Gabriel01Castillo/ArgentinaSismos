using System;
using System.Collections.Generic;
using System.Data.Entity;
using TestApp;

namespace DataAccessEF
{
    public class CreateIfNoExistInitilizer : CreateDatabaseIfNotExists<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {

           /* IList<Earthquake> earthquakes = new List<Earthquake>();

            Place place1 = new Place(Guid.NewGuid(), "CORDOBA", "ARGENTINA");
            Source source1 = new Source(Guid.NewGuid(), "INPRES", "");
            MagnitudeType magnitude = new MagnitudeType(Guid.NewGuid(), "ML");
            var latitude = Convert.ToDecimal(-31.4750000000);
            var longitude = Convert.ToDecimal(-65.0380555556);
            var depth = Convert.ToDecimal(15.0000000000);
            var maginutde = Convert.ToDecimal(2.6000000000);

            Earthquake e1 = new Earthquake(Guid.NewGuid(), DateTime.Now.ToUniversalTime(), latitude, longitude,
                depth, maginutde, false, place1, source1, magnitude, DateTime.Now.ToUniversalTime());
            earthquakes.Add(e1);

            Place place2 = new Place(Guid.NewGuid(), "MENDOZA", "ARGENTINA");
            Source source2 = new Source(Guid.NewGuid(), "EMSC-CSEM", "");
            MagnitudeType magnitude2 = new MagnitudeType(Guid.NewGuid(), "ML");
            var latitude2 = Convert.ToDecimal(-34.1338888889);
            var longitude2 = Convert.ToDecimal(-68.7611111111);
            var depth2 = Convert.ToDecimal(22.0000000000);
            var maginutde2 = Convert.ToDecimal(2.8000000000);

            Earthquake e2 = new Earthquake(Guid.NewGuid(), DateTime.Now.ToUniversalTime(), latitude2, longitude2,
                depth2, maginutde2, false, place2, source2, magnitude2, DateTime.Now.ToUniversalTime());

            earthquakes.Add(e2);
            Place place3 = new Place(Guid.NewGuid(), "BIO BIO", "CHILE");
            Source source3 = new Source(Guid.NewGuid(), "sismologia.cl", "");
            MagnitudeType magnitude3 = new MagnitudeType(Guid.NewGuid(), "ML");
            var latitude3 = Convert.ToDecimal(-34.1338888889);
            var longitude3 = Convert.ToDecimal(-68.7611111111);
            var depth3 = Convert.ToDecimal(22.0000000000);
            var maginutde3 = Convert.ToDecimal(2.8000000000);

            Earthquake e3 = new Earthquake(Guid.NewGuid(), DateTime.Now.ToUniversalTime(), latitude3, longitude3,
                depth3, maginutde3, false, place3, source3, magnitude3, DateTime.Now.ToUniversalTime());

            earthquakes.Add(e3);

            Place place4 = new Place(Guid.NewGuid(), "ALASKA", "ALASKA");
            Source source4 = new Source(Guid.NewGuid(), "emsc-csem", "");
            MagnitudeType magnitude4 = new MagnitudeType(Guid.NewGuid(), "ML");
            var latitude4 = Convert.ToDecimal(-34.1338888889);
            var longitude4 = Convert.ToDecimal(-68.7611111111);
            var depth4 = Convert.ToDecimal(22.0000000000);
            var maginutde4 = Convert.ToDecimal(2.8000000000);

            Earthquake e4 = new Earthquake(Guid.NewGuid(), DateTime.Now.ToUniversalTime(), latitude4, longitude4,
                depth4, maginutde4, false, place4, source4, magnitude4, DateTime.Now.ToUniversalTime());

            earthquakes.Add(e4);


            foreach (Earthquake earthquake in earthquakes)
            {

                context.Set<Earthquake>().Add(earthquake);
            }
            */
         
            base.Seed(context);
        }
        
    }
}