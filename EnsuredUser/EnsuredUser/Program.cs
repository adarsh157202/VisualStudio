using Microsoft.SharePoint;
using System;

namespace EnsuredUser
{
    class Program
    {
        static void Main(string[] args)
        {
            string siteUrl = "https://insightstage.tax.deloitteonline.com/sites/newametestclient17092020_us/CIT/", userKey= "i:0ǵ.t|adfs stg|";
            siteUrl=Console.ReadLine();
            userKey += Console.ReadLine();
            using (SPSite sp = new SPSite(siteUrl))
            {
                using (SPWeb web = sp.OpenWeb())
                {
                    try
                    {
                        web.AllowUnsafeUpdates = true;
                        SPUser spUser = web.EnsureUser(userKey);
                    }
                    catch (Exception ex)
                    {
                        //Error handling logic should go here
                        Console.WriteLine($"Exception : {ex}");
                        Console.ReadLine();
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = false;
                        Console.ReadLine();
                    }
                }
            }

        }
    }
}
