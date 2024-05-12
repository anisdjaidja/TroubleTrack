namespace TroubleTrack.Model
{
    public class StatisticsReport
    {
        public int ErrorCount { get; set; }
        public Dictionary<string, int>? Trend { get; set; }
        public List<BugReport> Critical { get; set; } = new();
        public List<BugReport> Major { get; set; } = new();
        public List<BugReport> Minor { get; set; } = new();
        public TimeSpan AverageResolutionTime { get; set; }

    }
}
