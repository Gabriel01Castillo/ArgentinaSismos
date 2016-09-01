using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MvcEarthquake
{
    public class Job
    {
        public string Title;
        public DateTime ExecutionTime;

        public Job(string title, DateTime executionTime)
        {
            this.Title = title;
            this.ExecutionTime = executionTime;
        }

        public void Execute()
        {
            Debug.WriteLine("Executing job at: " + DateTime.Now);
            Debug.WriteLine(this.Title);
            Debug.WriteLine(this.ExecutionTime);
        }

    }
}