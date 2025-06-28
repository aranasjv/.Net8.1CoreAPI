using AutoMapper;
using TestAPI.DTOs;
using TestAPI.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Map Department → DepartmentDto
            CreateMap<Department, DepartmentDto>();

            // Map DepartmentDto → Department
            CreateMap<DepartmentDto, Department>();
        }
    }
}
