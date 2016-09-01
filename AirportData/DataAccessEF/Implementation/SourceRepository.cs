using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessEF.Factories;
using TestApp;

namespace DataAccessEF.Implementation
{
    public class SourceRepository : GenericRepository<Source>, ISourceRepository
    {

        public SourceRepository(IAplicationContextFactory aplicationContextFactory)
                                                                        : base(aplicationContextFactory)
        {
            
        }
        
    }
}
