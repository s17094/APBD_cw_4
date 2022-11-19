using Microsoft.AspNetCore.Mvc;

namespace Crawler.Exceptions;

public class AnimalIdFormatException : AnimalException
{

    private readonly string id;

    public AnimalIdFormatException(string id)
    {
        this.id = id;
    }

    protected internal override IActionResult GetResponse()
    {
        return new ObjectResult("Input id: \"" + id + "\" is not integer number")
        {
            StatusCode = 400
        };
    }
}