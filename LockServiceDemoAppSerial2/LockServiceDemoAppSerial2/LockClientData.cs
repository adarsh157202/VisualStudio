using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockServiceDemoAppSerial2
{
    class LockedClientData
    {
        public int ProcessID { get; set; }
        public List<string> SiteURLs { get; set; }
        public string Action { get; set; }
    }
}
