using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiBase.Repository.Models;
using ApiBase.Service.Services.PriorityService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using ApiBase.Service.ViewModels.ProjectViewModel;
using ApiBase.Service.ViewModels.Task;
using bookingticketAPI.Filter;
using ApiBase.Service.Constants;
using ApiBase.Service.ViewModels;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using ApiBase.Service.Services.UserService;
using ApiBase.Repository.Repository;
using Project = ApiBase.Repository.Models.Project;

namespace ApiBase.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FilterTokenCyber]
    public class ProjectController : ControllerBase
    {
        IProjectService _projectService;
        IUserService _userService;
        IProjectRepository _projectRepository;
        public ProjectController(IProjectService projectService ,IUserService userService, IProjectRepository projectRepository)
        {
            _projectService = projectService;
            _userService = userService;
            _projectRepository = projectRepository;
        }


        [HttpPost("createProject")]
        public async Task<IActionResult> createProject([FromBody] ProjectInsert model)
        {
            return await _projectService.createProject(model,null);
        }

        [Authorize]
        [HttpPost("createProjectAuthorize")]
        public async Task<IActionResult> createProjectAuthorize([FromBody] ProjectInsert model)
        {   
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.createProject(model, accessToken);
        }

        [Authorize]
        [HttpGet("getProjectDetail")]
        public async Task<IActionResult> getProjectDetail(int id)
        {
            return await _projectService.getProjectById(id);
        }


       


        //[Authorize]
        [HttpGet("getAllProject")]
        public async Task<IActionResult> getAllProject(string keyword="")
        {
            return await _projectService.getAllProject(keyword);
        }


        [Authorize]
        [HttpDelete("deleteProject")]
        public async Task<IActionResult> deleteProject(int projectId)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            List<dynamic> lstId = new List<dynamic>();
            lstId.Add(projectId);
            UserJira user = _userService.getUserByToken(accessToken).Result;
            Project project = _projectRepository.GetSingleByIdAsync(projectId).Result;

            if (project == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Project is not found", MessageConstants.MESSAGE_ERROR_404);

            }
            if (project.creator != user.id)
            {
                return new ResponseEntity(StatusCodeConstants.FORBIDDEN, "Project không phải của bạn đâu đừng delete, nhiều bạn phàn nàn lắm đó !", MessageConstants.MESSAGE_ERROR_404);

            }
            return await _projectService.DeleteByIdAsync(lstId);
        }


        [Authorize]
        [HttpPut("updateProject")]
        public async Task<IActionResult> updateProject(int? projectId, ProjectUpdate projectUpdate)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            List<dynamic> lstId = new List<dynamic>();
            lstId.Add(projectId);
            UserJira user = _userService.getUserByToken(accessToken).Result;
            Project project = _projectRepository.GetSingleByIdAsync(projectId).Result;

            if (project == null)
            {
                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Project is not found", MessageConstants.MESSAGE_ERROR_404);

            }
            if (project.creator != user.id)
            {
                return new ResponseEntity(StatusCodeConstants.FORBIDDEN, "Project không phải của bạn đâu đừng yodate, nhiều bạn phàn nàn lắm đó !", MessageConstants.MESSAGE_ERROR_404);

            }

            return await _projectService.updateProject(projectId, projectUpdate, accessToken);
        }

        [Authorize]
        [HttpPost("assignUserProject")]
        public async Task<IActionResult> assignUserProject([FromBody] UserProject project)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            return await _projectService.addUserProject(project, accessToken);

        }
        [Authorize]
        [HttpPost("assignUserTask")]
        public async Task<IActionResult> assignUserTask([FromBody] TaskUser taskUser)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            return await _projectService.addTaskUser(taskUser, accessToken);

        }

        [Authorize]
        [HttpPost("removeUserFromTask")]
        public async Task<IActionResult> removeUserFromTask([FromBody] TaskUser taskUser)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            return await _projectService.removeUserFromTask(taskUser, accessToken);

        }


        [Authorize]
        [HttpPost("removeUserFromProject")]
        public async Task<IActionResult> removeUserFromProject([FromBody] UserProject project)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];

            return await _projectService.removeUSerFromProject(project, accessToken);

        }


        [Authorize]
        [HttpPut("updateStatus")]
        public async Task<IActionResult> updateStatus([FromBody] UpdateStatusVM model)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.updateStatusTask(model, accessToken);
        }


        [Authorize]
        [HttpPut("updatePriority")]
        public async Task<IActionResult> updatePriority([FromBody] UpdatePiority model)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.updatePiority(model, accessToken);
        }

        [Authorize]
        [HttpPut("updateDescription")]
        public async Task<IActionResult> updateDescription(UpdateDescription model)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.updateDescription(model, accessToken);
        }


        [Authorize]
        [HttpPut("updateTimeTracking")]
        public async Task<IActionResult> updateTimeTracking(TimeTrackingUpdate model)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.updateTimeTracking(model, accessToken);
        }

        [Authorize]
        [HttpPut("updateEstimate")]
        public async Task<IActionResult> updateEstimate(updateEstimate model)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.updateEstimate(model, accessToken);
        }


        [Authorize]
        [HttpPost("createTask")] 
        public async Task<IActionResult> createTask (taskInsert model)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.createTask(model, accessToken);
        }
        [Authorize]
        [HttpPost("updateTask")]
        public async Task<IActionResult> updateTask(TaskEdit model)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.updateTask(model, accessToken);
        }
        [Authorize]
        [HttpDelete("removeTask")]
        public async Task<IActionResult> removeTask(int taskId)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.removeTask(taskId, accessToken);
        }



        [Authorize]
        [HttpGet("getTaskDetail")]
        public async Task<IActionResult> getTaskDetail(int taskId)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            return await _projectService.getTaskDetail(taskId, accessToken);
        }

    }
}
