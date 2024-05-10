using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using TroubleTrack.Database;
using TroubleTrack.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace TroubleTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        ObservableCollection<Project> Database = new TestSeed().Projects; 
        

        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ILogger<ProjectController> logger)
        {
            _logger = logger;
        }

        // GET: api/Project
        [HttpGet (template:"{projectID}/errors")]
        public IEnumerable<BugReport>? Get(int projectID)
        {
            return Database.Where(x => x.ID == projectID).FirstOrDefault()?.Errors.ToArray();
        }

        // GET api/<ProjectController>/5
        [HttpGet(template: "{projectID}/errors/{errorID}")]
        public BugReport? Get(int projectID, int errorID)
        {
            return Database.Where(p => p.ID == projectID).FirstOrDefault()?.Errors.Where(e => e.ID == errorID).FirstOrDefault();
        }

        // POST api/<ProjectController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
            var errorPool = Database.Where(p => p.ID == projectID).FirstOrDefault()?.Errors;
            if (errorPool == null)
                return BadRequest($"No project with the identifier {projectID}");
            var error = errorPool.Where(e => e.ID == errorID).FirstOrDefault();
            if (error == null)
                return BadRequest($"No error with the identifier {errorID}");
            errorPool.Remove(error);
            return Ok($"error {error.BugName} deleted");
        }
    }
}
