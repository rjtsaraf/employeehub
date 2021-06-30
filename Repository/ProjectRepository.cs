using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmpDepAPI.Data;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace EmpDepAPI.Repository
{
    public class ProjectRepository
    {
        private readonly DataContext dbContext;
        private readonly IMapper mapper;

        public ProjectRepository(DataContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProjectWithEmployeeDTO>> getProjectsAsync()
        {
            // return await dbContext.Projects
            //         .Include(s=>s.Department)
            //         .Include(s=>s.Employees)
            //         .AsSplitQuery()
            //         .AsNoTracking()
            //         .ToListAsync();

            return await dbContext.Projects
                        .ProjectTo<ProjectWithEmployeeDTO>(mapper.ConfigurationProvider)
                        .ToListAsync();
        }

        public async Task<ProjectWithEmployeeDTO> getProjectByIdAsync(int id)
        {
            // var project=await dbContext.Projects
            //             .Include(s=>s.Department)
            //             .Include(s=>s.Employees)
            //             .FirstOrDefaultAsync(s=>s.Id==id);

            var project=await dbContext.Projects
                            .ProjectTo<ProjectWithEmployeeDTO>(mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(s=>s.Id==id);
            if(project==null)
            return null;
            return project;

        }

         public async Task<dynamic> addProjectAsync(Project project)
        {
            var existingProject=await dbContext.Projects.Where(s=>s.Name==project.Name).FirstOrDefaultAsync();
            if(existingProject!=null)
            return -1;
            var isDepartmentExist=await dbContext.Departments.FindAsync(project.DepartmentId);
            if(isDepartmentExist==null)
            return -2;
           dbContext.Projects.Add(project);
            await dbContext.SaveChangesAsync();
            return project.Id;
        }

        public async Task<dynamic> updateProjectAsync(int id, Project project)
        {
            var proj=await dbContext.Projects.FindAsync(id);
            if(proj==null)
            return 1;
            var existingProject=await dbContext.Projects.Where(s=>s.Name==project.Name && s.Id!=id).FirstOrDefaultAsync();
            if(existingProject!=null)
            return 2;
            var depttoUpdate=await dbContext.Departments.FindAsync(project.DepartmentId);
            if(depttoUpdate==null)
            return 3;
            project.Id=id;
            dbContext.Entry(proj).CurrentValues.SetValues(project);
            //dbContext.Entry(project).State=EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return 4;
        }

         public async Task<dynamic> updatePartialProjectAsync(int id,JsonPatchDocument<ProjectDTO> projectDTO)
         {
             var proj=await dbContext.Projects.FindAsync(id);
              if(proj==null)
              return 1;
              var existingProject=await dbContext.Projects.Where(s=>s.Name==proj.Name && s.Id!=id).FirstOrDefaultAsync();
              if(existingProject!=null)
              return 2;
              var dto=mapper.Map<ProjectDTO>(proj);
              projectDTO.ApplyTo(dto);
              //project.ApplyTo(proj);
              mapper.Map(dto,proj);
              dbContext.Update(proj);
             await dbContext.SaveChangesAsync();
             return 4;
         }

        public async Task<dynamic> deleteProjectAsync(int id)
        {
            var proj=await dbContext.Projects.FindAsync(id);
            if(proj==null)
            return 1;
            dbContext.Projects.Remove(proj);
            await dbContext.SaveChangesAsync();
            return 2;
        }
        public async Task<dynamic> mapEmployeesInProject(int id,List<EmployeeDTOWithoutProjects> employeeDTOWithoutProjects)
        {
            List<EmployeeDTOWithoutProjects> employeesNotExist=new List<EmployeeDTOWithoutProjects>(); 
            List<Employee> employeesExist=new List<Employee>(); 
            var project=await dbContext.Projects.FindAsync(id);
            if(project==null)
            return new List<EmployeeDTOWithoutProjects>();
            foreach(var emp in employeeDTOWithoutProjects)
            {
                var employee=await dbContext.Employees.FindAsync(emp.Id);
                if(employee==null)
                employeesNotExist.Add(emp);
                else
                employeesExist.Add(employee);
                
            }
            if(employeesNotExist.Count>0)
            return employeesNotExist;
            foreach(var employee in employeesExist)
            project.Employees.Add(employee);
            await dbContext.SaveChangesAsync();
            return null;
        }
        public async Task<dynamic> updateEmployeesInProject(int id,List<EmployeeDTOWithoutProjects> employeeDTOWithoutProjects)
        {
            List<EmployeeDTOWithoutProjects> employeesNotExist=new List<EmployeeDTOWithoutProjects>(); 
            List<Employee> employeesExist=new List<Employee>(); 
            var project=await dbContext.Projects.Include(s=>s.Employees).Where(s=>s.Id==id).FirstOrDefaultAsync();
            if(project==null)
            return new List<EmployeeDTOWithoutProjects>();
            foreach(var emp in employeeDTOWithoutProjects)
            {
                var employee=await dbContext.Employees.FindAsync(emp.Id);
                if(employee==null)
                employeesNotExist.Add(emp);
                else
                employeesExist.Add(employee);
                
            }
            if(employeesNotExist.Count>0)
            return employeesNotExist;
            project.Employees.Clear();
            foreach(var employee in employeesExist)
            project.Employees.Add(employee);
            await dbContext.SaveChangesAsync();
            return null;
        }
        public async Task<dynamic> deleteEmployeesInProject(int id,List<EmployeeDTO> employeeDTOs)
        {
            var project=await dbContext.Projects.Include(s=>s.Employees).Where(s=>s.Id==id).FirstOrDefaultAsync();
            if(project==null)
            return null;
            var dictionaryEmployees=new Dictionary<string,string>();
            List<Employee>removeEmployees=new List<Employee>();
            List<EmployeeDTO> invalidEmployee=new List<EmployeeDTO>();
            List<Employee> employeeNotMappedInSameProject=new List<Employee>();
            foreach(var emp in employeeDTOs)
            {
                var existingEmployeeInProject=project.Employees.Where(s=>s.Id==emp.Id).SingleOrDefault();
                if(existingEmployeeInProject!=null)
                {
                    removeEmployees.Add(existingEmployeeInProject);
                }
                else
                {
                    var validEmployee=await dbContext.Employees.FindAsync(emp.Id);
                    if(validEmployee!=null)
                    employeeNotMappedInSameProject.Add(validEmployee);
                    else
                    invalidEmployee.Add(emp);
                }
            }
            if(invalidEmployee.Count>0 || employeeNotMappedInSameProject.Count>0)
            {
                if(invalidEmployee.Count>0)
                dictionaryEmployees.Add("Employees in the following list does not exist",JsonSerializer.Serialize(invalidEmployee));
                if(employeeNotMappedInSameProject.Count>0)
                dictionaryEmployees.Add($"Employees in the following list does not belong to project {project.Name}",JsonSerializer.Serialize(employeeNotMappedInSameProject));
                return dictionaryEmployees;
            }
            else
            {
                foreach(var emp in removeEmployees)
                project.Employees.Remove(emp);
                await dbContext.SaveChangesAsync();
                return new Dictionary<string,string>();
            }
        }
        public async Task<dynamic> getProjectWithEmployeeCount()
        {
            var result=await dbContext.Projects.Select(e=>new{
                ProjectName=e.Name,
                EmployeeCount=e.Employees.Count
            }).ToListAsync();
            return result;
        }
    }
}