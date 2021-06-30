using System.Collections.Generic;

namespace EmpDepAPI.DTO
{
    public class ProjectWithEmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long Cost{get;set;}
        public ICollection<EmployeeDTOWithoutProjects> Employees { get; set;} 
    }
}