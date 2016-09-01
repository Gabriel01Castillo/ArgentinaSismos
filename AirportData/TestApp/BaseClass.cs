using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class BaseClass
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Description { get; set; }

        public BaseClass() { }

        public BaseClass(Guid id, int version, string description) {
            Id = id;
            Version = version;
            Description = description;
        }
    }
}
