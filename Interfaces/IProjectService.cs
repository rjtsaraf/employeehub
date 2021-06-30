using System.Collections.Generic;
using System.Threading.Tasks;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace EmpDepAPI.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectWithEmployeeDTO>> getProjects();
        Task<ProjectWithEmployeeDTO> getProjectById(int id);
        Task<dynamic> addProject(Project project);
        Task<dynamic> updateProject(int id,Project project);
        Task<dynamic> updatePartialProject(int id,JsonPatchDocument<ProjectDTO> projectDTO);
        Task<dynamic> deleteProject(int id);
        Task<dynamic> mapEmployeesInProject(int id,List<EmployeeDTOWithoutProjects> employeeDTOWithoutProjects);
        Task<dynamic> updateEmployeesInProject(int id,List<EmployeeDTOWithoutProjects> employeeDTOWithoutProjects);
        Task<dynamic> deleteEmployeesInProject(int id,List<EmployeeDTO> employeeDTOs);
        Task<dynamic> getProjectWithEmployeeCount();
    }
}