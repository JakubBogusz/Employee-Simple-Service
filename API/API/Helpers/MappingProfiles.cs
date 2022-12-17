using API.Dtos;
using API.Models;
using AutoMapper;

namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Employee, EmployeeDto>();
    }
}