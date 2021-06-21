using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LockServiceDemoApp
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
                string connectionString = ConfigurationManager.ConnectionStrings["TAXPORTAL"].ConnectionString;
                Dictionary<int, LockedClientData> idsAndSiteCollectionUrls = new Dictionary<int, LockedClientData>();
                bool flag = false;
                idsAndSiteCollectionUrls = GetIdsAndCollectionUrls();
                if (idsAndSiteCollectionUrls.Count != 0)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }

                //new code
                while (flag)
                {
                    //parallel threads
                    ParallelOptions options = new ParallelOptions();
                    options.MaxDegreeOfParallelism = 10;
                    Parallel.ForEach(idsAndSiteCollectionUrls, options, obj => {

                        using (SqlConnection cn2 = new SqlConnection(connectionString))
                        {
                            if (cn2.State == System.Data.ConnectionState.Closed || cn2.State == System.Data.ConnectionState.Broken)
                            {
                                cn2.Open();
                            }
                            try
                            {
                                LockedClientData clientData = new LockedClientData();
                                clientData = obj.Value;
                                //Console.WriteLine("Currently executing for client id" + obj.Key);
                                try
                                {
                                    SqlCommand cmd4 = new SqlCommand("update_site_locked_db_started_at_column_sp", cn2) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                    cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ProcessID", SqlDbType = SqlDbType.Int, Value = obj.Value.ProcessID });
                                    cmd4.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    //Logging.Error(string.Format("updating database column  started at Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message), ex);
                                    Console.WriteLine(string.Format("updating database column started at Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message));
                                }
                                if (clientData.IsClientLockedStatus == false)
                                {
                                    var scurls = clientData.SiteURLs;
                                    ParallelOptions options2 = new ParallelOptions();
                                    options2.MaxDegreeOfParallelism = 25;
                                    Parallel.ForEach(scurls, options2, siteCollectionURL =>
                                    {
                                        using (SqlConnection cn3 = new SqlConnection(connectionString))
                                        {
                                            if (cn3.State == System.Data.ConnectionState.Closed || cn3.State == System.Data.ConnectionState.Broken)
                                            {
                                                cn3.Open();
                                            }
                                            try
                                            {
                                                Console.WriteLine("Locking SiteCollection " + siteCollectionURL);
                                                //using (SPSite sc = new SPSite(siteCollectionURL))
                                                //{
                                                //    sc.ReadOnly = true;
                                                //}
                                                Console.WriteLine("Locked");
                                                SqlCommand cmd2 = new SqlCommand("sitelock_history_log_sp", cn3) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                                cmd2.Parameters.Add(new SqlParameter { ParameterName = "@ClientId", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                                cmd2.Parameters.Add(new SqlParameter { ParameterName = "@SiteCollectionURL", SqlDbType = SqlDbType.VarChar, Value = siteCollectionURL });
                                                cmd2.Parameters.Add(new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.VarChar, Value = _HistoryLogEndMessage });
                                                cmd2.Parameters.Add(new SqlParameter { ParameterName = "@MessageType", SqlDbType = SqlDbType.VarChar, Value = _HistoryLogMessageTypeInfo });
                                                cmd2.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Error in locking site collection: " + siteCollectionURL + ex.Message);
                                                SqlCommand cmd4 = new SqlCommand("update_site_locked_db_status_column_sp", cn2) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ClientId", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ProcessID", SqlDbType = SqlDbType.Int, Value = obj.Value.ProcessID });
                                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@OverAllStatus", SqlDbType = SqlDbType.VarChar, Value = "FAILED" });
                                                cmd4.ExecuteNonQuery();
                                                //Logging.Error(string.Format("Site Collection locked failed for siteurl {0} ,ClientID:{1} , Exception Message:{2}", siteCollectionURL, obj.Key, ex.Message), ex);
                                            }
                                        }
                                    });
                                }
                                else
                                {

                                    var scurls = clientData.SiteURLs;

                                    ParallelOptions options2 = new ParallelOptions();
                                    options2.MaxDegreeOfParallelism = 30;
                                    Parallel.ForEach(scurls, options2, siteCollectionURL =>
                                    {
                                        using (SqlConnection cn4 = new SqlConnection(connectionString))
                                        {
                                            if (cn4.State == System.Data.ConnectionState.Closed || cn4.State == System.Data.ConnectionState.Broken)
                                            {
                                                cn4.Open();
                                            }
                                            try
                                            {
                                                Console.WriteLine("Locking SiteCollection " + siteCollectionURL);
                                                //using (SPSite sc = new SPSite(siteCollectionURL))
                                                //{
                                                //    sc.ReadOnly = false;
                                                //}
                                                Console.WriteLine("Locked");
                                                SqlCommand cmd2 = new SqlCommand("sitelock_history_log_sp", cn4) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                                cmd2.Parameters.Add(new SqlParameter { ParameterName = "@ClientId", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                                cmd2.Parameters.Add(new SqlParameter { ParameterName = "@SiteCollectionURL", SqlDbType = SqlDbType.VarChar, Value = siteCollectionURL });
                                                cmd2.Parameters.Add(new SqlParameter { ParameterName = "@Message", SqlDbType = SqlDbType.VarChar, Value = _HistoryLogEndMessage });
                                                cmd2.Parameters.Add(new SqlParameter { ParameterName = "@MessageType", SqlDbType = SqlDbType.VarChar, Value = _HistoryLogMessageTypeInfo });
                                                cmd2.ExecuteNonQuery();
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("Error in unlocking site collection: " + ex.Message);
                                                SqlCommand cmd4 = new SqlCommand("update_site_locked_db_status_column_sp", cn2) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ClientId", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ProcessID", SqlDbType = SqlDbType.Int, Value = obj.Value.ProcessID });
                                                cmd4.Parameters.Add(new SqlParameter { ParameterName = "@OverAllStatus", SqlDbType = SqlDbType.VarChar, Value = "FAILED" });
                                                cmd4.ExecuteNonQuery();
                                                //Logging.Error(string.Format("Site Collection unlocked failed for siteurl {0} ,ClientID:{1} , Exception Message:{2}", siteCollectionURL, obj.Key, ex.Message), ex);
                                            }
                                        }
                                    });
                                }

                                //Logging.Debug(string.Format("Site Collections locked  - complete on  {0}", DateTime.Now.ToLongTimeString()));
                                Console.WriteLine(string.Format("Site Collection locked  - complete on  {0}", DateTime.Now.ToLongTimeString()));
                                //updating client at database
                                try
                                {
                                    SqlCommand cmd4 = new SqlCommand("update_site_locked_db_status_column_sp", cn2) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                    cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ClientId", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                    cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ProcessID", SqlDbType = SqlDbType.Int, Value = obj.Value.ProcessID });
                                    cmd4.Parameters.Add(new SqlParameter { ParameterName = "@OverAllStatus", SqlDbType = SqlDbType.VarChar, Value = "SUCCESS" });
                                    cmd4.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    //Logging.Error(string.Format("updating database column  Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message), ex);
                                    SqlCommand cmd4 = new SqlCommand("update_site_locked_db_status_column_sp", cn2) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                                    cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ClientId", SqlDbType = SqlDbType.Int, Value = obj.Key });
                                    cmd4.Parameters.Add(new SqlParameter { ParameterName = "@ProcessID", SqlDbType = SqlDbType.Int, Value = obj.Value.ProcessID });
                                    cmd4.Parameters.Add(new SqlParameter { ParameterName = "@OverAllStatus", SqlDbType = SqlDbType.VarChar, Value = "FAILED" });
                                    cmd4.ExecuteNonQuery();
                                    Console.WriteLine(string.Format("updating database column  Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message));
                                }
                                //Logging.Debug(string.Format("Updating column on Database for clientid: {0}  - complete on  {1}", obj.Key, DateTime.Now.ToLongTimeString()));
                                Console.WriteLine(string.Format("Updating column on Database for clientid: {0}  - complete on  {1}", obj.Key, DateTime.Now.ToLongTimeString()));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error in lock/unlock failed :" + ex.Message);
                                //Logging.Error(string.Format("Site Collection Lock  Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message), ex);
                                Console.WriteLine(string.Format("Site Collection Lock  Failed for clientid: " + obj.Key + "\nException Message: " + ex.Message));
                            }
                        }
                    });

                    idsAndSiteCollectionUrls = GetIdsAndCollectionUrls();
                    if (idsAndSiteCollectionUrls.Count != 0)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logging.Error(string.Format("Site Collection Lock  Failed : " + ex.Message), ex);
            }
        }
        public static Dictionary<int, LockedClientData> GetIdsAndCollectionUrls()
        {
            Dictionary<int, LockedClientData> idsAndSiteCollectionUrls = new Dictionary<int, LockedClientData>();
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
                int clientID;
                while (rdr.Read())
                {

                    clientID = Convert.ToInt32(rdr.GetValue(0));
                    if (idsAndSiteCollectionUrls.ContainsKey(clientID))
                    {

                        idsAndSiteCollectionUrls[clientID].SiteURLs.Add(rdr.GetString(2));
                        idsAndSiteCollectionUrls[clientID].IsClientLockedStatus = Convert.ToBoolean(rdr.GetValue(1));

                    }
                    else
                    {
                        LockedClientData clientData2 = new LockedClientData();
                        clientData2.IsClientLockedStatus = Convert.ToBoolean(rdr.GetValue(1));
                        clientData2.SiteURLs = new List<string>();
                        clientData2.SiteURLs.Add(rdr.GetString(2));
                        clientData2.ProcessID = Convert.ToInt32(rdr.GetValue(3));
                        idsAndSiteCollectionUrls.Add(clientID, clientData2);
                    }

                }
                rdr.Close();
                rdr.Dispose();
            }
            return idsAndSiteCollectionUrls;
        }
    }
}
