using System.Data;
using System.Data.SqlClient;
using Crawler.Exceptions;
using Crawler.Models;

namespace Crawler.Repositories;

public class AnimalsRepository : IAnimalsRepository
{
    private readonly IConfiguration _configuration;

    public AnimalsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public HashSet<Animal> GetAnimals(string orderBy)
    {
        var animals = new HashSet<Animal>();

        using var connection =
            new SqlConnection(_configuration.GetConnectionString("ProductionDb"));

        var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal ORDER BY " + orderBy;

        connection.Open();
        var dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
            animals.Add(new Animal
            {
                IdAnimal = int.Parse(dataReader["IdAnimal"].ToString()),
                Name = dataReader["Name"].ToString(),
                Description = dataReader["Description"].ToString(),
                Category = dataReader["Category"].ToString(),
                Area = dataReader["Area"].ToString(),
            });
        }

        return animals;
    }

    public Animal CreateAnimal(Animal animal)
    {
        using var connection =
            new SqlConnection(_configuration.GetConnectionString("ProductionDb"));

        var command = GetCreateAnimalCommand(animal);
        command.Connection = connection;
        connection.Open();

        var result = command.ExecuteScalar();
        result = (result == DBNull.Value) ? null : result;

        if (result == null)
        {
            var errorMessage = new ErrorMessage("Unexpected error occured while creating new animal.");
            throw new CreateAnimalException(errorMessage, 500);
        }

        var idAnimal = Convert.ToInt32(result);
        animal.IdAnimal = idAnimal;

        return animal;
    }

    private SqlCommand GetCreateAnimalCommand(Animal animal)
    {
        var command = new SqlCommand();
        command.CommandText = GetInsertSql();

        AddNVarCharParameter("@name", animal.Name, command);
        if (animal.Description == null)
        {
            AddNVarCharParameter("@description", DBNull.Value, command);
        }
        else
        {
            AddNVarCharParameter("@description", animal.Description, command);
        }
        AddNVarCharParameter("@category", animal.Category, command);
        AddNVarCharParameter("@area", animal.Area, command);

        return command;
    }

    private string GetInsertSql()
    {
        return "INSERT INTO Animal (name, description, category, area) " +
               "OUTPUT INSERTED.IdAnimal " +
               "VALUES(@name, @description, @category, @area)";
    }

    public Animal UpdateAnimal(int id, Animal animal)
    {
        var animalToUpdate = GetAnimal(id);
        if (animalToUpdate == null)
        {
            var errorMessage = new ErrorMessage("Animal with id " + id + " not found");
            throw new UpdateAnimalException(errorMessage, 404);
        }

        using var connection =
            new SqlConnection(_configuration.GetConnectionString("ProductionDb"));

        var command = GetUpdateAnimalCommand(id, animal);
        command.Connection = connection;
        connection.Open();

        int rowAffected = command.ExecuteNonQuery();
        if (rowAffected != 1)
        {
            var errorMessage = new ErrorMessage("Unexpected error occured while updating animal with id " + id);
            throw new UpdateAnimalException(errorMessage, 500);
        }

        animal.IdAnimal = id;

        return animal;

    }

    private SqlCommand GetUpdateAnimalCommand(int id, Animal animal)
    {
        var command = new SqlCommand();
        command.CommandText = GetUpdateSql();

        AddIntParameter("@id", id, command);
        AddNVarCharParameter("@name", animal.Name, command);
        if (animal.Description == null)
        {
            AddNVarCharParameter("@description", DBNull.Value, command);
        }
        else
        {
            AddNVarCharParameter("@description", animal.Description, command);
        }
        AddNVarCharParameter("@category", animal.Category, command);
        AddNVarCharParameter("@area", animal.Area, command);

        return command;
    }

    private void AddIntParameter(string parameter, object value, SqlCommand command)
    {
        var sqlParameter = new SqlParameter(parameter, value)
        {
            SqlDbType = SqlDbType.Int
        };
        command.Parameters.Add(sqlParameter);
    }

    private void AddNVarCharParameter(string parameter, object value, SqlCommand command)
    {
        var sqlParameter = new SqlParameter(parameter, value)
        {
            SqlDbType = SqlDbType.NVarChar
        };
        command.Parameters.Add(sqlParameter);
    }

    private string GetUpdateSql()
    {
        return "UPDATE Animal " +
               "SET name = @name, " +
               "description = @description, " +
               "category = @category, " +
               "area = @area " +
               "WHERE idAnimal = @id";
    }

    public Animal DeleteAnimal(int id)
    {

        var animal = GetAnimal(id);
        if (animal == null)
        {
            var errorMessage = new ErrorMessage("Animal with id " + id + " not found");
            throw new DeleteAnimalException(errorMessage, 404);
        }

        using var connection =
            new SqlConnection(_configuration.GetConnectionString("ProductionDb"));
        var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "DELETE FROM Animal WHERE IdAnimal = @id";
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        int rowAffected = command.ExecuteNonQuery();
        if (rowAffected != 1)
        {
            var errorMessage = new ErrorMessage("Unexpected error occured while deleting animal with id " + id);
            throw new DeleteAnimalException(errorMessage, 500);
        }

        return animal;
    }

    private Animal? GetAnimal(int id)
    {
        using var connection =
            new SqlConnection(_configuration.GetConnectionString("ProductionDb"));

        var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal WHERE IdAnimal = @id";
        command.Parameters.AddWithValue("@id", id);

        connection.Open();

        var dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            return new Animal
            {
                IdAnimal = int.Parse(dataReader["IdAnimal"].ToString()),
                Name = dataReader["Name"].ToString(),
                Description = dataReader["Description"].ToString(),
                Category = dataReader["Category"].ToString(),
                Area = dataReader["Area"].ToString(),
            };
        }

        return null;
    }
}