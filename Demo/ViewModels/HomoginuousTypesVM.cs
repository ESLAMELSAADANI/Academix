using ModelsLayer.Models;

namespace Demo.ViewModels
{
    public class HomoginuousTypesVM
    {
        public int x { set; get; }
        public string name { set; get; }
        public List<string> names { set; get; }
        public Student student { set; get; }
        public List<Student> students { set; get; }
    }
}
