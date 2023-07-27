using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Lab06.Models.DataAccess;

namespace Lab06.Models.DataAccess
{
    public partial class AcademicRecord
    {
        public string CourseCode { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public int? Grade { get; set; }

        public virtual Course CourseCodeNavigation { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
