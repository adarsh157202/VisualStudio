using az204akwebapp4.Models;
using az204akwebapp4.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace az204akwebapp4.Controller
{
    public class CourseController : ControllerBase
    {
        private readonly CourseService _course_service;

        public CourseController(CourseService _svc)
        {
            _course_service = _svc;
        }
        public IActionResult Index()
        {
            IEnumerable<Course> _course_list = _course_service.GetCourses();
            return View(_course_list);
        }

    }
}
