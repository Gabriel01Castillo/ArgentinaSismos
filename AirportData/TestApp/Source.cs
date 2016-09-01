using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Source : BaseClass
    {
        public string Name { get; set; }
        public string Direction { get; set; }


        public Source()
        {
           
        }

        public Source(string name, string direction) {
            Name = name;
            Direction = direction;        
        }

        public override string ToString()
        {
            return string.Concat(Name," ",Direction);
        }
    }
}
