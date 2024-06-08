using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace AspNetCore8Day4.Filters;

public class 記錄執行時間Attribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        context.HttpContext.Items["Stopwatch"] = stopwatch;
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var stopwatch = (Stopwatch)context.HttpContext.Items["Stopwatch"]!;
        stopwatch.Stop();
        var executionTime = stopwatch.ElapsedMilliseconds;

        Console.WriteLine($"執行時間: {executionTime} 毫秒");
    }
}
