using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebApplication5.Handlers
{
  public class CustomExceptionHandler : IExceptionFilter
  {
    private readonly ILogger logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
      this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
      HttpStatusCode status;
      string message;

      if (context.Exception is HttpRequestException) //Checking for my custom exception type
      {
        status = HttpStatusCode.ServiceUnavailable;
        message = "Error occured in communication with Stack Exchange API. This can be caused by overthrottling or network problems. For more information check log.";
      }
      else
      {
        status = HttpStatusCode.InternalServerError;
        message = "Server-side error makes it impossible to process request. For more information check log.";
      }

      logger.LogError(context.Exception, message);

      context.ExceptionHandled = true;
      HttpResponse response = context.HttpContext.Response;
      response.StatusCode = (int)status;
      response.ContentType = "text/plain";
      context.Result = new ObjectResult(message);
    }
  }
}
