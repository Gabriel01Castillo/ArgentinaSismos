using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessEF;

namespace TestApp
{
    public class MagnitudeType : Entity
    {
        public string Type { get; set; }

        public MagnitudeType() { }
        
        public MagnitudeType(Guid id, string type) {
            base.Id = id;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {

                return false;
            }

            else {
                var maginutType = (MagnitudeType)obj;
                return this.Type.Equals(maginutType.Type);
            }
            
        }
    }
}
