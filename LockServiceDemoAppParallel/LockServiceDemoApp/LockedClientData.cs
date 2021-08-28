using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockServiceDemoApp
{
    class LockedClientData
    {
        public int ProcessID { get; set; }
        public List<string> SiteURLs { get; set; }
        public Boolean IsClientLockedStatus { get; set; }
        public Boolean IsJurisdictionLockedStatus { get; set; }
    }
}
