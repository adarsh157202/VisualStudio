using System.Collections.Generic;

namespace LockServiceDemoAppSerial
{
    public class LockedClientData
    {
        public int ProcessID { get; set; }
        public List<string> SiteURLs { get; set; }
        public string Action { get; set; }
    }
}