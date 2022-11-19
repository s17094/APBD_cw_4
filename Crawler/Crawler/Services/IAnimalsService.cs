using Crawler.Models;

namespace Crawler.Services;

public interface IAnimalsService
{
    HashSet<Animal> GetAnimals(string? orderBy);
    Animal CreateAnimal(Animal animal);
    Animal UpdateAnimal(string id, Animal animal);
    Animal DeleteAnimal(string id);
}