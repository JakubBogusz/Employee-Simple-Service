using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public static class DbInitialDataSeed
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(serviceProvider
                   .GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            try
            {
                var employee1 = new Employee
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    StreetName = "Main Street",
                    HouseNumber = "1",
                    ApartmentNumber = null,
                    PostalCode = "12345",
                    Town = "New York",
                    PhoneNumber = "123-456-7890",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Age = 32
                };

                var employee2 = new Employee
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    StreetName = "Maple Avenue",
                    HouseNumber = "2",
                    ApartmentNumber = 1,
                    PostalCode = "23456",
                    Town = "Chicago",
                    PhoneNumber = "234-567-8901",
                    DateOfBirth = new DateTime(1985, 2, 14),
                    Age = 37
                };

                var employee3 = new Employee
                {
                    Id = 3,
                    FirstName = "Bob",
                    LastName = "Smith",
                    StreetName = "Park Avenue",
                    HouseNumber = "3",
                    ApartmentNumber = 2,
                    PostalCode = "34567",
                    Town = "Los Angeles",
                    PhoneNumber = "345-678-9012",
                    DateOfBirth = new DateTime(1980, 3, 21),
                    Age = 42
                };

                var employee4 = new Employee
                {
                    Id = 4,
                    FirstName = "Alice",
                    LastName = "Johnson",
                    StreetName = "Ocean Drive",
                    HouseNumber = "4",
                    ApartmentNumber = 3,
                    PostalCode = "45678",
                    Town = "Miami",
                    PhoneNumber = "456-789-0123",
                    DateOfBirth = new DateTime(1975, 4, 28),
                    Age = 47
                };

                var employee5 = new Employee
                {
                    Id = 5,
                    FirstName = "Tom",
                    LastName = "Williams",
                    StreetName = "River Road",
                    HouseNumber = "5",
                    ApartmentNumber = null,
                    PostalCode = "56789",
                    Town = "Dallas",
                    PhoneNumber = "567-890-1234",
                    DateOfBirth = new DateTime(1970, 5, 5),
                    Age = 52
                };

                context.Employees.AddRange(employee1, employee2, employee3, employee4, employee5);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initial data seed: {ex}");
                throw;
            }
        }
    }
}