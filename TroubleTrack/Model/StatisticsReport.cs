namespace TroubleTrack.Model
{
    public class StatisticsReport
    {
        public int ErrorCount { get; set; }
        public string Trend { get; set; }
        public List<BugReport> Critical;
        public List<BugReport> Major;
        public List<BugReport> Minor;
        public TimeSpan AverageResolutionTime { get; set; }

    }
}
