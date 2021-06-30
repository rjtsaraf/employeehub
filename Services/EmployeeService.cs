using System.Collections.Generic;
using System.Threading.Tasks;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;
using EmpDepAPI.Interfaces;
using EmpDepAPI.Repository;

namespace EmpDepAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeRepository employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDTO> addEmployee(Employee employee)
        {
            return await employeeRepository.addEmployeeAsync(employee);
        }

        public async Task<dynamic> deleteEmployee(int id)
        {
            return await employeeRepository.deleteEmployee(id);
        }

        public async Task<EmployeeDTO> getEmployeeById(int id)
        {
            return await employeeRepository.getEmployeeByIdAsync(id);
        }

        public async Task<IEnumerable<EmployeeDTO>> getEmployees()
        {
            return await employeeRepository.getEmployeesAsync();
        }

        public async Task<dynamic> mapProjectsInEmployee(int id, List<ProjectDTO> projectDTOs)
        {
            return await employeeRepository.mapProjectsInEmployee(id,projectDTOs);
        }
         public async Task<dynamic> updateProjectsInEmployee(int id,List<ProjectDTO> projectDTOs)
         {
             return await employeeRepository.updateProjectsInEmployee(id,projectDTOs);
         }
        public async Task<dynamic> updateEmployee(int id,Employee employee)
        {
            return await employeeRepository.updateEmployee(id,employee);
        }
        public async Task<dynamic> deleteProjectsInEmployee(int id,List<ProjectDTO> projectDTOs)
        {
            return await employeeRepository.deleteProjectsInEmployee(id,projectDTOs);
        }
        public async Task<dynamic> getEmployeewithProjectCount()
        {
            return await employeeRepository.EmployeewithProjectCount();
        }
        public async Task<dynamic> getEmployeesNotInProjectCount()
        {
            return await employeeRepository.EmployeesNotInProjectCount();
        }
        
    }
}