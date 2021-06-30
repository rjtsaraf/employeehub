using System.Collections.Generic;
using System.Threading.Tasks;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;
using EmpDepAPI.Interfaces;
using EmpDepAPI.Repository;

namespace EmpDepAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly DepartmentRepository departmentRepository;

        public DepartmentService(DepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }

        public async Task<Department> addDepartment(Department department)
        {
            return await departmentRepository.addDepartmentAsync(department);
        }

        public async Task<dynamic> addEmployeeInDepartment(int id, List<Employee> employees)
        {
            return await departmentRepository.addEmployeeInDepartmentAsync(id,employees);
        }

        public async Task<dynamic> deleteDepartment(int id)
        {
            return await departmentRepository.deleteDepartment(id);
        }

        public async Task<Department> getDepartmentById(int id)
        {
            return await departmentRepository.getDepartmentByIdAsync(id);
        }

        public async Task<IEnumerable<Department>> getDepartments()
        {
            return await departmentRepository.getDepartmentsAsync();
        }
        public async Task<IEnumerable<Department>> getDepatmentsByName()
        {
            return await departmentRepository.getDepatmentsByNameAsync();
        }

        public async Task<dynamic> updateDepartment(int id,Department department)
        {
            return await departmentRepository.updateDepartmentAsync(id,department);
        }
        public async Task<dynamic> getProjectCount()
        {
            return await departmentRepository.getProjectCount();
        }
        public async Task<dynamic> getDepartmentWithProjectCost()
        {
            return await departmentRepository.getDepartmentWithProjectCostAsync();
        }
        public async Task<dynamic> getProjectsByDepartmentName(string name)
        {
            return await departmentRepository.getProjectsByDepartmentName(name);
        }
    }
}