using ModelsLayer;
using ModelsLayer.Models;

namespace Demo.ViewModels
{
    public class DepartmentCoursesVM
    {
        public Department Department { get; set; }
        public List<Course> DepartmentCourses { get; set; } = new List<Course>();
        public List<Course> OtherCourses { get; set; } = new List<Course>();
        //=== For Delete&Add SelectedItems ======
        public List<int> CoursesToDelete { get; set; } = new List<int>();
        public List<int> CoursesToAdd { get; set; } = new List<int>();
    }
}
