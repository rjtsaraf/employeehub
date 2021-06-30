using AutoMapper;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;

namespace EmpDepAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Department,DepartmentDTO>();
            //.ForMember(dest=>dest.Employees,opt=>opt.MapFrom(src=>src.Employees))
            //.ForMember(dest=>dest.Projects,opt=>opt.MapFrom(src=>src.Projects));
            CreateMap<DepartmentDTO,Department>();
            CreateMap<Employee,EmployeeDTO>()
            .ForMember(dest=>dest.DepartmentName,opt=>opt.MapFrom(src=>src.Department.Name));
            CreateMap<EmployeeDTO,Employee>();
            CreateMap<Project,ProjectDTO>();
            //.ForMember(dest=>dest.DepartmentName,opt=>opt.MapFrom(src=>src.Department.Name));
            CreateMap<ProjectDTO,Project>();
            CreateMap<Project,ProjectWithEmployeeDTO>()
            .ForMember(dest=>dest.DepartmentName,opt=>opt.MapFrom(src=>src.Department.Name));
            CreateMap<Employee,EmployeeDTOWithoutProjects>();
        }
    }
}