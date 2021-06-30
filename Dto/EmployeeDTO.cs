using System.Collections.Generic;

namespace EmpDepAPI.DTO
{
    public class EmployeeDTO
    {   
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long Salary{get;set;}
        public ICollection<ProjectDTO> Projects { get; set; }
    }
}