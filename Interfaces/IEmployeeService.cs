using System.Collections.Generic;
using System.Threading.Tasks;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace EmpDepAPI.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> getEmployees();
        Task<EmployeeDTO> getEmployeeById(int id);
        Task<EmployeeDTO> addEmployee(Employee employee);
        Task<dynamic> updateEmployee(int id,Employee employee);
        Task<dynamic> updatePartialEmployee(int id,JsonPatchDocument<EmployeeDTO> employeeDTO);
        Task<dynamic> nthHighestSalary(int n);
        Task<dynamic> employeesBySalary(long salary);
        Task<dynamic> deleteEmployee(int id);
        Task<dynamic> mapProjectsInEmployee(int id,List<ProjectDTO> projectDTOs);
        Task<dynamic> updateProjectsInEmployee(int id,List<ProjectDTO> projectDTOs);
        Task<dynamic> deleteProjectsInEmployee(int id,List<ProjectDTO> projectDTOs);
        Task<dynamic> getEmployeewithProjectCount();
        Task<dynamic> getEmployeesNotInProjectCount();
    }
}