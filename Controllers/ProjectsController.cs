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
    public class ProjectsController : BaseApiController
    {
        private readonly IProjectService projectService;
        private readonly IMapper mapper;

        public ProjectsController(IProjectService projectService, IMapper mapper)
        {
            this.projectService = projectService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ProjectWithEmployeeDTO>> GetProjects()
        {
            var projects=await projectService.getProjects();
            //var projectsToReturn=mapper.Map<IEnumerable<ProjectWithEmployeeDTO>>(projects);
            return Ok(projects);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectWithEmployeeDTO>> GetProjectsById(int id)
        {
            var project=await projectService.getProjectById(id);
            if(project==null)
            return NotFound($"Project with id {id} not found");
            //var projectToReturn=mapper.Map<ProjectWithEmployeeDTO>(project);
            return Ok(project);
            
        }
        [HttpPost(Name ="AddProject")]
        public async Task<ActionResult<dynamic>> AddProject(ProjectDTO projectDTO)
        {
            var proj=mapper.Map<Project>(projectDTO);
            var resproj=await projectService.addProject(proj);
            if(resproj==-1)
            return BadRequest("Project with the same name already exists!!!");
            else if(resproj==-2)
            return BadRequest($"Department with id {projectDTO.DepartmentId} not found!!!");
            else
            {
                var resdata=await projectService.getProjectById(resproj);
                //var projectToReturn=mapper.Map<ProjectDTO>(resdata);
                return Created("AddProject",resdata);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<dynamic>> UpdateProjecte(int id,ProjectDTO projectDTO)
        {
            var proj=mapper.Map<Project>(projectDTO);
            var resproj=await projectService.updateProject(id,proj);
            if(resproj==1)
            return NotFound($"Project with id {id} does not exists");
            else if(resproj==2)
            return BadRequest("Project with the same name already exists!!!");
            else if(resproj==3)
            return NotFound($"Department with id {projectDTO.DepartmentId} does not exist!!!");
            return Ok($"Project with id {id} updated successfully");
            
        }
        [HttpPatch("updatePartial/{id}")]
        public async Task<ActionResult<dynamic>> updatePartialProject(int id, [FromBody] JsonPatchDocument<ProjectDTO> projectDTO)
        {
             var resproj=await projectService.updatePartialProject(id,projectDTO);
             if(resproj==1)
            return NotFound($"Project with id {id} does not exists");
            else if(resproj==2)
            return BadRequest("Project with the same name already exists!!!");
            return Ok($"Project with id {id} updated successfully");
        }

        [HttpPost("{id}/employees")]
        public async Task<ActionResult<dynamic>> mapEmployeesInProject(int id,List<EmployeeDTOWithoutProjects> employeeDTOWithoutProjects)
        {
            var res=await projectService.mapEmployeesInProject(id,employeeDTOWithoutProjects);
            if(res==null)
            return Ok("Employees mapped successfully");
            if(res.Count==0)
            return NotFound($"Project with id {id} not found!!!");
            return BadRequest("Employees in the following does not exist"+JsonSerializer.Serialize(res));

        }
        [HttpPut("{id}/employees")]
        public async Task<ActionResult<dynamic>> updateEmployeesInProject(int id,List<EmployeeDTOWithoutProjects> employeeDTOWithoutProjects)
        {
            var res=await projectService.updateEmployeesInProject(id,employeeDTOWithoutProjects);
            if(res==null)
            return Ok("Employees updated successfully");
            if(res.Count==0)
            return NotFound($"Project with id {id} not found!!!");
            return BadRequest("Employees in the following does not exist"+JsonSerializer.Serialize(res));

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<dynamic>> DeleteProject(int id)
        {
            var proj=await projectService.deleteProject(id);
            if(proj==1)
            return NotFound($"Project with id {id} not found");
            return Ok($"Project with id {id} deleted successfully!!!");
        }
         [HttpDelete("{id}/employees")]
        public async Task<ActionResult<dynamic>> deleteEmployeesInProject(int id,List<EmployeeDTO> employeeDTOs)
        {
            var res=await projectService.deleteEmployeesInProject(id,employeeDTOs);
            if(res==null)
            return BadRequest($"Project with id {id} not found!!!");
            if(res.Count==0)
            return Ok("Employees deleted successfully");
            return BadRequest(res);

        }
         [HttpGet("employeeCount")]
        public async Task<ActionResult<dynamic>> getProjectEmployeesCount()
        {
            return Ok(await projectService.getProjectWithEmployeeCount());
        }
    }
}