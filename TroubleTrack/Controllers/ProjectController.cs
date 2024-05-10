using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using TroubleTrack.Database;
using TroubleTrack.Model;
using TroubleTrack.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace TroubleTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        ObservableCollection<Project> Database = new TestSeed().Projects;
        ProjectsService _service;

        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger, ProjectsService projectsService)
        {
            _logger = logger;
            _service = projectsService;
        }

        // GET: api/Project
        [HttpGet (template:"{projectID}/errors")]
        public IEnumerable<BugReport>? Get(int projectID)
        {
            return _service.GetAllErrors().Result;
        }

        // GET api/<ProjectController>/5
        [HttpGet(template: "{projectID}/errors/{errorID}")]
        public BugReport? Get(int projectID, int errorID)
        {
           return _service.GetErrorByID(projectID, errorID).Result;
        }
        // POST api/<ProjectController>
        [HttpPost]
        public ActionResult PostProject([FromBody] Project project)
        {
            var response = _service.INSERT_PROJECT(project).Result;
            if (response == null)
                return BadRequest($"Please specify a valid project, check documentation for the valid format");
            return Ok(response);
        }
        // POST api/<ProjectController>
        [HttpPost(template: "{projectID}/errors/")]
        public ActionResult Post(int projectID, [FromBody] BugReport bugReport)
        {
            if(_service.GetProjectByID(projectID).Result == null)
                return BadRequest($"Project with ID{projectID} doesnt exist, please specify a valid project to report error");
            var response = _service.INSERT_ERROR(projectID, bugReport).Result;
            if (response == null)
                return BadRequest($"Please specify a valid BugReport, check documentation for the valid format");
            return Ok(response);
        }
        

        // PUT api/<ProjectController>/5
        [HttpPut(template: "{projectID}/errors/{errorID}")]
        public ActionResult Put(int projectID, int errorID, [FromBody] BugReport updatedBug)
        {
            if(updatedBug == null) 
                return BadRequest($"Please specify a valid bugReport, check documentation for the valid format");
            
            var errorPool = Database.Where(p => p.ID == projectID).FirstOrDefault()?.Errors;
            if (errorPool == null)
                return BadRequest($"No project with the identifier {projectID}");

            var error = errorPool.Where(e => e.ID == errorID).FirstOrDefault();
            if (error == null)
                return BadRequest($"No error with the identifier {errorID}");

            errorPool.Remove(error);
            errorPool.Add(updatedBug);
            return Ok($"error {updatedBug.BugName} status updated");
        }

        // DELETE api/<ProjectController>/5
        [HttpDelete(template: "{projectID}/errors/{errorID}")]
        public ActionResult Delete(int projectID, int errorID)
        {
            if (_service.GetProjectByID(projectID).Result == null)
                return BadRequest($"Project with ID{projectID} doesnt exist, please specify a valid project to delete error");


            var result = _service.DELETE(projectID, errorID).Result;
            if (result == null)
                return BadRequest($"No Bug report with the identifier {errorID}");

            return Ok($"error {projectID} - {errorID} deleted");
        }
    }
}
