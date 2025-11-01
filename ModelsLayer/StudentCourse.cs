using Microsoft.EntityFrameworkCore;
using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer
{
    [PrimaryKey("StudentId", "CourseId")]
    public class StudentCourse
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [Range(0, 100, ErrorMessage = "Keep Degree Between 0-100")]
        public int? Degree { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
