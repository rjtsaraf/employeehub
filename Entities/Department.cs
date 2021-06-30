using System.Collections.Generic;

namespace EmpDepAPI.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }=new List<Employee>();
        public ICollection<Project> Projects { get; set; }=new List<Project>();
    }
}