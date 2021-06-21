using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionApp2.models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace FunctionApp2
{
    public static class GetCourses
    {
        [FunctionName("GetCourses")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            List<course> _list = new List<course>();
            string _connectionstring = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_SQLConnectionString");
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("get_all_courses_sp", con) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter { ParameterName = "@Client", SqlDbType = SqlDbType.VarChar, Value = apData.Client, Size = 200 });
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        course _course = new course() { 
                            id=rdr.GetInt32(0),
                            coursename=rdr.GetString(1)
                        };
                        _list.Add(_course);
                    }
                    rdr.Close();
                }
                return new OkObjectResult(_list);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Something went wrong");
                return new StatusCodeResult(500);
            }
            
        }
    }
}
