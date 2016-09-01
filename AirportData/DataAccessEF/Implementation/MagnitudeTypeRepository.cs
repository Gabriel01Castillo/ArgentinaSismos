using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessEF.Factories;
using TestApp;

namespace DataAccessEF.Implementation
{
    public class MagnitudeTypeRepository : GenericRepository<MagnitudeType>, IMagnitudeTypeRepository
    {
        public MagnitudeTypeRepository(IAplicationContextFactory aplicationContextFactory)
                                                                        : base(aplicationContextFactory)
        {
            
        }

        public MagnitudeType FindByType(string type)
        {
            return DbContext.Set<MagnitudeType>().AsNoTracking().Where(m => m.Type.Equals(type)).FirstOrDefault();
        }
    }
}
