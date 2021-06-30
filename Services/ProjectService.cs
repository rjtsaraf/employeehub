using System.Collections.Generic;
using System.Threading.Tasks;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;
using EmpDepAPI.Interfaces;
using EmpDepAPI.Repository;
using Microsoft.AspNetCore.JsonPatch;

namespace EmpDepAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ProjectRepository projectRepository;

        public ProjectService(ProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        public async Task<dynamic> addProject(Project project)
        {
            return await projectRepository.addProjectAsync(project);
        }

        public async Task<dynamic> deleteProject(int id)
        {
            return await projectRepository.deleteProjectAsync(id);
        }

        public async Task<ProjectWithEmployeeDTO> getProjectById(int id)
        {
            return await projectRepository.getProjectByIdAsync(id);
        }

        public async Task<IEnumerable<ProjectWithEmployeeDTO>> getProjects()
        {
            return await projectRepository.getProjectsAsync();
        }

        public async Task<dynamic> mapEmployeesInProject(int id, List<EmployeeDTOWithoutProjects> employeeDTOWithoutProjects)
        {
            return await projectRepository.mapEmployeesInProject(id,employeeDTOWithoutProjects);
        }
        public async Task<dynamic> updateEmployeesInProject(int id,List<EmployeeDTOWithoutProjects> employeeDTOWithoutProjects)
        {
            return await projectRepository.updateEmployeesInProject(id,employeeDTOWithoutProjects);
        }

        public async Task<dynamic> updateProject(int id, Project project)
        {
            return await projectRepository.updateProjectAsync(id,project);
        }
        public async Task<dynamic> updatePartialProject(int id,JsonPatchDocument<ProjectDTO> projectDTO)
        {
            return await projectRepository.updatePartialProjectAsync(id,projectDTO);
        }
        public async Task<dynamic> deleteEmployeesInProject(int id,List<EmployeeDTO> employeeDTOs)
        {
            return await projectRepository.deleteEmployeesInProject(id,employeeDTOs);
        }
        public async Task<dynamic> getProjectWithEmployeeCount()
        {
            return await projectRepository.getProjectWithEmployeeCount();
        }
    }
}