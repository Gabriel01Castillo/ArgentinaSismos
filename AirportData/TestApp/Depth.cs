using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Depth : BaseClass
    {
        public decimal DepthValue { get; set; }
        public string Measure { get; set; }

        public Depth()
        {
           
        }

        public Depth(decimal depth, string measure)
        {
            DepthValue = depth;
            Measure = measure;
        }

        public override string ToString()
        {
            return string.Concat(DepthValue," ",Measure);
        }

    }
}
