using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Lab06.Models.DataAccess;

namespace Lab7.Models.DataAccess
{
    public class AllRecords
    {
        public List<AcademicRecord> AcademicRecordsList { get; set; }
        public List<Student> StudentsList { get; set; }
        public List<Course> CoursesList { get; set; }
        public List<Employee> EmployeesList { get; set; }

        public List<Role> RolesList { get; set; }

        public AllRecords()
        {

        }

    }
}
