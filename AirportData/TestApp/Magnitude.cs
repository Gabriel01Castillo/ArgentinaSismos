using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Magnitude : BaseClass
    {
        public decimal MagnitudeValue { get; set; }
        public string MagnitudeType { get; set; }

        public Magnitude()
        {
           

        }

        public Magnitude(decimal magnitude, string magnitudeType)
        {
            MagnitudeValue = magnitude;
            MagnitudeType = magnitudeType.ToUpper();
        }

        public override string ToString()
        {
            return string.Concat(MagnitudeValue," ",MagnitudeType);
        }

    }
}
