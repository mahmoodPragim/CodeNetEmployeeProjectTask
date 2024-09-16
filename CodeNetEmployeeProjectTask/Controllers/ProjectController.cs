using Microsoft.AspNetCore.Mvc;
using CodeNetEmployeeProjectTask.Models;
using CodeNetEmployeeProjectTask.Services;
using CodeNetEmployeeProjectTask.Validators;
using System.ComponentModel.DataAnnotations;
using CodeNetEmployeeProjectTask.DTO;
using FluentValidation;

namespace CodeNetEmployeeProjectTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ProjectValidator _validator;

        public ProjectController(IProjectService projectService, ProjectValidator validator)
        {
            _projectService = projectService;
            _validator = validator;

        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Project>>> PostProject(ProjectCreateDto projectDto)
        {
            Project project = new Project {
                ProjectId = projectDto.ProjectId.HasValue ?  projectDto.ProjectId.Value : 0,
                Name = projectDto.Name,
                Description=projectDto.Description
            };

            var validationResult = _validator.Validate(project);

            if (!validationResult.IsValid)
            {
                var response = new ApiResponse<Project>
                {
                    Status = "Error",
                    Data = null,
                    Error = "Validation failed",
                    Details = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).AsEnumerable())
                };

                return BadRequest(response);
            }

            if (projectDto.ProjectId.HasValue)
            {
                var existingProject = await _projectService.GetProjectByIdAsync(projectDto.ProjectId.Value);

                if (existingProject == null)
                {
                    return NotFound(new ApiResponse<Project>
                    {
                        Status = "Error",
                        Data = null,
                        Error = "Project not found."
                    });
                }

                existingProject.Name = projectDto.Name;
                existingProject.Description = projectDto.Description;

                await _projectService.UpdateProjectAsync(existingProject);

                var response = new ApiResponse<Project>
                {
                    Data = existingProject,
                    Status = "Success",
                    Error = null
                };

                return Ok(response);
            }
            else
            {
                var newProject = new Project
                {
                    Name = projectDto.Name,
                    Description = projectDto.Description
                };

                var createdProject = await _projectService.AddProjectAsync(newProject);

                var response = new ApiResponse<Project>
                {
                    Data = createdProject,
                    Status = "Success",
                    Error = null
                };

                return CreatedAtAction(nameof(GetProject), new { id = createdProject.ProjectId }, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }
    }
}
