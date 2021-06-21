using az204akwebapp4.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace az204akwebapp4.Services
{
    

    public class CourseService
    {
        private static string db_source = "az204akdbserver1.database.windows.net";
        private static string db_user = "kmradrsh";
        private static string db_password = "Adarsh@2Kumar";
        private static string db_database = "az204akdb1";

        private SqlConnection GetConnection()
        {
            var _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = db_source;
            _builder.UserID = db_user;
            _builder.Password = db_password;
            _builder.InitialCatalog = db_database;
            return new SqlConnection(_builder.ConnectionString);
        }

        public IEnumerable<Course> GetCourses()
        {
            List<Course> _lst = new List<Course>();
            string _statement = "select * from Course";
            SqlConnection _connection = GetConnection();
            _connection.Open();
            SqlCommand _sqlcommand = new SqlCommand(_statement,_connection);
            using (SqlDataReader _reader=_sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Course _course = new Course() { 
                        CourseID=_reader.GetInt32(0),
                        CourseName=_reader.GetString(1),
                        Rating=_reader.GetDecimal(2)
                     };

                    _lst.Add(_course);

                }
            }
            _connection.Close();
            return _lst;

        }
    }
}
