using Microsoft.AspNetCore.Mvc;
using TroubleTrack.Model;
using TroubleTrack.Services;
namespace TroubleTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        ProjectsService _service;

        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ILogger<ProjectsController> logger, ProjectsService projectsService)
        {
            _logger = logger;
            _service = projectsService;
        }

        // Get Platform wide statistics:
        // Get project statistics:
        [HttpGet]
        public StatisticsReport? GetStats()
        {
            var allprojects = _service.GetAllProjects().Result;
            var allbugs = _service.GetAllErrors().Result;
            return new StatisticsReport
            {
                ErrorCount = allbugs.Count,
                AverageResolutionTime = TimeSpan.FromHours(allprojects.Average(x => x.AverageResolutionTime.TotalHours)).Duration(),
                Critical = allbugs.Where(x => x.Severity >= 2).ToList(),
                Major = allbugs.Where(x => x.Severity == 1).ToList(),
                Minor = allbugs.Where(x => x.Severity <= 0).ToList(),
            };
        }

        // Get project statistics:
        [HttpGet(template: "{projectID}/summary")]
        public Project? GetProjectStats(int projectID)
        {
            var projDocument = _service.GetProjectByID(projectID).Result;
            if (projDocument == null)
                return null;

            return projDocument;
        }

        // GET all
        [HttpGet (template:"{projectID}/errors")]
        public IEnumerable<BugReport>? Get(int projectID)
        {
            return _service.GetAllErrors().Result;
        }

        // Get
        [HttpGet(template: "{projectID}/errors/{errorID}")]
        public BugReport? Get(int projectID, int errorID)
        {
           return _service.GetErrorByID(projectID, errorID).Result;
        }
        
        // Create new project
        [HttpPost]
        public ActionResult PostProject([FromBody]string ProjectName)
        {
            var response = _service.INSERT_PROJECT(new Project { projectName = ProjectName }).Result;
            if (response == null)
                return BadRequest($"Please specify a valid project, check documentation for the valid format");
            return Ok(response);
        }

        // Report new bug
        [HttpPost(template: "{projectID}/errors/")]
        public ActionResult Post(int projectID, [FromBody] BugReport bugReport)
        {
            if(_service.GetProjectByID(projectID).Result == null)
                return BadRequest($"Project with ID{projectID} doesnt exist, please specify a valid project to report error");
            bugReport.ResolutionDate = null;
            var response = _service.INSERT_ERROR(projectID, bugReport).Result;
            if (response == null)
                return BadRequest($"Please specify a valid BugReport, check documentation for the valid format");
            return Ok(response);
        }
        
        // update entire bug report document
        [HttpPut(template: "{projectID}/errors/{errorID}")]
        public ActionResult Put(int projectID, int errorID, [FromBody] BugReport updatedBug)
        {
            var result = _service.UPDATE_ERROR(projectID, errorID, updatedBug).Result;
            if (result == null)
                return BadRequest($"Please specify a valid request, check documentation for the valid format, " +
                    $"or check the project id, bugreport id");
            return Ok($"error {updatedBug.BugName} status updated");
        }

        // delete entire bug report document
        [HttpDelete(template: "{projectID}/errors/{errorID}")]
        public ActionResult Delete(int projectID, int errorID)
        {
            if (_service.GetProjectByID(projectID).Result == null)
                return BadRequest($"Project with ID{projectID} doesnt exist, please specify a valid project to delete error");


            var result = _service.DELETE_ERROR(projectID, errorID).Result;
            if (result == null)
                return BadRequest($"No Bug report with the identifier {errorID}");

            return Ok($"error {projectID} - {errorID} deleted");
        }
    }
}
