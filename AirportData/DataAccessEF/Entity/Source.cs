using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessEF;

namespace TestApp
{
    public class Source : Entity
    {
        public string SourceName { get; set; }
        public string SourceDirection { get; set; }
        public bool IsOnline { get; set; }

        public Source()
        {
           
        }

        public Source(Guid id,string name, string direction) {
            base.Id = id;
            SourceName = name;
            SourceDirection = direction;        
        }

        public override string ToString()
        {
            return string.Concat(SourceName," ",SourceDirection);
        }
    }
}
