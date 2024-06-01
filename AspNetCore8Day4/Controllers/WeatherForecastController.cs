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
        public IEnumerable<Course> Get()
        {
            return _context.Courses.ToList();
        }
    }
}
