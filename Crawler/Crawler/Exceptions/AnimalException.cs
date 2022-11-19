using Microsoft.AspNetCore.Mvc;

namespace Crawler.Exceptions;

public abstract class AnimalException : Exception
{
    protected internal abstract IActionResult GetResponse();
}