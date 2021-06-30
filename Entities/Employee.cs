using System.Collections.Generic;

namespace EmpDepAPI.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public long Salary{get;set;}
        public ICollection<Project> Projects { get; set; }=new List<Project>();
        // public ICollection<EmployeeProject> EmployeesProjects { get; set; }=new List<EmployeeProject>();
    }
}