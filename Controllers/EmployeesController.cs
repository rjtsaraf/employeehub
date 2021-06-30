using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using EmpDepAPI.DTO;
using EmpDepAPI.Entities;
using EmpDepAPI.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EmpDepAPI.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private readonly IEmployeeService employeeService;
        private readonly IMapper mapper;

        public EmployeesController(IEmployeeService employeeService,IMapper mapper)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<EmployeeDTO>> GetEmployees()
        {
            var employees=await employeeService.getEmployees();
            //List<EmployeeDTO> employees1=new List<EmployeeDTO>();
            //var employeesToReturn=mapper.Map<IEnumerable<EmployeeDTO>>(employees);
            // foreach(var emp in employees)
            // {
            //     EmployeeDTO dto=new EmployeeDTO();
            //     dto.FirstName=emp.FirstName;
            //     dto.LastName=emp.LastName;
            //     dto.DepartmentId=emp.DepartmentId;
            //     dto.DepartmentName=emp.Department.Name;
            //     employees1.Add(dto);
            // }
            return Ok(employees);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeesById(int id)
        {
            var employee=await employeeService.getEmployeeById(id);
            //var employeeToReturn=mapper.Map<EmployeeDTO>(employee);
            return Ok(employee);
            
        }
        [HttpPost(Name ="AddEmployee")]
        public async Task<ActionResult<EmployeeDTO>> AddEmployee(EmployeeDTO employeeDTO)
        {
            var emp=mapper.Map<Employee>(employeeDTO);
            var resemp=await employeeService.addEmployee(emp);
            if(resemp==null)
            return BadRequest("User with the same FirstName and LastName already exists!!!");
            //var employeeToReturn=mapper.Map<EmployeeDTO>(resemp);
            return Created("AddEmployee",resemp);
            
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<dynamic>> UpdateEmployee(int id,EmployeeDTO employeeDTO)
        {
            var emp=mapper.Map<Employee>(employeeDTO);
            var resemp=await employeeService.updateEmployee(id,emp);
            if(resemp==1)
            return NotFound($"Employee with id {id} does not exists");
            else if(resemp==2)
            return BadRequest("User with the same FirstName and LastName already exists!!!");
            else if(resemp==3)
            return NotFound($"Department with id {employeeDTO.DepartmentId} does not exist!!!");
            //  var employeeToReturn=mapper.Map<EmployeeDTO>(resemp);
            return Ok($"Employee with id {id} updated successfully");
            
        }

        [HttpPatch("updatePartial/{id}")]
        public async Task<ActionResult<dynamic>> UpdatePartialEmployee(int id,JsonPatchDocument<EmployeeDTO> employeeDTO)
        {
            var resemp=await employeeService.updatePartialEmployee(id,employeeDTO);
            if(resemp==1)
            return NotFound($"Employee with id {id} does not exists");
            //  var employeeToReturn=mapper.Map<EmployeeDTO>(resemp);
            return Ok($"Employee with id {id} updated successfully");
            
        }

        [HttpGet("highestsalary/{number}")]
        public async Task<ActionResult<dynamic>> NthHighestSalary(int number)
        {
            var res=await employeeService.nthHighestSalary(number);
            return Ok(res);
        }
        [HttpGet("bysalary")]
        public async Task<ActionResult<dynamic>> EmployeebySalary(long salary)
        {
            var res=await employeeService.employeesBySalary(salary);
            return Ok(res);
        }
        [HttpPost("{id}/projects")]
        public async Task<ActionResult<dynamic>> mapProjectsInEmployee(int id,List<ProjectDTO> projectDTOs)
        {
            var res=await employeeService.mapProjectsInEmployee(id,projectDTOs);
            if(res==null)
            return Ok("Projects mapped successfully");
            if(res.Count==0)
            return NotFound($"Employee with id {id} not found!!!");
            return BadRequest("Projects in the following does not exist"+JsonSerializer.Serialize(res));

        }
        [HttpPut("{id}/projects")]
        public async Task<ActionResult<dynamic>> updateProjectsInEmployee(int id,List<ProjectDTO> projectDTOs)
        {
            var res=await employeeService.updateProjectsInEmployee(id,projectDTOs);
            if(res==null)
            return Ok("Projects updated successfully");
            if(res.Count==0)
            return NotFound($"Employee with id {id} not found!!!");
            return BadRequest("Projects in the following does not exist"+JsonSerializer.Serialize(res));

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<dynamic>> DeleteEmployee(int id)
        {
            var emp=await employeeService.deleteEmployee(id);
            if(emp==1)
            return NotFound($"Employee with id {id} not found");
            //var empToReturn =mapper.Map<EmployeeDTO>(emp);
            return Ok($"Employee with id {id} deleted successfully!!!");
        }
        [HttpDelete("{id}/projects")]
        public async Task<ActionResult<dynamic>> deleteProjectsInEmployee(int id,List<ProjectDTO> projectDTOs)
        {
            var res=await employeeService.deleteProjectsInEmployee(id,projectDTOs);
            if(res==null)
            return BadRequest($"Employee with id {id} not found!!!");
            if(res.Count==0)
            return Ok("Projects deleted successfully");
            return BadRequest(res);

        }
        [HttpGet("projectCount")]
        public async Task<ActionResult<dynamic>> getEmployeesProjectCount()
        {
            return Ok(await employeeService.getEmployeewithProjectCount());
        }
        [HttpGet("notTaggedprojectCount")]
        public async Task<ActionResult<dynamic>> getEmployeesNotInProjectCount()
        {
            return Ok(await employeeService.getEmployeesNotInProjectCount());
        }
    }
}