namespace TroubleTrack.Model
{
    public class Project
    {
        public int ID { get; set; }
        public string projectName { get; set; }
        public List<BugReport> Errors = new List<BugReport>();
    }
}
