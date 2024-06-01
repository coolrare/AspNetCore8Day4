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
    }
}
