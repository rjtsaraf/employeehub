using System.Collections.Generic;

namespace EmpDepAPI.DTO
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<EmployeeDTO> Employees { get; set; }
        public ICollection<ProjectDTO> Projects { get; set; }
    }
}