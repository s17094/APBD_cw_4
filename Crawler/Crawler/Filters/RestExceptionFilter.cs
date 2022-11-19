using Crawler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Crawler.Filters;

public class RestExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is AnimalException animalException)
        {
            context.Result = animalException.GetResponse();
        }
    }
}