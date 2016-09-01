using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessEF;

namespace TestApp
{
    public class DeviceLog 
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public String DeviceId { get; set; }
        public string Log { get; set; }
    }
}
