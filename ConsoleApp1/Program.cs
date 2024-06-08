
using ConsoleApp1;

var http = new HttpClient();

var client = new AspNetCore8Day4Client("http://localhost:5076/", http);

//var courses = await client.GetCourseByIdAsync(2);

//Console.WriteLine(courses.Title);


var courses = await client.取得課程資訊Async(1, 10);

foreach (var course in courses.Data)
{
    Console.WriteLine($"{course.CourseId} {course.Title} {course.Credits}");
}

