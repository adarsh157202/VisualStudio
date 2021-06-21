using az204akwebapp3.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace az204akwebapp3.Controller
{
    [ApiController]
    [Route("/api/Course")]
    public class CourseController : ControllerBase
    {
        private courseServicecs _course_service;
        public CourseController(courseServicecs _svc)
        {
            _course_service = _svc;
        }
        [HttpGet]
        public IActionResult GetCourses()
        {
            return Ok(_course_service.GetCourses());
        }
    }
}
