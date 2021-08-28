using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDbConsoleApp.models
{
    class Course
    {
        public string id { get; set; }
        public string courseid { get; set; }
        public string courseName { get; set; }
        public decimal rating { get; set;  }
    }
}
