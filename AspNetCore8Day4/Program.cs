using AspNetCore8Day4.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ContosoUniversityContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

        options.EnableSensitiveDataLogging();
    });

//builder.Services.AddHttpContextAccessor();

//builder.Services.AddScoped(sp =>
//{
//    var conn = builder.Configuration.GetConnectionString("DefaultConnection")!;

//    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

//    httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("X-Database-Name", out var dbname);

//    conn = conn.Replace("ABC", dbname);

//    return new ContosoUniversityContext(
//            new DbContextOptionsBuilder<ContosoUniversityContext>()
//                .UseSqlServer(conn).Options
//        );
//});

builder.Services.AddControllers(options =>
{
    //options.Filters.Add<記錄執行時間Attribute>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
