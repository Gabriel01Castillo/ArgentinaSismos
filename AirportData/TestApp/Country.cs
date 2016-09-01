using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Country : BaseClass
    {
        public string CountryName {get;set;}
       

        public Country()
        {

        }

        public Country(string countryName)
        {
            CountryName = countryName;            
        }

        public override string ToString()
        {
            return CountryName;
        }
    }
}
