using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCore8Day4.Models;
using AspNetCore8Day4.Models.Dto;
using System.Drawing.Printing;

namespace AspNetCore8Day4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<IActionResult> GetCourses(int pageIndex = 1, int pageSize = 2)
        {
            // 1. 一定要先排序
            var data = _context.Courses.OrderBy(c => c.CourseId).AsQueryable();

            // 2. 計算總筆數
            var total = await data.CountAsync();

            // 3. 計算總頁數
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            // 4. 取得指定頁數的資料
            var courses = await data
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 5. 回傳資料
            return Ok(new
            {
                Total = total,
                TotalPages = totalPages,
                Data = courses
            });
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseUpdate course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            //_context.Attach(course);
            //_context.Entry(course).State = EntityState.Modified;

            //_context.Entry(course).State = EntityState.Modified;

            //_context.Update(course);

            var courseToUpdate = await _context.Courses.FindAsync(id);

            courseToUpdate.Title = course.Title;
            courseToUpdate.Credits = course.Credits;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CourseCreate course)
        {
            var newCourse = new Course()
            {
                Title = course.Title,
                Credits = course.Credits,
                DepartmentId = 1
            };

            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, newCourse);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
