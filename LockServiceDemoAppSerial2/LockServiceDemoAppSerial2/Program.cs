using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockServiceDemoAppSerial2
{
    class Program
    {
        private static System.Timers.Timer _timer;
        private const string _HistoryLogStartMessage = "LOCK/UNLOCK operation started";
        private const string _HistoryLogEndMessage = "LOCK/UNLOCK operation ended";
        private const string _HistoryLogMessageTypeInfo = "Info";
        private const string _HistoryLogMessageTypeError = "Error";
        static void Main(string[] args)
        {
            LockClients();
        }
        private static void LockClients()
        {
            try
            {
                //Logging.Debug(string.Format("Site Collection locked service  - started on  {0}", DateTime.Now.ToLongTimeString()));
                Console.WriteLine($"SiteLock service started");
                string connectionString = ConfigurationManager.ConnectionStrings["TAXPORTAL"].ConnectionString;
                Dictionary<int, LockedClientData> idsAndSiteCollectionUrls = new Dictionary<int, LockedClientData>();
                Console.WriteLine($"Getting all ensured support user");
                List<string> ensuredSupportUsers = GetEnsuredSupportUsers();
                Console.WriteLine($"Getting the SiteCollectionUrls ");
                idsAndSiteCollectionUrls = GetIdsAndCollectionUrls();

                foreach (KeyValuePair<int, LockedClientData> obj in idsAndSiteCollectionUrls)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        if (con.State == System.Data.ConnectionState.Closed || con.State == System.Data.ConnectionState.Broken)
                        {
                            con.Open();
                        }
                        try
                        {
                            LockedClientData data = obj.Value;
                            try
                            {
                                SqlCommand cmd = new SqlCommand("update_site_locked_db_started_at_column_sp", con) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                cmd.Parameters.Add(new SqlParameter { ParameterName = "@ProcessID", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                //Logging.Error(string.Format("updating database column  started at Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message), ex);
                                Console.WriteLine(string.Format("updating database column started at Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message));
                            }
                            //Processing siteURLS for each process
                            foreach (string siteUrl in data.SiteURLs)
                            {
                                Console.WriteLine($"Locking in progres for SiteURL {siteUrl}");
                                try
                                {
                                    //using (SPSite sp = new SPSite(siteUrl))
                                    //{
                                    //    using (SPWeb web = sp.OpenWeb())
                                    //    {
                                    //        foreach (string supportUser in ensuredSupportUsers)
                                    //        {
                                    //            web.EnsureUser(supportUser);
                                    //        }
                                    //    }
                                    //    if (data.Action == "lock")
                                    //    {
                                    //        sp.ReadOnly = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        sp.ReadOnly = false;
                                    //    }
                                    //}
                                    //update ETLClientSiteMapping
                                    try
                                    {
                                        SqlCommand cmd = new SqlCommand("update_ETLClientSiteMapping_sp", con) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@SiteURl", SqlDbType = SqlDbType.VarChar, Value = siteUrl });
                                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Action", SqlDbType = SqlDbType.VarChar, Value = data.Action });
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Failed to update column in the ETLClientSiteMapping for siteURL {siteUrl} \nException {ex.Message}");
                                    }
                                    //Check is TPE.client needs to be updated
                                    try
                                    {
                                        SqlCommand cmd = new SqlCommand("check_and_update_client_lock_sp", con) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@processId", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@Action", SqlDbType = SqlDbType.VarChar, Value = data.Action });
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Failed to update column in the ETLClientSiteMapping for siteURL {siteUrl} \nException {ex.Message}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Failed to  perform lock/unlock for siteurl= {siteUrl} \n Exception: {ex.Message}");

                                }
                            }

                            //updating sitelock table
                            try
                            {
                                SqlCommand cmd4 = new SqlCommand("update_site_locked_db_status_column_sp_2", con) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                //cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ClientId", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ProcessID", SqlDbType = SqlDbType.Int, Value = obj.Value.ProcessID });
                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@OverAllStatus", SqlDbType = SqlDbType.VarChar, Value = "SUCCESS" });
                                cmd4.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                //Logging.Error(string.Format("updating database column  Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message), ex);
                                SqlCommand cmd4 = new SqlCommand("update_site_locked_db_status_column_sp_2", con) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                //cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ClientId", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ProcessID", SqlDbType = SqlDbType.Int, Value = obj.Value.ProcessID });
                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@OverAllStatus", SqlDbType = SqlDbType.VarChar, Value = "FAILED" });
                                cmd4.ExecuteNonQuery();
                                Console.WriteLine(string.Format("updating database column  Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message));
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error in lock/unlock failed :" + ex.Message);
                            //Logging.Error(string.Format("Site Collection Lock  Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message), ex);
                            //Console.WriteLine(string.Format("Site Collection Lock  Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message));
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in lock/unlock failed :" + ex.Message);
                //Logging.Error(string.Format("Site Collection Lock  Failed : " + ex.Message), ex);
            }
        }
        public static List<string> GetEnsuredSupportUsers()
        {
            List<string> result = new List<string>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["TAXPORTAL"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("GetEnsuredSupportUsers", cn) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                    if (cn.State == System.Data.ConnectionState.Closed || cn.State == System.Data.ConnectionState.Broken)
                    {
                        cn.Open();
                    }
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        result.Add(rdr.GetString(0));
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
        public static Dictionary<int, LockedClientData> GetIdsAndCollectionUrls()
        {
            Dictionary<int, LockedClientData> idsAndSiteCollectionUrls = new Dictionary<int, LockedClientData>();
            try
            {

                string connectionString = ConfigurationManager.ConnectionStrings["TAXPORTAL"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    //getting site collections and Clients info  from db to lock/unlock
                    Console.WriteLine("Begin");
                    SqlCommand cmd = new SqlCommand("get_sitecollection_url_to_lock_sp_2", cn) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                    if (cn.State == System.Data.ConnectionState.Closed || cn.State == System.Data.ConnectionState.Broken)
                    {
                        cn.Open();
                    }
                    SqlDataReader rdr = cmd.ExecuteReader();
                    int processID;
                    while (rdr.Read())
                    {

                        processID = Convert.ToInt32(rdr.GetValue(0));
                        if (idsAndSiteCollectionUrls.ContainsKey(processID))
                        {

                            idsAndSiteCollectionUrls[processID].SiteURLs.Add(rdr.GetString(1));
                            idsAndSiteCollectionUrls[processID].Action = rdr.GetString(2);

                        }
                        else
                        {
                            LockedClientData clientData2 = new LockedClientData();

                            clientData2.ProcessID = processID;
                            clientData2.SiteURLs = new List<string>();
                            clientData2.SiteURLs.Add(rdr.GetString(1));
                            clientData2.Action = rdr.GetString(2);

                            idsAndSiteCollectionUrls.Add(processID, clientData2);
                        }

                    }
                    rdr.Close();
                    rdr.Dispose();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return idsAndSiteCollectionUrls;
        }
    }
}
