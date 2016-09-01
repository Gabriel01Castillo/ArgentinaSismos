using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessEF;

namespace DataAccessEF.Factories
{
    public interface IAplicationContextFactory
    {
        ApplicationContext GetApplicationContext();
    }
}
