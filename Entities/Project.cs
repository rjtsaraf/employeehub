using System.Collections.Generic;

namespace EmpDepAPI.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Employee> Employees { get; set; }=new List<Employee>();
        public long Cost{get;set;}
        //public ICollection<EmployeeProject> EmployeesProjects { get; set; }=new List<EmployeeProject>();
    }
}