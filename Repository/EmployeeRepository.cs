using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmpDepAPI.Data;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmpDepAPI.Repository
{
    public class EmployeeRepository
    {
        private readonly DataContext dbContext;
        private readonly IMapper mapper;

        public EmployeeRepository(DataContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<EmployeeDTO> addEmployeeAsync(Employee employee)
        {
            var existingEmployee=await dbContext.Employees.Where(s=>s.FirstName==employee.FirstName && s.LastName==employee.LastName).FirstOrDefaultAsync();
            if(existingEmployee!=null)
            return null;
            dbContext.Employees.Add(employee);
            await dbContext.SaveChangesAsync();
            return await getEmployeeByIdAsync(employee.Id);
        }

        public async Task<dynamic> deleteEmployee(int id)
        {
            var emp=await dbContext.Employees.FindAsync(id);
            if(emp==null)
            return 1;
            dbContext.Employees.Remove(emp);
            await dbContext.SaveChangesAsync();
            return 2;
        }

        public async Task<EmployeeDTO> getEmployeeByIdAsync(int id)
        {

            // return await dbContext.Employees
            //         .Include(s=>s.Department)
            //         .Include(s=>s.Projects)
            //         .AsNoTracking()
            //         .Where(s=>s.Id==id)
            //         .FirstOrDefaultAsync();
            //  var query= dbContext.Employees.AsQueryable();
            //  query=query.Include(s=>s.Department);
            //  return await query.Where(s=>s.Id==id).FirstOrDefaultAsync();

            var employee= await dbContext.Employees
                        .Where(s=>s.Id==id)
                        .ProjectTo<EmployeeDTO>(mapper.ConfigurationProvider)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
            if(employee==null)
            return null;
            return employee;
            // mapper.Map<Employee>(dto);
             

        }

        public async Task<IEnumerable<EmployeeDTO>> getEmployeesAsync()
        {
            // return await dbContext.Employees
            // .Include(s=>s.Department)
            // .Include(s=>s.Projects)
            // .AsSplitQuery()
            // .AsNoTracking()
            // .ToListAsync();
            return await dbContext.Employees
                    .ProjectTo<EmployeeDTO>(mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<dynamic> updateEmployee(int id,Employee employee)
        {
            // var emp=await dbContext.Employees.FindAsync(id);
            var emp=await dbContext.Employees.FindAsync(id);
            if(emp==null)
            return 1;
            var existingEmployee=await dbContext.Employees.Where(s=>s.FirstName==employee.FirstName && s.LastName==employee.LastName && s.Id!=id).FirstOrDefaultAsync();
            if(existingEmployee!=null)
            return 2;
            var depttoUpdate=await dbContext.Departments.FindAsync(employee.DepartmentId);
            if(depttoUpdate==null)
            return 3;

            //Method 1
            emp.FirstName=employee.FirstName;
            emp.LastName=employee.LastName;
            emp.DepartmentId=employee.DepartmentId;

            //Method 2
            //employee.Id=id;
            //dbContext.Entry(employee).State=EntityState.Modified;  --> Not working in this case

            //dbContext.Entry(emp).CurrentValues.SetValues(employee); --> Not working in this case
            await dbContext.SaveChangesAsync();
            return 4;
        }
        public async Task<dynamic> mapProjectsInEmployee(int id,List<ProjectDTO> projectDTOs)
        {
            List<ProjectDTO> projectNotExist=new List<ProjectDTO>(); 
            List<Project> projectExist=new List<Project>(); 
            var employee=await dbContext.Employees.FindAsync(id);
            if(employee==null)
            return new List<ProjectDTO>();
            foreach(var proj in projectDTOs)
            {
                var project=await dbContext.Projects.FindAsync(proj.Id);
                if(project==null)
                projectNotExist.Add(proj);
                else
                projectExist.Add(project);
                
            }
            if(projectNotExist.Count>0)
            return projectNotExist;
            foreach(var project in projectExist)
            employee.Projects.Add(project);
            await dbContext.SaveChangesAsync();
            return null;
        }
        public async Task<dynamic> updateProjectsInEmployee(int id,List<ProjectDTO> projectDTOs)
        {
            List<ProjectDTO> projectNotExist=new List<ProjectDTO>(); 
            List<Project> projectExist=new List<Project>(); 
            var employee=await dbContext.Employees.Include(s=>s.Projects).Where(s=>s.Id==id).FirstOrDefaultAsync();
            if(employee==null)
            return new List<ProjectDTO>();
            foreach(var proj in projectDTOs)
            {
                var project=await dbContext.Projects.FindAsync(proj.Id);
                if(project==null)
                projectNotExist.Add(proj);
                else
                projectExist.Add(project);
                
            }
            if(projectNotExist.Count>0)
            return projectNotExist;
            employee.Projects.Clear();
            foreach(var project in projectExist)
            employee.Projects.Add(project);
            await dbContext.SaveChangesAsync();
            return null;
        }
        public async Task<dynamic> deleteProjectsInEmployee(int id,List<ProjectDTO> projectDTOs)
        {
            var employee=await dbContext.Employees.Include(s=>s.Projects).Where(s=>s.Id==id).FirstOrDefaultAsync();
            if(employee==null)
            return null;
            var dictionaryProjects=new Dictionary<string,string>();
            List<Project>removeProjects=new List<Project>();
            List<ProjectDTO> invalidProject=new List<ProjectDTO>();
            List<Project> projectNotMappedInSameEmployee=new List<Project>();
            foreach(var proj in projectDTOs)
            {
                var existingEmployeeProject=employee.Projects.Where(s=>s.Id==proj.Id).SingleOrDefault();
                if(existingEmployeeProject!=null)
                {
                    removeProjects.Add(existingEmployeeProject);
                }
                else
                {
                    var validProject=await dbContext.Projects.FindAsync(proj.Id);
                    if(validProject!=null)
                    projectNotMappedInSameEmployee.Add(validProject);
                    else
                    invalidProject.Add(proj);
                }
            }
            if(invalidProject.Count>0 || projectNotMappedInSameEmployee.Count>0)
            {
                if(invalidProject.Count>0)
                dictionaryProjects.Add("Projects in the following list does not exist",JsonSerializer.Serialize(invalidProject));
                if(projectNotMappedInSameEmployee.Count>0)
                dictionaryProjects.Add($"Projects in the following list does not belong to employee {employee.FirstName}",JsonSerializer.Serialize(projectNotMappedInSameEmployee));
                return dictionaryProjects;
            }
            else
            {
                foreach(var proj in removeProjects)
                employee.Projects.Remove(proj);
                await dbContext.SaveChangesAsync();
                return new Dictionary<string,string>();
            }
        }

        public async Task<dynamic> EmployeewithProjectCount()
        {
            var result=await dbContext.Employees.Select(e=>new{
                EmployeeName=e.FirstName+" "+e.LastName,
                ProjectCount=e.Projects.Count
            }).ToListAsync();
            return result;
        }

        public async Task<dynamic> EmployeesNotInProjectCount()
        {
            var result=await dbContext.Employees.Where(e=>e.Projects.Count==0).Select(e=>new{
                EmployeeName=e.FirstName+" "+e.LastName,
                ProjectCount=e.Projects.Count
            }).ToListAsync();
            return result;
        }
    }
}