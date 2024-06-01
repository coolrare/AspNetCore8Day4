using AspNetCore8Day4.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore8Day4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ContosoUniversityContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ContosoUniversityContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get(string? q)
        {
            //var data = _context.Courses.Where(c => c.Title.Contains("Git"));

            var data = from c in _context.Courses
                       join d in _context.Departments on c.DepartmentId equals d.DepartmentId
                       where
                        d.StartDate.Date == DateTime.Parse("2015-03-21")
                       select new
                       {
                           c.CourseId,
                           c.Title,
                           c.Credits,
                           DepartmentName = d.Name
                       };

            if (!string.IsNullOrEmpty(q))
                data = data.Where(c => c.Title.StartsWith(q) || c.Title.EndsWith(q));

            return Ok(data);
        }

        // GET: WeatherForecast/MyDeptCourses
        //[HttpGet("MyDeptCourses", Name = "GetMyDeptCourses")]
        //public IActionResult GetMyDeptCourses(string? q)
        //{
        //    var data = from c in _context.MyDeptCourses
        //               select c;

        //    if (!string.IsNullOrEmpty(q))
        //        data = data.Where(c => c.Title.StartsWith(q) || c.Title.EndsWith(q));

        //    return Ok(data);
        //}

        // GET: WeatherForecast/MyDeptCourses
        //[HttpGet("GetMyDeptCourses", Name = "GetMyDeptCoursesSP")]
        //public async Task<IActionResult> GetMyDeptCoursesSPAsync(string? q)
        //{
        //    var data = await _context.GetProcedures().GetMyDeptCoursesAsync(q);

        //    return Ok(data);
        //}

        //[HttpGet("GetMyDeptCourses2", Name = "GetMyDeptCoursesSP2")]
        //public async Task<IActionResult> GetMyDeptCoursesSP2Async(string? q)
        //{
        //    var query = $"""
        //        SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
        //        FROM [Course] AS [c]
        //        INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
        //        WHERE [c].[Title] LIKE '%{q}%'
        //        """;

        //    //if (!string.IsNullOrEmpty(q))
        //    //{
        //    //    query += $" WHERE Title LIKE '%{q}%'";
        //    //}

        //    var data1 = await _context.MyDeptCourses.FromSqlRaw(query).ToListAsync();

        //    var data2 = await _context.MyDeptCourses.FromSql($"""
        //        SELECT [c].[CourseID] AS [CourseId], [c].[Title], [c].[Credits], [d].[Name] AS [DepartmentName]
        //        FROM [Course] AS [c]
        //        INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[DepartmentID]
        //        WHERE [c].[Title] LIKE '%{q}%'
        //        """).ToListAsync();

        //    return Ok(data2);
        //}

        // Get Single Course with Person
        [HttpGet("GetCourseWithPerson", Name = "GetCourseWithPerson")]
        public IEnumerable<InstructorsResponse> GetCourseWithPerson(string? q)
        {
            var item = _context.Courses.Include(c => c.Instructors)
                .SelectMany(c => c.Instructors, (c, i) => new InstructorsResponse
                {
                    Id = i.Id,
                    LastName = i.LastName,
                    FirstName = i.FirstName,
                    Discriminator = i.Discriminator
                });

            return item.AsEnumerable();
        }
    }
}
