using Crawler.Models;

namespace Crawler.Repositories;

public interface IAnimalsRepository
{
    HashSet<Animal> GetAnimals(string orderBy);
    Animal CreateAnimal(Animal animal);
    Animal UpdateAnimal(int id, Animal animal);
    Animal DeleteAnimal(int id);

}