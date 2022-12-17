namespace API.Dtos;

public class EmployeeDto
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string StreetName { get; set; }
    
    public string HouseNumber { get; set; }

    public int? ApartmentNumber { get; set; }
    
    public string PostalCode { get; set; }

    public string Town { get; set; }
    
    public string PhoneNumber { get; set; }

    public DateTime DateOfBirth { get; set; }
    
    public int Age { get; set; }
}