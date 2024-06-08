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
using AspNetCore8Day4.Filters;
using AspNetCore8Day4.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AspNetCore8Day4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Users", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Users", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "取得課程資訊")]
        [ProducesResponseType<PagedCourse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCourses(int pageIndex = 1, int pageSize = 2)
        {
            // User.Identity?.IsAuthenticated;
            // var name = User.Identity?.Name;


            // 1. 一定要先排序
            var data = _context.Courses.AsNoTracking().OrderBy(c => c.CourseId).AsQueryable();

            // 2. 計算總筆數
            var total = await data.CountAsync();

            // 3. 計算總頁數
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);

            if (totalPages > 0)
            {
                throw new GetDataException("ERROR");
            }

            // 4. 取得指定頁數的資料
            var courses = await data
                .Select(c => new CourseRead()
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Credits = c.Credits
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 5. 回傳資料
            return Ok(new PagedCourse(
                total,
                totalPages,
                courses
            ));
        }

        // GET: api/Courses/5
        [HttpGet("{id}", Name = "GetCourseById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [記錄執行時間]
        public async Task<ActionResult<CourseRead>> GetCourse(int id)
        {
            //HttpContext.Request.Headers.TryGetValue("Accept", out var accept);
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return new CourseRead()
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Credits = course.Credits
            };
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost("")]
        [HttpPost("{id}")]
        [HttpPost("{id}:update")]
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

            if (courseToUpdate == null)
            {
                return NotFound();
            }

            courseToUpdate.Title = course.Title;
            courseToUpdate.Credits = course.Credits;

            //courseToUpdate.DateModified = DateTime.Now;

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

        // 幫我建立一個 POST method，可以批次更新 Course 的 Credits 欄位，所有的值要全部 +1
        [HttpPost("BatchUpdateCredits")]
        public IActionResult BatchUpdateCredits()
        {
            _context.Courses
                .Where(c => c.CourseId < 2)
                .ExecuteUpdate(setter => setter.SetProperty(c => c.Credits, c => c.Credits - 1));

            return NoContent();
        }

        // 寫一個API可以回傳Excel檔案
        [HttpGet("ExportExcel")]
        public IActionResult ExportExcel()
        {
            var stream = new System.IO.MemoryStream();
            //using (var package = new OfficeOpenXml.ExcelPackage(stream))
            //{
            //    var worksheet = package.Workbook.Worksheets.Add("課程資訊");

            //    worksheet.Cells[1, 1].Value = "CourseId";
            //    worksheet.Cells[1, 2].Value = "Title";
            //    worksheet.Cells[1, 3].Value = "Credits";

            //    var courses = _context.Courses.AsNoTracking().ToList();

            //    for (int i = 0; i < courses.Count; i++)
            //    {
            //        worksheet.Cells[i + 2, 1].Value = courses[i].CourseId;
            //        worksheet.Cells[i + 2, 2].Value = courses[i].Title;
            //        worksheet.Cells[i + 2, 3].Value = courses[i].Credits;
            //    }

            //    package.Save();
            //}

            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "課程資訊.xlsx");
        }

        // 撰寫一份使用 IAsyncEnumerable <T> 的範例 API
        [HttpGet("GetCoursesAsyncEnumerable")]
        public async IAsyncEnumerable<CourseRead> GetCoursesAsyncEnumerable()
        {
            var courses = _context.Courses.AsNoTracking().OrderBy(c => c.CourseId).AsAsyncEnumerable();

            await foreach (var course in courses)
            {
                await Task.Delay(200);

                //await Task.Yield();

                yield return new CourseRead()
                {
                    CourseId = course.CourseId,
                    Title = course.Title,
                    Credits = course.Credits
                };
            }
        }


        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }

    internal class PagedCourse
    {
        public int Total { get; }
        public int TotalPages { get; }
        public List<CourseRead> Data { get; }

        public PagedCourse(int total, int totalPages, List<CourseRead> data)
        {
            Total = total;
            TotalPages = totalPages;
            Data = data;
        }

        public override bool Equals(object? obj)
        {
            return obj is PagedCourse other &&
                   Total == other.Total &&
                   TotalPages == other.TotalPages &&
                   EqualityComparer<List<CourseRead>>.Default.Equals(Data, other.Data);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Total, TotalPages, Data);
        }
    }
}
