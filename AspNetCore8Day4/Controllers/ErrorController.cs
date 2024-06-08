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
using Microsoft.OpenApi.Validations.Rules;
using AspNetCore8Day4.Exceptions;

namespace AspNetCore8Day4.Controllers;

[Route("[controller]")]
[ApiController]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var exception = context.Error;

        _logger.LogError(exception, exception.Message);

        if (exception is GetDataException ex)
        {
            return Ok(new
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "取資料錯誤",
            });
        }

        return Problem(
            detail: exception.StackTrace,
            title: exception.Message,
            statusCode: StatusCodes.Status500InternalServerError);

        //return Ok(new
        //{
        //    Status = StatusCodes.Status500InternalServerError,
        //    Title = exception.Message,
        //});
    }

}
