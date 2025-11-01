using ModelsLayer.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Demo.Controllers
{
    public class TestController : Controller
    {
        //Domain:Port/Test/Display
        public string Display()
        {
            return $"Hello From Display() Inside TestController!";
        }
        //Domain:Port/Test/ShowAddForm
        public ViewResult ShowAddForm()
        {
            return View();
        }
        //Domain:Port/Test/Add?x={value}&y={value}
        //public int Add(int? x, int y)
        //{
        //    return (x ?? 0) + y;
        //}

        //========= Day02 ===========
        //Model Binding
        //public int Add()
        //{
        //    //If RequestMethod Sended As [Get] so contain the query string
        //    if (Request.Method == "GET")
        //    {
        //        int? x = int.Parse(Request.Query["x"]);
        //        int? y = int.Parse(Request.Query["y"]);
        //        return (x ?? 0) + (y ?? 0);
        //    }
        //    //If RequestMethod Sended As [Post] so data not in query string, it's sended with form data not in query string in URL
        //    else
        //    {
        //        int? x = int.Parse(Request.Form["x"]);
        //        int? y = int.Parse(Request.Form["y"]);
        //        return (x ?? 0) + (y ?? 0);
        //    }
        //}

        //====== Model Binder ========
        public int Add([FromQuery]int x,int y)//Bind/SearchOn The Value Of x,y from [FormData - RouteDate - QueryString]
        //             Force ModelBinder To Search On value of x from QueryString
        {
            return x + y;
        }
        //Domain:Port/Test/Details
        public ViewResult Details()
        {
            int x = 150;
            string name = "Eslam";
            List<string> names = new List<string>() { "ali", "eslam", "sara", "fatma" };
            Student student = new Student() { Id = 10, Name = "Eslam", Age = 23 };
            List<Student> students = new List<Student>()
            {
                new Student(){ Id = 10, Name = "Ahmed", Age = 23 },
                new Student(){ Id = 20, Name = "Hanaa", Age = 25 },
                new Student(){ Id = 30, Name = "Sama", Age = 27 },
                new Student(){ Id = 40, Name = "Ashraf", Age = 14 },
                new Student(){ Id = 50, Name = "Randa", Age = 15 },
                new Student(){ Id = 60, Name = "Heba", Age = 33 },
            };

            //Use ViewData Collection (Key Value Pair) To send data from Controller to view
            //The Type of returned data is object.
            //ViewData["fname"] = name;
            //ViewData["x"] = x;
            //ViewData["student"] = student;
            //ViewData["students"] = students;

            //Use ViewBag Collection (Key Value Pair) To send data from Controller to view
            //The Type of returned data is dynamic => know type in runtime.
            //ViewBag.instructor = "Ahmed";
            //ViewBag.fname = name;
            //ViewBag.student = student;
            //ViewBag.students = students;

            //ViewBag is wrapper on ViewData
            //Mean That anything added in ViewData will add in ViewBag automatic and viceversa
            //
            //ViewData["Course"] = "Python";
            //@ViewBag.Course -> Python
            //
            //ViewBag.Instructor = "Eslam";
            //ViewData["instructor"] -> Eslam
            //
            //And when read data using ViewBag it will read it from ViewData as of type "dynamic"
            //Using Model
            HomoginuousTypesVM typesVM = new HomoginuousTypesVM()
            {
                name = name,
                names = names,
                student = student,
                students = students,
                x = x
            };
            return View(typesVM);
        }

        //========= Day02 ===========
        public IActionResult index(int x)
        {
            TempData["_x"] = x;

            //======= Return ContentResult =========
            //if (x == 0)
            //    //return Content($"Hello From index action method x = {x}");//Return object of type "ContentResult" which implement interface "IActionResult" 
            //    return new ContentResult()
            //    {
            //        Content = $"Hello From index action method x = {x}",
            //        ContentType = $"text/plain",//Default
            //        StatusCode = 200//Default
            //    };//This Make As Same As Function Content()
            //=======================================

            //======= Return NotFoundResult =========
            if (x == 0)
                return NotFound();//Return object of type NotFoundResult With Status Code 404
                                  //=======================================

            //======= Return JsonResult =========
            //Return Student object D ata In Json Format
            if (x == 2)
                return Json(new Student() { Name = "Eslam", Age = 23, Id = 10 });
            //=======================================
            //======= Return RedirectResult =========
            //Return object of type RedirectResult that redirect to specific URL
            if (x == 3)
            {
                //return Redirect("https://www.google.com");//Always Request Come Here.
                return RedirectPermanent("https://www.google.com");//Once Come Here For First Time For Request It Saves That when make another request: http:localhost:3026/test/index?x=3
                                                                   //It Will Go To Url google direct without come here to action.
                                                                   //So If You Change The URl like to facebook it will not go to it, it will go to facebook
            }
            //=======================================
            //======= Return VirtualFileResult =========
            //Return object of type RedirectResult that redirect to specific URL
            if (x == 4)
            {
                //For Download file from server to client
                //return File("~/files/names.txt","text/plain","students.txt");
                return File("~/files/Me_LinkedInCover.png", "image/png", "eslamelsaadany.png");
            }
            //=======================================

            //======== Return ViewResult ==========
            ////return View();
            return new ViewResult()
            {
                ViewName = "index",
            };//Return object of type "ViewResult" which implement interface "IActionResult" 
              //=======================================

        }
        public int Add02()
        {
            int? x = int.Parse(Request.Query["x"]);
            int? y = int.Parse(Request.Query["y"]);
            return (x ?? 0) + (y ?? 0);
        }
    }
}
