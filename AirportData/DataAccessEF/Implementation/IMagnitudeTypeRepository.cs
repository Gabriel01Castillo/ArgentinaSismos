using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp;

namespace DataAccessEF.Implementation
{
    public interface IMagnitudeTypeRepository:  IRepository<MagnitudeType>
    {
        MagnitudeType FindByType(string type);
    }
}
