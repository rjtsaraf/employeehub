using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;
using EmpDepAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace EmpDepAPI.Controllers
{
    public class DepartmentsController : BaseApiController
    {
        private readonly IDepartmentService departmentService;
        private readonly IMapper mapper;

        public DepartmentsController(IDepartmentService departmentService, IMapper mapper)
        {
            this.departmentService = departmentService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartments()
        {
            var departments=await departmentService.getDepartments();
            var departmentsToReturn=mapper.Map<IEnumerable<DepartmentDTO>>(departments);
            return Ok(departmentsToReturn);

        }

        [HttpGet("byname")]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetDepartmentsByName()
        {
            var departments=await departmentService.getDepatmentsByName();
            var departmentsToReturn=mapper.Map<IEnumerable<DepartmentDTO>>(departments);
            return Ok(departmentsToReturn);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartmentById(int id)
        {
            var department=await departmentService.getDepartmentById(id);
            if(department==null)
            return NotFound($"Department with id {id} does not exist");
            var departmentToReturn=mapper.Map<DepartmentDTO>(department);
            return Ok(department);
            
        }
        [HttpPost(Name ="AddDepartment")]
        public async Task<ActionResult<DepartmentDTO>> AddDepartment(DepartmentDTO departmentDTO)
        {
            var department=mapper.Map<Department>(departmentDTO);
            var dept=await departmentService.addDepartment(department);
            if(dept==null)
            return BadRequest($"Department with the name {departmentDTO.Name} already exists");
            var departmentToReturn=mapper.Map<DepartmentDTO>(dept);
            return Created("AddDepartment",departmentToReturn);
            
        }
        [HttpPost("{id}/Employees",Name ="EmployeeInDepartment")]
        public async Task<ActionResult<dynamic>> AddEmployeeInDepartment(int id,List<EmployeeDTO> employeesDTO)
        {
            var employees=mapper.Map<List<Employee>>(employeesDTO);
            IList<Employee> employeesList=await departmentService.addEmployeeInDepartment(id,employees);
            if(employeesList==null)
            return NotFound($"Deparment with id {id} does not exist!!!");
            if(employeesList.Count!=0)
            {
                var json = JsonSerializer.Serialize(employeesList);
                return BadRequest("Employees with the following names already exists!!! "+json);
            }
            else
            return NoContent();
            //var deptToReturn=mapper.Map<DepartmentDTO>(dept);
            //return Created("AddEmployeeInDepartment",deptToReturn);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<dynamic>> UpdateDepartment(int id,Department department)
        {
            //var deptfromdto=mapper.Map<Department>(departmentDTO);
            var dept=await departmentService.updateDepartment(id,department);
            if(dept==null)
            return Ok("Department data updated successfully");
            if(dept.Count==0)
            return NotFound($"Department with id {id} not found");
            else if(dept.Count>=0)
            return BadRequest(dept);
            var deptToReturn=mapper.Map<DepartmentDTO>(dept);
            return Ok(deptToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<dynamic>> DeleteDepartment(int id)
        {
            var emp=await departmentService.deleteDepartment(id);
            if(emp==1)
            return NotFound($"Department with id {id} not found");
            //var empToReturn =mapper.Map<EmployeeDTO>(emp);
            return Ok($"Department with id {id} deleted successfully!!!");
        }
        
        [HttpGet("projectCount")]
        public async Task<ActionResult<dynamic>> GetProjectCount()
        {
            return Ok(await departmentService.getProjectCount());
        }

        [HttpGet("projectcost")]
        public async Task<ActionResult<dynamic>> GetProjectCost()
        {
            return Ok(await departmentService.getDepartmentWithProjectCost());
        }
        
        [HttpGet("projects")]
        public async Task<ActionResult<dynamic>> GetProjectsByDepartmentName(string dname)
        {
            return Ok(await departmentService.getProjectsByDepartmentName(dname));
        }
    }
}