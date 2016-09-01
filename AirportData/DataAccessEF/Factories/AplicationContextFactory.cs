using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DataAccessEF;

namespace DataAccessEF.Factories
{
    public class AplicationContextFactory: IAplicationContextFactory
    {
        public ApplicationContext GetApplicationContext()
        {
            return new ApplicationContext();
        }
    }
}