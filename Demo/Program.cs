using Demo.DAL;
using Demo.Repos;
using Demo.ViewModels;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;
using ModelsLayer.Models;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<EntityRepo<Department>, DepartmentRepo>();
            builder.Services.AddScoped<EntityRepo<Student>, StudentRepo>();
            builder.Services.AddScoped<EntityRepo<Course>, CourseRepo>();
            builder.Services.AddScoped<EntityRepo<StudentCourse>, StudentCourseRepo>();
            builder.Services.AddScoped<IStudentCourseRepoExtra, StudentCourseRepo>();
            builder.Services.AddScoped<IStudentRepoExtra, StudentRepo>();
            builder.Services.AddScoped<IDepartmentRepoExtra, DepartmentRepo>();

            builder.Services.AddScoped<EntityRepo<User>, UserRepo>();
            builder.Services.AddScoped<EntityRepo<Role>, RoleRepo>();
            builder.Services.AddScoped<EntityRepo<UserRole>, UserRoleRepo>();
            builder.Services.AddScoped<IUserRoleRepoExtra, UserRoleRepo>();
            builder.Services.AddScoped<IUserRepoExtra, UserRepo>();
            builder.Services.AddScoped<IRoleRepoExtra, RoleRepo>();
            builder.Services.AddScoped<ICourseRepoExtra, CourseRepo>();

            builder.Services.AddAuthentication("MyCookieAuth")
                            .AddCookie("MyCookieAuth", options =>
                            {
                                options.Cookie.Name = "MyCookieAuth";
                                options.LoginPath = "/Account/Login";
                                options.AccessDeniedPath = "/Account/AccessDenied";
                            });
            //===== Use Default Constructor Of ITIDbContext When Make object from it and use OnConfiguring() of it which define the connectionString =======
            //builder.Services.AddScoped<ITIDbContext, ITIDbContext>();
            //builder.Services.AddScoped<ITIDbContext>();//Also Can Write This if the name of key same as value.

            //===== Use Parametrized Constructor Of ITIDbContext [ITIDbContext(DbContextOptions options)]When Make object from it and define the connection string and type of DB provider through [options] parameter=======
            //===== Life time of this service is scoped[Default] =======
            builder.Services.AddDbContext<ITIDbContext>(options =>
            {
                //options.UseSqlServer("Data Source=.;Initial Catalog=ITIMVC;Integrated Security=True;Trust Server Certificate=True");
                options.UseSqlServer(builder.Configuration.GetConnectionString("ITIMVC_Conn"));
            }/*,ServiceLifetime.Singleton*/);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=student}/{action=index}/{id:int?}");//Start With ControllerName
                                                                          //pattern: "{action=index}/{controller=student}/{id:int?}");//Start With ActionName
                                                                          //pattern: "ITI/{controller=student}/{action=index}/{id:int?}");//Start URL with static value
            app.Run();

            //=======================================================================

            //====== Make The MiddleWare Manually using Run() - Use() - Map() =====
            //app.Run() is short circuite => can't call the next middleware inside body of it.
            //app.Run(async (context,next) =>
            //{
            //    await context.Response.WriteAsync("Middleware 1 :: Before next()");
            //});



            //app.Use() - can act as short circuited or not bcz inside it you can call the next middleware 
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Middleware 1 :: Before next()\n");
                await next();
                await context.Response.WriteAsync("Middleware 1 :: After next()\n");
            });
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Middleware 2 :: Before next()\n");
                await next();
                await context.Response.WriteAsync("Middleware 2 :: After next()\n");
            });
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Middleware 3 :: Before next()\n");
                await next();
                await context.Response.WriteAsync("Middleware 3 :: After next()\n");
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello From The Final Middleware!\n");
            });
            app.Run();

            //For Any Request The Response Will Be Like This:

            //Middleware 1 :: Before next()
            //Middleware 2 :: Before next()
            //Middleware 3 :: Before next()
            //Hello From The Final Middleware!
            //Middleware 3 :: After next()
            //Middleware 2 :: After next()
            //Middleware 1 :: After next()

            //============================================================================

        }
    }
}
