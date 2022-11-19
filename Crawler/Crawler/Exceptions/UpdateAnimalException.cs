using Crawler.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Exceptions;

public class UpdateAnimalException : AnimalException
{
    private readonly ErrorMessage _errorMessage;
    private readonly int _statusCode;

    public UpdateAnimalException(ErrorMessage errorMessage, int statusCode)
    {
        _errorMessage = errorMessage;
        _statusCode = statusCode;
    }

    protected internal override IActionResult GetResponse()
    {
        return new ObjectResult(_errorMessage)
        {
            StatusCode = _statusCode
        };
    }
}