using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EmpDepAPI.Data;
using EmpDepAPI.Entities;
using Microsoft.EntityFrameworkCore;
namespace EmpDepAPI.Repository
{
    public class DepartmentRepository
    {
        private readonly DataContext dbContext;

        public DepartmentRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<Department> addDepartmentAsync(Department department)
        {
            var exisitngDepartment=await dbContext.Departments.Where(s=>s.Name==department.Name).FirstOrDefaultAsync();
            if(exisitngDepartment!=null)
            return null;
            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();
            return department;
        }

        public async Task<dynamic> deleteDepartment(int id)
        {
            var dept=await dbContext.Departments.FindAsync(id);
            if(dept==null)
            return 1;
            dbContext.Departments.Remove(dept);
            await dbContext.SaveChangesAsync();
            return 2;
        }

        public async Task<Department> getDepartmentByIdAsync(int id)
        {
            var dept=await dbContext.Departments.FindAsync(id);
            if(dept==null)
            return null;
            return dept;
        }

        public async Task<IEnumerable<Department>> getDepartmentsAsync()
        {
            return await dbContext.Departments
                        .Include(s=>s.Employees)
                        .Include(s=>s.Projects)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<IEnumerable<Department>> getDepatmentsByNameAsync()
        {
            return await dbContext.Departments
                        .Include(s=>s.Employees)
                        .Include(s=>s.Projects)
                        .OrderBy(s=>s.Name)
                        .AsSplitQuery()
                        .AsNoTracking()
                        .ToListAsync();
        }

        public async Task<dynamic> updateDepartmentAsync(int id,Department department)
        {
            // var dept=await dbContext.Departments.FindAsync(department.Id);
            // dept.Name=department.Name;
            // dept.Employees.Clear();
            // foreach(var e in department.Employees)
            // dept.Employees.Add(e);
            //dbContext.Departments.Update(department);
            //  dbContext.Entry(department).State=EntityState.Modified;
            // foreach(var emp in department.Employees)
            // {
            //     dbContext.Entry(emp).State =EntityState.Modified;
            // }
            department.Id=id;
            var existingParent = dbContext.Departments
                .Where(p => p.Id == department.Id)
                .Include(p => p.Employees)
                .SingleOrDefault();

        if (existingParent != null)
        {
            // Update parent
            dbContext.Entry(existingParent).CurrentValues.SetValues(department);

            // Delete children
            // foreach (var existingChild in existingParent.Employees.ToList())
            // {
            //     if (!department.Employees.Any(c => c.Id == existingChild.Id))
            //         dbContext.Employees.Remove(existingChild);
            // }

            // Update and Insert children
            var dictionaryEmployee=new Dictionary<string,string>();
            List<Employee> invalidEmployee=new List<Employee>();
            List<Employee> employeeDeptNotSame=new List<Employee>();
            List<Employee> employeeWithNameExists=new List<Employee>();
            foreach (var childModel in department.Employees)
            {
                var existingChild = existingParent.Employees
                    .Where(c => c.Id == childModel.Id && c.Id != default(int))
                    .SingleOrDefault();

                if (existingChild != null)
                    {
                        var sameNameEmployee=await dbContext.Employees.AnyAsync(s=>s.FirstName==childModel.FirstName && s.LastName==childModel.LastName && s.Id!=childModel.Id);
                        if(sameNameEmployee)
                        employeeWithNameExists.Add(childModel);
                        else
                        dbContext.Entry(existingChild).CurrentValues.SetValues(childModel);
                    }
                else
                {
                    var isEmployee=await dbContext.Employees.FindAsync(childModel.Id);
                    if(isEmployee==null)
                    invalidEmployee.Add(childModel);
                    else
                    employeeDeptNotSame.Add(childModel);
                }
            }
            if(invalidEmployee.Count>0 || employeeDeptNotSame.Count>0 || employeeWithNameExists.Count>0)
            {
                if(invalidEmployee.Count>0)
                dictionaryEmployee.Add("Employees in the following list does not exist",JsonSerializer.Serialize(invalidEmployee));
                if(employeeDeptNotSame.Count>0)
                dictionaryEmployee.Add($"Employee in the following list does not exist in the department with id {department.Id} and name {department.Name}",JsonSerializer.Serialize(employeeDeptNotSame));
                if(employeeWithNameExists.Count>0)
                dictionaryEmployee.Add($"Employee in the following list with same name already exists!!",JsonSerializer.Serialize(employeeWithNameExists));
                return dictionaryEmployee;
            }
            else
                await dbContext.SaveChangesAsync();
                return null;
        }
        return new Dictionary<string,string>();
    }

        public async Task<dynamic> addEmployeeInDepartmentAsync(int id,List<Employee> employees)
        {
            var dept=await dbContext.Departments.FindAsync(id);
            var employeeList=await dbContext.Employees.ToListAsync();
            if(dept==null)
            return null;
            var len=dept.Employees.Count;
            List<Employee> lerr=new List<Employee>();
            List<Employee> ladd=new List<Employee>();
            foreach(var e in employees)
            {
                var isExistingEmployee=employeeList.Any(s=>s.FirstName==e.FirstName && s.LastName==e.LastName);
                if(isExistingEmployee)
                lerr.Add(e);
                else
                ladd.Add(e);
            }
            if(lerr.Count==0)
            {
                 foreach(var e in ladd)
                 dept.Employees.Add(e);
                 await dbContext.SaveChangesAsync();
                 return new List<Employee>();
            }
            else 
            return lerr;
        }
        public async Task<dynamic> getProjectCount()
        {
            var projectCountDepartmentList=await dbContext.Departments.Where(s=>s.Projects.Count>0).Select(d=>new{
                departmentid=d.Id,
                departmentname=d.Name,
                ProjectCount=d.Projects.Count
            }).ToListAsync();
            return projectCountDepartmentList;

            // var result=from p in dbContext.Projects join d in dbContext.Departments on p.Department equals d select new {departmentid=d.Id,
            //     departmentname=d.Name,
            //     ProjectCount=d.Projects.Count};
            //     return result;
        }

        public async Task<dynamic> getDepartmentWithProjectCostAsync()
        {
            return await dbContext.Departments.Where(s=>s.Projects.Count>0).Select(d=>new{
                departmentid=d.Id,
                departmentname=d.Name,
                TotalCost=d.Projects.Sum(s=>s.Cost)
            }).ToListAsync();
        }
        // public async Task<dynamic> getDepartmentWithProjectCostAsync()
        // {
        //     return await dbContext.Departments.Where(s=>s.Projects.Count>0 && s.Projects.Sum(a=>a.Cost)>0).Select(d=>new{
        //         departmentid=d.Id,
        //         departmentname=d.Name,
        //         TotalCost=d.Projects.Sum(s=>s.Cost)
        //     }).ToListAsync();
        // }
        public async Task<dynamic> getProjectsByDepartmentName(string name)
        {
            var result=await dbContext.Departments.Where(d=>d.Name==name).Select(e=>new{Projects=e.Projects}).ToListAsync();
            return result;
        }
    }
}