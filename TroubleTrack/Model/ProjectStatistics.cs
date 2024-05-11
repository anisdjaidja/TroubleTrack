using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TroubleTrack.Model
{
    public class ProjectStatistics
    {
        public ProjectStatistics(Project project)
        {
            AverageResolutionTime = TimeSpan.FromHours(project.Errors.Average(x => x.ResolutionTime?.TotalHours) ?? 0);

            var result = new Dictionary<string, int>();
            foreach (var bug in project.Errors)
            {
                if (result.ContainsKey(bug.Type))
                    result[bug.Type]++;
                else
                    result.Add(bug.Type, 1);
            }
            BugDistribution = result;
        }

        #region Stats
        public TimeSpan AverageResolutionTime { get; set; }

        public Dictionary<string, int> BugDistribution { get; set; }
        #endregion
    }
}
