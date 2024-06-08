
using ConsoleApp1;

var http = new HttpClient();

var client = new AspNetCore8Day4Client("http://localhost:5076/", http);

var courses = await client.GetCourseByIdAsync(2);

Console.WriteLine(courses.Title);
