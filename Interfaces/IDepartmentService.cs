using System.Collections.Generic;
using System.Threading.Tasks;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;

namespace EmpDepAPI.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> getDepartments();
        Task<IEnumerable<Department>> getDepatmentsByName();
        Task<Department> getDepartmentById(int id);
        Task<Department> addDepartment(Department department);
        Task<dynamic> updateDepartment(int id,Department department);
        Task<dynamic> deleteDepartment(int id);
        Task<dynamic> addEmployeeInDepartment(int id,List<Employee> employees);
        Task<dynamic> getProjectCount();
        Task<dynamic> getDepartmentWithProjectCost();
        Task<dynamic> getProjectsByDepartmentName(string name);
    }
}