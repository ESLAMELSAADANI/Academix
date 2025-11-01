using ModelsLayer;
using ModelsLayer.Models;

namespace Demo.ViewModels
{
    public class DepartmentCourseStudentsVM
    {
        public Department Department { get; set; }
        public Course Course { get; set; }
        public List<StudentDegreeVM> StudentCourseDegrees { get; set; } = new List<StudentDegreeVM>();
        public List<StudentDegreeVM> OtherStudents { get; set; } = new List<StudentDegreeVM>();
        public List<int> SelectedStudentIds { get; set; } = new();
    }
}
