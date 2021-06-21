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
using System.Data.SqlClient;
using System.Data;

namespace FunctionApp2
{
    public static class AddCourse
    {
        [FunctionName("AddCourse")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
                //log.LogInformation("reqBody\n");
                //log.LogInformation(reqBody);
                course data = JsonConvert.DeserializeObject<course>(reqBody);

                string connectionString = "Server=tcp:az204akserver1.database.windows.net,1433;Initial Catalog=az204akdb1;Persist Security Info=False;User ID=kmradrsh;Password=Adarsh@2Kumar;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("add_course_sp", con) { CommandTimeout = 60000, CommandType = CommandType.StoredProcedure };
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.Int, Value = data.id });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@coursename", SqlDbType = SqlDbType.VarChar, Value = data.coursename, Size = 200 });
                    cmd.ExecuteNonQuery();
                      
                }
                return new OkObjectResult("Course Added");
                
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Something went wrong");
                //return new StatusCodeResult(500);
                return new BadRequestResult();
            }
           
            //return new OkObjectResult(responseMessage);
        }
    }
}
