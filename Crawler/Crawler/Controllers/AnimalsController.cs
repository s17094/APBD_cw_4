using Crawler.Models;
using Crawler.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {

        private readonly IAnimalsService _animalsService;

        public AnimalsController(IAnimalsService animalsService)
        {
            _animalsService = animalsService;
        }

        [HttpGet]
        public IActionResult GetAnimals(string? orderBy)
        {
            var animals = _animalsService.GetAnimals(orderBy);
            return Ok(animals);
        }

        [HttpPost]
        public IActionResult CreateAnimal(Animal animal)
        {
            var createdAnimal = _animalsService.CreateAnimal(animal);
            return Ok(createdAnimal);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnimal(string id, Animal animal)
        {
            var updatedAnimal = _animalsService.UpdateAnimal(id, animal);
            return Ok(updatedAnimal);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnimal(string id)
        {
            var deletedAnimal = _animalsService.DeleteAnimal(id);
            return Ok(deletedAnimal);
        }

    }
}
