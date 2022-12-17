using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class EmployeeDto
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(100)]
    public string StreetName { get; set; }
    
    [Required]
    public string HouseNumber { get; set; }

    public int? ApartmentNumber { get; set; }
    
    public string PostalCode { get; set; }

    [Required]
    public string Town { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; }

    public DateTime DateOfBirth { get; set; }
    
    public int Age { get; set; }
}