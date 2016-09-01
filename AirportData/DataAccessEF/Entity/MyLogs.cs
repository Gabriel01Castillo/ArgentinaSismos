using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class MyLogs
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public string Message { get;set; }
        public override string ToString()
        {
            return string.Concat(Date," ",Level," ", Message);
        }
    }
}
