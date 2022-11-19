using Crawler.Exceptions;
using Crawler.Models;
using Crawler.Repositories;

namespace Crawler.Services;

public class AnimalsService : IAnimalsService
{

    private readonly HashSet<string> _orderByTagArray = new();

    private readonly IAnimalsRepository _animalsRepository;

    public AnimalsService(IAnimalsRepository animalsRepository)
    {
        _animalsRepository = animalsRepository;
        _orderByTagArray.Add("name");
        _orderByTagArray.Add("description");
        _orderByTagArray.Add("category");
        _orderByTagArray.Add("area");
    }

    public HashSet<Animal> GetAnimals(string? orderBy)
    {
        orderBy = GetOrderByTag(orderBy);
        return _animalsRepository.GetAnimals(orderBy);
    }

    private string GetOrderByTag(string? orderBy)
    {
        orderBy = GetDefaultOrderByNameIfProperOrderTagNotOnList(orderBy);
        orderBy = AddSecondOrderByNameIfNameIsNotFirstOrderTag(orderBy);

        return orderBy;
    }

    private string GetDefaultOrderByNameIfProperOrderTagNotOnList(string? orderBy)
    {
        if (orderBy == null || !_orderByTagArray.Contains(orderBy))
        {
            orderBy = "name";
        }

        return orderBy;
    }

    private string AddSecondOrderByNameIfNameIsNotFirstOrderTag(string orderBy)
    {
        if (!orderBy.Equals("name"))
        {
            orderBy += ", name";
        }

        return orderBy;
    }

    public Animal CreateAnimal(Animal animal)
    {
        return _animalsRepository.CreateAnimal(animal);
    }

    public Animal UpdateAnimal(string id, Animal animal)
    {
        try
        {
            return _animalsRepository.UpdateAnimal(int.Parse(id), animal);
        }
        catch (FormatException)
        {
            throw new AnimalIdFormatException(id);
        }

    }

    public Animal DeleteAnimal(string id)
    {
        try
        {
            return _animalsRepository.DeleteAnimal(int.Parse(id));
        }
        catch (FormatException)
        {
            throw new AnimalIdFormatException(id);
        }

    }
}