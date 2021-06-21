using az204akwebapp3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace az204akwebapp3.Service
{
    public class courseServicecs
    {
        public List<corsclass> course_list;
        public courseServicecs()
        {
            course_list = new List<corsclass>()
            {
                new corsclass(){ courseId=1,courseName="az900" },
                new corsclass(){ courseId=2,courseName="az204" },
                new corsclass(){ courseId=3,courseName="az400" }
            };
            
        }
        public IEnumerable<corsclass> GetCourses()
        {
            return (course_list);
        }
    }
}
